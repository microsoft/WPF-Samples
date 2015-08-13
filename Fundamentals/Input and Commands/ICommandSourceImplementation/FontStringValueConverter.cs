// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace ICommandSourceImplementation
{
    [ValueConversion(typeof (double), typeof (int))]
    public class FontStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fontSize = (string) value;
            int intFont;

            try
            {
                intFont = int.Parse(fontSize);
                return intFont;
            }
            catch (FormatException e)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}