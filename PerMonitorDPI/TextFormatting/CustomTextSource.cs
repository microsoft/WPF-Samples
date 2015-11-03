using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.TextFormatting;

namespace TextFormatting
{
    // CustomTextSource is our implementation of TextSource.  This is required to use the WPF
    // text engine. This implementation is very simplistic as is DOES NOT monitor spans of text
    // for different properties. The entire text content is considered a single span and all 
    // changes to the size, alignment, font, etc. are applied across the entire text.
    class CustomTextSource : TextSource
    {
        public CustomTextSource(double pixelsPerDip)
        {
            PixelsPerDip = pixelsPerDip;
        }
        // Used by the TextFormatter object to retrieve a run of text from the text source.
        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
                throw new ArgumentOutOfRangeException("textSourceCharacterIndex", "Value must be greater than 0.");
            if (textSourceCharacterIndex >= _text.Length)
            {
                return new TextEndOfParagraph(1);
            }

            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < _text.Length)
            {
                return new TextCharacters(
                   _text,
                   textSourceCharacterIndex,
                   _text.Length - textSourceCharacterIndex,
                   new GenericTextRunProperties(_currentRendering, PixelsPerDip));
            }

            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
        {
            CharacterBufferRange cbr = new CharacterBufferRange(_text, 0, textSourceCharacterIndexLimit);
            return new TextSpan<CultureSpecificCharacterBufferRange>(
             textSourceCharacterIndexLimit,
             new CultureSpecificCharacterBufferRange(System.Globalization.CultureInfo.CurrentUICulture, cbr)
             );
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #region Properties
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public FontRendering FontRendering
        {
            get { return _currentRendering; }
            set { _currentRendering = value; }
        }
        #endregion

        #region Private Fields

        private string _text;      //text store
        private FontRendering _currentRendering;

        #endregion
    }
}
