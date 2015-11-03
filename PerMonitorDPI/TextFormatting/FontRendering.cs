using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace TextFormatting
{
    /// <summary>
    /// Class for combining Font and other text related properties. 
    /// (Typeface, Alignment, Decorations, etc)
    /// </summary>
    class FontRendering
    {
        #region Constructors
        public FontRendering(
           double emSize,
           TextAlignment alignment,
           TextDecorationCollection decorations,
           Brush textColor,
           Typeface face)
        {
            _fontSize = emSize;
            _alignment = alignment;
            _textDecorations = decorations;
            _textColor = textColor;
            _typeface = face;
        }

        public FontRendering()
        {
            _fontSize = 12.0f;
            _alignment = TextAlignment.Left;
            _textDecorations = new TextDecorationCollection();
            _textColor = Brushes.Black;
            _typeface = new Typeface(new FontFamily("Arial"),
               FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }
        #endregion

        #region Properties
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Parameter Must Be Greater Than Zero.");
                if (double.IsNaN(value))
                    throw new ArgumentOutOfRangeException("value", "Parameter Cannot Be NaN.");
                _fontSize = value;
            }
        }

        public TextAlignment TextAlignment
        {
            get { return _alignment; }
            set { _alignment = value; }
        }

        public TextDecorationCollection TextDecorations
        {
            get { return _textDecorations; }
            set { _textDecorations = value; }
        }

        public Brush TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public Typeface Typeface
        {
            get { return _typeface; }
            set { _typeface = value; }
        }
        #endregion

        #region Private Fields
        private double _fontSize;
        private TextAlignment _alignment;
        private TextDecorationCollection _textDecorations;
        private Brush _textColor;
        private Typeface _typeface;
        #endregion
    }
}
