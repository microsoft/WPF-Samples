// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace Colors
{
    // Two-way conversion between byte and double (TypeConverter for double should provide this)

    public class ByteToDoubleConverter : IValueConverter
    {
        object IValueConverter.Convert(object o, Type type, object parameter, CultureInfo culture) => Convert.ChangeType(o, typeof(double));

        object IValueConverter.ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            var d = (double) o;
            return (d < 0.0)
                ? (byte) 0
                : (d > 255.0)
                    ? (byte) 255
                    : Convert.ChangeType(o, typeof (byte));
        }
    }
}