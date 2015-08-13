// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace MediaGallery
{
    public class PositionConverter : IValueConverter
    {
        public object Convert(object o, Type type, object param, CultureInfo cul)
        {
            var currPosition = (TimeSpan) o;
            return currPosition.Seconds;
        }

        public object ConvertBack(object o, Type type, object param, CultureInfo cul) => null;
    }
}