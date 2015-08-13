// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace Colors
{
    // One-way conversion between ColorItem.Source and bool -
    // used to disable editing for builtin colors

    public class SourceToBoolConverter : IValueConverter
    {
        object IValueConverter.Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            var source = (ColorItem.Sources) o;
            return (source != ColorItem.Sources.BuiltIn);
        }

        object IValueConverter.ConvertBack(object o, Type type, object parameter, CultureInfo culture) => null;
    }
}