// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

// StringBuilder

// important TODOS: 
// TODO 1. Start tags: The ParseXmlElement function has been modified to be called after both the 
// angle bracket < and element name have been read, instead of just the < bracket and some valid name character, 
// previously the case. This change was made so that elements with optional closing tags could read a new
// element's start tag and decide whether they were required to close. However, there is a question of whether to
// handle this in the parser or lexical analyzer. It is currently handled in the parser - the lexical analyzer still
// recognizes a start tag opener as a '<' + valid name start char; it is the parser that reads the actual name. 
// this is correct behavior assuming that the name is a valid html name, because the lexical analyzer should not know anything
// about optional closing tags, etc. UPDATED: 10/13/2004: I am updating this to read the whole start tag of something 
// that is not an HTML, treat it as empty, and add it to the tree. That way the converter will know it's there, but
// it will hvae no content. We could also partially recover by trying to look up and match names if they are similar
// TODO 2. Invalid element names: However, it might make sense to give the lexical analyzer the ability to identify
// a valid html element name and not return something as a start tag otherwise. For example, if we type <good>, should
// the lexical analyzer return that it has found the start of an element when this is not the case in HTML? But this will
// require implementing a lookahead token in the lexical analyzer so that it can treat an invalid element name as text. One 
// character of lookahead will not be enough.
// TODO 3. Attributes: The attribute recovery is poor when reading attribute values in quotes - if no closing quotes are found,
// the lexical analyzer just keeps reading and if it eventually reaches the end of file, it would have just skipped everything.
// There are a couple of ways to deal with this: 1) stop reading attributes when we encounter a '>' character - this doesn't allow
// the '>' character to be used in attribute values, but it can still be used as an entity. 2) Maintain a HTML-specific list
// of attributes and their values that each html element can take, and if we find correct attribute namesand values for an
// element we use them regardless of the quotes, this way we could just ignore something invalid. One more option: 3) Read ahead
// in the quoted value and if we find an end of file, we can return to where we were and process as text. However this requires
// a lot of lookahead and a resettable reader.
// TODO 4: elements with optional closing tags: For elements with optional closing tags, we always close the element if we find
// that one of it's ancestors has closed. This condition may be too broad and we should develop a better heuristic. We should also
// improve the heuristics for closing certain elements when the next element starts
// TODO 5. Nesting: Support for unbalanced nesting, e.g. <b> <i> </b> </i>: this is not presently supported. To support it we may need
// to maintain two xml elements, one the element that represents what has already been read and another represents what we are presently reading.
// Then if we encounter an unbalanced nesting tag we could close the element that was supposed to close, save the current element
// and store it in the list of already-read content, and then open a new element to which all tags that are currently open
// can be applied. Is there a better way to do this? Should we do it at all?
// TODO 6. Elements with optional starting tags: there are 4 such elements in the HTML 4 specification - html, tbody, body and head.
// The current recovery doesn;t do anything for any of these elements except the html element, because it's not critical - head
// and body elementscan be contained within html element, and tbody is contained within table. To extend this for XHTML 
// extensions, and to recover in case other elements are missing start tags, we would need to insert an extra recursive call
// to ParseXmlElement for the missing start tag. It is suggested to do this by giving ParseXmlElement an argument that specifies
// a name to use. If this argument is null, it  assumes its name is the next token from the lexical analyzer and continues
// exactly as it does now. However, if the argument contains a valid html element name then it takes that value as its name
// and continues as before. This way, if the next token is the element that should actually be its child, it will see
// the name in the next step and initiate a recursive call. We would also need to add some logic in the loop for when a start tag
// is found - if the start tag is not compatible with current context and indicates that a start tag has been missed, then we
// can initiate the extra recursive call and give it the name of the missed start tag. The issues are when to insert this logic,
// and if we want to support it over multiple missing start tags. If we insert it at the time a start tag is read in element
// text,  then we can support only one missing start tag, since the extra call will read the next start tag and make a recursive
// call without checking the context. This is a conceptual problem, and the check should be made just before a recursive call,
// with the choice being whether we should supply an element name as argument, or leave it as NULL and read from the input
// TODO 7: Context: Is it appropriate to keep track of context here? For example, should we only expect td, tr elements when
// reading a table and ignore them otherwise? This may be too much of a load on the parser, I think it's better if the converter
// deals with it

