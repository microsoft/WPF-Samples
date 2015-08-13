// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FontDialog
{
    internal class TypefaceListItem : TextBlock, IComparable
    {
        private readonly string _displayName;
        private readonly bool _simulated;

        public TypefaceListItem(Typeface typeface)
        {
            _displayName = GetDisplayName(typeface);
            _simulated = typeface.IsBoldSimulated || typeface.IsObliqueSimulated;

            FontFamily = typeface.FontFamily;
            FontWeight = typeface.Weight;
            FontStyle = typeface.Style;
            FontStretch = typeface.Stretch;

            var itemLabel = _displayName;

            if (_simulated)
            {
                var formatString = Properties.Resources.ResourceManager.GetString(
                    "simulated",
                    CultureInfo.CurrentUICulture
                    );
                itemLabel = string.Format(formatString, itemLabel);
            }

            Text = itemLabel;
            ToolTip = itemLabel;

            // In the case of symbol font, apply the default message font to the text so it can be read.
            if (FontFamilyListItem.IsSymbolFont(typeface.FontFamily))
            {
                var range = new TextRange(ContentStart, ContentEnd);
                range.ApplyPropertyValue(FontFamilyProperty, SystemFonts.MessageFontFamily);
            }
        }

        public Typeface Typeface => new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

        int IComparable.CompareTo(object obj)
        {
            var item = obj as TypefaceListItem;
            if (item == null)
            {
                return -1;
            }

            // Sort all simulated faces after all non-simulated faces.
            if (_simulated != item._simulated)
            {
                return _simulated ? 1 : -1;
            }

            // If weight differs then sort based on weight (lightest first).
            var difference = FontWeight.ToOpenTypeWeight() - item.FontWeight.ToOpenTypeWeight();
            if (difference != 0)
            {
                return difference > 0 ? 1 : -1;
            }

            // If style differs then sort based on style (Normal, Italic, then Oblique).
            var thisStyle = FontStyle;
            var otherStyle = item.FontStyle;

            if (thisStyle != otherStyle)
            {
                if (thisStyle == FontStyles.Normal)
                {
                    // This item is normal style and should come first.
                    return -1;
                }
                if (otherStyle == FontStyles.Normal)
                {
                    // The other item is normal style and should come first.
                    return 1;
                }
                // Neither is normal so sort italic before oblique.
                return (thisStyle == FontStyles.Italic) ? -1 : 1;
            }

            // If stretch differs then sort based on stretch (Normal first, then numerically).
            var thisStretch = FontStretch;
            var otherStretch = item.FontStretch;

            if (thisStretch != otherStretch)
            {
                if (thisStretch == FontStretches.Normal)
                {
                    // This item is normal stretch and should come first.
                    return -1;
                }
                if (otherStretch == FontStretches.Normal)
                {
                    // The other item is normal stretch and should come first.
                    return 1;
                }
                // Neither is normal so sort numerically.
                return thisStretch.ToOpenTypeStretch() < otherStretch.ToOpenTypeStretch() ? -1 : 0;
            }

            // They're the same.
            return 0;
        }

        public override string ToString() => _displayName;

        internal static string GetDisplayName(Typeface typeface) => NameDictionaryHelper.GetDisplayName(typeface.FaceNames);
    }
}