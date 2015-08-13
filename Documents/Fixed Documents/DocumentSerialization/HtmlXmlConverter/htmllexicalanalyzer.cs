//---------------------------------------------------------------------------
// 
// File: HtmlLexicalAnalyzer.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Lexical analyzer for Html-to-Xaml converter
//
//---------------------------------------------------------------------------

using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Text;

namespace DocumentSerialization
{
    /// <summary>
    /// lexical analyzer class
    /// recognizes tokens as groups of characters separated by arbitrary amounts of whitespace
    /// also classifies tokens according to type
    /// </summary>
    internal class HtmlLexicalAnalyzer
    {
        // ---------------------------------------------------------------------
        //
        // Constructors
        //
        // ---------------------------------------------------------------------

        #region Constructors

        /// <summary>
        /// initializes the _inputStringReader member with the string to be read
        /// also sets initial values for _nextCharacterCode and _nextTokenType
        /// </summary>
        /// <param name="inputTextString">
        /// text string to be parsed for xml content
        /// </param>
        internal HtmlLexicalAnalyzer(string inputTextString)
        {
            _inputStringReader = new StringReader(inputTextString);
            _nextCharacterCode = 0;
            _nextCharacter = ' ';
            _lookAheadCharacterCode = _inputStringReader.Read();
            _lookAheadCharacter = (char)_lookAheadCharacterCode;
            _previousCharacter = ' ';
            _ignoreNextWhitespace = true;
            _nextToken = new StringBuilder(100);
            _nextTokenType = HtmlTokenType.Text;
            // read the first character so we have some value for the NextCharacter property
            this.GetNextCharacter();
        }

        #endregion Constructors

        // ---------------------------------------------------------------------
        //
        // Internal methods
        //
        // ---------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        /// retrieves next recognizable token from input string 
        /// and identifies its type
        /// if no valid token is found, the output parameters are set to null
        /// if end of stream is reached without matching any token, token type
        /// paramter is set to EOF
        /// </summary>
        internal void GetNextContentToken()
        {
            Debug.Assert(_nextTokenType != HtmlTokenType.EOF);
            _nextToken.Length = 0;
            if (this.IsAtEndOfStream)
            {
                _nextTokenType = HtmlTokenType.EOF;
                return;
            }

            if (this.IsAtTagStart)
            {
                this.GetNextCharacter();

                if (this.NextCharacter == '/')
                {
                    _nextToken.Append("</");
                    _nextTokenType = HtmlTokenType.ClosingTagStart;

                    // advance
                    this.GetNextCharacter();
                    _ignoreNextWhitespace = false; // Whitespaces after closing tags are significant
                }
                else
                {
                    _nextTokenType = HtmlTokenType.OpeningTagStart;
                    _nextToken.Append("<");
                    _ignoreNextWhitespace = true; // Whitespaces after opening tags are insignificant
                }
            }
            else if (this.IsAtDirectiveStart)
            {
                // either a comment or CDATA
                this.GetNextCharacter();
                if (_lookAheadCharacter == '[')
                {
                    // cdata
                    this.ReadDynamicContent();
                }
                else if (_lookAheadCharacter == '-')
                {
                    this.ReadComment();
                }
                else
                {
                    // neither a comment nor cdata, should be something like DOCTYPE
                    // skip till the next tag ender
                    this.ReadUnknownDirective();
                }
            }
            else
            {
                // read text content, unless you encounter a tag
                _nextTokenType = HtmlTokenType.Text;
                while (!this.IsAtTagStart && !this.IsAtEndOfStream && !this.IsAtDirectiveStart)
                {
                    if (this.NextCharacter == '<' && !this.IsNextCharacterEntity && _lookAheadCharacter == '?')
                    {
                        // ignore processing directive
                        this.SkipProcessingDirective();
                    }
                    else
                    {
                        if (this.NextCharacter <= ' ')
                        {
                            // TODO: Respect xml:preserve or its equivalents for whitespace processing
                            if (_ignoreNextWhitespace)
                            {
                                // Ignore repeated whitespaces
                            }
                            else
                            {
                                // Treat any control character sequence as one whitespace
                                _nextToken.Append(' ');
                            }
                            _ignoreNextWhitespace = true; // and keep ignoring the following whitespaces
                        }
                        else
                        {
                            _nextToken.Append(this.NextCharacter);
                            _ignoreNextWhitespace = false;
                        }
                        this.GetNextCharacter();
                    }
                }
            }
        }

