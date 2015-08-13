// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace FontDialog
{
    internal class TypographicFeatureListItem : TextBlock, IComparable
    {
        private readonly string _displayName;

        public TypographicFeatureListItem(string displayName, DependencyProperty chooserProperty)
        {
            _displayName = displayName;
            ChooserProperty = chooserProperty;
            Text = displayName;
        }

        public DependencyProperty ChooserProperty { get; }

        int IComparable.CompareTo(object obj) => string.Compare(_displayName, obj.ToString(), true, CultureInfo.CurrentCulture);

        public override string ToString() => _displayName;
    }
}