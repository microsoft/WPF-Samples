using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace TextFormatting
{
    /// <summary>
    /// Class to implement TextParagraphProperties, used by TextSource
    /// </summary>
    class GenericTextParagraphProperties : TextParagraphProperties
    {
        #region Constructors
        public GenericTextParagraphProperties(
           FlowDirection flowDirection,
           TextAlignment textAlignment,
           bool firstLineInParagraph,
           bool alwaysCollapsible,
           TextRunProperties defaultTextRunProperties,
           TextWrapping textWrap,
           double lineHeight,
           double indent)
        {
            _flowDirection = flowDirection;
            _textAlignment = textAlignment;
            _firstLineInParagraph = firstLineInParagraph;
            _alwaysCollapsible = alwaysCollapsible;
            _defaultTextRunProperties = defaultTextRunProperties;
            _textWrap = textWrap;
            _lineHeight = lineHeight;
            _indent = indent;
        }

        public GenericTextParagraphProperties(FontRendering newRendering, double pixelsPerDip)
        {
            _flowDirection = FlowDirection.LeftToRight;
            _textAlignment = newRendering.TextAlignment;
            _firstLineInParagraph = false;
            _alwaysCollapsible = false;
            _defaultTextRunProperties = new GenericTextRunProperties(
               newRendering.Typeface, pixelsPerDip, newRendering.FontSize, newRendering.FontSize,
               newRendering.TextDecorations, newRendering.TextColor, null,
               BaselineAlignment.Baseline, CultureInfo.CurrentUICulture);
            _textWrap = TextWrapping.Wrap;
            _lineHeight = 0;
            _indent = 0;
            _paragraphIndent = 0;
        }
        #endregion

        #region Properties
        public override FlowDirection FlowDirection
        {
            get { return _flowDirection; }
        }

        public override TextAlignment TextAlignment
        {
            get { return _textAlignment; }
        }

        public override bool FirstLineInParagraph
        {
            get { return _firstLineInParagraph; }
        }

        public override bool AlwaysCollapsible
        {
            get { return _alwaysCollapsible; }
        }

        public override TextRunProperties DefaultTextRunProperties
        {
            get { return _defaultTextRunProperties; }
        }

        public override TextWrapping TextWrapping
        {
            get { return _textWrap; }
        }

        public override double LineHeight
        {
            get { return _lineHeight; }
        }

        public override double Indent
        {
            get { return _indent; }
        }

        public override TextMarkerProperties TextMarkerProperties
        {
            get { return null; }
        }

        public override double ParagraphIndent
        {
            get { return _paragraphIndent; }
        }
        #endregion

        #region Private Fields
        private FlowDirection _flowDirection;
        private TextAlignment _textAlignment;
        private bool _firstLineInParagraph;
        private bool _alwaysCollapsible;
        private TextRunProperties _defaultTextRunProperties;
        private TextWrapping _textWrap;
        private double _indent;
        private double _paragraphIndent;
        private double _lineHeight;
        #endregion
    }

    /// <summary>
    /// Class used to implement TextRunProperties
    /// </summary>
    class GenericTextRunProperties : TextRunProperties
    {
        #region Constructors
        public GenericTextRunProperties(
           Typeface typeface,
           double pixelsPerDip,
           double size,
           double hintingSize,
           TextDecorationCollection textDecorations,
           Brush forgroundBrush,
           Brush backgroundBrush,
           BaselineAlignment baselineAlignment,
           CultureInfo culture)
        {
            if (typeface == null)
                throw new ArgumentNullException("typeface");

            ValidateCulture(culture);

            PixelsPerDip = pixelsPerDip;
            _typeface = typeface;
            _emSize = size;
            _emHintingSize = hintingSize;
            _textDecorations = textDecorations;
            _foregroundBrush = forgroundBrush;
            _backgroundBrush = backgroundBrush;
            _baselineAlignment = baselineAlignment;
            _culture = culture;
        }

        public GenericTextRunProperties(FontRendering newRender, double pixelsPerDip)
        {
            _typeface = newRender.Typeface;
            _emSize = newRender.FontSize;
            _emHintingSize = newRender.FontSize;
            _textDecorations = newRender.TextDecorations;
            _foregroundBrush = newRender.TextColor;
            _backgroundBrush = null;
            _baselineAlignment = BaselineAlignment.Baseline;
            _culture = CultureInfo.CurrentUICulture;
            PixelsPerDip = pixelsPerDip;
        }
        #endregion

        #region Private Methods
        private static void ValidateCulture(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (culture.IsNeutralCulture || culture.Equals(CultureInfo.InvariantCulture))
                throw new ArgumentException("Specific Culture Required", "culture");
        }

        private static void ValidateFontSize(double emSize)
        {
            if (emSize <= 0)
                throw new ArgumentOutOfRangeException("emSize", "Parameter Must Be Greater Than Zero.");
            //if (emSize > MaxFontEmSize)
            //   throw new ArgumentOutOfRangeException("emSize", "Parameter Is Too Large.");
            if (double.IsNaN(emSize))
                throw new ArgumentOutOfRangeException("emSize", "Parameter Cannot Be NaN.");
        }
        #endregion

        #region Properties
        public override Typeface Typeface
        {
            get { return _typeface; }
        }

        public override double FontRenderingEmSize
        {
            get { return _emSize; }
        }

        public override double FontHintingEmSize
        {
            get { return _emHintingSize; }
        }

        public override TextDecorationCollection TextDecorations
        {
            get { return _textDecorations; }
        }

        public override Brush ForegroundBrush
        {
            get { return _foregroundBrush; }
        }

        public override Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
        }

        public override BaselineAlignment BaselineAlignment
        {
            get { return _baselineAlignment; }
        }

        public override CultureInfo CultureInfo
        {
            get { return _culture; }
        }

        public override TextRunTypographyProperties TypographyProperties
        {
            get { return null; }
        }

        public override TextEffectCollection TextEffects
        {
            get { return null; }
        }

        public override NumberSubstitution NumberSubstitution
        {
            get { return null; }
        }
        #endregion

        #region Private Fields
        private Typeface _typeface;
        private double _emSize;
        private double _emHintingSize;
        private TextDecorationCollection _textDecorations;
        private Brush _foregroundBrush;
        private Brush _backgroundBrush;
        private BaselineAlignment _baselineAlignment;
        private CultureInfo _culture;
        #endregion
    }
}