        /// <summary>
        /// Unconditionally returns a token which is one of: TagEnd, EmptyTagEnd, Name, Atom or EndOfStream
        /// Does not guarantee token reader advancing.
        /// </summary>
        internal void GetNextTagToken()
        {
            _nextToken.Length = 0;
            if (this.IsAtEndOfStream)
            {
                _nextTokenType = HtmlTokenType.EOF;
                return;
            }

            this.SkipWhiteSpace();

            if (this.NextCharacter == '>' && !this.IsNextCharacterEntity)
            {
                // &gt; should not end a tag, so make sure it's not an entity
                _nextTokenType = HtmlTokenType.TagEnd;
                _nextToken.Append('>');
                this.GetNextCharacter();
                // Note: _ignoreNextWhitespace must be set appropriately on tag start processing
            }
            else if (this.NextCharacter == '/' && _lookAheadCharacter == '>')
            {
                // could be start of closing of empty tag
                _nextTokenType = HtmlTokenType.EmptyTagEnd;
                _nextToken.Append("/>");
                this.GetNextCharacter();
                this.GetNextCharacter();
                _ignoreNextWhitespace = false; // Whitespace after no-scope tags are sifnificant
            }
            else if (IsGoodForNameStart(this.NextCharacter))
            {
                _nextTokenType = HtmlTokenType.Name;

                // starts a name
                // we allow character entities here
                // we do not throw exceptions here if end of stream is encountered
                // just stop and return whatever is in the token
                // if the parser is not expecting end of file after this it will call
                // the get next token function and throw an exception
                while (IsGoodForName(this.NextCharacter) && !this.IsAtEndOfStream)
                {
                    _nextToken.Append(this.NextCharacter);
                    this.GetNextCharacter();
                }
            }
            else
            {
                // Unexpected type of token for a tag. Reprot one character as Atom, expecting that HtmlParser will ignore it.
                _nextTokenType = HtmlTokenType.Atom;
                _nextToken.Append(this.NextCharacter);
                this.GetNextCharacter();
            }
        }

        /// <summary>
        /// Unconditionally returns equal sign token. Even if there is no
        /// real equal sign in the stream, it behaves as if it were there.
        /// Does not guarantee token reader advancing.
        /// </summary>
        internal void GetNextEqualSignToken()
        {
            Debug.Assert(_nextTokenType != HtmlTokenType.EOF);
            _nextToken.Length = 0;

            _nextToken.Append('=');
            _nextTokenType = HtmlTokenType.EqualSign;

            this.SkipWhiteSpace();

            if (this.NextCharacter == '=')
            {
                // '=' is not in the list of entities, so no need to check for entities here
                this.GetNextCharacter();
            }
        }

