//////////////////////////////////////////////////////////////////////
//
// WordXmlReader.cs
//
// Provides a class to support reading Word 2003 XML files.
//
//////////////////////////////////////////////////////////////////////

namespace DocumentSerialization
{
    #region Namespaces.

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.Policy;
    using System.Xml;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Controls;
    using System.Windows.Media;

    #endregion Namespaces.

    /// <summary>Encapsulates the operation of reading WordXML documents.</summary>
    class WordXmlReader
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        #region Constructors.

        /// <summary>Initializes a new WordXmlReader instance.</summary>
        internal WordXmlReader()
        {
            _dir = LogicalDirection.Forward;
        }

        #endregion Constructors.

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
        /// Reads the document from the reader into the specified position.
        /// </summary>
        internal void Read(XmlReader reader, TextPointer position)
        {
            System.Diagnostics.Debug.Assert(reader != null);
            System.Diagnostics.Debug.Assert(position != null);

            ReadDocument(reader, position);
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
        /// Applies the correct style for the current paragraph.
        /// </summary>
        private void ApplyStyleToParagraph()
        {
            ResourceDictionary styleDictionary;

            System.Diagnostics.Debug.Assert(_cursor.Parent.GetType() == typeof(Paragraph));

            styleDictionary = GetResources(_cursor);
            foreach(Style style in styleDictionary.Values)
            {
                // TODO: consider applying the paragraph with the matching style id.
                if (style.TargetType == typeof(Paragraph))
                {
                    foreach (Setter setter in style.Setters)
                    {
                        _cursor.Parent.SetValue(setter.Property, setter.Value);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Creates an exception to be thrown when an unexpected XML node
        /// was found in the specfied reader.
        /// </summary>
        private static Exception CreateUnexpectedNodeException(XmlReader reader)
        {
            string location;
            IXmlLineInfo lineInfo;

            System.Diagnostics.Debug.Assert(reader != null);

            lineInfo = reader as IXmlLineInfo;
            if (lineInfo != null && lineInfo.HasLineInfo())
            {
                location = " at line " + lineInfo.LineNumber +
                    ", position " + lineInfo.LinePosition;
            }
            else
            {
                location = "";
            }

            return new InvalidOperationException("Unexpected node [" +
                reader.Name + "] of type " + reader.NodeType.ToString() +
                location + ".");
        }

        /// <summary>
        /// Moves the writing cursor out of the specified element. Because
        /// WordXML should be 'well-nested', there should be no need
        /// to move more than exactly one symbol.
        /// </summary>
        private void MoveOutOfElement(Type elementType)
        {
            System.Diagnostics.Debug.Assert(elementType != null);
            System.Diagnostics.Debug.Assert(_cursor.Parent.GetType() == elementType);
            System.Diagnostics.Debug.Assert(_cursor.GetPointerContext(_dir) == TextPointerContext.ElementEnd);
            _cursor = _cursor.GetNextContextPosition(_dir);
        }

        /// <summary>
        /// Sets a value on the specified property at the current cursor
        /// position.
        /// </summary>
        private void SetValue(DependencyProperty property, object value)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (IsPopulatingStyle)
            {
                System.Diagnostics.Debug.Assert(_currentStyle != null);
                _currentStyle.Setters.Add (new Setter(property, value));
            }
            else
            {
                _cursor.Parent.SetValue(property, value);
            }
        }

        /// <summary>
        /// Surrounds the writing cursor with an empty element of the
        /// specified type.
        /// </summary>
        private void SurroundWithElement(TextElement element)
        {
            System.Diagnostics.Debug.Assert(element != null);

            ThumbViewer._reflectionTextPointer_InsertTextElement.Invoke(_cursor, new object[1] { element });

            System.Diagnostics.Debug.Assert(
                _cursor.GetPointerContext(_otherDir) == TextPointerContext.ElementEnd);

            _cursor = _cursor.GetNextContextPosition(_otherDir);
        }

        /// <summary>
        /// Reads the document from the reader into the specified position.
        /// </summary>
        private void ReadDocument(XmlReader reader, TextPointer position)
        {
            // Initialize all fields.
            _cursor = position;
            _dir = LogicalDirection.Forward;
            _otherDir = LogicalDirection.Backward;
            _reader = reader;

            // Set the gravity of the cursor to always look forward.
            _cursor = _cursor.GetPositionAtOffset(0, _dir);

            while (_reader.Read())
            {
                switch (_reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        System.Diagnostics.Debug.Assert(false,
                            "Attributes should never be processed by top-level convertion loop.");
                        break;
                    case XmlNodeType.EndElement:
                        System.Diagnostics.Trace.WriteLine("WordXmlReader.ReadDocument - EndElement [" +
                            _reader.Name + "]");
                        switch (_reader.Name)
                        {
                            case WordXmlSerializer.WordParagraphTag:
                                MoveOutOfElement(typeof(Paragraph));
                                _inParagraph = false;
                                break;
                            case WordXmlSerializer.WordRangeTag:
                                MoveOutOfElement(typeof(Run));
                                _inRange = false;
                                break;
                            case WordXmlSerializer.WordStyleTag:
                                _currentStyle = null;
                                break;
                            case WordXmlSerializer.WordTextTag:
                                _inText = false;
                                break;
                        }
                        break;
                    case XmlNodeType.Element:
                        System.Diagnostics.Trace.WriteLine("WordXmlReader.ReadDocument - Element [" +
                            _reader.Name + "]");
                        switch (_reader.Name)
                        {
                            case WordXmlSerializer.WordParagraphTag:
                                if (_inParagraph)
                                {
                                    throw CreateUnexpectedNodeException(_reader);
                                }
                                SurroundWithElement(new Paragraph(new Run()));
                                ApplyStyleToParagraph();
                                _inParagraph = true;
                                if (_reader.IsEmptyElement)
                                {
                                    MoveOutOfElement(typeof(Paragraph));
                                    _inParagraph = false;
                                }
                                break;
                            case WordXmlSerializer.WordRangeTag:
                                SurroundWithElement(new Run());
                                _inRange = true;
                                break;
                            case WordXmlSerializer.WordNameTag:
                                if (!IsPopulatingStyle)
                                {
                                    throw new ArgumentException("w:name only supported on styles.");
                                }
                                break;
                            case WordXmlSerializer.WordStyleTag:
                                _currentStyle = new Style();
                                SetupCurrentStyle();
                                break;
                            case WordXmlSerializer.WordTextTag:
                                _inText = true;
                                break;
                            case WordXmlSerializer.WordBreakTag:
                                if (_inRange)
                                {
                                    MoveOutOfElement(typeof(Run));
                                    new LineBreak(_cursor);
                                    SurroundWithElement(new Run());
                                }
                                else
                                {
                                    new LineBreak(_cursor);
                                }
                                break;
                            default:
                                SetSimpleProperty();
                                break;
                        }
                        break;
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        if (_inText)
                        {
                            _cursor.InsertTextInRun(_reader.Value);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Best-effort to set a property value based on the current
        /// reader element.
        /// </summary>
        private void SetSimpleProperty()
        {
            string elementName;             // Name of element inspected.
            object propertyValue;           // Value to set on property.
            DependencyProperty property;    // Property to set.
            bool valueFound;                // Whether a valid value was found.

            valueFound = false;
            propertyValue = null;
            property = null;

            elementName = _reader.Name;
            foreach (SimplePropertyMap map in SimplePropertyMap.SimplePropertyMaps)
            {
                if (map.XmlName != elementName)
                    continue;

                // Store the default value to use after pass unless
                // something more specific comes along.
                if (map.IsDefaultValue)
                {
                    property = map.Property;
                    propertyValue = map.GetValue(_reader);
                    valueFound = true;
                }
                else if (_reader.GetAttribute(map.XmlAttribute) == map.XmlValue)
                {
                    property = map.Property;
                    propertyValue = map.GetValue(_reader);
                    valueFound = true;
                    break;
                }
            }

            if (valueFound && propertyValue != null)
            {
                SetValue(property, propertyValue);
            }
        }

        /// <summary>
        /// Sets the properties from the current element to the style
        /// being built, and adds the style to the Resources dictionary.
        /// </summary>
        private void SetupCurrentStyle()
        {
            string isDefault;   // Whether the style is the default for the document.
            string styleId;     // Style ID for the element.
            string styleType;   // Type of style (paragraph or text span).

            System.Diagnostics.Debug.Assert(IsPopulatingStyle);
            System.Diagnostics.Debug.Assert(_reader.Name == WordXmlSerializer.WordStyleTag);

            styleType = _reader.GetAttribute(WordXmlSerializer.WordType);
            if (styleType == WordXmlSerializer.WordParagraph)
            {
                // TODO: Maybe this needs a special flag.
                _currentStyle.TargetType = typeof(Paragraph);
            }
            else if (styleType == WordXmlSerializer.WordCharacter)
            {
                _currentStyle.TargetType = typeof(Run);
            }
            else if (styleType == WordXmlSerializer.WordTable)
            {
                _currentStyle.TargetType = typeof(Table);
            }
            else if (styleType == WordXmlSerializer.WordList)
            {
                _currentStyle.TargetType = typeof(ListItem);
            }

            styleId = _reader.GetAttribute(WordXmlSerializer.WordStyleId);
            GetResources(_cursor).Add(styleId, _currentStyle);

            // For defaults,
            isDefault = _reader.GetAttribute(WordXmlSerializer.WordDefault);
            if (isDefault == WordXmlSerializer.WordOn)
            {
                Style defaultStyle;
                defaultStyle = new Style();
                defaultStyle.BasedOn = _currentStyle;
                defaultStyle.TargetType = _currentStyle.TargetType;
                GetResources(_cursor).Add(defaultStyle.TargetType, defaultStyle);
            }
        }

        #endregion Private methods.

        //------------------------------------------------------
        //
        //  Private Properties
        //
        //------------------------------------------------------

        #region Private properties.

        /// <summary>
        /// Whether the reader is reading a style rather than content.
        /// </summary>
        private bool IsPopulatingStyle => _currentStyle != null;

        /// <summary>Resources used to hold styles.</summary>
        private ResourceDictionary GetResources(TextPointer position)
        {
            if (_resources == null)
            {
                _resources = new ResourceDictionary();
            }
            return _resources;
        }

        private void ToMakeCompilerHappy()
        {
            if (_inParagraph || _inRange)
            {
                return;
            }
        }

        #endregion Private properties.

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private fields.

        /// <summary>Cursor writing document.</summary>
        private TextPointer _cursor;

        /// <summary>Direction in which the cursor moves.</summary>
        /// <remarks>
        /// For the current implementation, the direction is always
        /// forward. This may change for more complex implementations.
        /// Until then, this acts as a constant (but constants cannot
        /// refer to enumeration valueS).
        /// </remarks>
        private LogicalDirection _dir;

        /// <summary>Whether writer has opened a paragraph.</summary>
        private bool _inParagraph;

        /// <summary>Whether writer has opened a range.</summary>
        private bool _inRange;

        /// <summary>Whether writer is ready to write text.</summary>
        private bool _inText;

        /// <summary>Reader used to read document.</summary>
        private XmlReader _reader;

        /// <summary>Direction opposite to that in which the cursor moves.</summary>
        private LogicalDirection _otherDir;

        /// <summary>Current style being defined.</summary>
        private Style _currentStyle;

        /// <summary>Resource dictionary used to work with styles.</summary>
        private ResourceDictionary _resources;

        #endregion Private fields.

        //------------------------------------------------------
        //
        //  Inner Types
        //
        //------------------------------------------------------

        #region Inner Types.

        /// <summary>
        /// Maps a WordXML element to a simple property set operation.
        /// </summary>
        struct SimplePropertyMap
        {
            #region Inner Types.

            /// <summary>
            /// Callback to evaluate the state in the reader
            /// to a DependencyProperty value.
            /// </summary>
            private delegate object CalculateValueCallback(XmlReader reader);

            #endregion Inner Types.

            #region Map data.

            /// <summary>Constant value for the mapping.</summary>
            private object Value;

            /// <summary>Callback to evaluate the value for the mapping.</summary>
            private CalculateValueCallback CalculateValue;

            /// <summary>Whether the XML attribute value should be used.</summary>
            private bool UseAttributeValue;

            /// <summary>Property to be set.</summary>
            internal DependencyProperty Property;

            /// <summary>XML attribute to evaluate.</summary>
            internal string XmlAttribute;

            /// <summary>XML name to evaluate.</summary>
            internal string XmlName;

            /// <summary>XML name to evaluate to.</summary>
            internal string XmlValue;

            /// <summary>
            /// Whether the specified mapping returns the default value for
            /// the current reader state.
            /// </summary>
            internal bool IsDefaultValue => XmlValue == null;

            #endregion Map data.

            #region Internal Methods.

            /// <summary>
            /// Returns the value to be set, given the context of the specified
            /// XmlReader in a WordXML document.
            /// </summary>
            internal object GetValue(XmlReader reader)
            {
                System.Diagnostics.Debug.Assert(reader != null);

                if (CalculateValue != null)
                {
                    return CalculateValue(reader);
                }
                else if (UseAttributeValue)
                {
                    return reader.GetAttribute(XmlAttribute);
                }
                else if (Value != null)
                {
                    return Value;
                }
                else
                    return null;
            }

            #endregion Internal Methods.

            #region Private Methods.

            /// <summary>
            /// Maps the current WordXML element in the specified XmlReader
            /// to an appropriate Brush value.
            /// </summary>
            private static object CalculateColorValue(XmlReader reader)
            {
                string xmlValue;            // XML value for the color.
                Color color;                // Color for resulting brush.
                int red, green, blue;       // Color components.

                System.Diagnostics.Debug.Assert(reader != null);

                xmlValue = reader.GetAttribute(WordXmlSerializer.WordValue);
                if (xmlValue == null)
                {
                    // Unknown color. Malformed WordXML here.
                    return null;
                }
                if (xmlValue == WordXmlSerializer.WordAuto)
                {
                    // maybe UnSet?
                    return null;
                }

                red = Int32.Parse(xmlValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                green = Int32.Parse(xmlValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                blue = Int32.Parse(xmlValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color = Color.FromRgb((byte)red, (byte)green, (byte)blue);
                return new SolidColorBrush(color);
            }

            /// <summary>
            /// Maps the current WordXML element in the specified XmlReader
            /// to an appropriate FontSize value.
            /// </summary>
            private static object CalculateFontSizeValue(XmlReader reader)
            {
                string xmlValue;        // String value of attribute.
                int sizeInHalfPoints;   // Size of font, in half-points.
                double sizeInPixels;    // Size of font, in device-independent pixels.

                xmlValue = reader.GetAttribute(WordXmlSerializer.WordValue);
                if (xmlValue == null)
                {
                    // Unknown font size. Malformed WordXML here.
                    return null;
                }
                if (xmlValue == WordXmlSerializer.WordAuto)
                {
                    // maybe UnSet?
                    return null;
                }

                sizeInHalfPoints = Int32.Parse(xmlValue, CultureInfo.InvariantCulture);
                sizeInPixels = ((double)sizeInHalfPoints / 2) * 96.0 / 72.0;
                return sizeInPixels;
            }

            /// <summary>
            /// Maps the current WordXML element in the specified XmlReader
            /// to an appropriate FontFamily value.
            /// </summary>
            private static object CalculateFontFamilyValue(XmlReader reader)
            {
                string xmlValue;        // String value of attribute.

                xmlValue = reader.GetAttribute(WordXmlSerializer.WordAuxiliaryValue);
                if (xmlValue == null)
                {
                    // Unknown font size. Malformed WordXML here.
                    return null;
                }
                if (xmlValue == WordXmlSerializer.WordAuto)
                {
                    // maybe UnSet?
                    return null;
                }

                return new FontFamily(xmlValue);
            }

            /// <summary>
            /// Initializes a new SimplePropertyMap instance mapping for the default
            /// value in an XML name/value pair.
            /// </summary>
            private static SimplePropertyMap FromAttribute(DependencyProperty property,
                object value, string xmlName, string xmlAttribute) => FromAttribute(property, value, xmlName, xmlAttribute, null);

            /// <summary>
            /// Initializes a new SimplePropertyMap instance mapping for the given
            /// value in an XML name/value pair.
            /// </summary>
            private static SimplePropertyMap FromAttribute(DependencyProperty property,
                object value, string xmlName, string xmlAttribute, string xmlValue)
            {
                SimplePropertyMap result = new SimplePropertyMap();

                result.Property = property;
                result.Value = value;
                result.XmlAttribute = xmlAttribute;
                result.XmlName = xmlName;
                result.XmlValue = xmlValue;

                return result;
            }

            /// <summary>
            /// Initializes a new SimplePropertyMap instance mapping for the given
            /// value in an XML name/value pair, such that the value applied is
            /// the vaule of the XML attribute.
            /// </summary>
            private static SimplePropertyMap FromAttributeValue(DependencyProperty property,
                string xmlName, string xmlAttribute)
            {
                SimplePropertyMap result = new SimplePropertyMap();

                result.Property = property;
                result.UseAttributeValue = true;
                result.XmlAttribute = xmlAttribute;
                result.XmlName = xmlName;

                return result;
            }

            /// <summary>
            /// Initializes a new SimplePropertyMap instance mapping for a calculated
            /// value.
            /// </summary>
            private static SimplePropertyMap FromDelegate(DependencyProperty property,
                CalculateValueCallback callback, string xmlName)
            {
                SimplePropertyMap result = new SimplePropertyMap();

                result.Property = property;
                result.CalculateValue = callback;
                result.XmlName = xmlName;

                return result;
            }

            #endregion Private Methods.

            #region Private Fields.

            /// <summary>Provides a map of name simple property mappings.</summary>
            internal static SimplePropertyMap[] SimplePropertyMaps = new SimplePropertyMap[] {
                // w:b maps to FontWeight values.
                FromAttribute(
                    TextBlock.FontWeightProperty, FontWeights.Normal,
                    WordXmlSerializer.WordBoldTag, WordXmlSerializer.WordValue, WordXmlSerializer.WordOff),
                FromAttribute(
                    TextBlock.FontWeightProperty, FontWeights.Bold,
                    WordXmlSerializer.WordBoldTag, WordXmlSerializer.WordValue),

                // w:c maps to Foreground values.
                FromDelegate(TextBlock.ForegroundProperty, CalculateColorValue, WordXmlSerializer.WordColorTag),

                // wx:font maps to FontFamily values.
                FromDelegate(
                    TextBlock.FontFamilyProperty, CalculateFontFamilyValue,
                    WordXmlSerializer.WordAuxiliaryFontTag),

                // w:i maps to FontStyle values.
                FromAttribute(
                    TextBlock.FontStyleProperty, FontStyles.Normal,
                    WordXmlSerializer.WordItalicTag, WordXmlSerializer.WordValue, WordXmlSerializer.WordOff),
                FromAttribute(
                    TextBlock.FontStyleProperty, FontStyles.Italic,
                    WordXmlSerializer.WordItalicTag, WordXmlSerializer.WordValue),

                // w:jc maps to AlignmentX values.
                FromAttribute(
                    TextBlock.TextAlignmentProperty, TextAlignment.Center,
                    WordXmlSerializer.WordJustificationTag, WordXmlSerializer.WordValue, WordXmlSerializer.WordCenter),
                FromAttribute(
                    TextBlock.TextAlignmentProperty, TextAlignment.Right,
                    WordXmlSerializer.WordJustificationTag, WordXmlSerializer.WordValue, WordXmlSerializer.WordRight),
                FromAttribute(
                    TextBlock.TextAlignmentProperty, TextAlignment.Left,
                    WordXmlSerializer.WordJustificationTag, WordXmlSerializer.WordValue),

                // w:sz maps to FontSize values.
                FromDelegate(
                    TextBlock.FontSizeProperty, CalculateFontSizeValue,
                    WordXmlSerializer.WordFontSizeTag),
            };

            #endregion Private Fields.
        }

        #endregion Inner Types.
    }
}
