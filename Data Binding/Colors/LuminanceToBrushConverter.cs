// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Colors
{
    // One-way conversion between Luminance and Brush - used to draw text in
    // a color tile using a color that contrasts with the background

    public class LuminanceToBrushConverter : IValueConverter
    {
        object IValueConverter.Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            var luminance = (double) o;
            return (luminance < 0.5) ? Brushes.White : Brushes.Black;
        }

        object IValueConverter.ConvertBack(object o, Type type, object parameter, CultureInfo culture) => null;
    }
}