// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Xml;

namespace HtmlToXamlDemo
{
    // DependencyProperty

    // TextElement

    /// <summary>
    ///     HtmlToXamlConverter is a static class that takes an HTML string
    ///     and converts it into XAML
    /// </summary>
    public static class HtmlToXamlConverter
    {
        // ----------------------------------------------------------------
        //
        // Internal Constants
        //
        // ----------------------------------------------------------------

        // The constants reprtesent all Xaml names used in a conversion
        public const string XamlFlowDocument = "FlowDocument";
        public const string XamlRun = "Run";
        public const string XamlSpan = "Span";
        public const string XamlHyperlink = "Hyperlink";
        public const string XamlHyperlinkNavigateUri = "NavigateUri";
        public const string XamlHyperlinkTargetName = "TargetName";
        public const string XamlSection = "Section";
        public const string XamlList = "List";
        public const string XamlListMarkerStyle = "MarkerStyle";
        public const string XamlListMarkerStyleNone = "None";
        public const string XamlListMarkerStyleDecimal = "Decimal";
        public const string XamlListMarkerStyleDisc = "Disc";
        public const string XamlListMarkerStyleCircle = "Circle";
        public const string XamlListMarkerStyleSquare = "Square";
        public const string XamlListMarkerStyleBox = "Box";
        public const string XamlListMarkerStyleLowerLatin = "LowerLatin";
        public const string XamlListMarkerStyleUpperLatin = "UpperLatin";
        public const string XamlListMarkerStyleLowerRoman = "LowerRoman";
        public const string XamlListMarkerStyleUpperRoman = "UpperRoman";
        public const string XamlListItem = "ListItem";
        public const string XamlLineBreak = "LineBreak";
        public const string XamlParagraph = "Paragraph";
        public const string XamlMargin = "Margin";
        public const string XamlPadding = "Padding";
        public const string XamlBorderBrush = "BorderBrush";
        public const string XamlBorderThickness = "BorderThickness";
        public const string XamlTable = "Table";
        // flowdocument table requires this element, take Table prefix because XMLReader cannot resolve the namespace of this element 
        public const string XamlTableColumnGroup = "Table.Columns";
        public const string XamlTableColumn = "TableColumn";
        public const string XamlTableRowGroup = "TableRowGroup";
        public const string XamlTableRow = "TableRow";
        public const string XamlTableCell = "TableCell";
        public const string XamlTableCellBorderThickness = "BorderThickness";
        public const string XamlTableCellBorderBrush = "BorderBrush";
        public const string XamlTableCellColumnSpan = "ColumnSpan";
        public const string XamlTableCellRowSpan = "RowSpan";
        public const string XamlWidth = "Width";
        public const string XamlBrushesBlack = "Black";
        public const string XamlFontFamily = "FontFamily";
        public const string XamlFontSize = "FontSize";
        public const string XamlFontSizeXxLarge = "22pt"; // "XXLarge";
        public const string XamlFontSizeXLarge = "20pt"; // "XLarge";
        public const string XamlFontSizeLarge = "18pt"; // "Large";
        public const string XamlFontSizeMedium = "16pt"; // "Medium";
        public const string XamlFontSizeSmall = "12pt"; // "Small";
        public const string XamlFontSizeXSmall = "10pt"; // "XSmall";
        public const string XamlFontSizeXxSmall = "8pt"; // "XXSmall";
        public const string XamlFontWeight = "FontWeight";
        public const string XamlFontWeightBold = "Bold";
        public const string XamlFontStyle = "FontStyle";
        public const string XamlForeground = "Foreground";
        public const string XamlBackground = "Background";
        public const string XamlTextDecorations = "TextDecorations";
        public const string XamlTextDecorationsUnderline = "Underline";
        public const string XamlTextIndent = "TextIndent";
        public const string XamlTextAlignment = "TextAlignment";
        // ---------------------------------------------------------------------
        //
        // Private Fields
        //
        // ---------------------------------------------------------------------

        #region Private Fields

        private static readonly string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        #endregion Private Fields

        // ---------------------------------------------------------------------
        //
        // Internal Methods
        //
        // ---------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        ///     Converts an html string into xaml string.
        /// </summary>
        /// <param name="htmlString">
        ///     Input html which may be badly formated xml.
        /// </param>
        /// <param name="asFlowDocument">
        ///     true indicates that we need a FlowDocument as a root element;
        ///     false means that Section or Span elements will be used
        ///     dependeing on StartFragment/EndFragment comments locations.
        /// </param>
        /// <returns>
        ///     Well-formed xml representing XAML equivalent for the input html string.
        /// </returns>
        public static string ConvertHtmlToXaml(string htmlString, bool asFlowDocument)
        {
            // Create well-formed Xml from Html string
            var htmlElement = HtmlParser.ParseHtml(htmlString);

            // Decide what name to use as a root
            var rootElementName = asFlowDocument ? XamlFlowDocument : XamlSection;

            // Create an XmlDocument for generated xaml
            var xamlTree = new XmlDocument();
            var xamlFlowDocumentElement = xamlTree.CreateElement(null, rootElementName, XamlNamespace);

            // Extract style definitions from all STYLE elements in the document
            var stylesheet = new CssStylesheet(htmlElement);

            // Source context is a stack of all elements - ancestors of a parentElement
            var sourceContext = new List<XmlElement>(10);

            // Clear fragment parent
            _inlineFragmentParentElement = null;

            // convert root html element
            AddBlock(xamlFlowDocumentElement, htmlElement, new Hashtable(), stylesheet, sourceContext);

            // In case if the selected fragment is inline, extract it into a separate Span wrapper
            if (!asFlowDocument)
            {
                xamlFlowDocumentElement = ExtractInlineFragment(xamlFlowDocumentElement);
            }

            // Return a string representing resulting Xaml
            xamlFlowDocumentElement.SetAttribute("xml:space", "preserve");
            var xaml = xamlFlowDocumentElement.OuterXml;

            return xaml;
        }

        /// <summary>
        ///     Returns a value for an attribute by its name (ignoring casing)
        /// </summary>
        /// <param name="element">
        ///     XmlElement in which we are trying to find the specified attribute
        /// </param>
        /// <param name="attributeName">
        ///     String representing the attribute name to be searched for
        /// </param>
        /// <returns></returns>
        public static string GetAttribute(XmlElement element, string attributeName)
        {
            attributeName = attributeName.ToLower();

            for (var i = 0; i < element.Attributes.Count; i++)
            {
                if (element.Attributes[i].Name.ToLower() == attributeName)
                {
                    return element.Attributes[i].Value;
                }
            }

            return null;
        }

