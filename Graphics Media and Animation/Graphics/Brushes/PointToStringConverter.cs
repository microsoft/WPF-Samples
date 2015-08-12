// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Brushes
{
    [ValueConversion(typeof (Point), typeof (string))]
    public class PointToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = (Point) value;
            return p.X.ToString("F4") + "," + p.Y.ToString("F4");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Point.Parse((string) value);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}