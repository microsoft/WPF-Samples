// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Diagnostics;

namespace EditingExaminerDemo
{
    /// <summary>
    ///     This class parses the command line commands.
    /// </summary>
    public class CommandLine
    {
        private readonly Hashtable _table;

        /// <summary>
        ///     constructor
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="table"></param>
        public CommandLine(string commandLine, Hashtable table)
        {
            _table = table;
            Result = new ObjectItem(null, null);
            DoParse(commandLine);
        }

        /// <summary>
        ///     Retrieve the parsed object.
        /// </summary>
        public ObjectItem Result { get; private set; }

        /// <summary>
        ///     Recursively parse the command line content
        /// </summary>
        /// <param name="commandLine"></param>
        private void DoParse(string commandLine)
        {
            string strLeft;
            string strRight;
            object left;
            var str = commandLine.Replace("  ", " ");
            str = str.Trim();
            //Asignment
            if (str.Contains("="))
            {
                var strs = str.Split('=');
                if (strs.Length != 2 || strs[0].Length == 0 || str[1] == 0)
                {
                    Result.Error = "syntax error: Don't know how to do Assignment!";
                    return;
                }
                strLeft = strRight = string.Empty;
                strLeft = strs[0];
                strRight = strs[1];
                strLeft = strLeft.Trim();
                strRight = strRight.Trim();
                Result = new CommandLine(strRight, _table).Result;

                //Set a value to a variable in the Table.
                if (_table.Contains(strLeft))
                {
                    _table.Remove(strLeft);
                    _table.Add(strLeft, Result.Value);
                }
                //Assign a value to a property.
                else if (strLeft.Contains("."))
                {
                    left =
                        new CommandLine(strLeft.Substring(0, strLeft.LastIndexOf(".", StringComparison.Ordinal)), _table)
                            .Result.Value;
                    if (left != null)
                    {
                        ReflectionUtils.SetProperty(left,
                            strLeft.Substring(strLeft.LastIndexOf(".", StringComparison.Ordinal) + 1),
                            Result.Value);
                    }
                    else
                    {
                        Result.Error = "Failed: can't set value to " + strLeft + "!";
                    }
                }
                //Make a declaration
                else
                {
                    _table.Add(strLeft, Result.Value);
                }
            }
            //Invoke a method, Creating a object.
            else if (str.EndsWith(")"))
            {
                if (str.StartsWith("new "))
                {
                    str = str.Substring(4).Trim();
                }
                left = null;
                strRight = strLeft = str.Substring(0, str.IndexOf("(", StringComparison.Ordinal));

                //If there is a dot, invoke a method from an object
                if (strLeft.LastIndexOf(".", StringComparison.Ordinal) >= 0)
                {
                    strLeft = strLeft.Substring(0, strLeft.LastIndexOf(".", StringComparison.Ordinal));
                    left = new CommandLine(strLeft, _table).Result.Value;
                }

                //When left is null, invoke a constructor, otherwise invoke a method.
                if (left != null)
                {
                    //Get the command line start from the method name
                    //this will get the argument list.
                    str = str.Substring(strRight.LastIndexOf(".", StringComparison.Ordinal) + 1);
                }

                Result.Value = InvokMethod(str, left);
            }

            //Retrive data
            else
            {
                if (_table.Contains(str))
                {
                    Result.Value = _table[str];
                    return;
                }
                left = null;
                strRight = str;

                if (str.Contains("."))
                {
                    strLeft = str.Substring(0, str.IndexOf(".", StringComparison.Ordinal));
                    left = _table[strLeft];

                    //If an object is found in a Table, the property name is needed in order to retrieve the value.
                    if (left != null && str.Length - (strLeft.Length + 1) > 0)
                    {
                        strRight = str.Substring(str.IndexOf(".", StringComparison.Ordinal) + 1,
                            str.Length - (strLeft.Length + 1));
                    }
                }

                Result = Get_Value(left, strRight);

                if (!string.IsNullOrEmpty(Result.Error))
                {
                    //If the expression is just for a static member of a class
                    Result = GetProperty(null, strRight);
                }
            }
        }

