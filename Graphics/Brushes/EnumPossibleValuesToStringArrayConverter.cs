// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Brushes
{
    [ValueConversion(typeof (object), typeof (string[]))]
    public class EnumPossibleValuesToStringArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => new ArrayList(Enum.GetNames(value.GetType()));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}