        /// <summary>
        /// Unconditionally returns an atomic value for an attribute
        /// Even if there is no appropriate token it returns Atom value
        /// Does not guarantee token reader advancing.
        /// </summary>
        internal void GetNextAtomToken()
        {
            Debug.Assert(_nextTokenType != HtmlTokenType.EOF);
            _nextToken.Length = 0;

            this.SkipWhiteSpace();

            _nextTokenType = HtmlTokenType.Atom;

            if ((this.NextCharacter == '\'' || this.NextCharacter == '"') && !this.IsNextCharacterEntity)
            {
                char startingQuote = this.NextCharacter;
                this.GetNextCharacter();

                // Consume all characters between quotes
                while (!(this.NextCharacter == startingQuote && !this.IsNextCharacterEntity) && !this.IsAtEndOfStream)
                {
                    _nextToken.Append(this.NextCharacter);
                    this.GetNextCharacter();
                }
                if (this.NextCharacter == startingQuote)
                {
                    this.GetNextCharacter();
                }

                // complete the quoted value
                // NOTE: our recovery here is different from IE's
                // IE keeps reading until it finds a closing quote or end of file
                // if end of file, it treats current value as text
                // if it finds a closing quote at any point within the text, it eats everything between the quotes
                // TODO: Suggestion:
                // however, we could stop when we encounter end of file or an angle bracket of any kind
                // and assume there was a quote there
                // so the attribute value may be meaningless but it is never treated as text
            }
            else
            {
                while (!this.IsAtEndOfStream && !Char.IsWhiteSpace(this.NextCharacter) && this.NextCharacter != '>')
                {
                    _nextToken.Append(this.NextCharacter);
                    this.GetNextCharacter();
                }
            }
        }

        #endregion Internal Methods

        // ---------------------------------------------------------------------
        //
        // Internal Properties
        //
        // ---------------------------------------------------------------------

        #region Internal Properties

        internal HtmlTokenType NextTokenType => _nextTokenType;

        internal string NextToken => _nextToken.ToString();

        #endregion Internal Properties

        // ---------------------------------------------------------------------
        //
        // Private methods
        //
        // ---------------------------------------------------------------------

        #region Private Methods

        /// <summary>
        /// Advances a reading position by one character code
        /// and reads the next availbale character from a stream.
        /// This character becomes available as NextCharacter property.
        /// </summary>
        /// <remarks>
        /// Throws InvalidOperationException if attempted to be called on EndOfStream
        /// condition.
        /// </remarks>
        private void GetNextCharacter()
        {
            if (_nextCharacterCode == -1)
            {
                throw new InvalidOperationException("GetNextCharacter method called at the end of a stream");
            }

            _previousCharacter = _nextCharacter;

            _nextCharacter = _lookAheadCharacter;
            _nextCharacterCode = _lookAheadCharacterCode;
            // next character not an entity as of now
            _isNextCharacterEntity = false;

            this.ReadLookAheadCharacter();

            if (_nextCharacter == '&')
            {
                if (_lookAheadCharacter == '#')
                {
                    // numeric entity - parse digits - &#DDDDD;
                    int entityCode;
                    entityCode = 0;
                    this.ReadLookAheadCharacter();

                    // largest numeric entity is 7 characters
                    for (int i = 0; i < 7 && Char.IsDigit(_lookAheadCharacter); i++)
                    {
                        entityCode = 10 * entityCode + (_lookAheadCharacterCode - (int)'0');
                        this.ReadLookAheadCharacter();
                    }
                    if (_lookAheadCharacter == ';')
                    {
                        // correct format - advance
                        this.ReadLookAheadCharacter();
                        _nextCharacterCode = entityCode;

                        // if this is out of range it will set the character to '?'
                        _nextCharacter = (char)_nextCharacterCode;

                        // as far as we are concerned, this is an entity
                        _isNextCharacterEntity = true;
                    }
                    else
                    {
                        // not an entity, set next character to the current lookahread character
                        // we would have eaten up some digits
                        _nextCharacter = _lookAheadCharacter;
                        _nextCharacterCode = _lookAheadCharacterCode;
                        this.ReadLookAheadCharacter();
                        _isNextCharacterEntity = false;
                    }
                }
                else if (Char.IsLetter(_lookAheadCharacter))
                {
                    // entity is written as a string
                    string entity = "";

                    // maximum length of string entities is 10 characters
                    for (int i = 0; i < 10 && (Char.IsLetter(_lookAheadCharacter) || Char.IsDigit(_lookAheadCharacter)); i++)
                    {
                        entity += _lookAheadCharacter;
                        this.ReadLookAheadCharacter();
                    }
                    if (_lookAheadCharacter == ';')
                    {
                        // advance
                        this.ReadLookAheadCharacter();

                        if (HtmlSchema.IsEntity(entity))
                        {
                            _nextCharacter = HtmlSchema.EntityCharacterValue(entity);
                            _nextCharacterCode = (int)_nextCharacter;
                            _isNextCharacterEntity = true;
                        }
                        else
                        {
                            // just skip the whole thing - invalid entity
                            // move on to the next character
                            _nextCharacter = _lookAheadCharacter;
                            _nextCharacterCode = _lookAheadCharacterCode;
                            this.ReadLookAheadCharacter();

                            // not an entity
                            _isNextCharacterEntity = false;
                        }
                    }
                    else
                    {
                        // skip whatever we read after the ampersand
                        // set next character and move on
                        _nextCharacter = _lookAheadCharacter;
                        this.ReadLookAheadCharacter();
                        _isNextCharacterEntity = false;
                    }
                }
            }
        }