        /// <summary>
        ///     Create an object.
        ///     1. new a object: new abc(p1, p2, p3...)
        ///     2. Create a instance for a primative values. for example
        ///     3. Create instance for Enum.
        ///     4. create a instance for struct.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="objectInstance"></param>
        /// <returns></returns>
        private object InvokMethod(string commandLine, object objectInstance)
        {
            string name;
            string arg;
            string[] args;
            object[] objs;
            object returnValue;
            commandLine = commandLine.Trim();
            objs = null;
            returnValue = false;
            name = commandLine.Substring(0, commandLine.IndexOf("(", StringComparison.Ordinal));
            arg = commandLine.Substring(commandLine.IndexOf("(", StringComparison.Ordinal) + 1,
                commandLine.IndexOf(")", StringComparison.Ordinal) -
                (commandLine.IndexOf("(", StringComparison.Ordinal) + 1));
            arg = commandLine.Substring(commandLine.IndexOf("(", StringComparison.Ordinal) + 1);
            arg = arg.Substring(0, arg.Length - 1);
            if (arg.Length > 0)
            {
                args = arg.Trim().Split(',');
                objs = new object[args.Length];

                for (var i = 0; i < args.Length; i++)
                {
                    objs[i] = new CommandLine(args[i], _table).Result.Value;
                }
            }
            else
            {
                objs = new object[0];
            }
            //If there is an instance, we should invoke a instance methods. 
            if (objectInstance != null)
            {
                try
                {
                    //Try the instance methods first.
                    returnValue = ReflectionUtils.InvokeInstanceMethod(objectInstance, name, objs);
                }
                catch (Exception ex)
                {
                    //Try the static methods.
                    //If there the invoking failures here, the Exception message is sent to the Error box.
                    returnValue = ReflectionUtils.InvokeStaticMethod(objectInstance as Type, name, objs);
                    Debug.Write(ex.ToString());
                }
            }
            //Create the object. 
            else
            {
                returnValue = ReflectionUtils.CreateInstanceOfType(name, objs);
            }

            return returnValue;
        }

        /// <summary>
        ///     This method helps recursivly retrieve propeties.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private ObjectItem Get_Value(object o, string name)
        {
            var parseError = "Parser error: the command line can't be evaluated!";

            var result = new ObjectItem(null, null);
            if (string.IsNullOrEmpty(name))
            {
                return result;
            }

            var index = name.IndexOf('.');
            index = (index > 0) ? index : name.Length;
            var left = name.Substring(0, index);

            result = GetProperty(o, left);

            if (!string.IsNullOrEmpty(result.Error))
            {
                result.Error = parseError;
            }

            if (left != name)
            {
                if (result.Value == null)
                {
                    result.Error = parseError;
                }
                else
                {
                    result = Get_Value(result.Value, name.Substring(index + 1));
                }
            }

            return result;
        }

        private ObjectItem GetProperty(object objectInstance, string propertyName)
        {
            object returnValue = null;
            for (var i = 0; i <= 8; i++)
            {
                try
                {
                    switch (i)
                    {
                        case 0:
                            returnValue = ReflectionUtils.GetField(objectInstance, propertyName);
                            break;
                        case 1:

                            returnValue = ReflectionUtils.GetProperty(objectInstance, propertyName);
                            break;
                        case 2:
                            returnValue = ReflectionUtils.GetStaticField(objectInstance as Type, propertyName);
                            break;
                        case 3:
                            returnValue = ReflectionUtils.GetInterfaceProperty(objectInstance, propertyName,
                                propertyName);
                            break;
                        case 4:
                            returnValue = Convert.ToInt32(propertyName);
                            break;
                        case 5:
                            returnValue = Convert.ToDouble(propertyName);
                            break;
                        case 6:
                            if (propertyName.StartsWith("\"") && propertyName.EndsWith("\""))
                            {
                                returnValue = propertyName.Substring(1, propertyName.Length - 2);
                            }
                            else
                            {
                                throw new Exception("error");
                            }
                            break;
                        case 7:

                            returnValue = ReflectionUtils.GetStaticProperty(objectInstance as Type, propertyName);
                            break;
                        case 8:
                            if (objectInstance == null)
                            {
                                returnValue = ReflectionUtils.FindType(propertyName);
                            }
                            else
                            {
                                throw new Exception("");
                            }
                            break;
                    }
                    return new ObjectItem(returnValue, "");
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.ToString());
                }
                if (returnValue != null)
                {
                    break;
                }
            }
            return new ObjectItem(null, "Parser error: the command line can't be evaluated!");
        }
    }
}