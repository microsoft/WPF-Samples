// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//---------------------------------------------------------------------------
//
// Description: ResourceTextReader class 
//              It reads values from a CSV file or tab-separated TXT file
//
//---------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Resources;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

namespace BamlLocalization
{
    /// <summary>
    /// Reader that reads value from a CSV file or Tab-separated TXT file
    /// </summary>
    internal class ResourceTextReader : IDisposable 
    {
        internal ResourceTextReader(FileType fileType, Stream stream)
        {
            _delimiter = LocBamlConst.GetDelimiter(fileType);
            if(stream == null)
                throw new ArgumentNullException("stream");

            _reader = new StreamReader(stream);
        }    

        internal bool ReadRow()
        {
            // currentChar is the first char after newlines
            int currentChar = SkipAllNewLine();  

            if (currentChar < 0)
            {
                // nothing else to read
                return false;
            }        
            
            ReadState currentState = ReadState.TokenStart;
            _columns = new ArrayList();            

            StringBuilder buffer = new StringBuilder();
            
            while (currentState != ReadState.LineEnd)
            {
                switch(currentState)
                {
                    // start of a token
                    case ReadState.TokenStart:                        
                    {
                        if (currentChar == _delimiter)
                        {
                            // it is the end of the token when we see a delimeter
                            // Store token, and reset state. and ignore this char
                            StoreTokenAndResetState(ref buffer, ref currentState);
                        }
                        else if (currentChar == '\"')
                        {
                            // jump to Quoted content if it token starts with a quote.
                            // and also ignore this quote
                            currentState = ReadState.QuotedContent;
                        }
                        else if (currentChar == '\n' || 
                                (currentChar == '\r' && _reader.Peek() == '\n'))
                        {
                            // we see a '\n' or '\r\n' sequence. Go to LineEnd
                            // ignore these chars
                            currentState = ReadState.LineEnd;                            
                        }
                        else 
                        {
                            // safe to say that this is part of a unquoted content
                            buffer.Append((Char) currentChar);
                            currentState = ReadState.UnQuotedContent;                            
                        }                        
                        break;
                    }         

                    // inside of an unquoted content
                    case ReadState.UnQuotedContent :
                    {
                        if (currentChar == _delimiter)
                        {
                            // It is then end of a toekn.
                            // Store the token value and reset state
                            // igore this char as well
                            StoreTokenAndResetState(ref buffer, ref currentState);
                        }
                        else if (currentChar == '\n' ||
                                (currentChar == '\r' && _reader.Peek() == '\n'))
                        {
                            // see a new line
                            // igorne these chars and jump to LineEnd
                            currentState = ReadState.LineEnd;
                        }
                        else
                        {
                            // we are good. store this char
                            // notice, even we see a '\"', we will just treat it like 
                            // a normal char
                            buffer.Append((Char) currentChar);                            
                        }
                        break;                        
                    }         

                    // inside of a quoted content
                    case ReadState.QuotedContent :
                    {
                        if (currentChar ==  '\"')
                        {   
                        
                            // now it depends on whether the next char is quote also
                            if (_reader.Peek() == '\"')
                            {
                                // we will ignore the next quote.
                                currentChar =  _reader.Read();
                                buffer.Append( (Char) currentChar);
                            }
                            else 
                            {   // we have a single quote. We fall back to unquoted content state
                                // and igorne the curernt quote
                                currentState = ReadState.UnQuotedContent;
                            }                            
                        }
                        else 
                        {
                            // we are still inside of a quote, anything is accepted
                            buffer.Append((Char) currentChar);                                                       
                        }
                        break;                           
                    }         
                }                

                // read in the next char
                currentChar = _reader.Read();
                
                if (currentChar < 0)
                {
                    // break out of the state machine if we reach the end of the file
                    break;
                }
            }   

            // we got to here either we are at LineEnd, or we are end of file
            if (buffer.Length > 0)
            {
                _columns.Add(buffer.ToString());
            }
            return true;
        }
        
        internal string GetColumn(int index)
        {
            if (_columns!= null && index < _columns.Count && index >= 0)
            {
                return (string) _columns[index];
            }
            else 
            {
                return null;
            }
        }
           
        internal void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        //---------------------------------
        // private functions
        //---------------------------------
        
        private void StoreTokenAndResetState(ref StringBuilder buffer, ref ReadState currentState)
        {
            // add the token into buffer. The token can be empty
            _columns.Add(buffer.ToString());

            // create a new buffer for the next token.
            buffer = new StringBuilder();

            // we continue to token state state
            currentState = ReadState.TokenStart;
        }

        // skip all new line and return the first char after newlines.
        // newline means '\r\n' or '\n'
        private int SkipAllNewLine()
        {
            int _char;
            while ((_char = _reader.Read())>=0)
            {
                if (_char == '\n')
                {
                    continue; // continue if it is '\n'
                }
                else if (_char == '\r' && _reader.Peek() == '\n')
                {
                    // skip the '\n' in the next position
                    _reader.Read();

                    // and continue
                    continue;   
                }
                else 
                {
                    // stop here
                    break;
                }
            }
            return _char;            
        }
        

        private TextReader _reader;     // internal text reader
        private int        _delimiter;  // delimiter
        private ArrayList  _columns;    // An arraylist storing all the columns of a row

        /// <summary>
        /// Enum representing internal states of the reader when reading 
        /// the CSV or tab-separated TXT file
        /// </summary>
        private enum ReadState
        {
            /// <summary>
            /// State in which the reader is at the start of a column
            /// </summary>
            TokenStart, 

            /// <summary>
            /// State in which the reader is reading contents that are quoted
            /// </summary>
            QuotedContent,

            /// <summary>
            /// State in which the reader is reading contents not in quotes
            /// </summary>
            UnQuotedContent,

            /// <summary>
            /// State in which the end of a line is reached
            /// </summary>
            LineEnd,                
        }
        
    }
}
