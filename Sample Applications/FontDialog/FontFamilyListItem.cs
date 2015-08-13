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
    internal class FontFamilyListItem : TextBlock, IComparable
    {
        private readonly string _displayName;

        public FontFamilyListItem(FontFamily fontFamily)
        {
            _displayName = GetDisplayName(fontFamily);

            FontFamily = fontFamily;
            Text = _displayName;
            ToolTip = _displayName;

            // In the case of symbol font, apply the default message font to the text so it can be read.
            if (IsSymbolFont(fontFamily))
            {
                var range = new TextRange(ContentStart, ContentEnd);
                range.ApplyPropertyValue(FontFamilyProperty, SystemFonts.MessageFontFamily);
            }
        }

        int IComparable.CompareTo(object obj) => string.Compare(_displayName, obj.ToString(), true, CultureInfo.CurrentCulture);

        public override string ToString() => _displayName;

        internal static bool IsSymbolFont(FontFamily fontFamily)
        {
            foreach (var typeface in fontFamily.GetTypefaces())
            {
                GlyphTypeface face;
                if (typeface.TryGetGlyphTypeface(out face))
                {
                    return face.Symbol;
                }
            }
            return false;
        }

        internal static string GetDisplayName(FontFamily family) => NameDictionaryHelper.GetDisplayName(family.FamilyNames);
    }
}