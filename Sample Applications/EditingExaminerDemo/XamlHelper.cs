// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace EditingExaminerDemo
{
    /// <summary>
    ///     Provides help functions for processing XAML.
    /// </summary>
    public class XamlHelper
    {
        /// <summary>
        ///     Get XAML from TextRange.Xml property
        /// </summary>
        /// <param name="range">TextRange</param>
        /// <returns>return a string serialized from the TextRange</returns>
        public static string TextRange_GetXml(TextRange range)
        {
            MemoryStream mstream;

            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            mstream = new MemoryStream();
            range.Save(mstream, DataFormats.Xaml);

            //must move the stream pointer to the beginning since range.save() will move it to the end.
            mstream.Seek(0, SeekOrigin.Begin);

            //Create a stream reader to read the xaml.
            var stringReader = new StreamReader(mstream);

            return stringReader.ReadToEnd();
        }

        /// <summary>
        ///     Set XML to TextRange.Xml property.
        /// </summary>
        /// <param name="range">TextRange</param>
        /// <param name="xaml">XAML to be set</param>
        public static void TextRange_SetXml(TextRange range, string xaml)
        {
            MemoryStream mstream;
            if (null == xaml)
            {
                throw new ArgumentNullException(nameof(xaml));
            }
            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            mstream = new MemoryStream();
            var sWriter = new StreamWriter(mstream);

            mstream.Seek(0, SeekOrigin.Begin); //this line may not be needed.
            sWriter.Write(xaml);
            sWriter.Flush();

            //move the stream pointer to the beginning. 
            mstream.Seek(0, SeekOrigin.Begin);

            range.Load(mstream, DataFormats.Xaml);
        }

        /// <summary>
        ///     Parse a string to WPF object.
        /// </summary>
        /// <param name="str">string to be parsed</param>
        /// <returns>return an object</returns>
        public static object ParseXaml(string str)
        {
            var ms = new MemoryStream(str.Length);
            var sw = new StreamWriter(ms);
            sw.Write(str);
            sw.Flush();

            ms.Seek(0, SeekOrigin.Begin);

            var pc = new ParserContext {BaseUri = new Uri(Environment.CurrentDirectory + "/")};


            return XamlReader.Load(ms, pc);
        }

        public static string IndentXaml(string xaml)
        {
            //open the string as an XML node
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xaml);
            var nodeReader = new XmlNodeReader(xmlDoc);

            //write it back onto a stringWriter
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            xmlWriter.WriteNode(nodeReader, false);

            var result = stringWriter.ToString();
            xmlWriter.Close();

            return result;
        }

        public static string RemoveIndentation(string xaml)
        {
            if (xaml.Contains("\r\n    "))
            {
                return RemoveIndentation(xaml.Replace("\r\n    ", "\r\n"));
            }
            return xaml.Replace("\r\n", "");
        }

        public static string ColoringXaml(string xaml)
        {
            string[] strs;
            var value = "";
            string s1, s2;
            s1 =
                "<Section xml:space=\"preserve\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph>";
            s2 = "</Paragraph></Section>";

            strs = xaml.Split('<');
            for (var i = 1; i < strs.Length; i++)
            {
                value += ProcessEachTag(strs[i]);
            }
            return s1 + value + s2;
        }

        private static string ProcessEachTag(string str)
        {
            // In high contrast themes, we need to ensure the syntax highlighting follows system colors.
            // Replace the normal colors here with visible ones so that we can still see the highlights.
            var BlueNormalSyntaxHighlight = (SystemParameters.HighContrast) ? SystemColors.WindowTextColor.ToString() : "Blue";
            var RedNormalSyntaxHighlight = (SystemParameters.HighContrast) ? SystemColors.HotTrackColor.ToString() : "Red";

            var front = $"<Run Foreground=\"{BlueNormalSyntaxHighlight}\">&lt;</Run>";
            var end = $"<Run Foreground=\"{BlueNormalSyntaxHighlight}\">&gt;</Run>";
            var frontWithSlash = $"<Run Foreground=\"{BlueNormalSyntaxHighlight}\">&lt;/</Run>";
            var endWithSlash = $"<Run Foreground=\"{BlueNormalSyntaxHighlight}\"> /&gt;</Run>"; //a space is added.
            var tagNameStart = "<Run FontWeight=\"Bold\">";
            var propertynameStart = $"<Run Foreground=\"{RedNormalSyntaxHighlight}\">";
            var propertyValueStart = $"\"<Run Foreground=\"{BlueNormalSyntaxHighlight}\">";
            var endRun = "</Run>";

            string returnValue;
            string[] strs;
            var i = 0;

            if (str.StartsWith("/"))
            {
                //if the tag is an end tag, remove the "/"
                returnValue = frontWithSlash;
                str = str.Substring(1).TrimStart();
            }
            else
            {
                returnValue = front;
            }
            strs = str.Split('>');
            str = strs[0];
            i = (str.EndsWith("/")) ? 1 : 0;

            str = str.Substring(0, str.Length - i).Trim();

            if (str.Contains("=")) //the tag has a property
            {
                //set tagName 
                returnValue += tagNameStart + str.Substring(0, str.IndexOf(" ", StringComparison.Ordinal)) + endRun +
                               " ";
                str = str.Substring(str.IndexOf(" ", StringComparison.Ordinal)).Trim();
            }
            else //no property
            {
                returnValue += tagNameStart + str.Trim() + endRun + " ";
                //nothing left to parse
                str = "";
            }

            //Take care of properties:
            while (str.Length > 0)
            {
                returnValue += propertynameStart + str.Substring(0, str.IndexOf("=", StringComparison.Ordinal)) + endRun +
                               "=";
                str = str.Substring(str.IndexOf("\"", StringComparison.Ordinal) + 1).Trim();
                returnValue += propertyValueStart + str.Substring(0, str.IndexOf("\"", StringComparison.Ordinal)) +
                               endRun + "\" ";
                str = str.Substring(str.IndexOf("\"", StringComparison.Ordinal) + 1).Trim();
            }

            if (returnValue.EndsWith(" "))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 1);
            }

            returnValue += (i == 1) ? endWithSlash : end;

            //Add the content after the ">"
            returnValue += strs[1];

            return returnValue;
        }
    }
}