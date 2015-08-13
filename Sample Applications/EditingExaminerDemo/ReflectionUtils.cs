// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

namespace EditingExaminerDemo
{

    #region Namespaces.

    #endregion Namespaces.

    /// <summary>
    ///     Provides Reflection utility methods for test cases.
    /// </summary>
    public static class ReflectionUtils
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public methods.

        /// <summary>Adds an event handler to a named event on an object.</summary>
        /// <param name='target'>Object on which to add event handler.</param>
        /// <param name='eventName'>Name of event to add to.</param>
        /// <param name='handler'>Delegate for event.</param>
        public static void AddInstanceEventHandler(object target, string eventName, Delegate handler)
        {
            AddRemoveInstanceEventHandler(target, eventName, handler, true);
        }

        /// <summary>
        ///     Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static object CreateInstance(Type type) => Activator.CreateInstance(type);

        /// <summary>
        ///     Supply type name and it will create the object for you.
        /// </summary>
        /// <param name="typeName">name of the type it will try to create</param>
        /// <param name="args">args to the ctor of the object</param>
        /// <returns>A new instance of the specified type.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object CreateInstanceOfType(string typeName, object[] args)
        {
            Type type; // Type to be created.

            if (typeName == null)
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            if (typeName.Length == 0)
            {
                throw new ArgumentException("typeName should not be empty.",
                    nameof(typeName));
            }

            new ReflectionPermission(
                PermissionState.Unrestricted).
                Assert();

            type = FindType(typeName);
            if (type == null)
            {
                throw new Exception("FindType failed to find type " + typeName);
            }

            // we let it throw exception when error occurs.
            // ============================================
            return Activator.CreateInstance(type, args, null);
        }


