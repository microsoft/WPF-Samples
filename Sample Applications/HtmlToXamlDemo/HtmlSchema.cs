// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics;

namespace HtmlToXamlDemo
{
    /// <summary>
    ///     HtmlSchema class
    ///     maintains static information about HTML structure
    ///     can be used by HtmlParser to check conditions under which an element starts or ends, etc.
    /// </summary>
    internal class HtmlSchema
    {
        // ---------------------------------------------------------------------
        //
        // Constructors
        //
        // ---------------------------------------------------------------------

        #region Constructors

        /// <summary>
        ///     static constructor, initializes the ArrayLists
        ///     that hold the elements in various sub-components of the schema
        ///     e.g _htmlEmptyElements, etc.
        /// </summary>
        static HtmlSchema()
        {
            // initializes the list of all html elements
            InitializeInlineElements();

            InitializeBlockElements();

            InitializeOtherOpenableElements();

            // initialize empty elements list
            InitializeEmptyElements();

            // initialize list of elements closing on the outer element end
            InitializeElementsClosingOnParentElementEnd();

            // initalize list of elements that close when a new element starts
            InitializeElementsClosingOnNewElementStart();

            // Initialize character entities
            InitializeHtmlCharacterEntities();
        }

        #endregion Constructors;

        // ---------------------------------------------------------------------
        //
        // Internal Methods
        //
        // ---------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        ///     returns true when xmlElementName corresponds to empty element
        /// </summary>
        /// <param name="xmlElementName">
        ///     string representing name to test
        /// </param>
        internal static bool IsEmptyElement(string xmlElementName) => _htmlEmptyElements.Contains(xmlElementName.ToLower());

        /// <summary>
        ///     returns true if xmlElementName represents a block formattinng element.
        ///     It used in an algorithm of transferring inline elements over block elements
        ///     in HtmlParser
        /// </summary>
        /// <param name="xmlElementName"></param>
        /// <returns></returns>
        internal static bool IsBlockElement(string xmlElementName) => _htmlBlockElements.Contains(xmlElementName);

        /// <summary>
        ///     returns true if the xmlElementName represents an inline formatting element
        /// </summary>
        /// <param name="xmlElementName"></param>
        /// <returns></returns>
        internal static bool IsInlineElement(string xmlElementName) => _htmlInlineElements.Contains(xmlElementName);

        /// <summary>
        ///     It is a list of known html elements which we
        ///     want to allow to produce bt HTML parser,
        ///     but don'tt want to act as inline, block or no-scope.
        ///     Presence in this list will allow to open
        ///     elements during html parsing, and adding the
        ///     to a tree produced by html parser.
        /// </summary>
        internal static bool IsKnownOpenableElement(string xmlElementName) => _htmlOtherOpenableElements.Contains(xmlElementName);

        /// <summary>
        ///     returns true when xmlElementName closes when the outer element closes
        ///     this is true of elements with optional start tags
        /// </summary>
        /// <param name="xmlElementName">
        ///     string representing name to test
        /// </param>
        internal static bool ClosesOnParentElementEnd(string xmlElementName) => _htmlElementsClosingOnParentElementEnd.Contains(xmlElementName.ToLower());

