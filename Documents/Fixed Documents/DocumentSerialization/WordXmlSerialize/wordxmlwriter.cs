//////////////////////////////////////////////////////////////////////
//
// WordXmlWriter.cs
//
// Provides a class to support writing Word 2003 XML files.
//
//////////////////////////////////////////////////////////////////////

namespace DocumentSerialization
{
    #region Namespaces.

    using System;
    using System.IO;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Policy;
    using System.Xml;
    using System.Xml.Xsl;
    using System.Xml.XPath;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Controls;
    using System.Windows.Media;

    #endregion Namespaces.

    /// <summary>Encapsulates the operation of writing WordXML documents.</summary>
    class WordXmlWriter
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors.

        /// <summary>Initializes a new WordXmlWriter instance.</summary>
        internal WordXmlWriter()
        {
            _dir = LogicalDirection.Forward;
        }

        #endregion Constructors.

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Protected Methods
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal methods.

        /// <summary>
        /// Writes the container into the specified XmlWriter.
        /// </summary>
        internal void Write(TextPointer start, TextPointer end, XmlWriter writer)
        {
            System.Diagnostics.Debug.Assert(start != null);
            System.Diagnostics.Debug.Assert(end != null);
            System.Diagnostics.Debug.Assert(start.CompareTo(end) <= 0);
            System.Diagnostics.Debug.Assert(writer != null);

            WriteContainer(start, end, writer);
        }

        #endregion Internal methods.

        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #region Private methods.

        /// <summary>
        /// Checks whether the specified property is applicable to paragraphs.
        /// </summary>
        private bool IsParagraphProperty(DependencyProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return false;
        }

        /// <summary>
        /// Checks whether the specified property is applicable to ranges.
        /// </summary>
        private bool IsRangeProperty(DependencyProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return
                (property == TextBlock.FontWeightProperty) ||
                (property == TextBlock.FontStyleProperty) ||
                (property == TextBlock.ForegroundProperty) ||
                (property == TextBlock.FontSizeProperty) ||
                (property == TextBlock.FontFamilyProperty);
        }

        /// <summary>
        /// Writes the container into the specified XmlWriter.
        /// </summary>
        private void WriteContainer(TextPointer start, TextPointer end, XmlWriter writer)
        {
            TextElement textElement;

            System.Diagnostics.Debug.Assert(start != null);
            System.Diagnostics.Debug.Assert(end != null);
            System.Diagnostics.Debug.Assert(writer != null);

            _writer = writer;

            WriteWordXmlHead();

            _cursor = start;
            while (_cursor.CompareTo(end) < 0)
            {
                switch (_cursor.GetPointerContext(_dir))
                {
                    case TextPointerContext.None:
                        System.Diagnostics.Debug.Assert(false,
                            "Next symbol should never be None if cursor < End.");
                        break;
                    case TextPointerContext.Text:
                        RequireOpenRange();
                        _writer.WriteStartElement(WordXmlSerializer.WordTextTag);
                        _writer.WriteString(_cursor.GetTextInRun(_dir));
                        _writer.WriteEndElement();
                        break;
                    case TextPointerContext.EmbeddedElement:
                        DependencyObject obj = _cursor.GetAdjacentElement(LogicalDirection.Forward);
                        if (obj is LineBreak)
                        {
                            RequireOpenRange();
                            _writer.WriteStartElement(WordXmlSerializer.WordBreakTag);
                            _writer.WriteEndElement();
                        }
                        // TODO: try to convert some known embedded objects.
                        break;
                    case TextPointerContext.ElementStart:
                        TextPointer position;
                        position = _cursor;
                        position = position.GetNextContextPosition(LogicalDirection.Forward);
                        textElement = position.Parent as TextElement;

                        if (textElement is Paragraph)
                        {
                            RequireClosedRange();
                            RequireOpenParagraph();
                        }
                        else if (textElement is Inline)
                        {
                            RequireClosedRange();
                            RequireOpenParagraph();
                            RequireOpenRange();
                        }
                        break;
                    case TextPointerContext.ElementEnd:
                        textElement = _cursor.Parent as TextElement;

                        if (textElement is Inline)
                        {
                            RequireClosedRange();
                        }
                        else if (textElement is Paragraph)
                        {
                            RequireClosedParagraph();
                        }
                        break;
                }
                _cursor = _cursor.GetNextContextPosition(_dir);
            }

            RequireClosedRange();
            WriteWordXmlTail();
        }

