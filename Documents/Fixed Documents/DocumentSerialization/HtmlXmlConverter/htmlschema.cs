//---------------------------------------------------------------------------
// 
// File: HtmlSchema.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Static information about HTML structure
//
//---------------------------------------------------------------------------

namespace DocumentSerialization
{
    using System.Diagnostics;
    using System.Collections;

    /// <summary>
    /// HtmlSchema class
    /// maintains static information about HTML structure
    /// can be used by HtmlParser to check conditions under which an element starts or ends, etc.
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
        /// static constructor, initializes the ArrayLists
        /// that hold the elements in various sub-components of the schema
        /// e.g _htmlEmptyElements, etc.
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
        /// returns true when xmlElementName corresponds to empty element 
        /// </summary>
        /// <param name="xmlElementName">
        /// string representing name to test
        /// </param>
        internal static bool IsEmptyElement(string xmlElementName) => _htmlEmptyElements.Contains(xmlElementName.ToLower());

        /// <summary>
        /// returns true if xmlElementName represents a block formattinng element.
        /// It used in an algorithm of transferring inline elements over block elements
        /// in HtmlParser
        /// </summary>
        /// <param name="xmlElementName"></param>
        /// <returns></returns>
        internal static bool IsBlockElement(string xmlElementName) => _htmlBlockElements.Contains(xmlElementName);

        /// <summary>
        /// returns true if the xmlElementName represents an inline formatting element
        /// </summary>
        /// <param name="xmlElementName"></param>
        /// <returns></returns>
        internal static bool IsInlineElement(string xmlElementName) => _htmlInlineElements.Contains(xmlElementName);

        /// <summary>
        /// It is a list of known html elements which we
        /// want to allow to produce bt HTML parser,
        /// but don'tt want to act as inline, block or no-scope.
        /// Presence in this list will allow to open
        /// elements during html parsing, and adding the
        /// to a tree produced by html parser.
        /// </summary>
        internal static bool IsKnownOpenableElement(string xmlElementName) => _htmlOtherOpenableElements.Contains(xmlElementName);

        /// <summary>
        /// returns true when xmlElementName closes when the outer element closes
        /// this is true of elements with optional start tags
        /// </summary>
        /// <param name="xmlElementName">
        /// string representing name to test
        /// </param>
        internal static bool ClosesOnParentElementEnd(string xmlElementName) => _htmlElementsClosingOnParentElementEnd.Contains(xmlElementName.ToLower());

        /// <summary>
        /// returns true if the current element closes when the new element, whose name has just been read, starts
        /// </summary>
        /// <param name="currentElementName">
        /// string representing current element name
        /// </param>
        /// <param name="elementName"></param>
        /// string representing name of the next element that will start
        internal static bool ClosesOnNextElementStart(string currentElementName, string nextElementName)
        {
            Debug.Assert(currentElementName == currentElementName.ToLower());
            switch (currentElementName)
            {
                case "colgroup":
                    return _htmlElementsClosingColgroup.Contains(nextElementName) && HtmlSchema.IsBlockElement(nextElementName);
                case "dd":
                    return _htmlElementsClosingDD.Contains(nextElementName) && HtmlSchema.IsBlockElement(nextElementName);
                case "dt":
                    return _htmlElementsClosingDT.Contains(nextElementName) && HtmlSchema.IsBlockElement(nextElementName);
                case "li":
                    return _htmlElementsClosingLI.Contains(nextElementName);
                case "p":
                    return HtmlSchema.IsBlockElement(nextElementName);
                case "tbody":
                    return _htmlElementsClosingTbody.Contains(nextElementName);
                case "tfoot":
                    return _htmlElementsClosingTfoot.Contains(nextElementName);
                case "thead":
                    return _htmlElementsClosingThead.Contains(nextElementName);
                case "tr":
                    return _htmlElementsClosingTR.Contains(nextElementName);
                case "td":
                    return _htmlElementsClosingTD.Contains(nextElementName);
                case "th":
                    return _htmlElementsClosingTH.Contains(nextElementName);
            }
            return false;
        }