        private void ReadLookAheadCharacter()
        {
            if (_lookAheadCharacterCode != -1)
            {
                _lookAheadCharacterCode = _inputStringReader.Read();
                _lookAheadCharacter = (char)_lookAheadCharacterCode;
            }
        }

        /// <summary>
        /// skips whitespace in the input string
        /// leaves the first non-whitespace character available in the NextCharacter property
        /// this may be the end-of-file character, it performs no checking 
        /// </summary>
        private void SkipWhiteSpace()
        {
            // TODO: handle character entities while processing comments, cdata, and directives
            // TODO: SUGGESTION: we could check if lookahead and previous characters are entities also
            while (true)
            {
                if (_nextCharacter == '<' && (_lookAheadCharacter == '?' || _lookAheadCharacter == '!'))
                {
                    this.GetNextCharacter();

                    if (_lookAheadCharacter == '[')
                    {
                        // Skip CDATA block and DTDs(?)
                        while (!this.IsAtEndOfStream && !(_previousCharacter == ']' && _nextCharacter == ']' && _lookAheadCharacter == '>'))
                        {
                            this.GetNextCharacter();
                        }
                        if (_nextCharacter == '>')
                        {
                            this.GetNextCharacter();
                        }
                    }
                    else
                    {
                        // Skip processing instruction, comments
                        while (!this.IsAtEndOfStream && _nextCharacter != '>')
                        {
                            this.GetNextCharacter();
                        }
                        if (_nextCharacter == '>')
                        {
                            this.GetNextCharacter();
                        }
                    }
                }


                if (!Char.IsWhiteSpace(this.NextCharacter))
                {
                    break;
                }

                this.GetNextCharacter();
            }
        }

        /// <summary>
        /// checks if a character can be used to start a name
        /// if this check is true then the rest of the name can be read
        /// </summary>
        /// <param name="character">
        /// character value to be checked
        /// </param>
        /// <returns>
        /// true if the character can be the first character in a name
        /// false otherwise
        /// </returns>
        private bool IsGoodForNameStart(char character) => character == '_' || Char.IsLetter(character);

        /// <summary>
        /// checks if a character can be used as a non-starting character in a name
        /// uses the IsExtender and IsCombiningCharacter predicates to see
        /// if a character is an extender or a combining character
        /// </summary>
        /// <param name="character">
        /// character to be checked for validity in a name
        /// </param>
        /// <returns>
        /// true if the character can be a valid part of a name
        /// </returns>
        private bool IsGoodForName(char character) => this.IsGoodForNameStart(character) ||
                character == '.' ||
                character == '-' ||
                character == ':' ||
                Char.IsDigit(character) ||
                IsCombiningCharacter(character) ||
                IsExtender(character);

