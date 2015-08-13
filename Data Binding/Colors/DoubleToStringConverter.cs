// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace Colors
{
    // One-way conversion between double and string (adding formatting)

    public class DoubleToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object o, Type type, object parameter, CultureInfo culture) => $"{o:f2}";

        object IValueConverter.ConvertBack(object o, Type type, object parameter, CultureInfo culture) => null;
    }
}