namespace HtmlToXamlDemo
{
    /// <summary>
    ///     HtmlParser class accepts a string of possibly badly formed Html, parses it and returns a string
    ///     of well-formed Html that is as close to the original string in content as possible
    /// </summary>
    internal class HtmlParser
    {
        // ---------------------------------------------------------------------
        //
        // Constructors
        //
        // ---------------------------------------------------------------------

        #region Constructors

        /// <summary>
        ///     Constructor. Initializes the _htmlLexicalAnalayzer element with the given input string
        /// </summary>
        /// <param name="inputString">
        ///     string to parsed into well-formed Html
        /// </param>
        private HtmlParser(string inputString)
        {
            // Create an output xml document
            _document = new XmlDocument();

            // initialize open tag stack
            _openedElements = new Stack<XmlElement>();

            _pendingInlineElements = new Stack<XmlElement>();

            // initialize lexical analyzer
            _htmlLexicalAnalyzer = new HtmlLexicalAnalyzer(inputString);

            // get first token from input, expecting text
            _htmlLexicalAnalyzer.GetNextContentToken();
        }

        #endregion Constructors

        // ---------------------------------------------------------------------
        //
        // Internal Methods
        //
        // ---------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        ///     Instantiates an HtmlParser element and calls the parsing function on the given input string
        /// </summary>
        /// <param name="htmlString">
        ///     Input string of pssibly badly-formed Html to be parsed into well-formed Html
        /// </param>
        /// <returns>
        ///     XmlElement rep
        /// </returns>
        internal static XmlElement ParseHtml(string htmlString)
        {
            var htmlParser = new HtmlParser(htmlString);

            var htmlRootElement = htmlParser.ParseHtmlContent();

            return htmlRootElement;
        }

        // .....................................................................
        //
        // Html Header on Clipboard
        //
        // .....................................................................

        // Html header structure.
        //      Version:1.0
        //      StartHTML:000000000
        //      EndHTML:000000000
        //      StartFragment:000000000
        //      EndFragment:000000000
        //      StartSelection:000000000
        //      EndSelection:000000000
        internal const string HtmlHeader =
            "Version:1.0\r\nStartHTML:{0:D10}\r\nEndHTML:{1:D10}\r\nStartFragment:{2:D10}\r\nEndFragment:{3:D10}\r\nStartSelection:{4:D10}\r\nEndSelection:{5:D10}\r\n";

        internal const string HtmlStartFragmentComment = "<!--StartFragment-->";
        internal const string HtmlEndFragmentComment = "<!--EndFragment-->";

        /// <summary>
        ///     Extracts Html string from clipboard data by parsing header information in htmlDataString
        /// </summary>
        /// <param name="htmlDataString">
        ///     String representing Html clipboard data. This includes Html header
        /// </param>
        /// <returns>
        ///     String containing only the Html data part of htmlDataString, without header
        /// </returns>
        internal static string ExtractHtmlFromClipboardData(string htmlDataString)
        {
            var startHtmlIndex = htmlDataString.IndexOf("StartHTML:", StringComparison.Ordinal);
            if (startHtmlIndex < 0)
            {
                return "ERROR: Urecognized html header";
            }
            // TODO: We assume that indices represented by strictly 10 zeros ("0123456789".Length),
            // which could be wrong assumption. We need to implement more flrxible parsing here
            startHtmlIndex =
                int.Parse(htmlDataString.Substring(startHtmlIndex + "StartHTML:".Length, "0123456789".Length));
            if (startHtmlIndex < 0 || startHtmlIndex > htmlDataString.Length)
            {
                return "ERROR: Urecognized html header";
            }

            var endHtmlIndex = htmlDataString.IndexOf("EndHTML:", StringComparison.Ordinal);
            if (endHtmlIndex < 0)
            {
                return "ERROR: Urecognized html header";
            }
            // TODO: We assume that indices represented by strictly 10 zeros ("0123456789".Length),
            // which could be wrong assumption. We need to implement more flrxible parsing here
            endHtmlIndex = int.Parse(htmlDataString.Substring(endHtmlIndex + "EndHTML:".Length, "0123456789".Length));
            if (endHtmlIndex > htmlDataString.Length)
            {
                endHtmlIndex = htmlDataString.Length;
            }

            return htmlDataString.Substring(startHtmlIndex, endHtmlIndex - startHtmlIndex);
        }