        /// <summary>
        ///     returns true if the current element closes when the new element, whose name has just been read, starts
        /// </summary>
        /// <param name="currentElementName">
        ///     string representing current element name
        /// </param>
        /// <param name="elementName"></param>
        /// string representing name of the next element that will start
        internal static bool ClosesOnNextElementStart(string currentElementName, string nextElementName)
        {
            Debug.Assert(currentElementName == currentElementName.ToLower());
            switch (currentElementName)
            {
                case "colgroup":
                    return _htmlElementsClosingColgroup.Contains(nextElementName) && IsBlockElement(nextElementName);
                case "dd":
                    return _htmlElementsClosingDd.Contains(nextElementName) && IsBlockElement(nextElementName);
                case "dt":
                    return _htmlElementsClosingDt.Contains(nextElementName) && IsBlockElement(nextElementName);
                case "li":
                    return _htmlElementsClosingLi.Contains(nextElementName);
                case "p":
                    return IsBlockElement(nextElementName);
                case "tbody":
                    return _htmlElementsClosingTbody.Contains(nextElementName);
                case "tfoot":
                    return _htmlElementsClosingTfoot.Contains(nextElementName);
                case "thead":
                    return _htmlElementsClosingThead.Contains(nextElementName);
                case "tr":
                    return _htmlElementsClosingTr.Contains(nextElementName);
                case "td":
                    return _htmlElementsClosingTd.Contains(nextElementName);
                case "th":
                    return _htmlElementsClosingTh.Contains(nextElementName);
            }
            return false;
        }

