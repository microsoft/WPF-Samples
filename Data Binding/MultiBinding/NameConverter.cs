// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace MultiBinding
{
    public class NameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string name;

            switch ((string) parameter)
            {
                case "FormatLastFirst":
                    name = values[1] + ", " + values[0];
                    break;
                default:
                    name = values[0] + " " + values[1];
                    break;
            }

            return name;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var splitValues = ((string) value).Split(' ');
            return splitValues;
        }
    }
}