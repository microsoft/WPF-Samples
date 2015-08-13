// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

// Type
// CultureInfo
// MemoryStream
// IValueConverter
// Section, TextRange

// XamlReader

namespace AnnotatedDocumentViewer
{
    public class AnnotationDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert 64 bit binary data into an 8 bit byte array and load
            // it into a memory buffer
            var data = System.Convert.FromBase64String(value as string);
            using (var buffer = new MemoryStream(data))
            {
                // Convert memory buffer to object and return text
                var section = (Section) XamlReader.Load(buffer);
                var range = new TextRange(section.ContentStart, section.ContentEnd);
                return range.Text;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}