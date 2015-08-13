// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace BindingToMethod
{
    public class DoubleToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var strValue = value as string;
            if (strValue != null)
            {
                double result;
                var converted = double.TryParse(strValue, out result);
                if (converted)
                {
                    return result;
                }
            }
            return null;
        }
    }
}