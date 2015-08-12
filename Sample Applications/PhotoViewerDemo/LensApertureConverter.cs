// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     Converts a lens aperture from a decimal into a human-preferred string (e.g. 1.8 becomes F1.8)
    /// </summary>
    public class LensApertureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return $"F{value:##.0}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            if (!string.IsNullOrEmpty((string) value))
            {
                return decimal.Parse(((string) value).Substring(1));
            }
            return null;
        }
    }
}