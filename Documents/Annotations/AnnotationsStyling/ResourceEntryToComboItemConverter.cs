// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AnnotationsStyling
{
    [ValueConversion(typeof (Collection<StyleMetaData>),
        typeof (Collection<ResourceDictionary>))]
    public class ResourceEntryToComboItemConverter : IValueConverter
    {
        // ----------------------------- Convert ------------------------------
        /// <summary>
        ///     Parses a collection of ResourceDictionaries and
        ///     extracts all StickyNote styles.
        /// </summary>
        /// <returns>
        ///     A list of Styles and their associated
        ///     keys (for identification).
        /// </returns>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var unfiltered =
                value as Collection<ResourceDictionary>;

            var filtered =
                new Collection<StyleMetaData>();

            foreach (var dict in unfiltered)
            {
                foreach (string key in dict.Keys)
                {
                    var aStyle = dict[key] as Style;
                    if (aStyle != null && aStyle.TargetType.Equals(typeof (StickyNoteControl)))
                        filtered.Add(new StyleMetaData(key, aStyle));
                }
            }

            return filtered;
        } // end:Convert
        // --------------------------- ConvertBack ----------------------------
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture) => null;
    }
}