        /// <summary>
        ///     returns true if the string passed as argument is an Html entity name
        /// </summary>
        /// <param name="entityName">
        ///     string to be tested for Html entity name
        /// </param>
        internal static bool IsEntity(string entityName)
        {
            // we do not convert entity strings to lowercase because these names are case-sensitive
            if (_htmlCharacterEntities.Contains(entityName))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     returns the character represented by the entity name string which is passed as an argument, if the string is an
        ///     entity name
        ///     as specified in _htmlCharacterEntities, returns the character value of 0 otherwise
        /// </summary>
        /// <param name="entityName">
        ///     string representing entity name whose character value is desired
        /// </param>
        internal static char EntityCharacterValue(string entityName)
        {
            if (_htmlCharacterEntities.Contains(entityName))
            {
                return (char) _htmlCharacterEntities[entityName];
            }
            return (char) 0;
        }

        #endregion Internal Methods

        // ---------------------------------------------------------------------
        //
        //  Internal Properties
        //
        // ---------------------------------------------------------------------

        #region Internal Properties

        #endregion Internal Indexers

        // ---------------------------------------------------------------------
        //
        // Private Methods
        //
        // ---------------------------------------------------------------------

        #region Private Methods

        private static void InitializeInlineElements()
        {
            _htmlInlineElements = new ArrayList
            {
                "a",
                "abbr",
                "acronym",
                "address",
                "b",
                "bdo",
                "big",
                "button",
                "code",
                "del",
                "dfn",
                "em",
                "font",
                "i",
                "ins",
                "kbd",
                "label",
                "legend",
                "q",
                "s",
                "samp",
                "small",
                "span",
                "strike",
                "strong",
                "sub",
                "sup",
                "u",
                "var"
            };
            // ???
            // deleted text
            // inserted text
            // text to entered by a user
            // ???
            // short inline quotation
            // strike-through text style
            // Specifies a code sample
            // indicates an instance of a program variable
        }

        private static void InitializeBlockElements()
        {
            _htmlBlockElements = new ArrayList
            {
                "blockquote",
                "body",
                "caption",
                "center",
                "cite",
                "dd",
                "dir",
                "div",
                "dl",
                "dt",
                "form",
                "h1",
                "h2",
                "h3",
                "h4",
                "h5",
                "h6",
                "html",
                "li",
                "menu",
                "ol",
                "p",
                "pre",
                "table",
                "tbody",
                "td",
                "textarea",
                "tfoot",
                "th",
                "thead",
                "tr",
                "tt",
                "ul"
            };

            //  treat as UL element
            // Not a block according to XHTML spec
            //  treat as UL element
            // Renders text in a fixed-width font
        }

        /// <summary>
        ///     initializes _htmlEmptyElements with empty elements in HTML 4 spec at
        ///     http://www.w3.org/TR/REC-html40/index/elements.html
        /// </summary>
        private static void InitializeEmptyElements()
        {
            // Build a list of empty (no-scope) elements 
            // (element not requiring closing tags, and not accepting any content)
            _htmlEmptyElements = new ArrayList
            {
                "area",
                "base",
                "basefont",
                "br",
                "col",
                "frame",
                "hr",
                "img",
                "input",
                "isindex",
                "link",
                "meta",
                "param"
            };
        }

        private static void InitializeOtherOpenableElements()
        {
            // It is a list of known html elements which we
            // want to allow to produce bt HTML parser,
            // but don'tt want to act as inline, block or no-scope.
            // Presence in this list will allow to open
            // elements during html parsing, and adding the
            // to a tree produced by html parser.
            _htmlOtherOpenableElements = new ArrayList
            {
                "applet",
                "base",
                "basefont",
                "colgroup",
                "fieldset",
                "frameset",
                "head",
                "iframe",
                "map",
                "noframes",
                "noscript",
                "object",
                "optgroup",
                "option",
                "script",
                "select",
                "style",
                "title"
            };
            //_htmlOtherOpenableElements.Add("form"); --> treated as block
        }

        /// <summary>
        ///     initializes _htmlElementsClosingOnParentElementEnd with the list of HTML 4 elements for which closing tags are
        ///     optional
        ///     we assume that for any element for which closing tags are optional, the element closes when it's outer element
        ///     (in which it is nested) does
        /// </summary>
        private static void InitializeElementsClosingOnParentElementEnd()
        {
            _htmlElementsClosingOnParentElementEnd = new ArrayList
            {
                "body",
                "colgroup",
                "dd",
                "dt",
                "head",
                "html",
                "li",
                "p",
                "tbody",
                "td",
                "tfoot",
                "thead",
                "th",
                "tr"
            };
        }

        private static void InitializeElementsClosingOnNewElementStart()
        {
            _htmlElementsClosingColgroup = new ArrayList {"colgroup", "tr", "thead", "tfoot", "tbody"};

            _htmlElementsClosingDd = new ArrayList {"dd", "dt"};
            // TODO: dd may end in other cases as well - if a new "p" starts, etc.
            // TODO: these are the basic "legal" cases but there may be more recovery

            _htmlElementsClosingDt = new ArrayList();
            _htmlElementsClosingDd.Add("dd");
            _htmlElementsClosingDd.Add("dt");
            // TODO: dd may end in other cases as well - if a new "p" starts, etc.
            // TODO: these are the basic "legal" cases but there may be more recovery

            _htmlElementsClosingLi = new ArrayList {"li"};
            // TODO: more complex recovery

            _htmlElementsClosingTbody = new ArrayList {"tbody", "thead", "tfoot"};
            // TODO: more complex recovery

            _htmlElementsClosingTr = new ArrayList {"thead", "tfoot", "tbody", "tr"};
            // NOTE: tr should not really close on a new thead
            // because if there are rows before a thead, it is assumed to be in tbody, whose start tag is optional
            // and thead can't come after tbody
            // however, if we do encounter this, it's probably best to end the row and ignore the thead or treat
            // it as part of the table
            // TODO: more complex recovery

            _htmlElementsClosingTd = new ArrayList {"td", "th", "tr", "tbody", "tfoot", "thead"};
            // TODO: more complex recovery

            _htmlElementsClosingTh = new ArrayList {"td", "th", "tr", "tbody", "tfoot", "thead"};
            // TODO: more complex recovery

            _htmlElementsClosingThead = new ArrayList {"tbody", "tfoot"};
            // TODO: more complex recovery

            _htmlElementsClosingTfoot = new ArrayList {"tbody", "thead"};
            // although thead comes before tfoot, we add it because if it is found the tfoot should close
            // and some recovery processing be done on the thead
            // TODO: more complex recovery
        }

        /// <summary>
        ///     initializes _htmlCharacterEntities hashtable with the character corresponding to entity names
        /// </summary>
        private static void InitializeHtmlCharacterEntities()
        {
            _htmlCharacterEntities = new Hashtable
            {
                ["Aacute"] = (char) 193,
                ["aacute"] = (char) 225,
                ["Acirc"] = (char) 194,
                ["acirc"] = (char) 226,
                ["acute"] = (char) 180,
                ["AElig"] = (char) 198,
                ["aelig"] = (char) 230,
                ["Agrave"] = (char) 192,
                ["agrave"] = (char) 224,
                ["alefsym"] = (char) 8501,
                ["Alpha"] = (char) 913,
                ["alpha"] = (char) 945,
                ["amp"] = (char) 38,
                ["and"] = (char) 8743,
                ["ang"] = (char) 8736,
                ["Aring"] = (char) 197,
                ["aring"] = (char) 229,
                ["asymp"] = (char) 8776,
                ["Atilde"] = (char) 195,
                ["atilde"] = (char) 227,
                ["Auml"] = (char) 196,
                ["auml"] = (char) 228,
                ["bdquo"] = (char) 8222,
                ["Beta"] = (char) 914,
                ["beta"] = (char) 946,
                ["brvbar"] = (char) 166,
                ["bull"] = (char) 8226,
                ["cap"] = (char) 8745,
                ["Ccedil"] = (char) 199,
                ["ccedil"] = (char) 231,
                ["cent"] = (char) 162,
                ["Chi"] = (char) 935,
                ["chi"] = (char) 967,
                ["circ"] = (char) 710,
                ["clubs"] = (char) 9827,
                ["cong"] = (char) 8773,
                ["copy"] = (char) 169,
                ["crarr"] = (char) 8629,
                ["cup"] = (char) 8746,
                ["curren"] = (char) 164,
                ["dagger"] = (char) 8224,
                ["Dagger"] = (char) 8225,
                ["darr"] = (char) 8595,
                ["dArr"] = (char) 8659,
                ["deg"] = (char) 176,
                ["Delta"] = (char) 916,
                ["delta"] = (char) 948,
                ["diams"] = (char) 9830,
                ["divide"] = (char) 247,
                ["Eacute"] = (char) 201,
                ["eacute"] = (char) 233,
                ["Ecirc"] = (char) 202,
                ["ecirc"] = (char) 234,
                ["Egrave"] = (char) 200,
                ["egrave"] = (char) 232,
                ["empty"] = (char) 8709,
                ["emsp"] = (char) 8195,
                ["ensp"] = (char) 8194,
                ["Epsilon"] = (char) 917,
                ["epsilon"] = (char) 949,
                ["equiv"] = (char) 8801,
                ["Eta"] = (char) 919,
                ["eta"] = (char) 951,
                ["ETH"] = (char) 208,
                ["eth"] = (char) 240,
                ["Euml"] = (char) 203,
                ["euml"] = (char) 235,
                ["euro"] = (char) 8364,
                ["exist"] = (char) 8707,
                ["fnof"] = (char) 402,
                ["forall"] = (char) 8704,
                ["frac12"] = (char) 189,
                ["frac14"] = (char) 188,
                ["frac34"] = (char) 190,
                ["frasl"] = (char) 8260,
                ["Gamma"] = (char) 915,
                ["gamma"] = (char) 947,
                ["ge"] = (char) 8805,
                ["gt"] = (char) 62,
                ["harr"] = (char) 8596,
                ["hArr"] = (char) 8660,
                ["hearts"] = (char) 9829,
                ["hellip"] = (char) 8230,
                ["Iacute"] = (char) 205,
                ["iacute"] = (char) 237,
                ["Icirc"] = (char) 206,
                ["icirc"] = (char) 238,
                ["iexcl"] = (char) 161,
                ["Igrave"] = (char) 204,
                ["igrave"] = (char) 236,
                ["image"] = (char) 8465,
                ["infin"] = (char) 8734,
                ["int"] = (char) 8747,
                ["Iota"] = (char) 921,
                ["iota"] = (char) 953,
                ["iquest"] = (char) 191,
                ["isin"] = (char) 8712,
                ["Iuml"] = (char) 207,
                ["iuml"] = (char) 239,
                ["Kappa"] = (char) 922,
                ["kappa"] = (char) 954,
                ["Lambda"] = (char) 923,
                ["lambda"] = (char) 955,
                ["lang"] = (char) 9001,
                ["laquo"] = (char) 171,
                ["larr"] = (char) 8592,
                ["lArr"] = (char) 8656,
                ["lceil"] = (char) 8968,
                ["ldquo"] = (char) 8220,
                ["le"] = (char) 8804,
                ["lfloor"] = (char) 8970,
                ["lowast"] = (char) 8727,
                ["loz"] = (char) 9674,
                ["lrm"] = (char) 8206,
                ["lsaquo"] = (char) 8249,
                ["lsquo"] = (char) 8216,
                ["lt"] = (char) 60,
                ["macr"] = (char) 175,
                ["mdash"] = (char) 8212,
                ["micro"] = (char) 181,
                ["middot"] = (char) 183,
                ["minus"] = (char) 8722,
                ["Mu"] = (char) 924,
                ["mu"] = (char) 956,
                ["nabla"] = (char) 8711,
                ["nbsp"] = (char) 160,
                ["ndash"] = (char) 8211,
                ["ne"] = (char) 8800,
                ["ni"] = (char) 8715,
                ["not"] = (char) 172,
                ["notin"] = (char) 8713,
                ["nsub"] = (char) 8836,
                ["Ntilde"] = (char) 209,
                ["ntilde"] = (char) 241,
                ["Nu"] = (char) 925,
                ["nu"] = (char) 957,
                ["Oacute"] = (char) 211,
                ["ocirc"] = (char) 244,
                ["OElig"] = (char) 338,
                ["oelig"] = (char) 339,
                ["Ograve"] = (char) 210,
                ["ograve"] = (char) 242,
                ["oline"] = (char) 8254,
                ["Omega"] = (char) 937,
                ["omega"] = (char) 969,
                ["Omicron"] = (char) 927,
                ["omicron"] = (char) 959,
                ["oplus"] = (char) 8853,
                ["or"] = (char) 8744,
                ["ordf"] = (char) 170,
                ["ordm"] = (char) 186,
                ["Oslash"] = (char) 216,
                ["oslash"] = (char) 248,
                ["Otilde"] = (char) 213,
                ["otilde"] = (char) 245,
                ["otimes"] = (char) 8855,
                ["Ouml"] = (char) 214,
                ["ouml"] = (char) 246,
                ["para"] = (char) 182,
                ["part"] = (char) 8706,
                ["permil"] = (char) 8240,
                ["perp"] = (char) 8869,
                ["Phi"] = (char) 934,
                ["phi"] = (char) 966,
                ["pi"] = (char) 960,
                ["piv"] = (char) 982,
                ["plusmn"] = (char) 177,
                ["pound"] = (char) 163,
                ["prime"] = (char) 8242,
                ["Prime"] = (char) 8243,
                ["prod"] = (char) 8719,
                ["prop"] = (char) 8733,
                ["Psi"] = (char) 936,
                ["psi"] = (char) 968,
                ["quot"] = (char) 34,
                ["radic"] = (char) 8730,
                ["rang"] = (char) 9002,
                ["raquo"] = (char) 187,
                ["rarr"] = (char) 8594,
                ["rArr"] = (char) 8658,
                ["rceil"] = (char) 8969,
                ["rdquo"] = (char) 8221,
                ["real"] = (char) 8476,
                ["reg"] = (char) 174,
                ["rfloor"] = (char) 8971,
                ["Rho"] = (char) 929,
                ["rho"] = (char) 961,
                ["rlm"] = (char) 8207,
                ["rsaquo"] = (char) 8250,
                ["rsquo"] = (char) 8217,
                ["sbquo"] = (char) 8218,
                ["Scaron"] = (char) 352,
                ["scaron"] = (char) 353,
                ["sdot"] = (char) 8901,
                ["sect"] = (char) 167,
                ["shy"] = (char) 173,
                ["Sigma"] = (char) 931,
                ["sigma"] = (char) 963,
                ["sigmaf"] = (char) 962,
                ["sim"] = (char) 8764,
                ["spades"] = (char) 9824,
                ["sub"] = (char) 8834,
                ["sube"] = (char) 8838,
                ["sum"] = (char) 8721,
                ["sup"] = (char) 8835,
                ["sup1"] = (char) 185,
                ["sup2"] = (char) 178,
                ["sup3"] = (char) 179,
                ["supe"] = (char) 8839,
                ["szlig"] = (char) 223,
                ["Tau"] = (char) 932,
                ["tau"] = (char) 964,
                ["there4"] = (char) 8756,
                ["Theta"] = (char) 920,
                ["theta"] = (char) 952,
                ["thetasym"] = (char) 977,
                ["thinsp"] = (char) 8201,
                ["THORN"] = (char) 222,
                ["thorn"] = (char) 254,
                ["tilde"] = (char) 732,
                ["times"] = (char) 215,
                ["trade"] = (char) 8482,
                ["Uacute"] = (char) 218,
                ["uacute"] = (char) 250,
                ["uarr"] = (char) 8593,
                ["uArr"] = (char) 8657,
                ["Ucirc"] = (char) 219,
                ["ucirc"] = (char) 251,
                ["Ugrave"] = (char) 217,
                ["ugrave"] = (char) 249,
                ["uml"] = (char) 168,
                ["upsih"] = (char) 978,
                ["Upsilon"] = (char) 933,
                ["upsilon"] = (char) 965,
                ["Uuml"] = (char) 220,
                ["uuml"] = (char) 252,
                ["weierp"] = (char) 8472,
                ["Xi"] = (char) 926,
                ["xi"] = (char) 958,
                ["Yacute"] = (char) 221,
                ["yacute"] = (char) 253,
                ["yen"] = (char) 165,
                ["Yuml"] = (char) 376,
                ["yuml"] = (char) 255,
                ["Zeta"] = (char) 918,
                ["zeta"] = (char) 950,
                ["zwj"] = (char) 8205,
                ["zwnj"] = (char) 8204
            };
        }

        #endregion Private Methods

        // ---------------------------------------------------------------------
        //
        // Private Fields
        //
        // ---------------------------------------------------------------------

        #region Private Fields

        // html element names
        // this is an array list now, but we may want to make it a hashtable later for better performance
        private static ArrayList _htmlInlineElements;

        private static ArrayList _htmlBlockElements;

        private static ArrayList _htmlOtherOpenableElements;

        // list of html empty element names
        private static ArrayList _htmlEmptyElements;

        // names of html elements for which closing tags are optional, and close when the outer nested element closes
        private static ArrayList _htmlElementsClosingOnParentElementEnd;

        // names of elements that close certain optional closing tag elements when they start

        // names of elements closing the colgroup element
        private static ArrayList _htmlElementsClosingColgroup;

        // names of elements closing the dd element
        private static ArrayList _htmlElementsClosingDd;

        // names of elements closing the dt element
        private static ArrayList _htmlElementsClosingDt;

        // names of elements closing the li element
        private static ArrayList _htmlElementsClosingLi;

        // names of elements closing the tbody element
        private static ArrayList _htmlElementsClosingTbody;

        // names of elements closing the td element
        private static ArrayList _htmlElementsClosingTd;

        // names of elements closing the tfoot element
        private static ArrayList _htmlElementsClosingTfoot;

        // names of elements closing the thead element
        private static ArrayList _htmlElementsClosingThead;

        // names of elements closing the th element
        private static ArrayList _htmlElementsClosingTh;

        // names of elements closing the tr element
        private static ArrayList _htmlElementsClosingTr;

        // html character entities hashtable
        private static Hashtable _htmlCharacterEntities;

        #endregion Private Fields
    }
}