        /// <summary>
        ///     Returns string extracted from quotation marks
        /// </summary>
        /// <param name="value">
        ///     String representing value enclosed in quotation marks
        /// </param>
        internal static string UnQuote(string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\"") || value.StartsWith("'") && value.EndsWith("'"))
            {
                value = value.Substring(1, value.Length - 2).Trim();
            }
            return value;
        }

        #endregion Internal Methods

        // ---------------------------------------------------------------------
        //
        // Private Methods
        //
        // ---------------------------------------------------------------------

        #region Private Methods

        /// <summary>
        ///     Analyzes the given htmlElement expecting it to be converted
        ///     into some of xaml Block elements and adds the converted block
        ///     to the children collection of xamlParentElement.
        ///     Analyzes the given XmlElement htmlElement, recognizes it as some HTML element
        ///     and adds it as a child to a xamlParentElement.
        ///     In some cases several following siblings of the given htmlElement
        ///     will be consumed too (e.g. LIs encountered without wrapping UL/OL,
        ///     which must be collected together and wrapped into one implicit List element).
        /// </summary>
        /// <param name="xamlParentElement">
        ///     Parent xaml element, to which new converted element will be added
        /// </param>
        /// <param name="htmlElement">
        ///     Source html element subject to convert to xaml.
        /// </param>
        /// <param name="inheritedProperties">
        ///     Properties inherited from an outer context.
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        /// <returns>
        ///     Last processed html node. Normally it should be the same htmlElement
        ///     as was passed as a paramater, but in some irregular cases
        ///     it could one of its following siblings.
        ///     The caller must use this node to get to next sibling from it.
        /// </returns>
        private static XmlNode AddBlock(XmlElement xamlParentElement, XmlNode htmlNode, Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            if (htmlNode is XmlComment)
            {
                DefineInlineFragmentParent((XmlComment) htmlNode, /*xamlParentElement:*/null);
            }
            else if (htmlNode is XmlText)
            {
                htmlNode = AddImplicitParagraph(xamlParentElement, htmlNode, inheritedProperties, stylesheet,
                    sourceContext);
            }
            else if (htmlNode is XmlElement)
            {
                // Identify element name
                var htmlElement = (XmlElement) htmlNode;

                var htmlElementName = htmlElement.LocalName; // Keep the name case-sensitive to check xml names
                var htmlElementNamespace = htmlElement.NamespaceURI;

                if (htmlElementNamespace != HtmlParser.XhtmlNamespace)
                {
                    // Non-html element. skip it
                    // Isn't it too agressive? What if this is just an error in html tag name?
                    // TODO: Consider skipping just a wparrer in recursing into the element tree,
                    // which may produce some garbage though coming from xml fragments.
                    return htmlElement;
                }

                // Put source element to the stack
                sourceContext.Add(htmlElement);

                // Convert the name to lowercase, because html elements are case-insensitive
                htmlElementName = htmlElementName.ToLower();

                // Switch to an appropriate kind of processing depending on html element name
                switch (htmlElementName)
                {
                    // Sections:
                    case "html":
                    case "body":
                    case "div":
                    case "form": // not a block according to xhtml spec
                    case "pre": // Renders text in a fixed-width font
                    case "blockquote":
                    case "caption":
                    case "center":
                    case "cite":
                        AddSection(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;

                    // Paragraphs:
                    case "p":
                    case "h1":
                    case "h2":
                    case "h3":
                    case "h4":
                    case "h5":
                    case "h6":
                    case "nsrtitle":
                    case "textarea":
                    case "dd": // ???
                    case "dl": // ???
                    case "dt": // ???
                    case "tt": // ???
                        AddParagraph(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;

                    case "ol":
                    case "ul":
                    case "dir": //  treat as UL element
                    case "menu": //  treat as UL element
                        // List element conversion
                        AddList(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;
                    case "li":
                        // LI outside of OL/UL
                        // Collect all sibling LIs, wrap them into a List and then proceed with the element following the last of LIs
                        htmlNode = AddOrphanListItems(xamlParentElement, htmlElement, inheritedProperties, stylesheet,
                            sourceContext);
                        break;

                    case "img":
                        // TODO: Add image processing
                        AddImage(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;

                    case "table":
                        // hand off to table parsing function which will perform special table syntax checks
                        AddTable(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;

                    case "tbody":
                    case "tfoot":
                    case "thead":
                    case "tr":
                    case "td":
                    case "th":
                        // Table stuff without table wrapper
                        // TODO: add special-case processing here for elements that should be within tables when the
                        // parent element is NOT a table. If the parent element is a table they can be processed normally.
                        // we need to compare against the parent element here, we can't just break on a switch
                        goto default; // Thus we will skip this element as unknown, but still recurse into it.

                    case "style": // We already pre-processed all style elements. Ignore it now
                    case "meta":
                    case "head":
                    case "title":
                    case "script":
                        // Ignore these elements
                        break;

                    default:
                        // Wrap a sequence of inlines into an implicit paragraph
                        htmlNode = AddImplicitParagraph(xamlParentElement, htmlElement, inheritedProperties, stylesheet,
                            sourceContext);
                        break;
                }

                // Remove the element from the stack
                Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == htmlElement);
                sourceContext.RemoveAt(sourceContext.Count - 1);
            }

            // Return last processed node
            return htmlNode;
        }

        // .............................................................
        //
        // Line Breaks
        //
        // .............................................................

        private static void AddBreak(XmlElement xamlParentElement, string htmlElementName)
        {
            // Create new xaml element corresponding to this html element
            var xamlLineBreak = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                null, /*localName:*/XamlLineBreak, XamlNamespace);
            xamlParentElement.AppendChild(xamlLineBreak);
            if (htmlElementName == "hr")
            {
                var xamlHorizontalLine = xamlParentElement.OwnerDocument.CreateTextNode("----------------------");
                xamlParentElement.AppendChild(xamlHorizontalLine);
                xamlLineBreak = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                    null, /*localName:*/XamlLineBreak, XamlNamespace);
                xamlParentElement.AppendChild(xamlLineBreak);
            }
        }

        // .............................................................
        //
        // Text Flow Elements
        //
        // .............................................................

        /// <summary>
        ///     Generates Section or Paragraph element from DIV depending whether it contains any block elements or not
        /// </summary>
        /// <param name="xamlParentElement">
        ///     XmlElement representing Xaml parent to which the converted element should be added
        /// </param>
        /// <param name="htmlElement">
        ///     XmlElement representing Html element to be converted
        /// </param>
        /// <param name="inheritedProperties">
        ///     properties inherited from parent context
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        /// true indicates that a content added by this call contains at least one block element
        /// </param>
        private static void AddSection(XmlElement xamlParentElement, XmlElement htmlElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Analyze the content of htmlElement to decide what xaml element to choose - Section or Paragraph.
            // If this Div has at least one block child then we need to use Section, otherwise use Paragraph
            var htmlElementContainsBlocks = false;
            for (var htmlChildNode = htmlElement.FirstChild;
                htmlChildNode != null;
                htmlChildNode = htmlChildNode.NextSibling)
            {
                if (htmlChildNode is XmlElement)
                {
                    var htmlChildName = ((XmlElement) htmlChildNode).LocalName.ToLower();
                    if (HtmlSchema.IsBlockElement(htmlChildName))
                    {
                        htmlElementContainsBlocks = true;
                        break;
                    }
                }
            }

            if (!htmlElementContainsBlocks)
            {
                // The Div does not contain any block elements, so we can treat it as a Paragraph
                AddParagraph(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
            }
            else
            {
                // The Div has some nested blocks, so we treat it as a Section

                // Create currentProperties as a compilation of local and inheritedProperties, set localProperties
                Hashtable localProperties;
                var currentProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties,
                    stylesheet,
                    sourceContext);

                // Create a XAML element corresponding to this html element
                var xamlElement = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                    null, /*localName:*/XamlSection, XamlNamespace);
                ApplyLocalProperties(xamlElement, localProperties, /*isBlock:*/true);

                // Decide whether we can unwrap this element as not having any formatting significance.
                if (!xamlElement.HasAttributes)
                {
                    // This elements is a group of block elements whitout any additional formatting.
                    // We can add blocks directly to xamlParentElement and avoid
                    // creating unnecessary Sections nesting.
                    xamlElement = xamlParentElement;
                }

                // Recurse into element subtree
                for (var htmlChildNode = htmlElement.FirstChild;
                    htmlChildNode != null;
                    htmlChildNode = htmlChildNode?.NextSibling)
                {
                    htmlChildNode = AddBlock(xamlElement, htmlChildNode, currentProperties, stylesheet, sourceContext);
                }

                // Add the new element to the parent.
                if (xamlElement != xamlParentElement)
                {
                    xamlParentElement.AppendChild(xamlElement);
                }
            }
        }

        /// <summary>
        ///     Generates Paragraph element from P, H1-H7, Center etc.
        /// </summary>
        /// <param name="xamlParentElement">
        ///     XmlElement representing Xaml parent to which the converted element should be added
        /// </param>
        /// <param name="htmlElement">
        ///     XmlElement representing Html element to be converted
        /// </param>
        /// <param name="inheritedProperties">
        ///     properties inherited from parent context
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        /// true indicates that a content added by this call contains at least one block element
        /// </param>
        private static void AddParagraph(XmlElement xamlParentElement, XmlElement htmlElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Create currentProperties as a compilation of local and inheritedProperties, set localProperties
            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties,
                stylesheet,
                sourceContext);

            // Create a XAML element corresponding to this html element
            var xamlElement = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                null, /*localName:*/XamlParagraph, XamlNamespace);
            ApplyLocalProperties(xamlElement, localProperties, /*isBlock:*/true);

            // Recurse into element subtree
            for (var htmlChildNode = htmlElement.FirstChild;
                htmlChildNode != null;
                htmlChildNode = htmlChildNode.NextSibling)
            {
                AddInline(xamlElement, htmlChildNode, currentProperties, stylesheet, sourceContext);
            }

            // Add the new element to the parent.
            xamlParentElement.AppendChild(xamlElement);
        }

        /// <summary>
        ///     Creates a Paragraph element and adds all nodes starting from htmlNode
        ///     converted to appropriate Inlines.
        /// </summary>
        /// <param name="xamlParentElement">
        ///     XmlElement representing Xaml parent to which the converted element should be added
        /// </param>
        /// <param name="htmlNode">
        ///     XmlNode starting a collection of implicitly wrapped inlines.
        /// </param>
        /// <param name="inheritedProperties">
        ///     properties inherited from parent context
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        /// true indicates that a content added by this call contains at least one block element
        /// </param>
        /// <returns>
        ///     The last htmlNode added to the implicit paragraph
        /// </returns>
        private static XmlNode AddImplicitParagraph(XmlElement xamlParentElement, XmlNode htmlNode,
            Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Collect all non-block elements and wrap them into implicit Paragraph
            var xamlParagraph = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                null, /*localName:*/XamlParagraph, XamlNamespace);
            XmlNode lastNodeProcessed = null;
            while (htmlNode != null)
            {
                if (htmlNode is XmlComment)
                {
                    DefineInlineFragmentParent((XmlComment) htmlNode, /*xamlParentElement:*/null);
                }
                else if (htmlNode is XmlText)
                {
                    if (htmlNode.Value.Trim().Length > 0)
                    {
                        AddTextRun(xamlParagraph, htmlNode.Value);
                    }
                }
                else if (htmlNode is XmlElement)
                {
                    var htmlChildName = ((XmlElement) htmlNode).LocalName.ToLower();
                    if (HtmlSchema.IsBlockElement(htmlChildName))
                    {
                        // The sequence of non-blocked inlines ended. Stop implicit loop here.
                        break;
                    }
                    AddInline(xamlParagraph, (XmlElement) htmlNode, inheritedProperties, stylesheet, sourceContext);
                }

                // Store last processed node to return it at the end
                lastNodeProcessed = htmlNode;
                htmlNode = htmlNode.NextSibling;
            }

            // Add the Paragraph to the parent
            // If only whitespaces and commens have been encountered,
            // then we have nothing to add in implicit paragraph; forget it.
            if (xamlParagraph.FirstChild != null)
            {
                xamlParentElement.AppendChild(xamlParagraph);
            }

            // Need to return last processed node
            return lastNodeProcessed;
        }

        // .............................................................
        //
        // Inline Elements
        //
        // .............................................................

        private static void AddInline(XmlElement xamlParentElement, XmlNode htmlNode, Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            if (htmlNode is XmlComment)
            {
                DefineInlineFragmentParent((XmlComment) htmlNode, xamlParentElement);
            }
            else if (htmlNode is XmlText)
            {
                AddTextRun(xamlParentElement, htmlNode.Value);
            }
            else if (htmlNode is XmlElement)
            {
                var htmlElement = (XmlElement) htmlNode;

                // Check whether this is an html element
                if (htmlElement.NamespaceURI != HtmlParser.XhtmlNamespace)
                {
                    return; // Skip non-html elements
                }

                // Identify element name
                var htmlElementName = htmlElement.LocalName.ToLower();

                // Put source element to the stack
                sourceContext.Add(htmlElement);

                switch (htmlElementName)
                {
                    case "a":
                        AddHyperlink(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;
                    case "img":
                        AddImage(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        break;
                    case "br":
                    case "hr":
                        AddBreak(xamlParentElement, htmlElementName);
                        break;
                    default:
                        if (HtmlSchema.IsInlineElement(htmlElementName) || HtmlSchema.IsBlockElement(htmlElementName))
                        {
                            // Note: actually we do not expect block elements here,
                            // but if it happens to be here, we will treat it as a Span.

                            AddSpanOrRun(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
                        }
                        break;
                }
                // Ignore all other elements non-(block/inline/image)

                // Remove the element from the stack
                Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == htmlElement);
                sourceContext.RemoveAt(sourceContext.Count - 1);
            }
        }

        private static void AddSpanOrRun(XmlElement xamlParentElement, XmlElement htmlElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Decide what XAML element to use for this inline element.
            // Check whether it contains any nested inlines
            var elementHasChildren = false;
            for (var htmlNode = htmlElement.FirstChild; htmlNode != null; htmlNode = htmlNode.NextSibling)
            {
                if (htmlNode is XmlElement)
                {
                    var htmlChildName = ((XmlElement) htmlNode).LocalName.ToLower();
                    if (HtmlSchema.IsInlineElement(htmlChildName) || HtmlSchema.IsBlockElement(htmlChildName) ||
                        htmlChildName == "img" || htmlChildName == "br" || htmlChildName == "hr")
                    {
                        elementHasChildren = true;
                        break;
                    }
                }
            }

            var xamlElementName = elementHasChildren ? XamlSpan : XamlRun;

            // Create currentProperties as a compilation of local and inheritedProperties, set localProperties
            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties,
                stylesheet,
                sourceContext);

            // Create a XAML element corresponding to this html element
            var xamlElement = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                null, /*localName:*/xamlElementName, XamlNamespace);
            ApplyLocalProperties(xamlElement, localProperties, /*isBlock:*/false);

            // Recurse into element subtree
            for (var htmlChildNode = htmlElement.FirstChild;
                htmlChildNode != null;
                htmlChildNode = htmlChildNode.NextSibling)
            {
                AddInline(xamlElement, htmlChildNode, currentProperties, stylesheet, sourceContext);
            }

            // Add the new element to the parent.
            xamlParentElement.AppendChild(xamlElement);
        }

        // Adds a text run to a xaml tree
        private static void AddTextRun(XmlElement xamlElement, string textData)
        {
            // Remove control characters
            for (var i = 0; i < textData.Length; i++)
            {
                if (char.IsControl(textData[i]))
                {
                    textData = textData.Remove(i--, 1); // decrement i to compensate for character removal
                }
            }

            // Replace No-Breaks by spaces (160 is a code of &nbsp; entity in html)
            //  This is a work around since WPF/XAML does not support &nbsp.
            textData = textData.Replace((char) 160, ' ');

            if (textData.Length > 0)
            {
                xamlElement.AppendChild(xamlElement.OwnerDocument.CreateTextNode(textData));
            }
        }

        private static void AddHyperlink(XmlElement xamlParentElement, XmlElement htmlElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Convert href attribute into NavigateUri and TargetName
            var href = GetAttribute(htmlElement, "href");
            if (href == null)
            {
                // When href attribute is missing - ignore the hyperlink
                AddSpanOrRun(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
            }
            else
            {
                // Create currentProperties as a compilation of local and inheritedProperties, set localProperties
                Hashtable localProperties;
                var currentProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties,
                    stylesheet,
                    sourceContext);

                // Create a XAML element corresponding to this html element
                var xamlElement = xamlParentElement.OwnerDocument.CreateElement( /*prefix:*/
                    null, /*localName:*/XamlHyperlink, XamlNamespace);
                ApplyLocalProperties(xamlElement, localProperties, /*isBlock:*/false);

                var hrefParts = href.Split('#');
                if (hrefParts.Length > 0 && hrefParts[0].Trim().Length > 0)
                {
                    xamlElement.SetAttribute(XamlHyperlinkNavigateUri, hrefParts[0].Trim());
                }
                if (hrefParts.Length == 2 && hrefParts[1].Trim().Length > 0)
                {
                    xamlElement.SetAttribute(XamlHyperlinkTargetName, hrefParts[1].Trim());
                }

                // Recurse into element subtree
                for (var htmlChildNode = htmlElement.FirstChild;
                    htmlChildNode != null;
                    htmlChildNode = htmlChildNode.NextSibling)
                {
                    AddInline(xamlElement, htmlChildNode, currentProperties, stylesheet, sourceContext);
                }

                // Add the new element to the parent.
                xamlParentElement.AppendChild(xamlElement);
            }
        }

        // Stores a parent xaml element for the case when selected fragment is inline.
        private static XmlElement _inlineFragmentParentElement;

        // Called when html comment is encountered to store a parent element
        // for the case when the fragment is inline - to extract it to a separate
        // Span wrapper after the conversion.
        private static void DefineInlineFragmentParent(XmlComment htmlComment, XmlElement xamlParentElement)
        {
            if (htmlComment.Value == "StartFragment")
            {
                _inlineFragmentParentElement = xamlParentElement;
            }
            else if (htmlComment.Value == "EndFragment")
            {
                if (_inlineFragmentParentElement == null && xamlParentElement != null)
                {
                    // Normally this cannot happen if comments produced by correct copying code
                    // in Word or IE, but when it is produced manually then fragment boundary
                    // markers can be inconsistent. In this case StartFragment takes precedence,
                    // but if it is not set, then we get the value from EndFragment marker.
                    _inlineFragmentParentElement = xamlParentElement;
                }
            }
        }

        // Extracts a content of an element stored as InlineFragmentParentElement
        // into a separate Span wrapper.
        // Note: when selected content does not cross paragraph boundaries,
        // the fragment is marked within
        private static XmlElement ExtractInlineFragment(XmlElement xamlFlowDocumentElement)
        {
            if (_inlineFragmentParentElement != null)
            {
                if (_inlineFragmentParentElement.LocalName == XamlSpan)
                {
                    xamlFlowDocumentElement = _inlineFragmentParentElement;
                }
                else
                {
                    xamlFlowDocumentElement = xamlFlowDocumentElement.OwnerDocument.CreateElement( /*prefix:*/
                        null, /*localName:*/XamlSpan, XamlNamespace);
                    while (_inlineFragmentParentElement.FirstChild != null)
                    {
                        var copyNode = _inlineFragmentParentElement.FirstChild;
                        _inlineFragmentParentElement.RemoveChild(copyNode);
                        xamlFlowDocumentElement.AppendChild(copyNode);
                    }
                }
            }

            return xamlFlowDocumentElement;
        }

        // .............................................................
        //
        // Images
        //
        // .............................................................

        private static void AddImage(XmlElement xamlParentElement, XmlElement htmlElement, Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            //  Implement images
        }

        // .............................................................
        //
        // Lists
        //
        // .............................................................

        /// <summary>
        ///     Converts Html ul or ol element into Xaml list element. During conversion if the ul/ol element has any children
        ///     that are not li elements, they are ignored and not added to the list element
        /// </summary>
        /// <param name="xamlParentElement">
        ///     XmlElement representing Xaml parent to which the converted element should be added
        /// </param>
        /// <param name="htmlListElement">
        ///     XmlElement representing Html ul/ol element to be converted
        /// </param>
        /// <param name="inheritedProperties">
        ///     properties inherited from parent context
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        private static void AddList(XmlElement xamlParentElement, XmlElement htmlListElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            var htmlListElementName = htmlListElement.LocalName.ToLower();

            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlListElement, inheritedProperties, out localProperties,
                stylesheet, sourceContext);

            // Create Xaml List element
            var xamlListElement = xamlParentElement.OwnerDocument.CreateElement(null, XamlList, XamlNamespace);

            // Set default list markers
            xamlListElement.SetAttribute(XamlListMarkerStyle,
                htmlListElementName == "ol" ? XamlListMarkerStyleDecimal : XamlListMarkerStyleDisc);

            // Apply local properties to list to set marker attribute if specified
            // TODO: Should we have separate list attribute processing function?
            ApplyLocalProperties(xamlListElement, localProperties, /*isBlock:*/true);

            // Recurse into list subtree
            for (var htmlChildNode = htmlListElement.FirstChild;
                htmlChildNode != null;
                htmlChildNode = htmlChildNode.NextSibling)
            {
                if (htmlChildNode is XmlElement && htmlChildNode.LocalName.ToLower() == "li")
                {
                    sourceContext.Add((XmlElement) htmlChildNode);
                    AddListItem(xamlListElement, (XmlElement) htmlChildNode, currentProperties, stylesheet,
                        sourceContext);
                    Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == htmlChildNode);
                    sourceContext.RemoveAt(sourceContext.Count - 1);
                }
            }

            // Add the List element to xaml tree - if it is not empty
            if (xamlListElement.HasChildNodes)
            {
                xamlParentElement.AppendChild(xamlListElement);
            }
        }

        /// <summary>
        ///     If li items are found without a parent ul/ol element in Html string, creates xamlListElement as their parent and
        ///     adds
        ///     them to it. If the previously added node to the same xamlParentElement was a List, adds the elements to that list.
        ///     Otherwise, we create a new xamlListElement and add them to it. Elements are added as long as li elements appear
        ///     sequentially.
        ///     The first non-li or text node stops the addition.
        /// </summary>
        /// <param name="xamlParentElement">
        ///     Parent element for the list
        /// </param>
        /// <param name="htmlLiElement">
        ///     Start Html li element without parent list
        /// </param>
        /// <param name="inheritedProperties">
        ///     Properties inherited from parent context
        /// </param>
        /// <returns>
        ///     XmlNode representing the first non-li node in the input after one or more li's have been processed.
        /// </returns>
        private static XmlElement AddOrphanListItems(XmlElement xamlParentElement, XmlElement htmlLiElement,
            Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            Debug.Assert(htmlLiElement.LocalName.ToLower() == "li");

            XmlElement lastProcessedListItemElement = null;

            // Find out the last element attached to the xamlParentElement, which is the previous sibling of this node
            var xamlListItemElementPreviousSibling = xamlParentElement.LastChild;
            XmlElement xamlListElement;
            if (xamlListItemElementPreviousSibling != null && xamlListItemElementPreviousSibling.LocalName == XamlList)
            {
                // Previously added Xaml element was a list. We will add the new li to it
                xamlListElement = (XmlElement) xamlListItemElementPreviousSibling;
            }
            else
            {
                // No list element near. Create our own.
                xamlListElement = xamlParentElement.OwnerDocument.CreateElement(null, XamlList, XamlNamespace);
                xamlParentElement.AppendChild(xamlListElement);
            }

            XmlNode htmlChildNode = htmlLiElement;
            var htmlChildNodeName = htmlChildNode == null ? null : htmlChildNode.LocalName.ToLower();

            //  Current element properties missed here.
            //currentProperties = GetElementProperties(htmlLIElement, inheritedProperties, out localProperties, stylesheet);

            // Add li elements to the parent xamlListElement we created as long as they appear sequentially
            // Use properties inherited from xamlParentElement for context 
            while (htmlChildNode != null && htmlChildNodeName == "li")
            {
                AddListItem(xamlListElement, (XmlElement) htmlChildNode, inheritedProperties, stylesheet, sourceContext);
                lastProcessedListItemElement = (XmlElement) htmlChildNode;
                htmlChildNode = htmlChildNode.NextSibling;
                htmlChildNodeName = htmlChildNode?.LocalName.ToLower();
            }

            return lastProcessedListItemElement;
        }

        /// <summary>
        ///     Converts htmlLIElement into Xaml ListItem element, and appends it to the parent xamlListElement
        /// </summary>
        /// <param name="xamlListElement">
        ///     XmlElement representing Xaml List element to which the converted td/th should be added
        /// </param>
        /// <param name="htmlLiElement">
        ///     XmlElement representing Html li element to be converted
        /// </param>
        /// <param name="inheritedProperties">
        ///     Properties inherited from parent context
        /// </param>
        private static void AddListItem(XmlElement xamlListElement, XmlElement htmlLiElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Parameter validation
            Debug.Assert(xamlListElement != null);
            Debug.Assert(xamlListElement.LocalName == XamlList);
            Debug.Assert(htmlLiElement != null);
            Debug.Assert(htmlLiElement.LocalName.ToLower() == "li");
            Debug.Assert(inheritedProperties != null);

            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlLiElement, inheritedProperties, out localProperties,
                stylesheet, sourceContext);

            var xamlListItemElement = xamlListElement.OwnerDocument.CreateElement(null, XamlListItem,
                XamlNamespace);

            // TODO: process local properties for li element

            // Process children of the ListItem
            for (var htmlChildNode = htmlLiElement.FirstChild;
                htmlChildNode != null;
                htmlChildNode = htmlChildNode?.NextSibling)
            {
                htmlChildNode = AddBlock(xamlListItemElement, htmlChildNode, currentProperties, stylesheet,
                    sourceContext);
            }

            // Add resulting ListBoxItem to a xaml parent
            xamlListElement.AppendChild(xamlListItemElement);
        }

        // .............................................................
        //
        // Tables
        //
        // .............................................................

        /// <summary>
        ///     Converts htmlTableElement to a Xaml Table element. Adds tbody elements if they are missing so
        ///     that a resulting Xaml Table element is properly formed.
        /// </summary>
        /// <param name="xamlParentElement">
        ///     Parent xaml element to which a converted table must be added.
        /// </param>
        /// <param name="htmlTableElement">
        ///     XmlElement reprsenting the Html table element to be converted
        /// </param>
        /// <param name="inheritedProperties">
        ///     Hashtable representing properties inherited from parent context.
        /// </param>
        private static void AddTable(XmlElement xamlParentElement, XmlElement htmlTableElement,
            Hashtable inheritedProperties,
            CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Parameter validation
            Debug.Assert(htmlTableElement.LocalName.ToLower() == "table");
            Debug.Assert(xamlParentElement != null);
            Debug.Assert(inheritedProperties != null);

            // Create current properties to be used by children as inherited properties, set local properties
            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlTableElement, inheritedProperties,
                out localProperties,
                stylesheet, sourceContext);

            // TODO: process localProperties for tables to override defaults, decide cell spacing defaults

            // Check if the table contains only one cell - we want to take only its content
            var singleCell = GetCellFromSingleCellTable(htmlTableElement);

            if (singleCell != null)
            {
                //  Need to push skipped table elements onto sourceContext
                sourceContext.Add(singleCell);

                // Add the cell's content directly to parent
                for (var htmlChildNode = singleCell.FirstChild;
                    htmlChildNode != null;
                    htmlChildNode = htmlChildNode?.NextSibling)
                {
                    htmlChildNode = AddBlock(xamlParentElement, htmlChildNode, currentProperties, stylesheet,
                        sourceContext);
                }

                Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == singleCell);
                sourceContext.RemoveAt(sourceContext.Count - 1);
            }
            else
            {
                // Create xamlTableElement
                var xamlTableElement = xamlParentElement.OwnerDocument.CreateElement(null, XamlTable,
                    XamlNamespace);

                // Analyze table structure for column widths and rowspan attributes
                var columnStarts = AnalyzeTableStructure(htmlTableElement, stylesheet);

                // Process COLGROUP & COL elements
                AddColumnInformation(htmlTableElement, xamlTableElement, columnStarts, currentProperties, stylesheet,
                    sourceContext);

                // Process table body - TBODY and TR elements
                var htmlChildNode = htmlTableElement.FirstChild;

                while (htmlChildNode != null)
                {
                    var htmlChildName = htmlChildNode.LocalName.ToLower();

                    // Process the element
                    if (htmlChildName == "tbody" || htmlChildName == "thead" || htmlChildName == "tfoot")
                    {
                        //  Add more special processing for TableHeader and TableFooter
                        var xamlTableBodyElement = xamlTableElement.OwnerDocument.CreateElement(null,
                            XamlTableRowGroup,
                            XamlNamespace);
                        xamlTableElement.AppendChild(xamlTableBodyElement);

                        sourceContext.Add((XmlElement) htmlChildNode);

                        // Get properties of Html tbody element
                        Hashtable tbodyElementLocalProperties;
                        var tbodyElementCurrentProperties = GetElementProperties((XmlElement) htmlChildNode,
                            currentProperties,
                            out tbodyElementLocalProperties, stylesheet, sourceContext);
                        // TODO: apply local properties for tbody

                        // Process children of htmlChildNode, which is tbody, for tr elements
                        AddTableRowsToTableBody(xamlTableBodyElement, htmlChildNode.FirstChild,
                            tbodyElementCurrentProperties,
                            columnStarts, stylesheet, sourceContext);
                        if (xamlTableBodyElement.HasChildNodes)
                        {
                            xamlTableElement.AppendChild(xamlTableBodyElement);
                            // else: if there is no TRs in this TBody, we simply ignore it
                        }

                        Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == htmlChildNode);
                        sourceContext.RemoveAt(sourceContext.Count - 1);

                        htmlChildNode = htmlChildNode.NextSibling;
                    }
                    else if (htmlChildName == "tr")
                    {
                        // Tbody is not present, but tr element is present. Tr is wrapped in tbody
                        var xamlTableBodyElement = xamlTableElement.OwnerDocument.CreateElement(null,
                            XamlTableRowGroup,
                            XamlNamespace);

                        // We use currentProperties of xamlTableElement when adding rows since the tbody element is artificially created and has 
                        // no properties of its own

                        htmlChildNode = AddTableRowsToTableBody(xamlTableBodyElement, htmlChildNode, currentProperties,
                            columnStarts,
                            stylesheet, sourceContext);
                        if (xamlTableBodyElement.HasChildNodes)
                        {
                            xamlTableElement.AppendChild(xamlTableBodyElement);
                        }
                    }
                    else
                    {
                        // Element is not tbody or tr. Ignore it.
                        // TODO: add processing for thead, tfoot elements and recovery for td elements
                        htmlChildNode = htmlChildNode.NextSibling;
                    }
                }

                if (xamlTableElement.HasChildNodes)
                {
                    xamlParentElement.AppendChild(xamlTableElement);
                }
            }
        }

        private static XmlElement GetCellFromSingleCellTable(XmlElement htmlTableElement)
        {
            XmlElement singleCell = null;

            for (var tableChild = htmlTableElement.FirstChild;
                tableChild != null;
                tableChild = tableChild.NextSibling)
            {
                var elementName = tableChild.LocalName.ToLower();
                if (elementName == "tbody" || elementName == "thead" || elementName == "tfoot")
                {
                    if (singleCell != null)
                    {
                        return null;
                    }
                    for (var tbodyChild = tableChild.FirstChild;
                        tbodyChild != null;
                        tbodyChild = tbodyChild.NextSibling)
                    {
                        if (tbodyChild.LocalName.ToLower() == "tr")
                        {
                            if (singleCell != null)
                            {
                                return null;
                            }
                            for (var trChild = tbodyChild.FirstChild;
                                trChild != null;
                                trChild = trChild.NextSibling)
                            {
                                var cellName = trChild.LocalName.ToLower();
                                if (cellName == "td" || cellName == "th")
                                {
                                    if (singleCell != null)
                                    {
                                        return null;
                                    }
                                    singleCell = (XmlElement) trChild;
                                }
                            }
                        }
                    }
                }
                else if (tableChild.LocalName.ToLower() == "tr")
                {
                    if (singleCell != null)
                    {
                        return null;
                    }
                    for (var trChild = tableChild.FirstChild; trChild != null; trChild = trChild.NextSibling)
                    {
                        var cellName = trChild.LocalName.ToLower();
                        if (cellName == "td" || cellName == "th")
                        {
                            if (singleCell != null)
                            {
                                return null;
                            }
                            singleCell = (XmlElement) trChild;
                        }
                    }
                }
            }

            return singleCell;
        }

        /// <summary>
        ///     Processes the information about table columns - COLGROUP and COL html elements.
        /// </summary>
        /// <param name="htmlTableElement">
        ///     XmlElement representing a source html table.
        /// </param>
        /// <param name="xamlTableElement">
        ///     XmlElement repesenting a resulting xaml table.
        /// </param>
        /// <param name="columnStartsAllRows">
        ///     Array of doubles - column start coordinates.
        ///     Can be null, which means that column size information is not available
        ///     and we must use source colgroup/col information.
        ///     In case wneh it's not null, we will ignore source colgroup/col information.
        /// </param>
        /// <param name="currentProperties"></param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        private static void AddColumnInformation(XmlElement htmlTableElement, XmlElement xamlTableElement,
            ArrayList columnStartsAllRows, Hashtable currentProperties, CssStylesheet stylesheet,
            List<XmlElement> sourceContext)
        {
            // Flow document table requires <Table.Columns> element to include <TableColumn/> element as 
            // defined in https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/how-to-define-a-table-with-xaml
            // Notic: CreateElement("Table", "Columns", XamlNamespace) would add xmlns attribute to <Table.Columns> and lead to XMLReader crash.
            XmlElement xamlTableColumnGroupElement = xamlTableElement.OwnerDocument.CreateElement(null, XamlTableColumnGroup, XamlNamespace);
            // Add column information
            if (columnStartsAllRows != null)
            {
                // We have consistent information derived from table cells; use it
                // The last element in columnStarts represents the end of the table
                for (var columnIndex = 0; columnIndex < columnStartsAllRows.Count - 1; columnIndex++)
                {
                    XmlElement xamlColumnElement;

                    xamlColumnElement = xamlTableColumnGroupElement.OwnerDocument.CreateElement(null, XamlTableColumn,
                        XamlNamespace);
                    xamlColumnElement.SetAttribute(XamlWidth,
                        ((double) columnStartsAllRows[columnIndex + 1] - (double) columnStartsAllRows[columnIndex])
                            .ToString(CultureInfo.InvariantCulture));
                    xamlTableColumnGroupElement.AppendChild(xamlColumnElement);
                }
            }
            else
            {
                // We do not have consistent information from table cells;
                // Translate blindly colgroups from html.                
                for (var htmlChildNode = htmlTableElement.FirstChild;
                    htmlChildNode != null;
                    htmlChildNode = htmlChildNode.NextSibling)
                {
                    if (htmlChildNode.LocalName.ToLower() == "colgroup")
                    {
                        // TODO: add column width information to this function as a parameter and process it
                        AddTableColumnGroup(xamlTableColumnGroupElement, (XmlElement) htmlChildNode, currentProperties, stylesheet,
                            sourceContext);
                    }
                    else if (htmlChildNode.LocalName.ToLower() == "col")
                    {
                        AddTableColumn(xamlTableColumnGroupElement, (XmlElement) htmlChildNode, currentProperties, stylesheet,
                            sourceContext);
                    }
                    else if (htmlChildNode is XmlElement)
                    {
                        // Some element which belongs to table body. Stop column loop.
                        break;
                    }
                }
            }
            if (xamlTableColumnGroupElement.HasChildNodes)
            {
                xamlTableElement.AppendChild(xamlTableColumnGroupElement);
            }
        }

        /// <summary>
        ///     Converts htmlColgroupElement into Xaml TableColumnGroup element, and appends it to the parent
        ///     xamlTableElement
        /// </summary>
        /// <param name="xamlTableColumnGroupElement">
        ///     XmlElement representing Xaml Table element to which the converted column group should be added
        /// </param>
        /// <param name="htmlColgroupElement">
        ///     XmlElement representing Html colgroup element to be converted
        ///     <param name="inheritedProperties">
        ///         Properties inherited from parent context
        ///     </param>
        private static void AddTableColumnGroup(XmlElement xamlTableColumnGroupElement, XmlElement htmlColgroupElement,
            Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlColgroupElement, inheritedProperties,
                out localProperties,
                stylesheet, sourceContext);

            // TODO: process local properties for colgroup

            // Process children of colgroup. Colgroup may contain only col elements.
            for (var htmlNode = htmlColgroupElement.FirstChild; htmlNode != null; htmlNode = htmlNode.NextSibling)
            {
                if (htmlNode is XmlElement && htmlNode.LocalName.ToLower() == "col")
                {
                    AddTableColumn(xamlTableColumnGroupElement, (XmlElement) htmlNode, currentProperties, stylesheet, sourceContext);
                }
            }
        }

        /// <summary>
        ///     Converts htmlColElement into Xaml TableColumn element, and appends it to the parent
        ///     xamlTableColumnGroupElement
        /// </summary>
        /// <param name="xamlTableColumnGroupElement"></param>
        /// <param name="htmlColElement">
        ///     XmlElement representing Html col element to be converted
        /// </param>
        /// <param name="inheritedProperties">
        ///     properties inherited from parent context
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        private static void AddTableColumn(XmlElement xamlTableColumnGroupElement, XmlElement htmlColElement,
            Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            Hashtable localProperties;
            var currentProperties = GetElementProperties(htmlColElement, inheritedProperties, out localProperties,
                stylesheet, sourceContext);

            var xamlTableColumnElement = xamlTableColumnGroupElement.OwnerDocument.CreateElement(null, XamlTableColumn,
                XamlNamespace);

            // TODO: process local properties for TableColumn element

            // Col is an empty element, with no subtree 
            xamlTableColumnGroupElement.AppendChild(xamlTableColumnElement);
        }

        /// <summary>
        ///     Adds TableRow elements to xamlTableBodyElement. The rows are converted from Html tr elements that
        ///     may be the children of an Html tbody element or an Html table element with tbody missing
        /// </summary>
        /// <param name="xamlTableBodyElement">
        ///     XmlElement representing Xaml TableRowGroup element to which the converted rows should be added
        /// </param>
        /// <param name="htmlTrStartNode">
        ///     XmlElement representing the first tr child of the tbody element to be read
        /// </param>
        /// <param name="currentProperties">
        ///     Hashtable representing current properties of the tbody element that are generated and applied in the
        ///     AddTable function; to be used as inheritedProperties when adding tr elements
        /// </param>
        /// <param name="columnStarts"></param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        /// <returns>
        ///     XmlNode representing the current position of the iterator among tr elements
        /// </returns>
        private static XmlNode AddTableRowsToTableBody(XmlElement xamlTableBodyElement, XmlNode htmlTrStartNode,
            Hashtable currentProperties, ArrayList columnStarts, CssStylesheet stylesheet,
            List<XmlElement> sourceContext)
        {
            // Parameter validation
            Debug.Assert(xamlTableBodyElement.LocalName == XamlTableRowGroup);
            Debug.Assert(currentProperties != null);

            // Initialize child node for iteratimg through children to the first tr element
            var htmlChildNode = htmlTrStartNode;
            ArrayList activeRowSpans = null;
            if (columnStarts != null)
            {
                activeRowSpans = new ArrayList();
                InitializeActiveRowSpans(activeRowSpans, columnStarts.Count);
            }

            while (htmlChildNode != null && htmlChildNode.LocalName.ToLower() != "tbody")
            {
                if (htmlChildNode.LocalName.ToLower() == "tr")
                {
                    var xamlTableRowElement = xamlTableBodyElement.OwnerDocument.CreateElement(null,
                        XamlTableRow,
                        XamlNamespace);

                    sourceContext.Add((XmlElement) htmlChildNode);

                    // Get tr element properties
                    Hashtable trElementLocalProperties;
                    var trElementCurrentProperties = GetElementProperties((XmlElement) htmlChildNode,
                        currentProperties,
                        out trElementLocalProperties, stylesheet, sourceContext);
                    // TODO: apply local properties to tr element

                    AddTableCellsToTableRow(xamlTableRowElement, htmlChildNode.FirstChild, trElementCurrentProperties,
                        columnStarts,
                        activeRowSpans, stylesheet, sourceContext);
                    if (xamlTableRowElement.HasChildNodes)
                    {
                        xamlTableBodyElement.AppendChild(xamlTableRowElement);
                    }

                    Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == htmlChildNode);
                    sourceContext.RemoveAt(sourceContext.Count - 1);

                    // Advance
                    htmlChildNode = htmlChildNode.NextSibling;
                }
                else if (htmlChildNode.LocalName.ToLower() == "td")
                {
                    // Tr element is not present. We create one and add td elements to it
                    var xamlTableRowElement = xamlTableBodyElement.OwnerDocument.CreateElement(null,
                        XamlTableRow,
                        XamlNamespace);

                    // This is incorrect formatting and the column starts should not be set in this case
                    Debug.Assert(columnStarts == null);

                    htmlChildNode = AddTableCellsToTableRow(xamlTableRowElement, htmlChildNode, currentProperties,
                        columnStarts,
                        activeRowSpans, stylesheet, sourceContext);
                    if (xamlTableRowElement.HasChildNodes)
                    {
                        xamlTableBodyElement.AppendChild(xamlTableRowElement);
                    }
                }
                else
                {
                    // Not a tr or td  element. Ignore it.
                    // TODO: consider better recovery here
                    htmlChildNode = htmlChildNode.NextSibling;
                }
            }
            return htmlChildNode;
        }

        /// <summary>
        ///     Adds TableCell elements to xamlTableRowElement.
        /// </summary>
        /// <param name="xamlTableRowElement">
        ///     XmlElement representing Xaml TableRow element to which the converted cells should be added
        /// </param>
        /// <param name="htmlTdStartNode">
        ///     XmlElement representing the child of tr or tbody element from which we should start adding td elements
        /// </param>
        /// <param name="currentProperties">
        ///     properties of the current html tr element to which cells are to be added
        /// </param>
        /// <returns>
        ///     XmlElement representing the current position of the iterator among the children of the parent Html tbody/tr element
        /// </returns>
        private static XmlNode AddTableCellsToTableRow(XmlElement xamlTableRowElement, XmlNode htmlTdStartNode,
            Hashtable currentProperties, ArrayList columnStarts, ArrayList activeRowSpans, CssStylesheet stylesheet,
            List<XmlElement> sourceContext)
        {
            // parameter validation
            Debug.Assert(xamlTableRowElement.LocalName == XamlTableRow);
            Debug.Assert(currentProperties != null);
            if (columnStarts != null)
            {
                Debug.Assert(activeRowSpans.Count == columnStarts.Count);
            }

            var htmlChildNode = htmlTdStartNode;
            double columnStart = 0;
            double columnWidth = 0;
            var columnIndex = 0;
            var columnSpan = 0;

            while (htmlChildNode != null && htmlChildNode.LocalName.ToLower() != "tr" &&
                   htmlChildNode.LocalName.ToLower() != "tbody" && htmlChildNode.LocalName.ToLower() != "thead" &&
                   htmlChildNode.LocalName.ToLower() != "tfoot")
            {
                if (htmlChildNode.LocalName.ToLower() == "td" || htmlChildNode.LocalName.ToLower() == "th")
                {
                    var xamlTableCellElement = xamlTableRowElement.OwnerDocument.CreateElement(null,
                        XamlTableCell,
                        XamlNamespace);

                    sourceContext.Add((XmlElement) htmlChildNode);

                    Hashtable tdElementLocalProperties;
                    var tdElementCurrentProperties = GetElementProperties((XmlElement) htmlChildNode,
                        currentProperties,
                        out tdElementLocalProperties, stylesheet, sourceContext);

                    // TODO: determine if localProperties can be used instead of htmlChildNode in this call, and if they can,
                    // make necessary changes and use them instead.
                    ApplyPropertiesToTableCellElement((XmlElement) htmlChildNode, xamlTableCellElement);

                    if (columnStarts != null)
                    {
                        Debug.Assert(columnIndex < columnStarts.Count - 1);
                        while (columnIndex < activeRowSpans.Count && (int) activeRowSpans[columnIndex] > 0)
                        {
                            activeRowSpans[columnIndex] = (int) activeRowSpans[columnIndex] - 1;
                            Debug.Assert((int) activeRowSpans[columnIndex] >= 0);
                            columnIndex++;
                        }
                        Debug.Assert(columnIndex < columnStarts.Count - 1);
                        columnStart = (double) columnStarts[columnIndex];
                        columnWidth = GetColumnWidth((XmlElement) htmlChildNode);
                        columnSpan = CalculateColumnSpan(columnIndex, columnWidth, columnStarts);
                        var rowSpan = GetRowSpan((XmlElement) htmlChildNode);

                        // Column cannot have no span
                        Debug.Assert(columnSpan > 0);
                        Debug.Assert(columnIndex + columnSpan < columnStarts.Count);

                        xamlTableCellElement.SetAttribute(XamlTableCellColumnSpan, columnSpan.ToString());

                        // Apply row span
                        for (var spannedColumnIndex = columnIndex;
                            spannedColumnIndex < columnIndex + columnSpan;
                            spannedColumnIndex++)
                        {
                            Debug.Assert(spannedColumnIndex < activeRowSpans.Count);
                            activeRowSpans[spannedColumnIndex] = (rowSpan - 1);
                            Debug.Assert((int) activeRowSpans[spannedColumnIndex] >= 0);
                        }

                        columnIndex = columnIndex + columnSpan;
                    }

                    AddDataToTableCell(xamlTableCellElement, htmlChildNode.FirstChild, tdElementCurrentProperties,
                        stylesheet,
                        sourceContext);
                    if (xamlTableCellElement.HasChildNodes)
                    {
                        xamlTableRowElement.AppendChild(xamlTableCellElement);
                    }

                    Debug.Assert(sourceContext.Count > 0 && sourceContext[sourceContext.Count - 1] == htmlChildNode);
                    sourceContext.RemoveAt(sourceContext.Count - 1);

                    htmlChildNode = htmlChildNode.NextSibling;
                }
                else
                {
                    // Not td element. Ignore it.
                    // TODO: Consider better recovery
                    htmlChildNode = htmlChildNode.NextSibling;
                }
            }
            return htmlChildNode;
        }

        /// <summary>
        ///     adds table cell data to xamlTableCellElement
        /// </summary>
        /// <param name="xamlTableCellElement">
        ///     XmlElement representing Xaml TableCell element to which the converted data should be added
        /// </param>
        /// <param name="htmlDataStartNode">
        ///     XmlElement representing the start element of data to be added to xamlTableCellElement
        /// </param>
        /// <param name="currentProperties">
        ///     Current properties for the html td/th element corresponding to xamlTableCellElement
        /// </param>
        private static void AddDataToTableCell(XmlElement xamlTableCellElement, XmlNode htmlDataStartNode,
            Hashtable currentProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Parameter validation
            Debug.Assert(xamlTableCellElement.LocalName == XamlTableCell);
            Debug.Assert(currentProperties != null);

            for (var htmlChildNode = htmlDataStartNode;
                htmlChildNode != null;
                htmlChildNode = htmlChildNode?.NextSibling)
            {
                // Process a new html element and add it to the td element
                htmlChildNode = AddBlock(xamlTableCellElement, htmlChildNode, currentProperties, stylesheet,
                    sourceContext);
            }
        }

        /// <summary>
        ///     Performs a parsing pass over a table to read information about column width and rowspan attributes. This
        ///     information
        ///     is used to determine the starting point of each column.
        /// </summary>
        /// <param name="htmlTableElement">
        ///     XmlElement representing Html table whose structure is to be analyzed
        /// </param>
        /// <returns>
        ///     ArrayList of type double which contains the function output. If analysis is successful, this ArrayList contains
        ///     all the points which are the starting position of any column in the table, ordered from left to right.
        ///     In case if analisys was impossible we return null.
        /// </returns>
        private static ArrayList AnalyzeTableStructure(XmlElement htmlTableElement, CssStylesheet stylesheet)
        {
            // Parameter validation
            Debug.Assert(htmlTableElement.LocalName.ToLower() == "table");
            if (!htmlTableElement.HasChildNodes)
            {
                return null;
            }

            var columnWidthsAvailable = true;

            var columnStarts = new ArrayList();
            var activeRowSpans = new ArrayList();
            Debug.Assert(columnStarts.Count == activeRowSpans.Count);

            var htmlChildNode = htmlTableElement.FirstChild;
            double tableWidth = 0; // Keep track of table width which is the width of its widest row

            // Analyze tbody and tr elements
            while (htmlChildNode != null && columnWidthsAvailable)
            {
                Debug.Assert(columnStarts.Count == activeRowSpans.Count);

                switch (htmlChildNode.LocalName.ToLower())
                {
                    case "tbody":
                        // Tbody element, we should analyze its children for trows
                        var tbodyWidth = AnalyzeTbodyStructure((XmlElement) htmlChildNode, columnStarts,
                            activeRowSpans, tableWidth,
                            stylesheet);
                        if (tbodyWidth > tableWidth)
                        {
                            // Table width must be increased to supported newly added wide row
                            tableWidth = tbodyWidth;
                        }
                        else if (tbodyWidth == 0)
                        {
                            // Tbody analysis may return 0, probably due to unprocessable format. 
                            // We should also fail.
                            columnWidthsAvailable = false; // interrupt the analisys
                        }
                        break;
                    case "tr":
                        // Table row. Analyze column structure within row directly
                        var trWidth = AnalyzeTrStructure((XmlElement) htmlChildNode, columnStarts, activeRowSpans,
                            tableWidth,
                            stylesheet);
                        if (trWidth > tableWidth)
                        {
                            tableWidth = trWidth;
                        }
                        else if (trWidth == 0)
                        {
                            columnWidthsAvailable = false; // interrupt the analisys
                        }
                        break;
                    case "td":
                        // Incorrect formatting, too deep to analyze at this level. Return null.
                        // TODO: implement analysis at this level, possibly by creating a new tr
                        columnWidthsAvailable = false; // interrupt the analisys
                        break;
                    default:
                        // Element should not occur directly in table. Ignore it.
                        break;
                }

                htmlChildNode = htmlChildNode.NextSibling;
            }

            if (columnWidthsAvailable)
            {
                // Add an item for whole table width
                columnStarts.Add(tableWidth);
                VerifyColumnStartsAscendingOrder(columnStarts);
            }
            else
            {
                columnStarts = null;
            }

            return columnStarts;
        }

        /// <summary>
        ///     Performs a parsing pass over a tbody to read information about column width and rowspan attributes. Information
        ///     read about width
        ///     attributes is stored in the reference ArrayList parameter columnStarts, which contains a list of all starting
        ///     positions of all columns in the table, ordered from left to right. Row spans are taken into consideration when
        ///     computing column starts
        /// </summary>
        /// <param name="htmlTbodyElement">
        ///     XmlElement representing Html tbody whose structure is to be analyzed
        /// </param>
        /// <param name="columnStarts">
        ///     ArrayList of type double which contains the function output. If analysis fails, this parameter is set to null
        /// </param>
        /// <param name="tableWidth">
        ///     Current width of the table. This is used to determine if a new column when added to the end of table should
        ///     come after the last column in the table or is actually splitting the last column in two. If it is only splitting
        ///     the last column it should inherit row span for that column
        /// </param>
        /// <returns>
        ///     Calculated width of a tbody.
        ///     In case of non-analizable column width structure return 0;
        /// </returns>
        private static double AnalyzeTbodyStructure(XmlElement htmlTbodyElement, ArrayList columnStarts,
            ArrayList activeRowSpans, double tableWidth, CssStylesheet stylesheet)
        {
            // Parameter validation
            Debug.Assert(htmlTbodyElement.LocalName.ToLower() == "tbody");
            Debug.Assert(columnStarts != null);

            double tbodyWidth = 0;
            var columnWidthsAvailable = true;

            if (!htmlTbodyElement.HasChildNodes)
            {
                return tbodyWidth;
            }

            // Set active row spans to 0 - thus ignoring row spans crossing tbody boundaries
            ClearActiveRowSpans(activeRowSpans);

            var htmlChildNode = htmlTbodyElement.FirstChild;

            // Analyze tr elements
            while (htmlChildNode != null && columnWidthsAvailable)
            {
                switch (htmlChildNode.LocalName.ToLower())
                {
                    case "tr":
                        var trWidth = AnalyzeTrStructure((XmlElement) htmlChildNode, columnStarts, activeRowSpans,
                            tbodyWidth,
                            stylesheet);
                        if (trWidth > tbodyWidth)
                        {
                            tbodyWidth = trWidth;
                        }
                        break;
                    case "td":
                        columnWidthsAvailable = false; // interrupt the analisys
                        break;
                    default:
                        break;
                }
                htmlChildNode = htmlChildNode.NextSibling;
            }

            // Set active row spans to 0 - thus ignoring row spans crossing tbody boundaries
            ClearActiveRowSpans(activeRowSpans);

            return columnWidthsAvailable ? tbodyWidth : 0;
        }

        /// <summary>
        ///     Performs a parsing pass over a tr element to read information about column width and rowspan attributes.
        /// </summary>
        /// <param name="htmlTrElement">
        ///     XmlElement representing Html tr element whose structure is to be analyzed
        /// </param>
        /// <param name="columnStarts">
        ///     ArrayList of type double which contains the function output. If analysis is successful, this ArrayList contains
        ///     all the points which are the starting position of any column in the tr, ordered from left to right. If analysis
        ///     fails,
        ///     the ArrayList is set to null
        /// </param>
        /// <param name="activeRowSpans">
        ///     ArrayList representing all columns currently spanned by an earlier row span attribute. These columns should
        ///     not be used for data in this row. The ArrayList actually contains notation for all columns in the table, if the
        ///     active row span is set to 0 that column is not presently spanned but if it is > 0 the column is presently spanned
        /// </param>
        /// <param name="tableWidth">
        ///     Double value representing the current width of the table.
        ///     Return 0 if analisys was insuccessful.
        /// </param>
        private static double AnalyzeTrStructure(XmlElement htmlTrElement, ArrayList columnStarts,
            ArrayList activeRowSpans,
            double tableWidth, CssStylesheet stylesheet)
        {
            double columnWidth;

            // Parameter validation
            Debug.Assert(htmlTrElement.LocalName.ToLower() == "tr");
            Debug.Assert(columnStarts != null);
            Debug.Assert(activeRowSpans != null);
            Debug.Assert(columnStarts.Count == activeRowSpans.Count);

            if (!htmlTrElement.HasChildNodes)
            {
                return 0;
            }

            var columnWidthsAvailable = true;

            double columnStart = 0; // starting position of current column
            var htmlChildNode = htmlTrElement.FirstChild;
            var columnIndex = 0;
            double trWidth = 0;

            // Skip spanned columns to get to real column start
            if (columnIndex < activeRowSpans.Count)
            {
                Debug.Assert((double) columnStarts[columnIndex] >= columnStart);
                if ((double) columnStarts[columnIndex] == columnStart)
                {
                    // The new column may be in a spanned area
                    while (columnIndex < activeRowSpans.Count && (int) activeRowSpans[columnIndex] > 0)
                    {
                        activeRowSpans[columnIndex] = (int) activeRowSpans[columnIndex] - 1;
                        Debug.Assert((int) activeRowSpans[columnIndex] >= 0);
                        columnIndex++;
                        columnStart = (double) columnStarts[columnIndex];
                    }
                }
            }

            while (htmlChildNode != null && columnWidthsAvailable)
            {
                Debug.Assert(columnStarts.Count == activeRowSpans.Count);

                VerifyColumnStartsAscendingOrder(columnStarts);

                switch (htmlChildNode.LocalName.ToLower())
                {
                    case "td":
                        Debug.Assert(columnIndex <= columnStarts.Count);
                        if (columnIndex < columnStarts.Count)
                        {
                            Debug.Assert(columnStart <= (double) columnStarts[columnIndex]);
                            if (columnStart < (double) columnStarts[columnIndex])
                            {
                                columnStarts.Insert(columnIndex, columnStart);
                                // There can be no row spans now - the column data will appear here
                                // Row spans may appear only during the column analysis
                                activeRowSpans.Insert(columnIndex, 0);
                            }
                        }
                        else
                        {
                            // Column start is greater than all previous starts. Row span must still be 0 because
                            // we are either adding after another column of the same row, in which case it should not inherit
                            // the previous column's span. Otherwise we are adding after the last column of some previous
                            // row, and assuming the table widths line up, we should not be spanned by it. If there is
                            // an incorrect tbale structure where a columns starts in the middle of a row span, we do not
                            // guarantee correct output
                            columnStarts.Add(columnStart);
                            activeRowSpans.Add(0);
                        }
                        columnWidth = GetColumnWidth((XmlElement) htmlChildNode);
                        if (columnWidth != -1)
                        {
                            int nextColumnIndex;
                            var rowSpan = GetRowSpan((XmlElement) htmlChildNode);

                            nextColumnIndex = GetNextColumnIndex(columnIndex, columnWidth, columnStarts, activeRowSpans);
                            if (nextColumnIndex != -1)
                            {
                                // Entire column width can be processed without hitting conflicting row span. This means that
                                // column widths line up and we can process them
                                Debug.Assert(nextColumnIndex <= columnStarts.Count);

                                // Apply row span to affected columns
                                for (var spannedColumnIndex = columnIndex;
                                    spannedColumnIndex < nextColumnIndex;
                                    spannedColumnIndex++)
                                {
                                    activeRowSpans[spannedColumnIndex] = rowSpan - 1;
                                    Debug.Assert((int) activeRowSpans[spannedColumnIndex] >= 0);
                                }

                                columnIndex = nextColumnIndex;

                                // Calculate columnsStart for the next cell
                                columnStart = columnStart + columnWidth;

                                if (columnIndex < activeRowSpans.Count)
                                {
                                    Debug.Assert((double) columnStarts[columnIndex] >= columnStart);
                                    if ((double) columnStarts[columnIndex] == columnStart)
                                    {
                                        // The new column may be in a spanned area
                                        while (columnIndex < activeRowSpans.Count &&
                                               (int) activeRowSpans[columnIndex] > 0)
                                        {
                                            activeRowSpans[columnIndex] = (int) activeRowSpans[columnIndex] - 1;
                                            Debug.Assert((int) activeRowSpans[columnIndex] >= 0);
                                            columnIndex++;
                                            columnStart = (double) columnStarts[columnIndex];
                                        }
                                    }
                                    // else: the new column does not start at the same time as a pre existing column
                                    // so we don't have to check it for active row spans, it starts in the middle
                                    // of another column which has been checked already by the GetNextColumnIndex function
                                }
                            }
                            else
                            {
                                // Full column width cannot be processed without a pre existing row span.
                                // We cannot analyze widths
                                columnWidthsAvailable = false;
                            }
                        }
                        else
                        {
                            // Incorrect column width, stop processing
                            columnWidthsAvailable = false;
                        }
                        break;
                    default:
                        break;
                }

                htmlChildNode = htmlChildNode.NextSibling;
            }

            // The width of the tr element is the position at which it's last td element ends, which is calculated in
            // the columnStart value after each td element is processed
            trWidth = columnWidthsAvailable ? columnStart : 0;

            return trWidth;
        }

        /// <summary>
        ///     Gets row span attribute from htmlTDElement. Returns an integer representing the value of the rowspan attribute.
        ///     Default value if attribute is not specified or if it is invalid is 1
        /// </summary>
        /// <param name="htmlTdElement">
        ///     Html td element to be searched for rowspan attribute
        /// </param>
        private static int GetRowSpan(XmlElement htmlTdElement)
        {
            string rowSpanAsString;
            int rowSpan;

            rowSpanAsString = GetAttribute(htmlTdElement, "rowspan");
            if (rowSpanAsString != null)
            {
                if (!int.TryParse(rowSpanAsString, out rowSpan))
                {
                    // Ignore invalid value of rowspan; treat it as 1
                    rowSpan = 1;
                }
            }
            else
            {
                // No row span, default is 1
                rowSpan = 1;
            }
            return rowSpan;
        }

        /// <summary>
        ///     Gets index at which a column should be inseerted into the columnStarts ArrayList. This is
        ///     decided by the value columnStart. The columnStarts ArrayList is ordered in ascending order.
        ///     Returns an integer representing the index at which the column should be inserted
        /// </summary>
        /// <param name="columnStarts">
        ///     Array list representing starting coordinates of all columns in the table
        /// </param>
        /// <param name="columnStart">
        ///     Starting coordinate of column we wish to insert into columnStart
        /// </param>
        /// <param name="columnIndex">
        ///     Int representing the current column index. This acts as a clue while finding the insertion index.
        ///     If the value of columnStarts at columnIndex is the same as columnStart, then this position alrady exists
        ///     in the array and we can jsut return columnIndex.
        /// </param>
        /// <returns></returns>
        private static int GetNextColumnIndex(int columnIndex, double columnWidth, ArrayList columnStarts,
            ArrayList activeRowSpans)
        {
            double columnStart;
            int spannedColumnIndex;

            // Parameter validation
            Debug.Assert(columnStarts != null);
            Debug.Assert(0 <= columnIndex && columnIndex <= columnStarts.Count);
            Debug.Assert(columnWidth > 0);

            columnStart = (double) columnStarts[columnIndex];
            spannedColumnIndex = columnIndex + 1;

            while (spannedColumnIndex < columnStarts.Count &&
                   (double) columnStarts[spannedColumnIndex] < columnStart + columnWidth && spannedColumnIndex != -1)
            {
                if ((int) activeRowSpans[spannedColumnIndex] > 0)
                {
                    // The current column should span this area, but something else is already spanning it
                    // Not analyzable
                    spannedColumnIndex = -1;
                }
                else
                {
                    spannedColumnIndex++;
                }
            }

            return spannedColumnIndex;
        }


        /// <summary>
        ///     Used for clearing activeRowSpans array in the beginning/end of each tbody
        /// </summary>
        /// <param name="activeRowSpans">
        ///     ArrayList representing currently active row spans
        /// </param>
        private static void ClearActiveRowSpans(ArrayList activeRowSpans)
        {
            for (var columnIndex = 0; columnIndex < activeRowSpans.Count; columnIndex++)
            {
                activeRowSpans[columnIndex] = 0;
            }
        }

        /// <summary>
        ///     Used for initializing activeRowSpans array in the before adding rows to tbody element
        /// </summary>
        /// <param name="activeRowSpans">
        ///     ArrayList representing currently active row spans
        /// </param>
        /// <param name="count">
        ///     Size to be give to array list
        /// </param>
        private static void InitializeActiveRowSpans(ArrayList activeRowSpans, int count)
        {
            for (var columnIndex = 0; columnIndex < count; columnIndex++)
            {
                activeRowSpans.Add(0);
            }
        }


        /// <summary>
        ///     Calculates width of next TD element based on starting position of current element and it's width, which
        ///     is calculated byt he function
        /// </summary>
        /// <param name="htmlTdElement">
        ///     XmlElement representing Html td element whose width is to be read
        /// </param>
        /// <param name="columnStart">
        ///     Starting position of current column
        /// </param>
        private static double GetNextColumnStart(XmlElement htmlTdElement, double columnStart)
        {
            double columnWidth;
            double nextColumnStart;

            // Parameter validation
            Debug.Assert(htmlTdElement.LocalName.ToLower() == "td" || htmlTdElement.LocalName.ToLower() == "th");
            Debug.Assert(columnStart >= 0);

            nextColumnStart = -1; // -1 indicates inability to calculate columnStart width

            columnWidth = GetColumnWidth(htmlTdElement);

            if (columnWidth == -1)
            {
                nextColumnStart = -1;
            }
            else
            {
                nextColumnStart = columnStart + columnWidth;
            }

            return nextColumnStart;
        }


        private static double GetColumnWidth(XmlElement htmlTdElement)
        {
            string columnWidthAsString;
            double columnWidth;

            columnWidthAsString = null;
            columnWidth = -1;

            // Get string valkue for the width
            columnWidthAsString = GetAttribute(htmlTdElement, "width") ??
                                  GetCssAttribute(GetAttribute(htmlTdElement, "style"), "width");

            // We do not allow column width to be 0, if specified as 0 we will fail to record it
            if (!TryGetLengthValue(columnWidthAsString, out columnWidth) || columnWidth == 0)
            {
                columnWidth = -1;
            }
            return columnWidth;
        }

        /// <summary>
        ///     Calculates column span based the column width and the widths of all other columns. Returns an integer representing
        ///     the column span
        /// </summary>
        /// <param name="columnIndex">
        ///     Index of the current column
        /// </param>
        /// <param name="columnWidth">
        ///     Width of the current column
        /// </param>
        /// <param name="columnStarts">
        ///     ArrayList repsenting starting coordinates of all columns
        /// </param>
        private static int CalculateColumnSpan(int columnIndex, double columnWidth, ArrayList columnStarts)
        {
            // Current status of column width. Indicates the amount of width that has been scanned already
            double columnSpanningValue;
            int columnSpanningIndex;
            int columnSpan;
            double subColumnWidth; // Width of the smallest-grain columns in the table

            Debug.Assert(columnStarts != null);
            Debug.Assert(columnIndex < columnStarts.Count - 1);
            Debug.Assert((double) columnStarts[columnIndex] >= 0);
            Debug.Assert(columnWidth > 0);

            columnSpanningIndex = columnIndex;
            columnSpanningValue = 0;
            columnSpan = 0;
            subColumnWidth = 0;

            while (columnSpanningValue < columnWidth && columnSpanningIndex < columnStarts.Count - 1)
            {
                subColumnWidth = (double) columnStarts[columnSpanningIndex + 1] -
                                 (double) columnStarts[columnSpanningIndex];
                Debug.Assert(subColumnWidth > 0);
                columnSpanningValue += subColumnWidth;
                columnSpanningIndex++;
            }

            // Now, we have either covered the width we needed to cover or reached the end of the table, in which
            // case the column spans all the columns until the end
            columnSpan = columnSpanningIndex - columnIndex;
            Debug.Assert(columnSpan > 0);

            return columnSpan;
        }

        /// <summary>
        ///     Verifies that values in columnStart, which represent starting coordinates of all columns, are arranged
        ///     in ascending order
        /// </summary>
        /// <param name="columnStarts">
        ///     ArrayList representing starting coordinates of all columns
        /// </param>
        private static void VerifyColumnStartsAscendingOrder(ArrayList columnStarts)
        {
            Debug.Assert(columnStarts != null);

            double columnStart;

            columnStart = -0.01;

            foreach (object t in columnStarts)
            {
                Debug.Assert(columnStart < (double) t);
                columnStart = (double) t;
            }
        }

        // .............................................................
        //
        // Attributes and Properties
        //
        // .............................................................

        /// <summary>
        ///     Analyzes local properties of Html element, converts them into Xaml equivalents, and applies them to xamlElement
        /// </summary>
        /// <param name="xamlElement">
        ///     XmlElement representing Xaml element to which properties are to be applied
        /// </param>
        /// <param name="localProperties">
        ///     Hashtable representing local properties of Html element that is converted into xamlElement
        /// </param>
        private static void ApplyLocalProperties(XmlElement xamlElement, Hashtable localProperties, bool isBlock)
        {
            var marginSet = false;
            var marginTop = "0";
            var marginBottom = "0";
            var marginLeft = "0";
            var marginRight = "0";

            var paddingSet = false;
            var paddingTop = "0";
            var paddingBottom = "0";
            var paddingLeft = "0";
            var paddingRight = "0";

            string borderColor = null;

            var borderThicknessSet = false;
            var borderThicknessTop = "0";
            var borderThicknessBottom = "0";
            var borderThicknessLeft = "0";
            var borderThicknessRight = "0";

            var propertyEnumerator = localProperties.GetEnumerator();
            while (propertyEnumerator.MoveNext())
            {
                switch ((string) propertyEnumerator.Key)
                {
                    case "font-family":
                        //  Convert from font-family value list into xaml FontFamily value
                        xamlElement.SetAttribute(XamlFontFamily, (string) propertyEnumerator.Value);
                        break;
                    case "font-style":
                        xamlElement.SetAttribute(XamlFontStyle, (string) propertyEnumerator.Value);
                        break;
                    case "font-variant":
                        //  Convert from font-variant into xaml property
                        break;
                    case "font-weight":
                        xamlElement.SetAttribute(XamlFontWeight, (string) propertyEnumerator.Value);
                        break;
                    case "font-size":
                        //  Convert from css size into FontSize
                        xamlElement.SetAttribute(XamlFontSize, (string) propertyEnumerator.Value);
                        break;
                    case "color":
                        SetPropertyValue(xamlElement, TextElement.ForegroundProperty, (string) propertyEnumerator.Value);
                        break;
                    case "background-color":
                        SetPropertyValue(xamlElement, TextElement.BackgroundProperty, (string) propertyEnumerator.Value);
                        break;
                    case "text-decoration-underline":
                        if (!isBlock)
                        {
                            if ((string) propertyEnumerator.Value == "true")
                            {
                                xamlElement.SetAttribute(XamlTextDecorations, XamlTextDecorationsUnderline);
                            }
                        }
                        break;
                    case "text-decoration-none":
                    case "text-decoration-overline":
                    case "text-decoration-line-through":
                    case "text-decoration-blink":
                        //  Convert from all other text-decorations values
                        if (!isBlock)
                        {
                        }
                        break;
                    case "text-transform":
                        //  Convert from text-transform into xaml property
                        break;

                    case "text-indent":
                        if (isBlock)
                        {
                            xamlElement.SetAttribute(XamlTextIndent, (string) propertyEnumerator.Value);
                        }
                        break;

                    case "text-align":
                        if (isBlock)
                        {
                            xamlElement.SetAttribute(XamlTextAlignment, (string) propertyEnumerator.Value);
                        }
                        break;

                    case "width":
                    case "height":
                        //  Decide what to do with width and height propeties
                        break;

                    case "margin-top":
                        marginSet = true;
                        marginTop = (string) propertyEnumerator.Value;
                        break;
                    case "margin-right":
                        marginSet = true;
                        marginRight = (string) propertyEnumerator.Value;
                        break;
                    case "margin-bottom":
                        marginSet = true;
                        marginBottom = (string) propertyEnumerator.Value;
                        break;
                    case "margin-left":
                        marginSet = true;
                        marginLeft = (string) propertyEnumerator.Value;
                        break;

                    case "padding-top":
                        paddingSet = true;
                        paddingTop = (string) propertyEnumerator.Value;
                        break;
                    case "padding-right":
                        paddingSet = true;
                        paddingRight = (string) propertyEnumerator.Value;
                        break;
                    case "padding-bottom":
                        paddingSet = true;
                        paddingBottom = (string) propertyEnumerator.Value;
                        break;
                    case "padding-left":
                        paddingSet = true;
                        paddingLeft = (string) propertyEnumerator.Value;
                        break;

                    // NOTE: css names for elementary border styles have side indications in the middle (top/bottom/left/right)
                    // In our internal notation we intentionally put them at the end - to unify processing in ParseCssRectangleProperty method
                    case "border-color-top":
                        borderColor = (string) propertyEnumerator.Value;
                        break;
                    case "border-color-right":
                        borderColor = (string) propertyEnumerator.Value;
                        break;
                    case "border-color-bottom":
                        borderColor = (string) propertyEnumerator.Value;
                        break;
                    case "border-color-left":
                        borderColor = (string) propertyEnumerator.Value;
                        break;
                    case "border-style-top":
                    case "border-style-right":
                    case "border-style-bottom":
                    case "border-style-left":
                        //  Implement conversion from border style
                        break;
                    case "border-width-top":
                        borderThicknessSet = true;
                        borderThicknessTop = (string) propertyEnumerator.Value;
                        break;
                    case "border-width-right":
                        borderThicknessSet = true;
                        borderThicknessRight = (string) propertyEnumerator.Value;
                        break;
                    case "border-width-bottom":
                        borderThicknessSet = true;
                        borderThicknessBottom = (string) propertyEnumerator.Value;
                        break;
                    case "border-width-left":
                        borderThicknessSet = true;
                        borderThicknessLeft = (string) propertyEnumerator.Value;
                        break;

                    case "list-style-type":
                        if (xamlElement.LocalName == XamlList)
                        {
                            string markerStyle;
                            switch (((string) propertyEnumerator.Value).ToLower())
                            {
                                case "disc":
                                    markerStyle = XamlListMarkerStyleDisc;
                                    break;
                                case "circle":
                                    markerStyle = XamlListMarkerStyleCircle;
                                    break;
                                case "none":
                                    markerStyle = XamlListMarkerStyleNone;
                                    break;
                                case "square":
                                    markerStyle = XamlListMarkerStyleSquare;
                                    break;
                                case "box":
                                    markerStyle = XamlListMarkerStyleBox;
                                    break;
                                case "lower-latin":
                                    markerStyle = XamlListMarkerStyleLowerLatin;
                                    break;
                                case "upper-latin":
                                    markerStyle = XamlListMarkerStyleUpperLatin;
                                    break;
                                case "lower-roman":
                                    markerStyle = XamlListMarkerStyleLowerRoman;
                                    break;
                                case "upper-roman":
                                    markerStyle = XamlListMarkerStyleUpperRoman;
                                    break;
                                case "decimal":
                                    markerStyle = XamlListMarkerStyleDecimal;
                                    break;
                                default:
                                    markerStyle = XamlListMarkerStyleDisc;
                                    break;
                            }
                            xamlElement.SetAttribute(XamlListMarkerStyle, markerStyle);
                        }
                        break;

                    case "float":
                    case "clear":
                        if (isBlock)
                        {
                            //  Convert float and clear properties
                        }
                        break;

                    case "display":
                        break;
                }
            }

            if (isBlock)
            {
                if (marginSet)
                {
                    ComposeThicknessProperty(xamlElement, XamlMargin, marginLeft, marginRight, marginTop, marginBottom);
                }

                if (paddingSet)
                {
                    ComposeThicknessProperty(xamlElement, XamlPadding, paddingLeft, paddingRight, paddingTop,
                        paddingBottom);
                }

                if (borderColor != null)
                {
                    //  We currently ignore possible difference in brush colors on different border sides. Use the last colored side mentioned
                    xamlElement.SetAttribute(XamlBorderBrush, borderColor);
                }

                if (borderThicknessSet)
                {
                    ComposeThicknessProperty(xamlElement, XamlBorderThickness, borderThicknessLeft,
                        borderThicknessRight,
                        borderThicknessTop, borderThicknessBottom);
                }
            }
        }

        // Create syntactically optimized four-value Thickness
        private static void ComposeThicknessProperty(XmlElement xamlElement, string propertyName, string left,
            string right,
            string top, string bottom)
        {
            // Xaml syntax:
            // We have a reasonable interpreation for one value (all four edges), two values (horizontal, vertical),
            // and four values (left, top, right, bottom).
            //  switch (i) {
            //    case 1: return new Thickness(lengths[0]);
            //    case 2: return new Thickness(lengths[0], lengths[1], lengths[0], lengths[1]);
            //    case 4: return new Thickness(lengths[0], lengths[1], lengths[2], lengths[3]);
            //  }
            string thickness;

            // We do not accept negative margins
            if (left[0] == '0' || left[0] == '-') left = "0";
            if (right[0] == '0' || right[0] == '-') right = "0";
            if (top[0] == '0' || top[0] == '-') top = "0";
            if (bottom[0] == '0' || bottom[0] == '-') bottom = "0";

            if (left == right && top == bottom)
            {
                if (left == top)
                {
                    thickness = left;
                }
                else
                {
                    thickness = left + "," + top;
                }
            }
            else
            {
                thickness = left + "," + top + "," + right + "," + bottom;
            }

            //  Need safer processing for a thickness value
            xamlElement.SetAttribute(propertyName, thickness);
        }

        private static void SetPropertyValue(XmlElement xamlElement, DependencyProperty property, string stringValue)
        {
            var typeConverter =
                TypeDescriptor.GetConverter(property.PropertyType);
            try
            {
                var convertedValue = typeConverter.ConvertFromInvariantString(stringValue);
                if (convertedValue != null)
                {
                    xamlElement.SetAttribute(property.Name, stringValue);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     Analyzes the tag of the htmlElement and infers its associated formatted properties.
        ///     After that parses style attribute and adds all inline css styles.
        ///     The resulting style attributes are collected in output parameter localProperties.
        /// </summary>
        /// <param name="htmlElement">
        /// </param>
        /// <param name="inheritedProperties">
        ///     set of properties inherited from ancestor elements. Currently not used in the code. Reserved for the future
        ///     development.
        /// </param>
        /// <param name="localProperties">
        ///     returns all formatting properties defined by this element - implied by its tag, its attributes, or its css inline
        ///     style
        /// </param>
        /// <param name="stylesheet"></param>
        /// <param name="sourceContext"></param>
        /// <returns>
        ///     returns a combination of previous context with local set of properties.
        ///     This value is not used in the current code - inntended for the future development.
        /// </returns>
        private static Hashtable GetElementProperties(XmlElement htmlElement, Hashtable inheritedProperties,
            out Hashtable localProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
        {
            // Start with context formatting properties
            var currentProperties = new Hashtable();
            var propertyEnumerator = inheritedProperties.GetEnumerator();
            while (propertyEnumerator.MoveNext())
            {
                currentProperties[propertyEnumerator.Key] = propertyEnumerator.Value;
            }

            // Identify element name
            var elementName = htmlElement.LocalName.ToLower();
            var elementNamespace = htmlElement.NamespaceURI;

            // update current formatting properties depending on element tag

            localProperties = new Hashtable();
            switch (elementName)
            {
                // Character formatting
                case "i":
                case "italic":
                case "em":
                    localProperties["font-style"] = "italic";
                    break;
                case "b":
                case "bold":
                case "strong":
                case "dfn":
                    localProperties["font-weight"] = "bold";
                    break;
                case "u":
                case "underline":
                    localProperties["text-decoration-underline"] = "true";
                    break;
                case "font":
                    var attributeValue = GetAttribute(htmlElement, "face");
                    if (attributeValue != null)
                    {
                        localProperties["font-family"] = attributeValue;
                    }
                    attributeValue = GetAttribute(htmlElement, "size");
                    if (attributeValue != null)
                    {
                        var fontSize = double.Parse(attributeValue)*(12.0/3.0);
                        if (fontSize < 1.0)
                        {
                            fontSize = 1.0;
                        }
                        else if (fontSize > 1000.0)
                        {
                            fontSize = 1000.0;
                        }
                        localProperties["font-size"] = fontSize.ToString(CultureInfo.InvariantCulture);
                    }
                    attributeValue = GetAttribute(htmlElement, "color");
                    if (attributeValue != null)
                    {
                        localProperties["color"] = attributeValue;
                    }
                    break;
                case "samp":
                    localProperties["font-family"] = "Courier New"; // code sample
                    localProperties["font-size"] = XamlFontSizeXxSmall;
                    localProperties["text-align"] = "Left";
                    break;
                case "sub":
                    break;
                case "sup":
                    break;

                // Hyperlinks
                case "a": // href, hreflang, urn, methods, rel, rev, title
                    //  Set default hyperlink properties
                    break;
                case "acronym":
                    break;

                // Paragraph formatting:
                case "p":
                    //  Set default paragraph properties
                    break;
                case "div":
                    //  Set default div properties
                    break;
                case "pre":
                    localProperties["font-family"] = "Courier New"; // renders text in a fixed-width font
                    localProperties["font-size"] = XamlFontSizeXxSmall;
                    localProperties["text-align"] = "Left";
                    break;
                case "blockquote":
                    localProperties["margin-left"] = "16";
                    break;

                case "h1":
                    localProperties["font-size"] = XamlFontSizeXxLarge;
                    break;
                case "h2":
                    localProperties["font-size"] = XamlFontSizeXLarge;
                    break;
                case "h3":
                    localProperties["font-size"] = XamlFontSizeLarge;
                    break;
                case "h4":
                    localProperties["font-size"] = XamlFontSizeMedium;
                    break;
                case "h5":
                    localProperties["font-size"] = XamlFontSizeSmall;
                    break;
                case "h6":
                    localProperties["font-size"] = XamlFontSizeXSmall;
                    break;
                // List properties
                case "ul":
                    localProperties["list-style-type"] = "disc";
                    break;
                case "ol":
                    localProperties["list-style-type"] = "decimal";
                    break;

                case "table":
                case "body":
                case "html":
                    break;
            }

            // Override html defaults by css attributes - from stylesheets and inline settings
            HtmlCssParser.GetElementPropertiesFromCssAttributes(htmlElement, elementName, stylesheet, localProperties,
                sourceContext);

            // Combine local properties with context to create new current properties
            propertyEnumerator = localProperties.GetEnumerator();
            while (propertyEnumerator.MoveNext())
            {
                currentProperties[propertyEnumerator.Key] = propertyEnumerator.Value;
            }

            return currentProperties;
        }

        /// <summary>
        ///     Extracts a value of css attribute from css style definition.
        /// </summary>
        /// <param name="cssStyle">
        ///     Source csll style definition
        /// </param>
        /// <param name="attributeName">
        ///     A name of css attribute to extract
        /// </param>
        /// <returns>
        ///     A string rrepresentation of an attribute value if found;
        ///     null if there is no such attribute in a given string.
        /// </returns>
        private static string GetCssAttribute(string cssStyle, string attributeName)
        {
            //  This is poor man's attribute parsing. Replace it by real css parsing
            if (cssStyle != null)
            {
                string[] styleValues;

                attributeName = attributeName.ToLower();

                // Check for width specification in style string
                styleValues = cssStyle.Split(';');

                foreach (string t in styleValues)
                {
                    string[] styleNameValue;

                    styleNameValue = t.Split(':');
                    if (styleNameValue.Length == 2)
                    {
                        if (styleNameValue[0].Trim().ToLower() == attributeName)
                        {
                            return styleNameValue[1].Trim();
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     Converts a length value from string representation to a double.
        /// </summary>
        /// <param name="lengthAsString">
        ///     Source string value of a length.
        /// </param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static bool TryGetLengthValue(string lengthAsString, out double length)
        {
            length = double.NaN;

            if (lengthAsString != null)
            {
                lengthAsString = lengthAsString.Trim().ToLower();

                // We try to convert currentColumnWidthAsString into a double. This will eliminate widths of type "50%", etc.
                if (lengthAsString.EndsWith("pt"))
                {
                    lengthAsString = lengthAsString.Substring(0, lengthAsString.Length - 2);
                    if (double.TryParse(lengthAsString, out length))
                    {
                        length = (length*96.0)/72.0; // convert from points to pixels
                    }
                    else
                    {
                        length = double.NaN;
                    }
                }
                else if (lengthAsString.EndsWith("px"))
                {
                    lengthAsString = lengthAsString.Substring(0, lengthAsString.Length - 2);
                    if (!double.TryParse(lengthAsString, out length))
                    {
                        length = double.NaN;
                    }
                }
                else
                {
                    if (!double.TryParse(lengthAsString, out length)) // Assuming pixels
                    {
                        length = double.NaN;
                    }
                }
            }

            return !double.IsNaN(length);
        }

        // .................................................................
        //
        // Pasring Color Attribute
        //
        // .................................................................

        private static string GetColorValue(string colorValue) => colorValue;

        /// <summary>
        ///     Applies properties to xamlTableCellElement based on the html td element it is converted from.
        /// </summary>
        /// <param name="htmlChildNode">
        ///     Html td/th element to be converted to xaml
        /// </param>
        /// <param name="xamlTableCellElement">
        ///     XmlElement representing Xaml element for which properties are to be processed
        /// </param>
        /// <remarks>
        ///     TODO: Use the processed properties for htmlChildNode instead of using the node itself
        /// </remarks>
        private static void ApplyPropertiesToTableCellElement(XmlElement htmlChildNode, XmlElement xamlTableCellElement)
        {
            // Parameter validation
            Debug.Assert(htmlChildNode.LocalName.ToLower() == "td" || htmlChildNode.LocalName.ToLower() == "th");
            Debug.Assert(xamlTableCellElement.LocalName == XamlTableCell);

            // set default border thickness for xamlTableCellElement to enable gridlines
            xamlTableCellElement.SetAttribute(XamlTableCellBorderThickness, "1,1,1,1");
            xamlTableCellElement.SetAttribute(XamlTableCellBorderBrush, XamlBrushesBlack);
            var rowSpanString = GetAttribute(htmlChildNode, "rowspan");
            if (rowSpanString != null)
            {
                xamlTableCellElement.SetAttribute(XamlTableCellRowSpan, rowSpanString);
            }
        }

        #endregion Private Methods
    }
}