        /// <summary>
        /// returns true if the string passed as argument is an Html entity name
        /// </summary>
        /// <param name="entityName">
        /// string to be tested for Html entity name 
        /// </param>
        internal static bool IsEntity(string entityName)
        {
            // we do not convert entity strings to lowercase because these names are case-sensitive
            if (_htmlCharacterEntities.Contains(entityName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// returns the character represented by the entity name string which is passed as an argument, if the string is an entity name
        /// as specified in _htmlCharacterEntities, returns the character value of 0 otherwise 
        /// </summary>
        /// <param name="entityName">
        /// string representing entity name whose character value is desired
        /// </param>
        internal static char EntityCharacterValue(string entityName)
        {
            if (_htmlCharacterEntities.Contains(entityName))
            {
                return (char) _htmlCharacterEntities[entityName];
            }
            else
            {
                return (char)0;
            }
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
            _htmlInlineElements = new ArrayList();
            _htmlInlineElements.Add("a");
            _htmlInlineElements.Add("abbr");
            _htmlInlineElements.Add("acronym");
            _htmlInlineElements.Add("address");
            _htmlInlineElements.Add("b");
            _htmlInlineElements.Add("bdo"); // ???
            _htmlInlineElements.Add("big");
            _htmlInlineElements.Add("button");
            _htmlInlineElements.Add("code");
            _htmlInlineElements.Add("del"); // deleted text
            _htmlInlineElements.Add("dfn");
            _htmlInlineElements.Add("em");
            _htmlInlineElements.Add("font");
            _htmlInlineElements.Add("i");
            _htmlInlineElements.Add("ins"); // inserted text
            _htmlInlineElements.Add("kbd"); // text to entered by a user
            _htmlInlineElements.Add("label");
            _htmlInlineElements.Add("legend"); // ???
            _htmlInlineElements.Add("q"); // short inline quotation
            _htmlInlineElements.Add("s"); // strike-through text style
            _htmlInlineElements.Add("samp"); // Specifies a code sample
            _htmlInlineElements.Add("small");
            _htmlInlineElements.Add("span");
            _htmlInlineElements.Add("strike");
            _htmlInlineElements.Add("strong");
            _htmlInlineElements.Add("sub");
            _htmlInlineElements.Add("sup");
            _htmlInlineElements.Add("u");
            _htmlInlineElements.Add("var"); // indicates an instance of a program variable
        }

        private static void InitializeBlockElements()
        {
            _htmlBlockElements = new ArrayList();

            _htmlBlockElements.Add("blockquote");
            _htmlBlockElements.Add("body");
            _htmlBlockElements.Add("caption");
            _htmlBlockElements.Add("center");
            _htmlBlockElements.Add("cite");
            _htmlBlockElements.Add("dd");
            _htmlBlockElements.Add("dir"); // TODO: treat as UL element
            _htmlBlockElements.Add("div");
            _htmlBlockElements.Add("dl");
            _htmlBlockElements.Add("dt");
            _htmlBlockElements.Add("form"); // Not a block according to XHTML spec
            _htmlBlockElements.Add("h1");
            _htmlBlockElements.Add("h2");
            _htmlBlockElements.Add("h3");
            _htmlBlockElements.Add("h4");
            _htmlBlockElements.Add("h5");
            _htmlBlockElements.Add("h6");
            _htmlBlockElements.Add("html");
            _htmlBlockElements.Add("li");
            _htmlBlockElements.Add("menu"); // TODO: treat as UL element
            _htmlBlockElements.Add("ol");
            _htmlBlockElements.Add("p");
            _htmlBlockElements.Add("pre"); // Renders text in a fixed-width font
            _htmlBlockElements.Add("table");
            _htmlBlockElements.Add("tbody");
            _htmlBlockElements.Add("td");
            _htmlBlockElements.Add("textarea");
            _htmlBlockElements.Add("tfoot");
            _htmlBlockElements.Add("th");
            _htmlBlockElements.Add("thead");
            _htmlBlockElements.Add("tr");
            _htmlBlockElements.Add("tt");
            _htmlBlockElements.Add("ul");
        }

        /// <summary>
        /// initializes _htmlEmptyElements with empty elements in HTML 4 spec at
        /// http://www.w3.org/TR/REC-html40/index/elements.html
        /// </summary>
        private static void InitializeEmptyElements()
        {
            // Build a list of empty (no-scope) elements 
            // (element not requiring closing tags, and not accepting any content)
            _htmlEmptyElements = new ArrayList();
            _htmlEmptyElements.Add("area");
            _htmlEmptyElements.Add("base");
            _htmlEmptyElements.Add("basefont");
            _htmlEmptyElements.Add("br");
            _htmlEmptyElements.Add("col");
            _htmlEmptyElements.Add("frame");
            _htmlEmptyElements.Add("hr");
            _htmlEmptyElements.Add("img");
            _htmlEmptyElements.Add("input");
            _htmlEmptyElements.Add("isindex");
            _htmlEmptyElements.Add("link");
            _htmlEmptyElements.Add("meta");
            _htmlEmptyElements.Add("param");
        }
        
        private static void InitializeOtherOpenableElements()
        {
            // It is a list of known html elements which we
            // want to allow to produce bt HTML parser,
            // but don'tt want to act as inline, block or no-scope.
            // Presence in this list will allow to open
            // elements during html parsing, and adding the
            // to a tree produced by html parser.
            _htmlOtherOpenableElements = new ArrayList();
            _htmlOtherOpenableElements.Add("applet");
            _htmlOtherOpenableElements.Add("base");
            _htmlOtherOpenableElements.Add("basefont");
            _htmlOtherOpenableElements.Add("colgroup");
            _htmlOtherOpenableElements.Add("fieldset");
            //_htmlOtherOpenableElements.Add("form"); --> treated as block
            _htmlOtherOpenableElements.Add("frameset");
            _htmlOtherOpenableElements.Add("head");
            _htmlOtherOpenableElements.Add("iframe");
            _htmlOtherOpenableElements.Add("map");
            _htmlOtherOpenableElements.Add("noframes");
            _htmlOtherOpenableElements.Add("noscript");
            _htmlOtherOpenableElements.Add("object");
            _htmlOtherOpenableElements.Add("optgroup");
            _htmlOtherOpenableElements.Add("option");
            _htmlOtherOpenableElements.Add("script");
            _htmlOtherOpenableElements.Add("select");
            _htmlOtherOpenableElements.Add("style");
            _htmlOtherOpenableElements.Add("title");
        }

        /// <summary>
        /// initializes _htmlElementsClosingOnParentElementEnd with the list of HTML 4 elements for which closing tags are optional
        /// we assume that for any element for which closing tags are optional, the element closes when it's outer element
        /// (in which it is nested) does
        /// </summary>
        private static void InitializeElementsClosingOnParentElementEnd()
        {
            _htmlElementsClosingOnParentElementEnd = new ArrayList();
            _htmlElementsClosingOnParentElementEnd.Add("body");
            _htmlElementsClosingOnParentElementEnd.Add("colgroup");
            _htmlElementsClosingOnParentElementEnd.Add("dd");
            _htmlElementsClosingOnParentElementEnd.Add("dt");
            _htmlElementsClosingOnParentElementEnd.Add("head");
            _htmlElementsClosingOnParentElementEnd.Add("html");
            _htmlElementsClosingOnParentElementEnd.Add("li");
            _htmlElementsClosingOnParentElementEnd.Add("p");
            _htmlElementsClosingOnParentElementEnd.Add("tbody");
            _htmlElementsClosingOnParentElementEnd.Add("td");
            _htmlElementsClosingOnParentElementEnd.Add("tfoot");
            _htmlElementsClosingOnParentElementEnd.Add("thead");
            _htmlElementsClosingOnParentElementEnd.Add("th");
            _htmlElementsClosingOnParentElementEnd.Add("tr");
        }

        private static void InitializeElementsClosingOnNewElementStart()
        {
            _htmlElementsClosingColgroup = new ArrayList();
            _htmlElementsClosingColgroup.Add("colgroup");
            _htmlElementsClosingColgroup.Add("tr");
            _htmlElementsClosingColgroup.Add("thead");
            _htmlElementsClosingColgroup.Add("tfoot");
            _htmlElementsClosingColgroup.Add("tbody");

            _htmlElementsClosingDD = new ArrayList();
            _htmlElementsClosingDD.Add("dd");
            _htmlElementsClosingDD.Add("dt");
            // TODO: dd may end in other cases as well - if a new "p" starts, etc.
            // TODO: these are the basic "legal" cases but there may be more recovery

            _htmlElementsClosingDT = new ArrayList();
            _htmlElementsClosingDD.Add("dd");
            _htmlElementsClosingDD.Add("dt");
            // TODO: dd may end in other cases as well - if a new "p" starts, etc.
            // TODO: these are the basic "legal" cases but there may be more recovery

            _htmlElementsClosingLI = new ArrayList();
            _htmlElementsClosingLI.Add("li");
            // TODO: more complex recovery

            _htmlElementsClosingTbody = new ArrayList();
            _htmlElementsClosingTbody.Add("tbody");
            _htmlElementsClosingTbody.Add("thead");
            _htmlElementsClosingTbody.Add("tfoot");
            // TODO: more complex recovery

            _htmlElementsClosingTR = new ArrayList();
            // NOTE: tr should not really close on a new thead
            // because if there are rows before a thead, it is assumed to be in tbody, whose start tag is optional
            // and thead can't come after tbody
            // however, if we do encounter this, it's probably best to end the row and ignore thead or treat
            // it as part of the table
            _htmlElementsClosingTR.Add("thead");
            _htmlElementsClosingTR.Add("tfoot");
            _htmlElementsClosingTR.Add("tbody");
            _htmlElementsClosingTR.Add("tr");
            // TODO: more complex recovery

            _htmlElementsClosingTD = new ArrayList();
            _htmlElementsClosingTD.Add("td");
            _htmlElementsClosingTD.Add("th");
            _htmlElementsClosingTD.Add("tr");
            _htmlElementsClosingTD.Add("tbody");
            _htmlElementsClosingTD.Add("tfoot");
            _htmlElementsClosingTD.Add("thead");
            // TODO: more complex recovery

            _htmlElementsClosingTH = new ArrayList();
            _htmlElementsClosingTH.Add("td");
            _htmlElementsClosingTH.Add("th");
            _htmlElementsClosingTH.Add("tr");
            _htmlElementsClosingTH.Add("tbody");
            _htmlElementsClosingTH.Add("tfoot");
            _htmlElementsClosingTH.Add("thead");
            // TODO: more complex recovery

            _htmlElementsClosingThead = new ArrayList();
            _htmlElementsClosingThead.Add("tbody");
            _htmlElementsClosingThead.Add("tfoot");
            // TODO: more complex recovery

            _htmlElementsClosingTfoot = new ArrayList();
            _htmlElementsClosingTfoot.Add("tbody");
            // although thead comes before tfoot, we add it because if it is found the tfoot should close
            // and some recovery processing be done on thead
            _htmlElementsClosingTfoot.Add("thead");
            // TODO: more complex recovery
        }

        /// <summary>
        /// initializes _htmlCharacterEntities hashtable with the character corresponding to entity names
        /// </summary>
        private static void InitializeHtmlCharacterEntities()
        {
            _htmlCharacterEntities = new Hashtable();
            _htmlCharacterEntities["Aacute"] = (char)193;
            _htmlCharacterEntities["aacute"] = (char)225;
            _htmlCharacterEntities["Acirc"] = (char)194;
            _htmlCharacterEntities["acirc"] = (char)226;
            _htmlCharacterEntities["acute"] = (char)180;
            _htmlCharacterEntities["AElig"] = (char)198;
            _htmlCharacterEntities["aelig"] = (char)230;
            _htmlCharacterEntities["Agrave"] = (char)192;
            _htmlCharacterEntities["agrave"] = (char)224;
            _htmlCharacterEntities["alefsym"] = (char)8501;
            _htmlCharacterEntities["Alpha"] = (char)913;
            _htmlCharacterEntities["alpha"] = (char)945;
            _htmlCharacterEntities["amp"] = (char)38;
            _htmlCharacterEntities["and"] = (char)8743;
            _htmlCharacterEntities["ang"] = (char)8736;
            _htmlCharacterEntities["Aring"] = (char)197;
            _htmlCharacterEntities["aring"] = (char)229;
            _htmlCharacterEntities["asymp"] = (char)8776;
            _htmlCharacterEntities["Atilde"] = (char)195;
            _htmlCharacterEntities["atilde"] = (char)227;
            _htmlCharacterEntities["Auml"] = (char)196;
            _htmlCharacterEntities["auml"] = (char)228;
            _htmlCharacterEntities["bdquo"] = (char)8222;
            _htmlCharacterEntities["Beta"] = (char)914;
            _htmlCharacterEntities["beta"] = (char)946;
            _htmlCharacterEntities["brvbar"] = (char)166;
            _htmlCharacterEntities["bull"] = (char)8226;
            _htmlCharacterEntities["cap"] = (char)8745;
            _htmlCharacterEntities["Ccedil"] = (char)199;
            _htmlCharacterEntities["ccedil"] = (char)231;
            _htmlCharacterEntities["cent"] = (char)162;
            _htmlCharacterEntities["Chi"] = (char)935;
            _htmlCharacterEntities["chi"] = (char)967;
            _htmlCharacterEntities["circ"] = (char)710;
            _htmlCharacterEntities["clubs"] = (char)9827;
            _htmlCharacterEntities["cong"] = (char)8773;
            _htmlCharacterEntities["copy"] = (char)169;
            _htmlCharacterEntities["crarr"] = (char)8629;
            _htmlCharacterEntities["cup"] = (char)8746;
            _htmlCharacterEntities["curren"] = (char)164;
            _htmlCharacterEntities["dagger"] = (char)8224;
            _htmlCharacterEntities["Dagger"] = (char)8225;
            _htmlCharacterEntities["darr"] = (char)8595;
            _htmlCharacterEntities["dArr"] = (char)8659;
            _htmlCharacterEntities["deg"] = (char)176;
            _htmlCharacterEntities["Delta"] = (char)916;
            _htmlCharacterEntities["delta"] = (char)948;
            _htmlCharacterEntities["diams"] = (char)9830;
            _htmlCharacterEntities["divide"] = (char)247;
            _htmlCharacterEntities["Eacute"] = (char)201;
            _htmlCharacterEntities["eacute"] = (char)233;
            _htmlCharacterEntities["Ecirc"] = (char)202;
            _htmlCharacterEntities["ecirc"] = (char)234;
            _htmlCharacterEntities["Egrave"] = (char)200;
            _htmlCharacterEntities["egrave"] = (char)232;
            _htmlCharacterEntities["empty"] = (char)8709;
            _htmlCharacterEntities["emsp"] = (char)8195;
            _htmlCharacterEntities["ensp"] = (char)8194;
            _htmlCharacterEntities["Epsilon"] = (char)917;
            _htmlCharacterEntities["epsilon"] = (char)949;
            _htmlCharacterEntities["equiv"] = (char)8801;
            _htmlCharacterEntities["Eta"] = (char)919;
            _htmlCharacterEntities["eta"] = (char)951;
            _htmlCharacterEntities["ETH"] = (char)208;
            _htmlCharacterEntities["eth"] = (char)240;
            _htmlCharacterEntities["Euml"] = (char)203;
            _htmlCharacterEntities["euml"] = (char)235;
            _htmlCharacterEntities["euro"] = (char)8364;
            _htmlCharacterEntities["exist"] = (char)8707;
            _htmlCharacterEntities["fnof"] = (char)402;
            _htmlCharacterEntities["forall"] = (char)8704;
            _htmlCharacterEntities["frac12"] = (char)189;
            _htmlCharacterEntities["frac14"] = (char)188;
            _htmlCharacterEntities["frac34"] = (char)190;
            _htmlCharacterEntities["frasl"] = (char)8260;
            _htmlCharacterEntities["Gamma"] = (char)915;
            _htmlCharacterEntities["gamma"] = (char)947;
            _htmlCharacterEntities["ge"] = (char)8805;
            _htmlCharacterEntities["gt"] = (char)62;
            _htmlCharacterEntities["harr"] = (char)8596;
            _htmlCharacterEntities["hArr"] = (char)8660;
            _htmlCharacterEntities["hearts"] = (char)9829;
            _htmlCharacterEntities["hellip"] = (char)8230;
            _htmlCharacterEntities["Iacute"] = (char)205;
            _htmlCharacterEntities["iacute"] = (char)237;
            _htmlCharacterEntities["Icirc"] = (char)206;
            _htmlCharacterEntities["icirc"] = (char)238;
            _htmlCharacterEntities["iexcl"] = (char)161;
            _htmlCharacterEntities["Igrave"] = (char)204;
            _htmlCharacterEntities["igrave"] = (char)236;
            _htmlCharacterEntities["image"] = (char)8465;
            _htmlCharacterEntities["infin"] = (char)8734;
            _htmlCharacterEntities["int"] = (char)8747;
            _htmlCharacterEntities["Iota"] = (char)921;
            _htmlCharacterEntities["iota"] = (char)953;
            _htmlCharacterEntities["iquest"] = (char)191;
            _htmlCharacterEntities["isin"] = (char)8712;
            _htmlCharacterEntities["Iuml"] = (char)207;
            _htmlCharacterEntities["iuml"] = (char)239;
            _htmlCharacterEntities["Kappa"] = (char)922;
            _htmlCharacterEntities["kappa"] = (char)954;
            _htmlCharacterEntities["Lambda"] = (char)923;
            _htmlCharacterEntities["lambda"] = (char)955;
            _htmlCharacterEntities["lang"] = (char)9001;
            _htmlCharacterEntities["laquo"] = (char)171;
            _htmlCharacterEntities["larr"] = (char)8592;
            _htmlCharacterEntities["lArr"] = (char)8656;
            _htmlCharacterEntities["lceil"] = (char)8968;
            _htmlCharacterEntities["ldquo"] = (char)8220;
            _htmlCharacterEntities["le"] = (char)8804;
            _htmlCharacterEntities["lfloor"] = (char)8970;
            _htmlCharacterEntities["lowast"] = (char)8727;
            _htmlCharacterEntities["loz"] = (char)9674;
            _htmlCharacterEntities["lrm"] = (char)8206;
            _htmlCharacterEntities["lsaquo"] = (char)8249;
            _htmlCharacterEntities["lsquo"] = (char)8216;
            _htmlCharacterEntities["lt"] = (char)60;
            _htmlCharacterEntities["macr"] = (char)175;
            _htmlCharacterEntities["mdash"] = (char)8212;
            _htmlCharacterEntities["micro"] = (char)181;
            _htmlCharacterEntities["middot"] = (char)183;
            _htmlCharacterEntities["minus"] = (char)8722;
            _htmlCharacterEntities["Mu"] = (char)924;
            _htmlCharacterEntities["mu"] = (char)956;
            _htmlCharacterEntities["nabla"] = (char)8711;
            _htmlCharacterEntities["nbsp"] = (char)160;
            _htmlCharacterEntities["ndash"] = (char)8211;
            _htmlCharacterEntities["ne"] = (char)8800;
            _htmlCharacterEntities["ni"] = (char)8715;
            _htmlCharacterEntities["not"] = (char)172;
            _htmlCharacterEntities["notin"] = (char)8713;
            _htmlCharacterEntities["nsub"] = (char)8836;
            _htmlCharacterEntities["Ntilde"] = (char)209;
            _htmlCharacterEntities["ntilde"] = (char)241;
            _htmlCharacterEntities["Nu"] = (char)925;
            _htmlCharacterEntities["nu"] = (char)957;
            _htmlCharacterEntities["Oacute"] = (char)211;
            _htmlCharacterEntities["ocirc"] = (char)244;
            _htmlCharacterEntities["OElig"] = (char)338;
            _htmlCharacterEntities["oelig"] = (char)339;
            _htmlCharacterEntities["Ograve"] = (char)210;
            _htmlCharacterEntities["ograve"] = (char)242;
            _htmlCharacterEntities["oline"] = (char)8254;
            _htmlCharacterEntities["Omega"] = (char)937;
            _htmlCharacterEntities["omega"] = (char)969;
            _htmlCharacterEntities["Omicron"] = (char)927;
            _htmlCharacterEntities["omicron"] = (char)959;
            _htmlCharacterEntities["oplus"] = (char)8853;
            _htmlCharacterEntities["or"] = (char)8744;
            _htmlCharacterEntities["ordf"] = (char)170;
            _htmlCharacterEntities["ordm"] = (char)186;
            _htmlCharacterEntities["Oslash"] = (char)216;
            _htmlCharacterEntities["oslash"] = (char)248;
            _htmlCharacterEntities["Otilde"] = (char)213;
            _htmlCharacterEntities["otilde"] = (char)245;
            _htmlCharacterEntities["otimes"] = (char)8855;
            _htmlCharacterEntities["Ouml"] = (char)214;
            _htmlCharacterEntities["ouml"] = (char)246;
            _htmlCharacterEntities["para"] = (char)182;
            _htmlCharacterEntities["part"] = (char)8706;
            _htmlCharacterEntities["permil"] = (char)8240;
            _htmlCharacterEntities["perp"] = (char)8869;
            _htmlCharacterEntities["Phi"] = (char)934;
            _htmlCharacterEntities["phi"] = (char)966;
            _htmlCharacterEntities["pi"] = (char)960;
            _htmlCharacterEntities["piv"] = (char)982;
            _htmlCharacterEntities["plusmn"] = (char)177;
            _htmlCharacterEntities["pound"] = (char)163;
            _htmlCharacterEntities["prime"] = (char)8242;
            _htmlCharacterEntities["Prime"] = (char)8243;
            _htmlCharacterEntities["prod"] = (char)8719;
            _htmlCharacterEntities["prop"] = (char)8733;
            _htmlCharacterEntities["Psi"] = (char)936;
            _htmlCharacterEntities["psi"] = (char)968;
            _htmlCharacterEntities["quot"] = (char)34;
            _htmlCharacterEntities["radic"] = (char)8730;
            _htmlCharacterEntities["rang"] = (char)9002;
            _htmlCharacterEntities["raquo"] = (char)187;
            _htmlCharacterEntities["rarr"] = (char)8594;
            _htmlCharacterEntities["rArr"] = (char)8658;
            _htmlCharacterEntities["rceil"] = (char)8969;
            _htmlCharacterEntities["rdquo"] = (char)8221;
            _htmlCharacterEntities["real"] = (char)8476;
            _htmlCharacterEntities["reg"] = (char)174;
            _htmlCharacterEntities["rfloor"] = (char)8971;
            _htmlCharacterEntities["Rho"] = (char)929;
            _htmlCharacterEntities["rho"] = (char)961;
            _htmlCharacterEntities["rlm"] = (char)8207;
            _htmlCharacterEntities["rsaquo"] = (char)8250;
            _htmlCharacterEntities["rsquo"] = (char)8217;
            _htmlCharacterEntities["sbquo"] = (char)8218;
            _htmlCharacterEntities["Scaron"] = (char)352;
            _htmlCharacterEntities["scaron"] = (char)353;
            _htmlCharacterEntities["sdot"] = (char)8901;
            _htmlCharacterEntities["sect"] = (char)167;
            _htmlCharacterEntities["shy"] = (char)173;
            _htmlCharacterEntities["Sigma"] = (char)931;
            _htmlCharacterEntities["sigma"] = (char)963;
            _htmlCharacterEntities["sigmaf"] = (char)962;
            _htmlCharacterEntities["sim"] = (char)8764;
            _htmlCharacterEntities["spades"] = (char)9824;
            _htmlCharacterEntities["sub"] = (char)8834;
            _htmlCharacterEntities["sube"] = (char)8838;
            _htmlCharacterEntities["sum"] = (char)8721;
            _htmlCharacterEntities["sup"] = (char)8835;
            _htmlCharacterEntities["sup1"] = (char)185;
            _htmlCharacterEntities["sup2"] = (char)178;
            _htmlCharacterEntities["sup3"] = (char)179;
            _htmlCharacterEntities["supe"] = (char)8839;
            _htmlCharacterEntities["szlig"] = (char)223;
            _htmlCharacterEntities["Tau"] = (char)932;
            _htmlCharacterEntities["tau"] = (char)964;
            _htmlCharacterEntities["there4"] = (char)8756;
            _htmlCharacterEntities["Theta"] = (char)920;
            _htmlCharacterEntities["theta"] = (char)952;
            _htmlCharacterEntities["thetasym"] = (char)977;
            _htmlCharacterEntities["thinsp"] = (char)8201;
            _htmlCharacterEntities["THORN"] = (char)222;
            _htmlCharacterEntities["thorn"] = (char)254;
            _htmlCharacterEntities["tilde"] = (char)732;
            _htmlCharacterEntities["times"] = (char)215;
            _htmlCharacterEntities["trade"] = (char)8482;
            _htmlCharacterEntities["Uacute"] = (char)218;
            _htmlCharacterEntities["uacute"] = (char)250;
            _htmlCharacterEntities["uarr"] = (char)8593;
            _htmlCharacterEntities["uArr"] = (char)8657;
            _htmlCharacterEntities["Ucirc"] = (char)219;
            _htmlCharacterEntities["ucirc"] = (char)251;
            _htmlCharacterEntities["Ugrave"] = (char)217;
            _htmlCharacterEntities["ugrave"] = (char)249;
            _htmlCharacterEntities["uml"] = (char)168;
            _htmlCharacterEntities["upsih"] = (char)978;
            _htmlCharacterEntities["Upsilon"] = (char)933;
            _htmlCharacterEntities["upsilon"] = (char)965;
            _htmlCharacterEntities["Uuml"] = (char)220;
            _htmlCharacterEntities["uuml"] = (char)252;
            _htmlCharacterEntities["weierp"] = (char)8472;
            _htmlCharacterEntities["Xi"] = (char)926;
            _htmlCharacterEntities["xi"] = (char)958;
            _htmlCharacterEntities["Yacute"] = (char)221;
            _htmlCharacterEntities["yacute"] = (char)253;
            _htmlCharacterEntities["yen"] = (char)165;
            _htmlCharacterEntities["Yuml"] = (char)376;
            _htmlCharacterEntities["yuml"] = (char)255;
            _htmlCharacterEntities["Zeta"] = (char)918;
            _htmlCharacterEntities["zeta"] = (char)950;
            _htmlCharacterEntities["zwj"] = (char)8205;
            _htmlCharacterEntities["zwnj"] = (char)8204;
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
        private static ArrayList _htmlElementsClosingDD;

        // names of elements closing the dt element
        private static ArrayList _htmlElementsClosingDT;

        // names of elements closing the li element
        private static ArrayList _htmlElementsClosingLI;

        // names of elements closing the tbody element
        private static ArrayList _htmlElementsClosingTbody;

        // names of elements closing the td element
        private static ArrayList _htmlElementsClosingTD;

        // names of elements closing the tfoot element
        private static ArrayList _htmlElementsClosingTfoot;

        // names of elements closing thead element
        private static ArrayList _htmlElementsClosingThead;

        // names of elements closing the th element
        private static ArrayList _htmlElementsClosingTH;

        // names of elements closing the tr element
        private static ArrayList _htmlElementsClosingTR;

        // html character entities hashtable
        private static Hashtable _htmlCharacterEntities;

        #endregion Private Fields
    }
}

