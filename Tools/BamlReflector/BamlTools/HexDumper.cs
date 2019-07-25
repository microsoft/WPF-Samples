// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BamlTools
{
    public class HexDumper
    {
        static string Symbols = "0123456789ABCDEF";

        Stream _reader;
        bool _showAddresses;
        bool _showText;

        public HexDumper(Stream stream)
        {
            _reader = stream;
            _showText = true;
        }

        public string DumpOneBigString()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                foreach (string line in GetLines())
                {
                    sb.AppendLine(line);
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }

            return sb.ToString();
        }

        public IEnumerable<String> GetLines()
        {
            string hexString;
            byte[] inputBuffer = new byte[FullHexByteLength];

            while (_reader.Position < _reader.Length)
            {
                int count = _reader.Read(inputBuffer, 0, inputBuffer.Length);
                hexString = HexStringConvert(inputBuffer, count);
                if (ShowText)
                {
                    string textString = TextConvert(inputBuffer, count);
                    hexString = Pad(hexString, FullHexStringLength, ' ');
                    hexString += "  " + textString;
                }

                if (ShowAddresses)
                {
                    hexString = String.Format("{0:x5}: {1}", _reader.Position, hexString);
                }
                yield return hexString;
            }
        }

        public bool ShowAddresses
        {
            get { return _showAddresses; }
            set { _showAddresses = value; }
        }

        public bool ShowText
        {
            get { return _showText; }
            set { _showText = value; }
        }

        private const int FullHexByteLength = 16;

        private int FullHexStringLength
        {
            get { return FullHexByteLength * 3; }
        }

        private string HexStringConvert(byte[] input, int count)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                int high = (input[i] >> 4) & 0xF;
                int low = input[i] & 0xF;
                sb.AppendFormat("{0}{1} ", Symbols[high], Symbols[low]);
            }
            return sb.ToString();
        }

        private string TextConvert(byte[] input, int count)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                sb.Append((input[i] < ' ' || (int)input[i] > 127) ? '.' : (char)input[i]);
            }
            return sb.ToString();
        }

        private string Pad(string str, int length, char fill)
        {
            // this is somewhat inefficent, but it is only called once.
            for (int i = str.Length; i < length; i++)
            {
                str += fill;
            }
            return str;
        }
    }
}
