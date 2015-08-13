// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BindConversion
{
    public class MyConverter : IValueConverter
    {
        public object Convert(object o, Type type,
            object parameter, CultureInfo culture)
        {
            var date = (DateTime) o;
            switch (type.Name)
            {
                case "String":
                    return date.ToString("F", culture);
                case "Brush":
                    return Brushes.Red;
                default:
                    return o;
            }
        }

        public object ConvertBack(object o, Type type,
            object parameter, CultureInfo culture) => null;
    }
}