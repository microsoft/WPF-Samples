//---------------------------------------------------------------------------
// 
// File: XamlRtfConverter.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Prototype for Xaml-Rtf conversion(XamlToRtf or RtfToXaml)
//
//---------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace DocumentSerialization
{
    /// <summary>
    /// XamlRtfConverter is a static class that convert from/to rtf string to/from xaml string.
    /// </summary>
    internal static class XamlRtfConverter
    {
        // ---------------------------------------------------------------------
        //
        // Internal Methods
        //
        // ---------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        /// Converts an xaml string into rtf string.
        /// </summary>
        /// <param name="xamlString">
        /// Input xaml string.
        /// </param>
        /// <returns>
        /// Well-formed representing rtf equivalent for the input xaml string.
        /// </returns>
        internal static string ConvertXamlToRtf(string xamlContent)
        {
            // Get XamlRtfConverter type and get the internal ConvertXamlToRtf method
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(System.Windows.FrameworkElement));
            Type xamlRtfConverterType = assembly.GetType("System.Windows.Documents.XamlRtfConverter");

            // Create an instance of XamlToRtfConverter class
            object xamlRtfConverter = Activator.CreateInstance(xamlRtfConverterType, /*nonPublic:*/true);

            // Converts xaml to rtf by using XamlRtfConverter
            System.Reflection.MethodInfo convertXamlToRtf = xamlRtfConverterType.GetMethod("ConvertXamlToRtf", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            string rtfContent = (string)convertXamlToRtf.Invoke(xamlRtfConverter, new object[] { xamlContent });

            // Return rtf content as string
            return rtfContent;
        }

        /// <summary>
        /// Converts an rtf string into xaml string.
        /// </summary>
        /// <param name="rtfString">
        /// Input rtf string.
        /// </param>
        /// <returns>
        /// Well-formed xml representing XAML equivalent for the input rtf string.
        /// </returns>
        internal static string ConvertRtfToXaml(string rtfContent)
        {
            // Get XamlRtfConverter type and get the internal ConvertRtfToXaml method
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(System.Windows.FrameworkElement));
            Type xamlRtfConverterType = assembly.GetType("System.Windows.Documents.XamlRtfConverter");

            // Create an instance of XamlToRtfConverter class
            object xamlRtfConverter = Activator.CreateInstance(xamlRtfConverterType, /*nonPublic:*/true);

            // Converts xaml from rtf by using XamlRtfConverter
            System.Reflection.MethodInfo convertRtfToXaml = xamlRtfConverterType.GetMethod("ConvertRtfToXaml", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            // Converts rtf to xaml by using XamlRtfConverter
            string xamlContent = (string)convertRtfToXaml.Invoke(xamlRtfConverter, new object[] { rtfContent });

            // Return xaml content as string
            return xamlContent;
        }

        #endregion Internal Methods

        // ---------------------------------------------------------------------
        //
        // Private Fields
        //
        // ---------------------------------------------------------------------

        #region Private Fields

        private const int RtfCodePage = 1252;

        #endregion Private Fields
    }
}
