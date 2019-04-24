// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//---------------------------------------------------------------------------
//
// Description: ResourceTextWriter class 
//              It writes values to a CSV file or tab-separated TXT file
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
    /// the class that writes to a text file either tab delimited or comma delimited. 
    /// </summary>
    internal sealed class ResourceTextWriter : IDisposable
    {
        //-------------------------------
        // constructor 
        //-------------------------------
        internal ResourceTextWriter(FileType fileType, Stream output)
        {
            
            _delimiter = LocBamlConst.GetDelimiter(fileType);

            if (output == null)
                throw new ArgumentNullException("output");

            // show utf8 byte order marker
            UTF8Encoding encoding = new UTF8Encoding(true);           
            _writer      = new StreamWriter(output, encoding);
            _firstColumn = true;
       }

       #region internal methods
       //-----------------------------------
       // Internal methods
        //-----------------------------------
        internal void WriteColumn(string value)
        {    
            if (value == null)
                    value = string.Empty;

            // if it contains delimeter, quote, newline, we need to escape them
            if (value.IndexOfAny(new char[]{'\"', '\r', '\n', _delimiter}) >= 0)
            {
                // make a string builder at the minimum required length;
                StringBuilder builder = new StringBuilder(value.Length + 2);

                // put in the opening quote
                builder.Append('\"');
                
                // double quote each quote
                for (int i = 0; i < value.Length; i++)
                {
                    builder.Append(value[i]);
                    if (value[i] == '\"')
                    {
                        builder.Append('\"');
                    }                       
                }

                // put in the closing quote
                builder.Append('\"');
                value = builder.ToString();
            }

            if (!_firstColumn)
            {
                // if we are not the first column, we write delimeter
                // to seperate the new cell from the previous ones.
                _writer.Write(_delimiter);                
            }
            else
            {
                _firstColumn = false;   // set false
            }

            _writer.Write(value);            
        }

        internal void EndLine()
        {
            // write a new line
            _writer.WriteLine();

            // set first column to true    
            _firstColumn = true;
        }
        internal void Close()
        {
            if (_writer != null)
            {
                _writer.Close();
            }
        }
       #endregion 

        void IDisposable.Dispose()
        {
            Close();
        }

        
        #region private members
        private char        _delimiter;
        private TextWriter  _writer;        
        private bool        _firstColumn;
        #endregion
    }    
}





    
