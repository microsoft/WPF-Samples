// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BamlTools
{
    public class OldBamlDisassembler
    {
        const int LineLength = 16;    // hexbyte length
        static Type _formatVersionType = typeof(MockFormatVersion);

        BamlBinaryReader _reader;
        bool _showAddresses;
        bool _showRecordNumbers;
        bool _showDebugRecords;
        bool _showTableRecords;

        int _recordNumber;
        Dictionary<int, string> _attributeIdMap;

        enum TextType { Record, Size, Data, String, BrokenString };

        public struct TextLineSections
        {
            public string addrStr;
            public string hexStr;
            public string meaningStr;

            public TextLineSections(string pos, string hex, string meaning)
            {
                addrStr = pos;
                hexStr = hex;
                meaningStr = meaning;
            }

            public TextLineSections(long pos, byte[] hex, string meaning)
            {
                addrStr = string.Format("{0:x5}: ", pos);
                hexStr = FormatHexBytes(hex);
                meaningStr = meaning;
            }
        };

        public OldBamlDisassembler(Stream stream)
        {
            _reader = new BamlBinaryReader(stream);
            _attributeIdMap = new Dictionary<int, string>();
        }

        public string DasmOneBigString()
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
            foreach (string line in DumpVersionHeader())
            {
                yield return line;
            }

            yield return "";   // blank line;

            while (!Done())
            {
                foreach (string line in DumpRecord())
                {
                    yield return line;
                }
            }
        }

        public Type FormatVersionType
        {
            get { return _formatVersionType; }
        }

        public bool ShowAddresses
        {
            get { return _showAddresses; }
            set { _showAddresses = value; }
        }

        public bool ShowRecordNumbers
        {
            get { return _showRecordNumbers; }
            set { _showRecordNumbers = value; }
        }

        public bool ShowDebugRecords
        {
            get { return _showDebugRecords; }
            set { _showDebugRecords = value; }
        }

        public bool ShowTableRecords
        {
            get { return _showTableRecords; }
            set { _showTableRecords = value; }
        }

        public long Position
        {
            get { return _reader.BaseStream.Position; }
            set { _reader.BaseStream.Position = value; }
        }

        public long Length
        {
            get { return _reader.BaseStream.Length; }
        }

        public bool Done()
        {
            return (Position >= Length - 1);
        }

        public IEnumerable<string> DumpVersionHeader()
        {
            TextLineSections[] textSections;

            ReadFormatVersion(out textSections);
            foreach (string line in OutputTextLines(textSections, TextType.Data))
            {
                yield return line;
            }
        }

        public IEnumerable<string> DumpRecord()
        {
            _recordNumber += 1;
            foreach (string line in DefaultDumpRecord())
            {
                yield return line;
            }
            yield return ""; // blank line seperating records.
        }

        public IEnumerable<string> DefaultDumpRecord()
        {
            BamlRecord record;
            BamlRecordType recordType;
            int variableSize = 0;
            TextLineSections text;
            TextLineSections[] textList;

            recordType = ReadRecordType(out text);
            yield return OutputTextLine(text, TextType.Record);

            long startPosition = Position;

            // If we read a bad Record type, dump a few more bytes to help debug.
            if ((int)recordType >= (int)BamlRecordType.LastRecordType
                || null == StaticBamlRecords.GetRecord(recordType))
            {
                byte[] bytes = _reader.ReadBytes(64);
                textList = CutIntoLines(startPosition, bytes, "", "");
                foreach (string line in OutputTextLines(textList, TextType.Data))
                {
                    yield return line;
                }
                // Go ahead and throw the nullRef/outofBounds exception below...
            }

            record = StaticBamlRecords.GetRecord(recordType);
            Debug.Assert(record != null);

            if (record.IsVariableSize)
            {
                variableSize = Read7bitEncodedInt("size", out text);
                yield return OutputTextLine(text, TextType.Size);
            }

            for (int i = 0; i < record.Fields.Length; i++)
            {
                object val = ReadField(record.Fields[i], out textList);
                record.Fields[i].Value = val;

                if (textList.Length == 1)
                {
                    ImproveMeaning(record.Fields[i], textList);
                }
                TextType textType = TextType.Data;
                if (val is String)
                    textType = TextType.String;

                foreach (string line in OutputTextLines(textList, textType))
                {
                    yield return line;
                }
            }

            // this deals with the rare case of undefined padding
            // at the end of a record.  Only allowed for PropertyCustom
            if (record.IsVariableSize)
            {
                int processedSize = (int)(Position - startPosition);
                int extraPadding = variableSize - processedSize;
                if (extraPadding < 0)
                {
                    System.Diagnostics.Debugger.Break();
                }
                if (extraPadding > 0)
                {
                    long pos = Position;
                    byte[] bytes = _reader.ReadBytes(extraPadding);

                    string meaning = "?? custom";
                    if (recordType == BamlRecordType.PropertyCustom)
                    {
                        meaning = "Custom Data (variable size)";
                    }
                    textList = CutIntoLines(pos, bytes, meaning, "");
                    foreach (string line in OutputTextLines(textList, TextType.Data))
                    {
                        yield return line;
                    }
                }
            }

            ProcessRecord(record);
        }

        private BamlRecordType ReadRecordType(out TextLineSections text)
        {
            long pos = Position;
            int recordId = _reader.ReadByte();
            Position = pos;

            BamlRecordType val = (BamlRecordType)recordId;
            string meaning = string.Format("{0} [BamlRecord]", val.ToString());

            byte[] bytes = _reader.ReadBytes(1);
            text = new TextLineSections(pos, bytes, meaning);
            return val;
        }

        private object ReadField(BamlField field, out TextLineSections[] textList)
        {
            TextLineSections text = new TextLineSections("Err", "Err", "Err");
            object val = null;
            Type t = field.ClrType;

            textList = null;
            if (t == typeof(short))
            {
                val = (Int16)ReadIntType(2, field.Name, out text);
            }
            else if (t == typeof(int))
            {
                val = (Int32)ReadIntType(4, field.Name, out text);
            }
            else if (t == typeof(string))
            {
                val = (string)ReadString(field.Name, out textList);
            }
            else if (t == typeof(bool))
            {
                val = (bool)ReadBool(field.Name, out text);
            }
            else if (t == typeof(byte))
            {
                val = (byte)ReadIntType(1, field.Name, out text);
            }
            else if (t == FormatVersionType)
            {
                ReadFormatVersion(out textList);
            }
            else if (t == typeof(Int16[]))
            {
                val = (Int16[])ReadIntArrayType(field.Name, out textList);
            }
            else
            {
                throw new InvalidOperationException("Field Type not known");
            }
            if (null == textList)
            {
                textList = new TextLineSections[1];
                textList[0] = text;
            }

            return val;
        }

        private void ReadFormatVersion(out TextLineSections[] text)
        {
            TextLineSections UnicodeLength;
            TextLineSections[] Unicode;
            TextLineSections Version1;
            TextLineSections Version2;
            TextLineSections Version3;

            int strByteLength = (int)ReadIntType(4, "Unicode String Length(bytes)", out UnicodeLength);
            ReadByteLengthPrefixedDWordPaddedUnicodeString(strByteLength, out Unicode);
            ReadVersion("Reader Version", out Version1);
            ReadVersion("Update Version", out Version2);
            ReadVersion("Writer Version", out Version3);

            text = new TextLineSections[4 + Unicode.Length];
            text[0] = UnicodeLength;
            for (int i = 0; i < Unicode.Length; i++)
                text[1 + i] = Unicode[i];
            text[1 + Unicode.Length] = Version1;
            text[2 + Unicode.Length] = Version2;
            text[3 + Unicode.Length] = Version3;
            return;
        }

        private void ReadByteLengthPrefixedDWordPaddedUnicodeString(int strByteLength, out TextLineSections[] textList)
        {
            string val = "";

            long pos = Position;
            {
                BinaryReader tempReader = new BinaryReader(_reader.BaseStream, System.Text.Encoding.Unicode);
                char[] chars = tempReader.ReadChars(strByteLength / 2);
                val = new string(chars);
            }
            Position = pos;

            byte[] bytes = _reader.ReadBytes(strByteLength);
            string meaning = string.Format("\"{0}\"", val);

            textList = CutIntoLines(pos, bytes, meaning, "");
        }

        private void ReadVersion(string label, out TextLineSections text)
        {
            short val1;
            short val2;

            long pos = Position;
            val1 = _reader.ReadInt16();
            val2 = _reader.ReadInt16();
            Position = pos;

            byte[] bytes = _reader.ReadBytes(4);
            string meaning = string.Format("{0}={1}.{2}", label, val1.ToString(), val2.ToString());

            text = new TextLineSections(pos, bytes, meaning);
        }

        private long ReadIntType(int size, string label, out TextLineSections text)
        {
            long val = -1;


            string meaning = "";

            long pos = Position;
            switch (size)
            {
                case 1:
                    val = (sbyte)_reader.ReadSByte();
                    break;

                case 2:
                    val = (short)_reader.ReadInt16();
                    break;

                case 4:
                    val = (int)_reader.ReadInt32();
                    break;

                case 8:
                    val = (long)_reader.ReadInt64();
                    break;

                default:
                    throw new InvalidOperationException("Bad integral datatype size");
            }
            Position = pos;

            byte[] bytes = _reader.ReadBytes(size);

            sbyte val8 = -1;
            short val16 = -1;
            int val32 = -1;
            long val64 = -1;
            string formatString = (0 <= val && val < 10) ? "{0}={1}" : "{0}={1}({2})";
            switch (size)
            {
                case 1:
                    val8 = (sbyte)val;
                    meaning = string.Format(formatString, label, val8.ToString("x"), val8.ToString("d"));
                    break;

                case 2:
                    val16 = (short)val;
                    meaning = string.Format(formatString, label, val16.ToString("x"), val16.ToString("d"));
                    break;

                case 4:
                    val32 = (int)val;
                    meaning = string.Format(formatString, label, val32.ToString("x"), val32.ToString("d"));
                    break;

                case 8:
                    val64 = (long)val;
                    meaning = string.Format(formatString, label, val64.ToString("x"), val64.ToString("d"));
                    break;
            }

            text = new TextLineSections(pos, bytes, meaning);
            return val;
        }

        private bool ReadBool(string label, out TextLineSections text)
        {
            long pos = Position;
            bool val = _reader.ReadBoolean();
            Position = pos;

            byte[] bytes = _reader.ReadBytes(1);
            string meaning = string.Format("{0}={1}", label, val.ToString());

            text = new TextLineSections(pos, bytes, meaning);
            return val;
        }

        private int Read7bitEncodedInt(string label, out TextLineSections text)
        {
            long pos = Position;
            int val = _reader.Read7BitEncodedInt();
            int sizeOfEncoding = (int)(Position - pos);
            Position = pos;

            byte[] bytes = _reader.ReadBytes(sizeOfEncoding);
            string meaning = string.Format("{0}={1}", label, val.ToString());
            text = new TextLineSections(pos, bytes, meaning);
            return val;
        }

        private string ReadString(string label, out TextLineSections[] textList)
        {
            long pos = Position;
            string val = _reader.ReadString();
            int sizeInBytesOfString = (int)(Position - pos);
            Position = pos;

            // Can't use val.Length to compute number of bytes because multiple
            // bytes might make one Unicode character.
            // For Example:   the bytes 03 E2 97 8f make one Unicode char (The
            // Dot used in password boxes).
            byte[] bytes = _reader.ReadBytes(sizeInBytesOfString);
            if (sizeInBytesOfString <= LineLength)
            {
                string meaning = string.Format("{0}='{1}'", label, val);
                textList = new TextLineSections[1];
                textList[0] = new TextLineSections(pos, bytes, meaning);
            }
            else
            {
                TextLineSections[] textListCut = CutIntoLines(pos, bytes, val, label);
                textList = new TextLineSections[textListCut.Length + 1];
                Array.Copy(textListCut, textList, textListCut.Length);
                textList[textListCut.Length] = new TextLineSections(pos, new byte[0], val);
            }
            return val;
        }

        private short[] ReadIntArrayType(string label, out TextLineSections[] textList)
        {
            long pos = Position;

            short arrayLength = (short)_reader.ReadInt16();
            short[] array = new short[arrayLength];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (short)_reader.ReadInt16();
            }
            int sizeOfArray = (int)(Position - pos);
            Position = pos;

            textList = new TextLineSections[arrayLength + 1];
            string meaning = string.Format("{0}={1}", label, array.Length);
            byte[] subBytes = _reader.ReadBytes(2);
            textList[0] = new TextLineSections(Position - 2, subBytes, meaning);

            for (int i = 0; i < array.Length; i++)
            {
                meaning = string.Format("Assembly ID[{0}]={1}", i, array[i]);
                subBytes = _reader.ReadBytes(2);
                textList[i + 1] = new TextLineSections(Position - 2, subBytes, meaning);
            }
            return array;
        }


        // OUTPUT methods.


        private string OutputTextLine(TextLineSections text, TextType textType)
        {
            string outputLine = String.Empty;
            if (ShowRecordNumbers)
            {
                string numString = String.Format("#{0:d3}: ", _recordNumber);
                if (textType == TextType.Record)
                {
                    outputLine += numString;
                }
                else
                {
                    outputLine += FormatPadToColumn(numString.Length, 0);
                }
            }
            if (ShowAddresses)
            {
                outputLine += text.addrStr;
            }
            outputLine += string.Format(" {0}", text.hexStr);

            int column = 0;
            if (outputLine.Length < 40)
                column = 40;
            else
                column = 60;
            outputLine += this.FormatPadToColumn(column, outputLine.Length);

            outputLine += text.meaningStr;

            return outputLine;
        }

        private IEnumerable<string> OutputTextLines(TextLineSections[] textList, TextType textType)
        {
            foreach (TextLineSections text in textList)
            {
                yield return OutputTextLine(text, textType);
            }
        }

        // Output a long block of bytes possibly over several lines.
        // If "label" is non-null then there is a header line of output.
        // If "val" is non-null then the byte block has a parallel "string" version.
        private TextLineSections[] CutIntoLines(long position, byte[] bytes, string val, string label)
        {
            TextLineSections[] textList;

            bool hasLabel = (label != null && label.Length > 0);
            bool hasString = (val != null && val.Length > 0);

            // Output the first "header" line.
            // If there is a Lable then the header line is the string length
            //   and label text.
            // If there is a Lable but no string, then the bytes is not a string
            //   is the just a blob of bytes.
            int headerOffset = 0;
            int currentLine = 0;
            if (!hasLabel)
            {
                int lineCount = (bytes.Length + LineLength - 1) / LineLength;
                textList = new TextLineSections[lineCount];
            }
            else
            {
                int sizeOfHeader = 0;
                byte[] headerBytes = null;
                TextLineSections labelLine;

                if (!hasString)
                {
                    sizeOfHeader = (bytes.Length > LineLength) ? LineLength : bytes.Length;
                    headerBytes = SnipBytes(bytes, 0, LineLength);
                    labelLine = new TextLineSections(position, headerBytes, label);
                    headerOffset = sizeOfHeader;
                }
                else
                {
                    sizeOfHeader = ComputeLengthOf7bitEncodedLengthPrefix(bytes.Length);
                    headerBytes = SnipBytes(bytes, 0, sizeOfHeader);
                    int stringLength = val.Length;
                    string meaning = string.Format("{0}= string of length {1}({2})", label, stringLength.ToString("x2"), stringLength);
                    labelLine = new TextLineSections(position, headerBytes, meaning);
                    headerOffset = sizeOfHeader;
                }
                int lineCount = 1 + ((bytes.Length - sizeOfHeader) + LineLength - 1) / LineLength;
                textList = new TextLineSections[lineCount];
                textList[currentLine++] = labelLine;
            }

            // Output the middle "full length lines"
            int byteOffset = 0;
            int valOffset = 0;
            for (valOffset = 0; (byteOffset = valOffset + headerOffset) < bytes.Length - LineLength; valOffset += LineLength)
            {
                byte[] subBytes = SnipBytes(bytes, byteOffset, LineLength);
                string subText = "";
                if (hasString)
                {   // The unicode string may run short early.
                    int remainingVal = val.Length - valOffset;
                    if (remainingVal > 0)
                    {
                        int subValLen = (remainingVal < LineLength) ? remainingVal : LineLength;
                        char[] subChars = val.ToCharArray(valOffset, subValLen);
                        subText = new string(subChars);
                    }
                }
                // March 2010: removed the subStrings at the ends of the lines.  
                textList[currentLine++] = new TextLineSections(position + byteOffset, subBytes, /*subText*/String.Empty);
            }

            // output the last line.
            int remaining = bytes.Length - byteOffset;
            if (remaining > 0)
            {
                byte[] subBytes = SnipBytes(bytes, byteOffset, remaining);
                string subText = "";
                if (hasString)
                {
                    int remainingVal = val.Length - valOffset;
                    if (remainingVal > 0)
                    {
                        char[] subChars = val.ToCharArray(valOffset, remainingVal);
                        subText = new string(subChars);
                    }
                }
                // March 2010: removed the subStrings at the ends of the lines.  
                textList[currentLine++] = new TextLineSections(position + byteOffset, subBytes, /*subText*/String.Empty);
            }
            return textList;
        }

        private byte[] SnipBytes(byte[] src, int start, int count)
        {
            byte[] dest = new byte[count];
            for (int i = 0; i < count; i++)
                dest[i] = src[start + i];
            return dest;
        }

        // this function is passed the length of a buffer, including the
        // variable size length prefix.   Given that total length, how 
        // many bytes long is the length prefix.
        private int ComputeLengthOf7bitEncodedLengthPrefix(int length)
        {
            if (length <= 0x80)  // 0 to 7F have 1 byte headers 7F+1=0x80.
                return 1;
            if (length <= 0x4001) // 80 to 3FFF have 2 byte headers 3FFF+2=0x4001
                return 2;
            if (length < 0x200002) // 1FFFFF + 3 = 0x200002
                return 3;
            return 4;       // this could go bigger and bigger, but reasonable numbers...
        }

        private static string FormatHexBytes(byte[] bytes)
        {
            if (null == bytes || bytes.Length > 16 || bytes.Length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            string outStr = "";

            if (bytes.Length == 16)
            {
                outStr += string.Format("{0} {1} {2} {3} {4} {5} {6} {7}",
                    bytes[0].ToString("x2"), bytes[1].ToString("x2"),
                    bytes[2].ToString("x2"), bytes[3].ToString("x2"),
                    bytes[4].ToString("x2"), bytes[5].ToString("x2"),
                    bytes[6].ToString("x2"), bytes[7].ToString("x2"));
                outStr += string.Format("  {0} {1} {2} {3} {4} {5} {6} {7}",
                    bytes[8].ToString("x2"), bytes[9].ToString("x2"),
                    bytes[10].ToString("x2"), bytes[11].ToString("x2"),
                    bytes[12].ToString("x2"), bytes[13].ToString("x2"),
                    bytes[14].ToString("x2"), bytes[15].ToString("x2"));
            }
            else
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    outStr += string.Format("{0} ", bytes[i].ToString("x2"));
                    if (7 == i % 8)
                        outStr += " ";
                }
            }

            return outStr;
        }

        private string FormatPadToColumn(int column, int strLength)
        {
            int diff = column - strLength;
            string padding = "";
            while (diff > 0)
            {
                if (diff >= 8)
                {
                    padding += "        ";
                    diff -= 8;
                }
                else if (diff >= 4)
                {
                    padding += "    ";
                    diff -= 4;
                }
                else if (diff >= 2)
                {
                    padding += "  ";
                    diff -= 2;
                }
                else if (diff >= 1)
                {
                    padding += " ";
                    diff -= 1;
                }
            }
            return padding;
        }

        private void ProcessRecord(BamlRecord record)
        {
            switch (record.Id)
            {
                case BamlRecordType.AttributeInfo:
                    ProcessAttributeInfo(record);
                    break;
            }
        }

        private void ProcessAttributeInfo(BamlRecord record)
        {
            System.Diagnostics.Debug.Assert(record.Fields[0].BamlFieldType == BamlFieldType.AttributeId);
            Int16 attrId = (Int16)record.Fields[0].Value;

            System.Diagnostics.Debug.Assert(record.Fields[3].BamlFieldType == BamlFieldType.Value);
            string strValue = (string)record.Fields[3].Value;

            _attributeIdMap[attrId] = strValue;
        }

        private void ImproveMeaning(BamlField field, TextLineSections[] textList)
        {
            string oldMeaning = textList[0].meaningStr;
            string strValue = null;
            switch (field.BamlFieldType)
            {
                case BamlFieldType.AttributeId:
                    Int16 attrId = (Int16)field.Value;
                    if (attrId < 0)
                    {
                        strValue = KnownProperties.Properties[-attrId].PropertyName;
                    }
                    else if (_attributeIdMap.ContainsKey(attrId))
                    {
                        strValue = _attributeIdMap[attrId];
                    }
                    break;
                case BamlFieldType.TypeId:
                case BamlFieldType.ConverterTypeId:
                    Int16 typeId = (Int16)field.Value;
                    if (typeId < 0)
                    {
                        strValue = KnownElements.Elements[-typeId].ShortName;
                    }
                    break;
                case BamlFieldType.AttributeUsage:
                    byte usage = (byte)field.Value;
                    switch (usage)
                    {
                        case 0: strValue = "Default"; break;
                        case 1: strValue = "XmlLang"; break;
                        case 2: strValue = "XmlSpace"; break;
                        case 3: strValue = "RuntimeName"; break;
                    }
                    break;
            }
            if (null != strValue)
            {
                textList[0].meaningStr = string.Format("{0}  <{1}>", oldMeaning, strValue);
            }
        }
    }

}