        /// <summary>
        ///     Adds Xhtml header information to Html data string so that it can be placed on clipboard
        /// </summary>
        /// <param name="htmlString">
        ///     Html string to be placed on clipboard with appropriate header
        /// </param>
        /// <returns>
        ///     String wrapping htmlString with appropriate Html header
        /// </returns>
        internal static string AddHtmlClipboardHeader(string htmlString)
        {
            var stringBuilder = new StringBuilder();

            // each of 6 numbers is represented by "{0:D10}" in the format string
            // must actually occupy 10 digit positions ("0123456789")
            var startHtml = HtmlHeader.Length + 6*("0123456789".Length - "{0:D10}".Length);
            var endHtml = startHtml + htmlString.Length;
            var startFragment = htmlString.IndexOf(HtmlStartFragmentComment, 0, StringComparison.Ordinal);
            if (startFragment >= 0)
            {
                startFragment = startHtml + startFragment + HtmlStartFragmentComment.Length;
            }
            else
            {
                startFragment = startHtml;
            }
            var endFragment = htmlString.IndexOf(HtmlEndFragmentComment, 0, StringComparison.Ordinal);
            if (endFragment >= 0)
            {
                endFragment = startHtml + endFragment;
            }
            else
            {
                endFragment = endHtml;
            }

            // Create HTML clipboard header string
            stringBuilder.AppendFormat(HtmlHeader, startHtml, endHtml, startFragment, endFragment, startFragment,
                endFragment);

            // Append HTML body.
            stringBuilder.Append(htmlString);

            return stringBuilder.ToString();
        }

        #endregion Internal Methods

        // ---------------------------------------------------------------------
        //
        // Private methods
        //
        // ---------------------------------------------------------------------

        #region Private Methods