        /// <summary>
        ///     Retrieves a type given its name.
        /// </summary>
        /// <param name='typeName'>
        ///     Simple name, name with namespace, or type name with
        ///     partial or fully qualified assembly to look for.
        /// </param>
        /// <returns>The specified Type.</returns>
        /// <remarks>
        ///     If an assembly is specified, then this method is equivalent
        ///     to calling Type.GetType(). Otherwise, all loaded assemblies
        ///     will be used to look for the type (unlike Type.GetType(),
        ///     which would only look in the calling assembly and in mscorlib).
        /// </remarks>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static Type FindType(string typeName)
        {
            Type returnValue = null;
            var preferedType = "System.Windows";
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            // A comma indicates a type with an assembly reference
            if (typeName.IndexOf(',') != -1)
                return Type.GetType(typeName, true);

            // A period indicates a namespace is present.
            var hasNamespace = typeName.IndexOf('.') != -1;

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (hasNamespace)
                {
                    var t = assembly.GetType(typeName);
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    // For type names that are not fully qualified
                    // and don't even have namespaces, explicitly
                    // opt out of WinForms (avoids collision with WPF
                    // the more common case).
                    if (assembly.FullName.Contains("Forms"))
                    {
                        continue;
                    }
                    var testTypes = SafeGetTypes(assembly);
                    foreach (var type in testTypes)
                    {
                        if (type != null && type.Name == typeName)
                        {
                            if (type.FullName.Contains(preferedType))
                            {
                                return type;
                            }
                            if (returnValue == null)
                            {
                                returnValue = type;
                            }
                        }
                    }
                }
            }
            if (returnValue != null)
            {
                return returnValue;
            }
            throw new InvalidOperationException(
                "Unable to find type [" + typeName + "] in loaded assemblies");
        }

        /// <summary>
        ///     Gets the name of a type from the (possibly) full-qualified name.
        /// </summary>
        /// <param name="fullTypeName">Full name of type.</param>
        /// <returns>The bare name.</returns>
        /// <example>
        ///     The following code shows how to use this method.
        ///     <code>...
        /// private void LogTypeName() {
        ///   string typeName = "My.Namespace.Type.HelloThere, myassembly ver1.2.";
        ///   System.Diagnostics.Debug.Assert(TestFinder.GetNameFromFullTypeName(typeName) == "HelloThere");
        /// }
        /// </code>
        /// </example>
        public static string GetNameFromFullTypeName(string fullTypeName)
        {
            if (fullTypeName == null)
            {
                throw new ArgumentNullException(nameof(fullTypeName));
            }
            if (fullTypeName.Length == 0)
            {
                throw new ArgumentException("Full type name cannot be empty", nameof(fullTypeName));
            }
            // Remove the assembly reference.
            var comma = fullTypeName.IndexOf(',');
            if (comma != -1)
            {
                fullTypeName = fullTypeName.Substring(0, comma);
            }
            // Remove the prefixed namespaces.
            var lastPeriod = fullTypeName.LastIndexOf('.');
            if (lastPeriod == -1)
                return fullTypeName;
            return fullTypeName.Substring(lastPeriod + 1);
        }

        /// <summary>
        ///     Retrieves the value of a named field, disregarding the
        ///     visibility.
        /// </summary>
        /// <param name="target">Object from which to get value.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The value of the specified field.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object GetField(object target, string fieldName)
        {
            Type type;
            Type queryType;
            FieldInfo fieldInfo;

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            if (fieldName.Length == 0)
            {
                throw new ArgumentException("Field name cannot be blank", fieldName);
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            const BindingFlags bindingAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.FlattenHierarchy;

            type = target.GetType();
            queryType = type;
            fieldInfo = queryType.GetField(fieldName, bindingAttr);
            while (fieldInfo == null && queryType != typeof (object))
            {
                queryType = queryType.BaseType;
                fieldInfo = queryType?.GetField(fieldName, bindingAttr);
            }
            if (fieldInfo == null)
            {
                throw new InvalidOperationException(
                    "Unable to retrieve field information for field [" +
                    fieldName + "] with attributes [" + bindingAttr +
                    "] from type [" + type + "]");
            }
            return fieldInfo.GetValue(target);
        }

        /// <summary>
        ///     Retrieves the value of a named property as implementing the
        ///     specified interface.
        /// </summary>
        /// <param name="target">Object from which to get value.</param>
        /// <param name="interfaceName">Name of the interface on which property is declared.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the specified property.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object GetInterfaceProperty(object target,
            string interfaceName, string propertyName)
        {
            BindingFlags bindingFlags; // Flags to match property to get.
            Type type; // Type to get property from.

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (interfaceName == null)
            {
                throw new ArgumentNullException(nameof(interfaceName));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            type = target.GetType();
            type = type.GetInterface(interfaceName);
            bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                           BindingFlags.Instance | BindingFlags.GetProperty;
            return type.InvokeMember(propertyName, bindingFlags, null, target, null);
        }

        /// <summary>
        ///     Retrieves the value of a named property.
        /// </summary>
        /// <param name="target">Object from which to get value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the specified property.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object GetProperty(object target, string propertyName)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentException("Property name cannot be blank", propertyName);
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            const BindingFlags bindingAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.FlattenHierarchy;
            var type = target.GetType();
            var propertyInfo = type.GetProperty(propertyName, bindingAttr);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(
                    "Unable to retrieve property information for property [" +
                    propertyName + "] with attributes [" + bindingAttr +
                    "] from type [" + type + "]");
            }
            return propertyInfo.GetValue(target, null);
        }

        /// <summary>
        ///     Retrieves the value of a static named field.
        /// </summary>
        /// <param name="type">Type from which to get value.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The value of the specified field.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object GetStaticField(Type type, string fieldName)
        {
            FieldInfo fieldInfo;

            const BindingFlags bindingAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Static | BindingFlags.FlattenHierarchy;

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            if (fieldName.Length == 0)
            {
                throw new ArgumentException("Field name cannot be blank", fieldName);
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            fieldInfo = type.GetField(fieldName, bindingAttr);
            if (fieldInfo == null)
            {
                throw new InvalidOperationException(
                    "Unable to retrieve field information for property [" +
                    fieldName + "] with attributes [" + bindingAttr +
                    "] from type [" + type + "]");
            }
            return fieldInfo.GetValue(null);
        }

        /// <summary>
        ///     Retrieves the value of a static named property.
        /// </summary>
        /// <param name="type">Type from which to get value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the specified property.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object GetStaticProperty(Type type, string propertyName)
        {
            PropertyInfo propertyInfo;

            const BindingFlags bindingAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Static | BindingFlags.FlattenHierarchy;

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (propertyName.Length == 0)
            {
                throw new ArgumentException("Property name cannot be blank", propertyName);
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            propertyInfo = type.GetProperty(propertyName, bindingAttr);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(
                    "Unable to retrieve property information for property [" +
                    propertyName + "] with attributes [" + bindingAttr +
                    "] from type [" + type + "]");
            }
            return propertyInfo.GetValue(null, null);
        }

        /// <summary>
        ///     Retrieves the type of the named property on a type.
        /// </summary>
        /// <param name="type">Type that has the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     The Type of the specified property.
        /// </returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static Type GetPropertyType(Type type, string propertyName)
        {
            const BindingFlags bindingAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.FlattenHierarchy;

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            var propertyInfo = type.GetProperty(propertyName, bindingAttr);
            return propertyInfo.PropertyType;
        }

        /// <summary>
        ///     Returns an object suitable for comparison with the specified
        ///     target type from the given object.
        /// </summary>
        /// <param name='o'>Object to return value for.</param>
        /// <param name='targetType'>Target type.</param>
        /// <remarks>
        ///     This method is similar to the Change.ChangeType API call.
        ///     However, if o is a string with value '*null', a null value
        ///     will be returned. If targetType is System.String and o is
        ///     not null, o.ToString() will be returned rather than the
        ///     simple ChangeType (*null if o is null). If the target is an
        ///     enumeration and o is a string, then the Enum.Parse method
        ///     is invoked. Also, all conversions are done with the invariant
        ///     culture.
        /// </remarks>
        public static object GetValueForComparison(object o, Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            const string nullString = "*null";
            var isTargetString = targetType == typeof (string);
            var isTargetEnum = targetType.IsSubclassOf(typeof (Enum));

            // Handle the case of a null object.
            if (o == null)
            {
                return (isTargetString) ? nullString : null;
            }
            if (isTargetString)
            {
                return o.ToString();
            }
            if ((o is string) && isTargetEnum)
            {
                return Enum.Parse(targetType, (string) o, true);
            }
            return Convert.ChangeType(o, targetType,
                CultureInfo.InvariantCulture);
        }

        /// <summary>Removes an event handler from a named event on an object.</summary>
        /// <param name='target'>Object from which to remove event handler.</param>
        /// <param name='eventName'>Name of event to remove from.</param>
        /// <param name='handler'>Delegate for event.</param>
        public static void RemoveInstanceEventHandler(object target, string eventName, Delegate handler)
        {
            AddRemoveInstanceEventHandler(target, eventName, handler, false);
        }

        /// <summary>
        ///     Retrieves types, guaranteeing that no exceptions will be thown.
        /// </summary>
        /// <param name="assembly">Assembly to get types from.</param>
        /// <returns>
        ///     The types in the specifies assembly, or a zero-length array if an
        ///     exception was thrown and no types could be accessed. If only some
        ///     types are loaded, these are returned, and the returned array
        ///     will have null elements.
        /// </returns>
        public static Type[] SafeGetTypes(Assembly assembly)
        {
            // AssemblySW assemblySW = AssemblySW.Wrap(assembly);
            var testTypes = Type.EmptyTypes;
            try
            {
                var types = assembly.GetTypes();
                testTypes = new Type[types.Length];
                for (var i = 0; i < types.Length; i++)
                {
                    testTypes[i] = types[i];
                }
            }
            catch (ReflectionTypeLoadException re)
            {
                var sb = new StringBuilder();
                sb.Append("ReflectionTypeLoadException thrown while getting types for ");
                sb.Append(assembly.FullName);
                sb.Append(Environment.NewLine);
                sb.Append(re);
                sb.Append("Loader exceptions: ");
                sb.Append(re.LoaderExceptions.Length);
                sb.Append(Environment.NewLine);
                foreach (var exception in re.LoaderExceptions)
                {
                    sb.Append(exception);
                    sb.Append(Environment.NewLine);
                }

                return re.Types;
            }
            catch (Exception exception)
            {
                Debug.Write(exception.ToString());
            }
            return testTypes;
        }

        /// <summary>
        ///     Invokes a method defined on a specific interface on the
        ///     given object.
        /// </summary>
        /// <param name="instance">Object to invoke method on.</param>
        /// <param name="interfaceName">Name of interface defining the method.</param>
        /// <param name="methodName">Name of method to invoke.</param>
        /// <param name="methodArguments">Arguments for the method.</param>
        /// <returns>The return value of the invoked member.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object InvokeInterfaceMethod(object instance, string interfaceName,
            string methodName, object[] methodArguments)
        {
            BindingFlags bindingFlags; // Flags to match method to invoke.
            Type type; // Type to invoke method on.

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (interfaceName == null)
            {
                throw new ArgumentNullException(nameof(interfaceName));
            }
            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            if (methodArguments == null)
            {
                throw new ArgumentNullException(nameof(methodArguments));
            }

            type = instance.GetType();
            type = type.GetInterface(interfaceName);
            bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                           BindingFlags.Instance | BindingFlags.InvokeMethod;
            return type.InvokeMember(methodName, bindingFlags, null, instance,
                methodArguments);
        }

        /// <summary>
        ///     Invokes a method on an object.
        /// </summary>
        /// <param name="instance">Object to invoke method on.</param>
        /// <param name="methodName">Name of method to invoke.</param>
        /// <param name="methodArguments">Arguments for the method.</param>
        /// <returns>The return value of the invoked member.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object InvokeInstanceMethod(object instance, string methodName,
            object[] methodArguments)
        {
            BindingFlags bindingFlags; // Flags to match method to invoke.
            Type type; // Type to invoke method on.

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            if (methodArguments == null)
            {
                throw new ArgumentNullException(nameof(methodArguments));
            }

            type = instance.GetType();
            bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                           BindingFlags.Instance | BindingFlags.InvokeMethod;
            return type.InvokeMember(methodName, bindingFlags, null, instance,
                methodArguments);
        }

        /// <summary>
        ///     Invokes a method on an type.
        /// </summary>
        /// <param name="type">Type to invoke method on.</param>
        /// <param name="methodName">Name of method to invoke.</param>
        /// <param name="methodArguments">Arguments for the method.</param>
        /// <returns>The return value of the invoked member.</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object InvokeStaticMethod(Type type, string methodName,
            object[] methodArguments)
        {
            BindingFlags bindingFlags; // Flags to match method to invoke.

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            if (methodArguments == null)
            {
                throw new ArgumentNullException(nameof(methodArguments));
            }

            bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                           BindingFlags.Static | BindingFlags.InvokeMethod;
            return type.InvokeMember(methodName, bindingFlags, null, null,
                methodArguments);
        }

        /// <summary>
        ///     call className.methodName static method
        /// </summary>
        /// <param name="className">name of the class where the method exists.</param>
        /// <param name="methodName">name of the method to invoke</param>
        /// <param name="parameters">parameters to the method</param>
        /// <param name="invokeType">Invoke type. See Test.Uis.Utils.InvokeType for details</param>
        /// <returns>return value of hte invoked method</returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static object InvokePropertyOrMethod(string className,
            string methodName, object[] parameters, InvokeType invokeType)
        {
            var perm =
                new ReflectionPermission(
                    PermissionState.Unrestricted);
            perm.Assert();

            Type type;
            bool isGetProperty;
            object[] newargs;
            object instance;
            BindingFlags defaultFlags;
            object resultObject;

            //if (ActionManager.IsInvokeTypeStatic(invokeType))
            //{
            //    if (String.IsNullOrEmpty(className))
            //    {
            //        throw new ArgumentException(
            //            "className cannot be null or empty for static invocations",
            //            "className");
            //    }
            //    type = ReflectionUtils.FindType(className);
            //    if (type == null)
            //    {
            //       throw new InvalidOperationException("FindType fails");
            //    }
            //}
            //else
            //{
            if (parameters == null || parameters.Length == 0)
            {
                throw new ArgumentException(
                    "parameters cannot be null or empty for instance invocations",
                    nameof(parameters));
            }
            if (parameters[0] == null)
            {
                throw new ArgumentException(
                    "parameters cannot have a null first element for instance invocations",
                    nameof(parameters));
            }
            type = parameters[0].GetType();
            //}
            Debug.Assert(type != null);

            defaultFlags = BindingFlags.ExactBinding | BindingFlags.Public;
            newargs = null;
            instance = null;

            switch (invokeType)
            {
                case InvokeType.StaticMethod:
                    defaultFlags |= BindingFlags.InvokeMethod;
                    defaultFlags |= BindingFlags.Static;
                    break;
                case InvokeType.InstanceMethod:
                    defaultFlags |= BindingFlags.InvokeMethod;
                    defaultFlags |= BindingFlags.Instance;
                    instance = ExtractFirstParameterAsInstance(parameters, ref newargs);
                    parameters = newargs;
                    break;
                case InvokeType.GetStaticProperty:
                    defaultFlags |= BindingFlags.GetProperty;
                    defaultFlags |= BindingFlags.Static;
                    break;
                case InvokeType.SetStaticProperty:
                    defaultFlags |= BindingFlags.SetProperty;
                    defaultFlags |= BindingFlags.Static;
                    break;
                case InvokeType.GetInstanceProperty:
                    defaultFlags |= BindingFlags.GetProperty;
                    defaultFlags |= BindingFlags.Instance;
                    instance = ExtractFirstParameterAsInstance(parameters, ref newargs);
                    parameters = newargs;
                    break;
                case InvokeType.SetInstanceProperty:
                    defaultFlags |= BindingFlags.SetProperty;
                    defaultFlags |= BindingFlags.Instance;
                    instance = ExtractFirstParameterAsInstance(parameters, ref newargs);
                    parameters = newargs;
                    break;
                case InvokeType.GetStaticField:
                    defaultFlags |= BindingFlags.GetField;
                    defaultFlags |= BindingFlags.Static;
                    break;
                case InvokeType.SetStaticField:
                    defaultFlags |= BindingFlags.SetField;
                    defaultFlags |= BindingFlags.Static;
                    break;
                case InvokeType.GetInstanceField:
                    defaultFlags |= BindingFlags.GetField;
                    defaultFlags |= BindingFlags.Instance;
                    instance = ExtractFirstParameterAsInstance(parameters, ref newargs);
                    parameters = newargs;
                    break;
                case InvokeType.SetInstanceField:
                    defaultFlags |= BindingFlags.SetField;
                    defaultFlags |= BindingFlags.Instance;
                    instance = ExtractFirstParameterAsInstance(parameters, ref newargs);
                    parameters = newargs;
                    break;
            }

            try
            {
                resultObject = type.InvokeMember(
                    methodName, defaultFlags, null, instance, parameters);
            }
            catch (MissingMethodException e)
            {
                isGetProperty = invokeType == InvokeType.GetInstanceProperty ||
                                invokeType == InvokeType.GetStaticProperty;
                if (parameters.Length > 0 && isGetProperty)
                {
                }
                throw e;
            }
            catch (AmbiguousMatchException e)
            {
                throw e;
            }
            return resultObject;
        }

        /// <summary>
        ///     Sets the value of the named property.
        /// </summary>
        /// <param name='target'>Object on which to set value.</param>
        /// <param name='propertyName'>Name of property to set.</param>
        /// <param name='value'>Value of property.</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void SetProperty(object target, string propertyName, object value)
        {
            const BindingFlags invokeAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetProperty;

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();

            object[] args = {value};
            var type = target.GetType();
            type.InvokeMember(propertyName, invokeAttr, null, target, args);
        }

        #endregion Public methods.

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #region Private methods.

        /// <summary>Adds or removes an event handler to a named event on an object.</summary>
        /// <param name='target'>Object on which to add/remove event handler.</param>
        /// <param name='eventName'>Name of event to add/remove to.</param>
        /// <param name='handler'>Delegate for event.</param>
        /// <param name='add'>true to add the handler, false to remove it.</param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void AddRemoveInstanceEventHandler(object target, string eventName,
            Delegate handler, bool add)
        {
            const BindingFlags bindingAttr =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance;

            EventInfo eventInfo;
            Type type;

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (eventName == null)
            {
                throw new ArgumentNullException(nameof(eventName));
            }
            if (eventName.Length == 0)
            {
                throw new ArgumentException("Event name should not be blank.", nameof(eventName));
            }

            new ReflectionPermission(
                PermissionState.Unrestricted)
                .Assert();
            type = target.GetType();
            eventInfo = type.GetEvent(eventName, bindingAttr);
            if (eventInfo == null)
            {
                throw new InvalidOperationException(
                    "Unable to retrieve event information for event [" +
                    eventName + "] with attributes [" + bindingAttr +
                    "] from type [" + type + "]");
            }
            if (add)
            {
                eventInfo.AddEventHandler(target, handler);
            }
            else
            {
                eventInfo.RemoveEventHandler(target, handler);
            }
        }

        /// <summary>
        ///     Describes the specified array of parameters.
        /// </summary>
        /// <param name="parameters">Parameters to describe.</param>
        /// <returns>A string describing the specified parameters.</returns>
        private static string DescribeParameters(object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return "No parameter supplied.";
            }
            var result = "";
            for (var i = 0; i < parameters.Length; i++)
            {
                result += "Parameter " + i + ": ";
                if (parameters[i] == null)
                {
                    result += "null\r\n";
                }
                else
                {
                    result += "[" + parameters[i] + "]" +
                              " (" + parameters[i].GetType().Name + ")\r\n";
                }
            }
            return result;
        }

        /// <summary>
        ///     Extract first parameter in args and return it in instance parameter,
        ///     the rest of objects in args are copied to newargs.
        /// </summary>
        /// <param name="args">array of objects to be extracted</param>
        /// <param name="newargs">aray of objects in args excluding the first one</param>
        /// <returns>args[0] as object</returns>
        private static object ExtractFirstParameterAsInstance(object[] args, ref object[] newargs)
        {
            if (args.Length > 0)
            {
                var instance = args[0];
                newargs = null;
                if (args.Length > 1)
                {
                    // if we call instance method the first object in args is the instance
                    // we need to extract that.
                    // ===================================================================
                    newargs = new object[args.Length - 1];
                    for (var i = 1; i < args.Length; i++)
                    {
                        newargs[i - 1] = args[i];
                    }
                }
                return instance;
            }
            throw new ArgumentException("args doesn't have any element", nameof(args));
        }

        #endregion Private methods.
    }
}