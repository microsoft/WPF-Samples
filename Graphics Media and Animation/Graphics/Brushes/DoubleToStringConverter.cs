// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace Brushes
{
    [ValueConversion(typeof (double), typeof (string))]
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double) value;
            return d.ToString("F4");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return double.Parse((string) value);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}