        private void InvariantAssert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("Assertion error: " + message);
            }
        }

        /// <summary>
        ///     Parses the stream of html tokens starting
        ///     from the name of top-level element.
        ///     Returns XmlElement representing the top-level
        ///     html element
        /// </summary>
        private XmlElement ParseHtmlContent()
        {
            // Create artificial root elelemt to be able to group multiple top-level elements
            // We create "html" element which may be a duplicate of real HTML element, which is ok, as HtmlConverter will swallow it painlessly..
            var htmlRootElement = _document.CreateElement("html", XhtmlNamespace);
            OpenStructuringElement(htmlRootElement);

            while (_htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.Eof)
            {
                if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.OpeningTagStart)
                {
                    _htmlLexicalAnalyzer.GetNextTagToken();
                    if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Name)
                    {
                        var htmlElementName = _htmlLexicalAnalyzer.NextToken.ToLower();
                        _htmlLexicalAnalyzer.GetNextTagToken();

                        // Create an element
                        var htmlElement = _document.CreateElement(htmlElementName, XhtmlNamespace);

                        // Parse element attributes
                        ParseAttributes(htmlElement);

                        if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.EmptyTagEnd ||
                            HtmlSchema.IsEmptyElement(htmlElementName))
                        {
                            // It is an element without content (because of explicit slash or based on implicit knowledge aboout html)
                            AddEmptyElement(htmlElement);
                        }
                        else if (HtmlSchema.IsInlineElement(htmlElementName))
                        {
                            // Elements known as formatting are pushed to some special
                            // pending stack, which allows them to be transferred
                            // over block tags - by doing this we convert
                            // overlapping tags into normal heirarchical element structure.
                            OpenInlineElement(htmlElement);
                        }
                        else if (HtmlSchema.IsBlockElement(htmlElementName) ||
                                 HtmlSchema.IsKnownOpenableElement(htmlElementName))
                        {
                            // This includes no-scope elements
                            OpenStructuringElement(htmlElement);
                        }
                    }
                }
                else if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.ClosingTagStart)
                {
                    _htmlLexicalAnalyzer.GetNextTagToken();
                    if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Name)
                    {
                        var htmlElementName = _htmlLexicalAnalyzer.NextToken.ToLower();

                        // Skip the name token. Assume that the following token is end of tag,
                        // but do not check this. If it is not true, we simply ignore one token
                        // - this is our recovery from bad xml in this case.
                        _htmlLexicalAnalyzer.GetNextTagToken();

                        CloseElement(htmlElementName);
                    }
                }
                else if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Text)
                {
                    AddTextContent(_htmlLexicalAnalyzer.NextToken);
                }
                else if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Comment)
                {
                    AddComment(_htmlLexicalAnalyzer.NextToken);
                }

                _htmlLexicalAnalyzer.GetNextContentToken();
            }

            // Get rid of the artificial root element
            if (htmlRootElement.FirstChild is XmlElement &&
                htmlRootElement.FirstChild == htmlRootElement.LastChild &&
                htmlRootElement.FirstChild.LocalName.ToLower() == "html")
            {
                htmlRootElement = (XmlElement) htmlRootElement.FirstChild;
            }

            return htmlRootElement;
        }

        private XmlElement CreateElementCopy(XmlElement htmlElement)
        {
            var htmlElementCopy = _document.CreateElement(htmlElement.LocalName, XhtmlNamespace);
            for (var i = 0; i < htmlElement.Attributes.Count; i++)
            {
                var attribute = htmlElement.Attributes[i];
                htmlElementCopy.SetAttribute(attribute.Name, attribute.Value);
            }
            return htmlElementCopy;
        }

        private void AddEmptyElement(XmlElement htmlEmptyElement)
        {
            InvariantAssert(_openedElements.Count > 0,
                "AddEmptyElement: Stack of opened elements cannot be empty, as we have at least one artificial root element");
            var htmlParent = _openedElements.Peek();
            htmlParent.AppendChild(htmlEmptyElement);
        }

        private void OpenInlineElement(XmlElement htmlInlineElement)
        {
            _pendingInlineElements.Push(htmlInlineElement);
        }

        // Opens structurig element such as Div or Table etc.
        private void OpenStructuringElement(XmlElement htmlElement)
        {
            // Close all pending inline elements
            // All block elements are considered as delimiters for inline elements
            // which forces all inline elements to be closed and re-opened in the following
            // structural element (if any).
            // By doing that we guarantee that all inline elements appear only within most nested blocks
            if (HtmlSchema.IsBlockElement(htmlElement.LocalName))
            {
                while (_openedElements.Count > 0 && HtmlSchema.IsInlineElement(_openedElements.Peek().LocalName))
                {
                    var htmlInlineElement = _openedElements.Pop();
                    InvariantAssert(_openedElements.Count > 0,
                        "OpenStructuringElement: stack of opened elements cannot become empty here");

                    _pendingInlineElements.Push(CreateElementCopy(htmlInlineElement));
                }
            }

            // Add this block element to its parent
            if (_openedElements.Count > 0)
            {
                var htmlParent = _openedElements.Peek();

                // Check some known block elements for auto-closing (LI and P)
                if (HtmlSchema.ClosesOnNextElementStart(htmlParent.LocalName, htmlElement.LocalName))
                {
                    _openedElements.Pop();
                    htmlParent = _openedElements.Count > 0 ? _openedElements.Peek() : null;
                }

                // NOTE:
                // Actually we never expect null - it would mean two top-level P or LI (without a parent).
                // In such weird case we will loose all paragraphs except the first one...
                htmlParent?.AppendChild(htmlElement);
            }

            // Push it onto a stack
            _openedElements.Push(htmlElement);
        }

        private bool IsElementOpened(string htmlElementName) => _openedElements.Any(openedElement => openedElement.LocalName == htmlElementName);

        private void CloseElement(string htmlElementName)
        {
            // Check if the element is opened and already added to the parent
            InvariantAssert(_openedElements.Count > 0,
                "CloseElement: Stack of opened elements cannot be empty, as we have at least one artificial root element");

            // Check if the element is opened and still waiting to be added to the parent
            if (_pendingInlineElements.Count > 0 && _pendingInlineElements.Peek().LocalName == htmlElementName)
            {
                // Closing an empty inline element.
                // Note that HtmlConverter will skip empty inlines, but for completeness we keep them here on parser level.
                var htmlInlineElement = _pendingInlineElements.Pop();
                InvariantAssert(_openedElements.Count > 0,
                    "CloseElement: Stack of opened elements cannot be empty, as we have at least one artificial root element");
                var htmlParent = _openedElements.Peek();
                htmlParent.AppendChild(htmlInlineElement);
            }
            else if (IsElementOpened(htmlElementName))
            {
                while (_openedElements.Count > 1) // we never pop the last element - the artificial root
                {
                    // Close all unbalanced elements.
                    var htmlOpenedElement = _openedElements.Pop();

                    if (htmlOpenedElement.LocalName == htmlElementName)
                    {
                        return;
                    }

                    if (HtmlSchema.IsInlineElement(htmlOpenedElement.LocalName))
                    {
                        // Unbalances Inlines will be transfered to the next element content
                        _pendingInlineElements.Push(CreateElementCopy(htmlOpenedElement));
                    }
                }
            }

            // If element was not opened, we simply ignore the unbalanced closing tag
        }

        private void AddTextContent(string textContent)
        {
            OpenPendingInlineElements();

            InvariantAssert(_openedElements.Count > 0,
                "AddTextContent: Stack of opened elements cannot be empty, as we have at least one artificial root element");

            var htmlParent = _openedElements.Peek();
            var textNode = _document.CreateTextNode(textContent);
            htmlParent.AppendChild(textNode);
        }

        private void AddComment(string comment)
        {
            OpenPendingInlineElements();

            InvariantAssert(_openedElements.Count > 0,
                "AddComment: Stack of opened elements cannot be empty, as we have at least one artificial root element");

            var htmlParent = _openedElements.Peek();
            var xmlComment = _document.CreateComment(comment);
            htmlParent.AppendChild(xmlComment);
        }

        // Moves all inline elements pending for opening to actual document
        // and adds them to current open stack.
        private void OpenPendingInlineElements()
        {
            if (_pendingInlineElements.Count > 0)
            {
                var htmlInlineElement = _pendingInlineElements.Pop();

                OpenPendingInlineElements();

                InvariantAssert(_openedElements.Count > 0,
                    "OpenPendingInlineElements: Stack of opened elements cannot be empty, as we have at least one artificial root element");

                var htmlParent = _openedElements.Peek();
                htmlParent.AppendChild(htmlInlineElement);
                _openedElements.Push(htmlInlineElement);
            }
        }

        private void ParseAttributes(XmlElement xmlElement)
        {
            while (_htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.Eof && //
                   _htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.TagEnd && //
                   _htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.EmptyTagEnd)
            {
                // read next attribute (name=value)
                if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Name)
                {
                    var attributeName = _htmlLexicalAnalyzer.NextToken;
                    _htmlLexicalAnalyzer.GetNextEqualSignToken();

                    _htmlLexicalAnalyzer.GetNextAtomToken();

                    var attributeValue = _htmlLexicalAnalyzer.NextToken;
                    xmlElement.SetAttribute(attributeName, attributeValue);
                }
                _htmlLexicalAnalyzer.GetNextTagToken();
            }
        }

        #endregion Private Methods

        // ---------------------------------------------------------------------
        //
        // Private Fields
        //
        // ---------------------------------------------------------------------

        #region Private Fields

        internal const string XhtmlNamespace = "http://www.w3.org/1999/xhtml";

        private readonly HtmlLexicalAnalyzer _htmlLexicalAnalyzer;

        // document from which all elements are created
        private readonly XmlDocument _document;

        // stack for open elements
        private readonly Stack<XmlElement> _openedElements;
        private readonly Stack<XmlElement> _pendingInlineElements;

        #endregion Private Fields
    }
}