        /// <summary>
        /// Writes the given property to the output document.
        /// </summary>
        /// <param name="property">Property to write.</param>
        /// <param name="value">Value of property to write.</param>
        /// <remarks>
        /// If the property is not supported, this method does nothing.
        /// </remarks>
        private void WriteProperty(DependencyProperty property, object value)
        {
            if (property == TextBlock.FontWeightProperty)
            {
                FontWeight weight;

                weight = (FontWeight)value;
                _writer.WriteStartElement(WordXmlSerializer.WordBoldTag);
                _writer.WriteAttributeString(WordXmlSerializer.WordValue,
                    (weight > FontWeights.Medium)? WordXmlSerializer.WordOn : WordXmlSerializer.WordOff);
                _writer.WriteEndElement();
            }
            else if (property == TextBlock.FontStyleProperty)
            {
                FontStyle style;

                style = (FontStyle)value;
                _writer.WriteStartElement(WordXmlSerializer.WordItalicTag);
                _writer.WriteAttributeString(WordXmlSerializer.WordValue,
                    (style != FontStyles.Normal)? WordXmlSerializer.WordOn : WordXmlSerializer.WordOff);
                _writer.WriteEndElement();
            }
            else if (property == TextBlock.ForegroundProperty)
            {
                SolidColorBrush brush;
                Color color;

                // Note: gradient and other brushes not supported.
                brush = value as SolidColorBrush;
                if (brush != null)
                {
                    color = brush.Color;
                    _writer.WriteStartElement(WordXmlSerializer.WordColorTag);
                    _writer.WriteAttributeString(WordXmlSerializer.WordValue,
                        String.Format( "{0:x2}{1:x2}{2:x2}",
                        color.R, color.G, color.B));
                    _writer.WriteEndElement();
                }
            }
            else if (property == TextBlock.FontSizeProperty)
            {
                double size;

                // w:sz is specified in 1/144ths of an inch
                size = (double)value;
                _writer.WriteStartElement(WordXmlSerializer.WordFontSizeTag);
                _writer.WriteAttributeString(WordXmlSerializer.WordValue,
                    ((int)((size * 72.0 / 96.0) * 2)).ToString());
                _writer.WriteEndElement();
            }
            else if (property == TextBlock.FontFamilyProperty)
            {
                string fontFamily;

                fontFamily = value.ToString();
                _writer.WriteStartElement("w:rFonts");
                _writer.WriteAttributeString("w:ascii", fontFamily);
                _writer.WriteAttributeString("w:h-ansi", fontFamily);
                _writer.WriteAttributeString("w:cs", fontFamily);
                _writer.WriteEndElement();

                _writer.WriteStartElement("wx:font");
                _writer.WriteAttributeString("wx:val", fontFamily);
                _writer.WriteEndElement();
            }
        }

        /// <summary>Writes the properties for a range of text (w:rPr).</summary>
        private void WriteRangeProperties()
        {
            System.Diagnostics.Debug.Assert(_inRange);
            System.Diagnostics.Debug.Assert(_dir == LogicalDirection.Forward);

            WriteProperties(WordXmlSerializer.WordRangePropertiesTag,
                RangeProperties);
        }

        /// <summary>Writes the properties for a range of text (w:pPr).</summary>
        private void WriteParagraphProperties()
        {
            System.Diagnostics.Debug.Assert(_inParagraph);
            System.Diagnostics.Debug.Assert(_dir == LogicalDirection.Forward);

            WriteProperties(WordXmlSerializer.WordParagraphPropertiesTag,
                ParagraphProperties);
        }

