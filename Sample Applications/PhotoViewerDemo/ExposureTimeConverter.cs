// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     Converts an exposure time from a decimal (e.g. 0.0125) into a string (e.g. 1/80)
    /// </summary>
    public class ExposureTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        {
            try
            {
                var exposure = (decimal) value;

                exposure = decimal.Round(1/exposure);
                return $"1/{exposure}";
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            var exposure = ((string) value).Substring(2);
            return (1/decimal.Parse(exposure));
        }
    }
}