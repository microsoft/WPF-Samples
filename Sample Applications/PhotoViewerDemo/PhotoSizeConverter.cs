// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     Converts an x,y size pair into a string value (e.g. 1600x1200)
    /// </summary>
    public class PhotoSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == null) || (values[1] == null))
            {
                return string.Empty;
            }
            return $"{values[0]}x{values[1]}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if ((string) value == string.Empty)
            {
                return new object[2];
            }
            string[] sSize;
            sSize = ((string) value).Split('x');

            var size = new object[2];
            size[0] = uint.Parse(sSize[0]);
            size[1] = uint.Parse(sSize[1]);
            return size;
        }
    }
}