        /// <summary>
        /// identifies a character as being a combining character, permitted in a name
        /// TODO: only a placeholder for now but later to be replaced with comparisons against
        /// the list of combining characters in the XML documentation
        /// </summary>
        /// <param name="character">
        /// character to be checked
        /// </param>
        /// <returns>
        /// true if the character is a combining character, false otherwise
        /// </returns>
        private bool IsCombiningCharacter(char character) => false;

        /// <summary>
        /// identifies a character as being an extender, permitted in a name
        /// TODO: only a placeholder for now but later to be replaced with comparisons against
        /// the list of extenders in the XML documentation
        /// </summary>
        /// <param name="character">
        /// character to be checked
        /// </param>
        /// <returns>
        /// true if the character is an extender, false otherwise
        /// </returns>
        private bool IsExtender(char character) => false;

        /// <summary>
        /// skips dynamic content starting with '<![' and ending with ']>' 
        /// </summary>
        private void ReadDynamicContent()
        {
            // verify that we are at dynamic content, which may include CDATA
            Debug.Assert(_previousCharacter == '<' && _nextCharacter == '!' && _lookAheadCharacter == '[');

            // Let's treat this as empty text
            _nextTokenType = HtmlTokenType.Text;
            _nextToken.Length = 0;

            // advance twice, once to get the lookahead character and then to reach the start of the cdata
            this.GetNextCharacter();
            this.GetNextCharacter();
            
            // NOTE: 10/12/2004: modified this function to check when called if's reading CDATA or something else
            // some directives may start with a <![ and then have some data and they will just end with a ]>
            // this function is modified to stop at the sequence ]> and not ]]>
            // this means that CDATA and anything else expressed in their own set of [] within the <! [...]>
            // directive cannot contain a ]> sequence. However it is doubtful that cdata could contain such
            // sequence anyway, it probably stops at the first ]
            while (!(_nextCharacter == ']' && _lookAheadCharacter == '>') && !this.IsAtEndOfStream)
            {
                // advance
                this.GetNextCharacter();
            }

            if (!this.IsAtEndOfStream)
            {
                // advance, first to the last >
                this.GetNextCharacter();

                // then advance past it to the next character after processing directive
                this.GetNextCharacter();
            }
        }

        /// <summary>
        /// skips comments starting with '<!-' and ending with '-->' 
        /// NOTE: 10/06/2004: processing changed, will now skip anything starting with
        /// the "<!-"  sequence and ending in "!>" or "->", because in practice many html pages do not
        /// use the full comment specifying conventions
        /// </summary>
        private void ReadComment()
        {
            // verify that we are at a comment
            Debug.Assert(_previousCharacter == '<' && _nextCharacter == '!' && _lookAheadCharacter == '-');

            // Initialize a token
            _nextTokenType = HtmlTokenType.Comment;
            _nextToken.Length = 0;

            // advance to the next character, so that to be at the start of comment value
            this.GetNextCharacter(); // get first '-'
            this.GetNextCharacter(); // get second '-'
            this.GetNextCharacter(); // get first character of comment content
 
            while (true)
            {
                // Read text until end of comment
                // Note that in many actual html pages comments end with "!>" (while xml standard is "-->")
                while (!this.IsAtEndOfStream && !(_nextCharacter == '-' && _lookAheadCharacter == '-' || _nextCharacter == '!' && _lookAheadCharacter == '>'))
                {
                    _nextToken.Append(this.NextCharacter);
                    this.GetNextCharacter();
                }

                // Finish comment reading
                this.GetNextCharacter();
                if (_previousCharacter == '-' && _nextCharacter == '-' && _lookAheadCharacter == '>')
                {
                    // Standard comment end. Eat it and exit the loop
                    this.GetNextCharacter(); // get '>'
                    break;
                }
                else if (_previousCharacter == '!' && _nextCharacter == '>')
                {
                    // Nonstandard but possible comment end - '!>'. Exit the loop
                    break;
                }
                else
                {
                    // Not an end. Save character and continue continue reading
                    _nextToken.Append(_previousCharacter);
                    continue;
                }
            }

            // Read end of comment combination
            if (_nextCharacter == '>')
            {
                this.GetNextCharacter();
            }
        }

