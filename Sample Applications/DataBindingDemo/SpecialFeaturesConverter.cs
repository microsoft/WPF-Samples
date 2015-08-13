// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataBindingDemo
{
    internal class SpecialFeaturesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return false;
            if (values[0] == DependencyProperty.UnsetValue) return false;
            if (values[1] == DependencyProperty.UnsetValue) return false;
            var rating = (int) values[0];
            var date = (DateTime) values[1];

            // if the user has a good rating (10+) and has been a member for more than a year, special features are available
            if ((rating >= 10) && (date.Date < (DateTime.Now.Date - new TimeSpan(365, 0, 0, 0))))
            {
                return true;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => new[] { Binding.DoNothing, Binding.DoNothing };
    }
}