        /// <summary>Writes properties in the specified properties element.</summary>
        /// <param name="tagName">Name of element to write properties in.</param>
        /// <param name="properties">Properties to write.</param>
        private void WriteProperties(string tagName, DependencyProperty[] properties)
        {
            DependencyObject parent;
            TextPointer position;

            position = _cursor;
            position = position.GetNextContextPosition(LogicalDirection.Forward);
            parent = position.Parent;

            _writer.WriteStartElement(tagName);
            foreach(DependencyProperty property in properties)
            {
                WriteProperty(property, parent.GetValue(property));
            }
            _writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the XML document header and all required WordXML
        /// opening tags and namespace declarations.
        /// </summary>
        /// <remarks>This is symmetrical with WriteWordXmlTail.</remarks>
        private void WriteWordXmlHead()
        {
            _writer.WriteStartDocument(true);
            _writer.WriteProcessingInstruction(
                "mso-application", "progid=\"Word.Document\"");
            _writer.WriteStartElement("w", "wordDocument",
                "http://schemas.microsoft.com/office/word/2003/wordml");

            _writer.WriteAttributeString("xmlns", "wx", null, "http://schemas.microsoft.com/office/word/2003/auxHint");
            // NOTE: this will be required for more advanced conversions.
            // _writer.WriteAttributeString("xmlns", "v", null, "urn:schemas-microsoft-com:vml");
            // _writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
            // _writer.WriteAttributeString("xmlns", "w10", null, "urn:schemas-microsoft-com:office:word");
            // _writer.WriteAttributeString("xmlns", "sl", null, "http://schemas.microsoft.com/schemaLibrary/2003/core");
            // _writer.WriteAttributeString("xmlns", "aml", null, "http://schemas.microsoft.com/aml/2001/core");
            // _writer.WriteAttributeString("xmlns", "dt", null, "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");

            _writer.WriteAttributeString("xml:space", "preserve");

            _writer.WriteStartElement("w:body");
            _writer.WriteStartElement("w:sect");
        }

        /// <summary>
        /// Writes the tail of the WordXML file.
        /// </summary>
        /// <remarks>This is symmetrical with WriteWordXmlHead.</remarks>
        private void WriteWordXmlTail()
        {
            _writer.WriteEndElement();   // Close w:sect.
            _writer.WriteEndElement();   // Close w:body.
            _writer.WriteEndElement();   // Close w:wordDocument.
        }

        /// <summary>Ensures that an open range is available.</summary>
        private void RequireOpenRange()
        {
            RequireOpenParagraph();
            if (!_inRange)
            {
                _writer.WriteStartElement(WordXmlSerializer.WordRangeTag);
                _inRange = true;
                WriteRangeProperties();
            }
        }

        /// <summary>Ensures than an open paragraph is available.</summary>
        private void RequireOpenParagraph()
        {
            if (!_inParagraph)
            {
                _writer.WriteStartElement(WordXmlSerializer.WordParagraphTag);
                _inParagraph = true;
                WriteParagraphProperties();
            }
        }

        /// <summary>Ensures that there is no open range.</summary>
        private void RequireClosedRange()
        {
            if (_inRange)
            {
                // Close w:r.
                _writer.WriteEndElement();
                _inRange = false;
            }
        }

        /// <summary>Ensures that there is no open paragraph.</summary>
        private void RequireClosedParagraph()
        {
            RequireClosedRange();
            if (_inParagraph)
            {
                // Close w:p.
                _writer.WriteEndElement();
                _inParagraph = false;
            }
        }

        #endregion Private methods.

        //------------------------------------------------------
        //
        //  Private Properties
        //
        //------------------------------------------------------

        #region Private properties.

        /// <summary>Paragraph-level properties in Word object model.</summary>
        private DependencyProperty[] ParagraphProperties
        {
            get
            {
                if (_paragraphProperties == null)
                {
                    _paragraphProperties = new DependencyProperty[] {
                        Block.MarginProperty,
                        Block.TextAlignmentProperty
                    };
                }
                return _paragraphProperties;
            }
        }

        /// <summary>Range-level properties in Word object model.</summary>
        private DependencyProperty[] RangeProperties
        {
            get
            {
                if (_rangeProperties == null)
                {
                    _rangeProperties = new DependencyProperty[] {
                        TextElement.FontFamilyProperty,
                        TextElement.FontSizeProperty,
                        TextElement.FontWeightProperty,
                        TextElement.FontStyleProperty,
                        TextElement.ForegroundProperty,
                    };
                }
                return _rangeProperties;
            }
        }

        #endregion Private properties.

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private fields.

        /// <summary>Cursor moving through document.</summary>
        private TextPointer _cursor;

        /// <summary>Direction in which the cursor moves.</summary>
        /// <remarks>
        /// For the current implementatino, the direction is always
        /// forward. This may change for more complex implementations.
        /// Until then, this acts as a constant (but constants cannot
        /// refer to enumeration valueS).
        /// </remarks>
        private readonly LogicalDirection _dir;

        /// <summary>Whether writer has opened a paragraph.</summary>
        private bool _inParagraph;

        /// <summary>Whether writer has opened a range.</summary>
        private bool _inRange;

        /// <summary>Paragraph-level properties in Word object model</summary>
        private static DependencyProperty[] _paragraphProperties;

        /// <summary>Range-level properties in Word object model</summary>
        private static DependencyProperty[] _rangeProperties;

        /// <summary>Writer used to output document.</summary>
        private XmlWriter _writer;

        #endregion Private fields.
    }
}
