// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace DialogBox
{
    public class FontPropertyLists
    {
        private static Collection<FontFamily> _fontFaces;
        private static Collection<FontStyle> _fontStyles;
        private static Collection<FontWeight> _fontWeights;
        private static Collection<double> _fontSizes;

        /// <summary>
        ///     Return a collection of avaliable font styles
        /// </summary>
        public static ICollection<FontFamily> FontFaces
        {
            get
            {
                if (_fontFaces == null) _fontFaces = new Collection<FontFamily>();
                foreach (var fontFamily in Fonts.SystemFontFamilies)
                {
                    _fontFaces.Add(fontFamily);
                }
                return _fontFaces;
            }
        }

        /// <summary>
        ///     Return a collection of avaliable font styles
        /// </summary>
        public static ICollection FontStyles => _fontStyles ?? (_fontStyles = new Collection<FontStyle>
        {
            System.Windows.FontStyles.Normal,
            System.Windows.FontStyles.Oblique,
            System.Windows.FontStyles.Italic
        });

        /// <summary>
        ///     Return a collection of avaliable FontWeight
        /// </summary>
        public static ICollection FontWeights => _fontWeights ?? (_fontWeights = new Collection<FontWeight>
        {
            System.Windows.FontWeights.Thin,
            System.Windows.FontWeights.Light,
            System.Windows.FontWeights.Regular,
            System.Windows.FontWeights.Normal,
            System.Windows.FontWeights.Medium,
            System.Windows.FontWeights.Heavy,
            System.Windows.FontWeights.SemiBold,
            System.Windows.FontWeights.DemiBold,
            System.Windows.FontWeights.Bold,
            System.Windows.FontWeights.Black,
            System.Windows.FontWeights.ExtraLight,
            System.Windows.FontWeights.ExtraBold,
            System.Windows.FontWeights.ExtraBlack,
            System.Windows.FontWeights.UltraLight,
            System.Windows.FontWeights.UltraBold,
            System.Windows.FontWeights.UltraBlack
        });

        /// <summary>
        ///     Return a collection of font sizes.
        /// </summary>
        public static Collection<double> FontSizes
        {
            get
            {
                if (_fontSizes == null)
                {
                    _fontSizes = new Collection<double>();
                    for (double i = 8; i <= 40; i++) _fontSizes.Add(i);
                }
                return _fontSizes;
            }
        }

        public static bool CanParseFontStyle(string fontStyleName)
        {
            try
            {
                var converter = new FontStyleConverter();
                converter.ConvertFromString(fontStyleName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static FontStyle ParseFontStyle(string fontStyleName)
        {
            var converter = new FontStyleConverter();
            return (FontStyle) converter.ConvertFromString(fontStyleName);
        }

        public static bool CanParseFontWeight(string fontWeightName)
        {
            try
            {
                var converter = new FontWeightConverter();
                converter.ConvertFromString(fontWeightName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static FontWeight ParseFontWeight(string fontWeightName)
        {
            var converter = new FontWeightConverter();
            return (FontWeight) converter.ConvertFromString(fontWeightName);
        }
    }
}