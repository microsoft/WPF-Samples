//////////////////////////////////////////////////////////////////////
//
// WordXmlSerializer.cs
//
// Provides a class to support reading and writing Word 2003 XML
// files.
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

    /// <summary>
    /// Reads and writes WordXML files, interoperating with TextContainer
    /// instances.
    /// </summary>
    static class WordXmlSerializer
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public methods.

        /// <summary>
        /// Determines whether the specified file is a WordML file.
        /// </summary>
        /// <param name='filename'>Name of file to inspect.</param>
        /// <returns>true if the specified file name is WordML; false otherwise.</returns>
        /// <remarks>
        /// This method identifies WordML files as XML files that have
        /// a Word.Document prog-id in a mso-application processing
        /// instruction in its leading nodes.
        /// </remarks>
        public static bool IsFileWordML(string filename)
        {
            const bool detectEncodingTrue = true;

            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            try
            {
                using (TextReader textReader = new StreamReader(filename, detectEncodingTrue))
                using (XmlTextReader xmlReader = new XmlTextReader(textReader))
                {
                    while (xmlReader.Read())
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.ProcessingInstruction:
                                if (xmlReader.Name == "mso-application")
                                {
                                    return xmlReader.Value.Contains("Word.Document");
                                }
                                break;
                            case XmlNodeType.Comment:
                            case XmlNodeType.DocumentType:
                            case XmlNodeType.Notation:
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.Whitespace:
                                continue;
                            default:
                                return false;
                        }
                    }
                    return false;
                }
            }
            catch(XmlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Saves the container of the specified range into the given
        /// file.
        /// </summary>
        /// <param name='filename'>Name of file to save to.</param>
        /// <param name='range'>Range from container to save.</param>
        public static void SaveToFile(string filename, TextPointer start, TextPointer end)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }
            if (end == null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            // Do a programmatic serialization of the container.
            using (XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.Unicode))
            {
                // Set up formatting options for debugability.
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.IndentChar = ' ';

                new WordXmlWriter().Write(start, end, writer);
            }
        }

        /// <summary>
        /// Loads the specified file into the given container.
        /// </summary>
        /// <param name='filename'>Name of file to load content from..</param>
        /// <param name='position'>Position to load content into.</param>
        public static void LoadFromFile(string filename, TextPointer position)
        {
            const bool detectEncodingTrue = true;

            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            using (TextReader textReader = new StreamReader(filename, detectEncodingTrue))
            using (XmlTextReader xmlReader = new XmlTextReader(textReader))
            {
                new WordXmlReader().Read(xmlReader, position);
            }
        }

        #endregion Public methods.

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

        //------------------------------------------------------
        //
        //  Internal Properties
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Internal Events
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Internal Constants
        //
        //------------------------------------------------------

        #region Internal constants.

        #region WordXML element constants.

        /// <summary>
        /// WordXML element name to indicate the font used by Word to
        /// display text. This saves consumers from having to select
        /// one of of ascii, fareast, h-ansi or cs.
        /// </summary>
        internal const string WordAuxiliaryFontTag = "wx:font";

        /// <summary>WordXML element name to indicate bold formatting.</summary>
        internal const string WordBoldTag = "w:b";

        /// <summary>
        /// WordXML element name to indicate a break in the document (eg:
        /// a line break).
        /// </summary>
        internal const string WordBreakTag = "w:br";

        /// <summary>WordXML element name to indicate text color.</summary>
        internal const string WordColorTag = "w:color";

        /// <summary>WordXML element name to indicate font size usage.</summary>
        internal const string WordFontSizeTag = "w:sz";

        /// <summary>WordXML element name to indicate italic formatting.</summary>
        internal const string WordItalicTag = "w:i";

        /// <summary>WordXML element name to indicate paragraph justification.</summary>
        internal const string WordJustificationTag = "w:jc";

        /// <summary>WordXML element name to indicate a resource name (eg: style name).</summary>
        internal const string WordNameTag = "w:name";

        /// <summary>
        /// WordXML element name to indicate a set of paragraph properties.
        /// </summary>
        internal const string WordParagraphPropertiesTag = "w:pPr";

        /// <summary>WordXML element name to indicate a paragraph.</summary>
        internal const string WordParagraphTag = "w:p";

        /// <summary>
        /// WordXML element name to indicate a set of range properties.
        /// </summary>
        internal const string WordRangePropertiesTag = "w:rPr";

        /// <summary>WordXML element name to indicate a range.</summary>
        internal const string WordRangeTag = "w:r";

        /// <summary>WordXML element name to indicate a style definition.</summary>
        internal const string WordStyleTag = "w:style";

        /// <summary>
        /// WordXML element name to indicate text with similar formatting.
        /// </summary>
        internal const string WordTextTag = "w:t";

        #endregion WordXML element constants.

        #region WordXML attribute constants.

        /// <summary>WordXML attribute value to indicate a default value.</summary>
        internal const string WordAuto = "auto";

        /// <summary>WordXML attribute name to indicate the value for an element.</summary>
        internal const string WordAuxiliaryValue = "wx:val";

        /// <summary>WordXML attribute value to indicate centered content.</summary>
        internal const string WordCenter = "center";

        /// <summary>WordXML attribute value to indicate a style of character type.</summary>
        internal const string WordCharacter = "character";

        /// <summary>WordXML attribute name to indicate whether an element is the default.</summary>
        internal const string WordDefault = "w:default";

        /// <summary>WordXML attribute value to indicate left-aligned content.</summary>
        internal const string WordLeft = "left";

        /// <summary>WordXML attribute value to indicate a style of list type.</summary>
        internal const string WordList = "list";

        /// <summary>WordXML attribute value to indicate a property is unused.</summary>
        internal const string WordOff = "off";

        /// <summary>WordXML attribute value to indicate a property is used.</summary>
        internal const string WordOn = "on";

        /// <summary>WordXML attribute value to indicate a style o fparagraph type.</summary>
        internal const string WordParagraph = "paragraph";

        /// <summary>WordXML attribute value to indicate right-aligned content.</summary>
        internal const string WordRight = "right";

        /// <summary>WordXML attribute name to indicate the style ID for an style.</summary>
        internal const string WordStyleId = "w:styleId";

        /// <summary>WordXML attribute value to indicate a style of table type.</summary>
        internal const string WordTable = "table";

        /// <summary>WordXML attribute name to indicate the type of a style.</summary>
        internal const string WordType = "w:type";

        /// <summary>WordXML attribute name to indicate the value for an element.</summary>
        internal const string WordValue = "w:val";

        #endregion WordXML attribute constants.

        #endregion Internal constants.

        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Private Properties
        //
        //------------------------------------------------------

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

    }
}