        /// <summary>
        /// skips past unknown directives that start with "<!" but are not comments or Cdata
        /// ignores content of such directives until the next ">" character
        /// applies to directives such as DOCTYPE, etc that we do not presently support
        /// </summary>
        private void ReadUnknownDirective()
        {
            // verify that we are at an unknown directive
            Debug.Assert(_previousCharacter == '<' && _nextCharacter == '!' && !(_lookAheadCharacter == '-' || _lookAheadCharacter == '['));

            // Let's treat this as empty text
            _nextTokenType = HtmlTokenType.Text;
            _nextToken.Length = 0;

            // advance to the next character
            this.GetNextCharacter();

            // skip to the first tag end we find
            while (!(_nextCharacter == '>' && !IsNextCharacterEntity) && !this.IsAtEndOfStream)
            {
                this.GetNextCharacter();
            }

            if (!this.IsAtEndOfStream)
            {
                // advance past the tag end
                this.GetNextCharacter();
            }
        }

        /// <summary>
        /// skips processing directives starting with the characters '<?' and ending with '?>' 
        /// NOTE: 10/14/2004: IE also ends processing directives with a />, so this function is
        /// being modified to recognize that condition as well
        /// </summary>
        private void SkipProcessingDirective()
        {
            // verify that we are at a processing directive
            Debug.Assert(_nextCharacter == '<' && _lookAheadCharacter == '?');

            // advance twice, once to get the lookahead character and then to reach the start of the drective
            this.GetNextCharacter();
            this.GetNextCharacter();

            while (!((_nextCharacter == '?' || _nextCharacter == '/') && _lookAheadCharacter == '>') && !this.IsAtEndOfStream)
            {
                // advance
                // we don't need to check for entities here because '?' is not an entity
                // and even though > is an entity there is no entity processing when reading lookahead character
                this.GetNextCharacter();
            }

            if (!this.IsAtEndOfStream)
            {
                // advance, first to the last >
                this.GetNextCharacter();

                // then advance past it to the next character after processing directive
                this.GetNextCharacter();
            }
        }

        #endregion Private Methods

        // ---------------------------------------------------------------------
        //
        // Private Properties
        //
        // ---------------------------------------------------------------------

        #region Private Properties

        private char NextCharacter => _nextCharacter;

        private bool IsAtEndOfStream => _nextCharacterCode == -1;

        private bool IsAtTagStart => _nextCharacter == '<' && (_lookAheadCharacter == '/' || IsGoodForNameStart(_lookAheadCharacter)) && !_isNextCharacterEntity;

        private bool IsAtTagEnd => (_nextCharacter == '>' || (_nextCharacter == '/' && _lookAheadCharacter == '>')) && !_isNextCharacterEntity;

        private bool IsAtDirectiveStart => (_nextCharacter == '<' && _lookAheadCharacter == '!' && !this.IsNextCharacterEntity);

        private bool IsNextCharacterEntity => _isNextCharacterEntity;

        #endregion Private Properties

        // ---------------------------------------------------------------------
        //
        // Private Fields
        //
        // ---------------------------------------------------------------------

        #region Private Fields

        // string reader which will move over input text
        private StringReader _inputStringReader;
        // next character code read from input that is not yet part of any token
        // and the character it represents
        private int _nextCharacterCode;
        private char _nextCharacter;
        private int _lookAheadCharacterCode;
        private char _lookAheadCharacter;
        private char _previousCharacter;
        private bool _ignoreNextWhitespace;
        private bool _isNextCharacterEntity;

        // store token and type in local variables before copying them to output parameters
        StringBuilder _nextToken;
        HtmlTokenType _nextTokenType;

        #endregion Private Fields
    }
}
