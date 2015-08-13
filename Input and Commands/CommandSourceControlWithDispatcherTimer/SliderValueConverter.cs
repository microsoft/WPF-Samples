// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace CommandSourceControlWithDispatcherTimer
{
    [ValueConversion(typeof (double), typeof (int))]
    public class SliderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var sliderValue = (double) value;

            return (int) sliderValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter,
            CultureInfo culture) => null;
    }
}