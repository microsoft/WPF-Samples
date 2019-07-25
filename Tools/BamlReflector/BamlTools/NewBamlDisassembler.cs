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
    public class NewBamlDisassembler
    {
        BamlBinaryReader _reader;
        bool _showAddresses;
        bool _showRecordNumbers;
        bool _showDebugRecords;
        bool _showTableRecords;

        private string _indent;
        private int _indentLevel;
        private int _recordNumber;
        private Dictionary<int, string> _typeInfoTable;
        private Dictionary<int, string> _attributeInfoTable;
        private Dictionary<int, string> _assemblyInfoTable;

        public NewBamlDisassembler(Stream reader)
        {
            _reader = new BamlBinaryReader(reader);
            _typeInfoTable = new Dictionary<int, string>();
            _attributeInfoTable = new Dictionary<int, string>();
            _assemblyInfoTable = new Dictionary<int, string>();
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
            DataFormatVersion version = ReadFormatVersion();
            yield return VersionToString(version);

            while (!Done())
            {
                long address = _reader.BaseStream.Position;
                BamlRecord record = ReadRecord();

                if ((record.Flags & BamlRecordFlags.End) != 0)
                {
                    IndentLevel -= 1;
                }
                string indent = Indent;  // save the indent because ProcessBamlRecord() may change it.

                ProcessBamlRecord(record);

                if (record.Flags == BamlRecordFlags.Debug && !ShowDebugRecords)
                {
                    continue;
                }

                if (record.Flags == BamlRecordFlags.Table && !ShowTableRecords)
                {
                    continue;
                }

                string recNumString = String.Format("#{0:d3}: ", _recordNumber);
                string addressString = String.Format("{0:x5}: ", address);

                string preamble = String.Empty;
                preamble += (ShowRecordNumbers) ? recNumString : String.Empty;
                preamble += (ShowAddresses) ? addressString : String.Empty;
                preamble += indent;
                yield return preamble + RecordToString(record);
            }
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


        // This property should only be accessed after a disassembly.
        // It is running the disassembly that populates the table.
        public List<TypeInfoTableEntry> TypeInfoTable
        {
            get
            {
                List<TypeInfoTableEntry> list = new List<TypeInfoTableEntry>();
                foreach (KeyValuePair<int, string> entry in _typeInfoTable)
                {
                    list.Add(new TypeInfoTableEntry(entry.Key, entry.Value));
                }
                return list;
            }
        }

        public List<AttributeInfoTableEntry> AttributeInfoTable
        {
            get
            {
                List<AttributeInfoTableEntry> list = new List<AttributeInfoTableEntry>();
                foreach (KeyValuePair<int, string> entry in _attributeInfoTable)
                {
                    list.Add(new AttributeInfoTableEntry(entry.Key, entry.Value, 0));
                }
                return list;
            }
        }

        private DataFormatVersion ReadFormatVersion()
        {
            DataFormatVersion version = new DataFormatVersion();
            int strByteLength = (int)_reader.ReadInt32();

            BinaryReader tempReader = new BinaryReader(_reader.BaseStream, System.Text.Encoding.Unicode);
            char[] chars = tempReader.ReadChars(strByteLength / 2);

            version.name = new string(chars);
            version.readerVersion = _reader.ReadInt32();
            version.updateVersion = _reader.ReadInt32();
            version.writerVersion = _reader.ReadInt32();
            return version;
        }


        public BamlRecord ReadRecord()
        {
            BamlRecordType recordType = (BamlRecordType)_reader.ReadByte();
            BamlRecord record = StaticBamlRecords.GetRecord(recordType);

            long startPosition = Position;
            int variableSize = 0;

            if (record.IsVariableSize)
            {
                variableSize = _reader.Read7BitEncodedInt();
            }

            for (int i = 0; i < record.Fields.Length; i++)
            {
                record.Fields[i].Value = ReadField(record.Fields[i]);
            }

            // this deals with the rare case of undefined padding
            // at the end of a record.  Only allowed for PropertyCustom
            if (record.IsVariableSize)
            {
                int processedSize = (int)(Position - startPosition);
                int extraPadding = variableSize - processedSize;
                Debug.Assert(extraPadding >= 0);
                if (extraPadding > 0)
                {
                    Debug.Assert(recordType == BamlRecordType.PropertyCustom);
                    byte[] bytes = _reader.ReadBytes(extraPadding);
                }
            }
            return record;
        }

        private object ReadField(BamlField field)
        {
            object val = null;
            Type t = field.ClrType;

            if (t == typeof(short))
            {
                val = _reader.ReadInt16();
            }
            else if (t == typeof(int))
            {
                val = _reader.ReadInt32();
            }
            else if (t == typeof(string))
            {
                val = _reader.ReadString();
            }
            else if (t == typeof(bool))
            {
                val = _reader.ReadBoolean();
            }
            else if (t == typeof(byte))
            {
                val = _reader.ReadByte();
            }
            else if (t == typeof(Int16[]))
            {
                short arrayLength = (short)_reader.ReadInt16();
                short[] array = new short[arrayLength];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (short)_reader.ReadInt16();
                }
                val = array;
            }
            else
            {
                throw new InvalidOperationException("Field Type not known");
            }

            return val;
        }

        private string VersionToString(DataFormatVersion version)
        {
            return String.Format("'{0}' {1}.{2} {3}.{4} {5}.{6}", version.name,
                    version.readerVersion & 0xFFFF, (version.readerVersion >> 16) & 0xFFFF,
                    version.updateVersion & 0xFFFF, (version.updateVersion >> 16) & 0xFFFF,
                    version.writerVersion & 0xFFFF, (version.writerVersion >> 16) & 0xFFFF);
        }

        private string RecordToString(BamlRecord record)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(record.Id.ToString());

            for (int i = 0; i < record.Fields.Length; i++)
            {
                BamlField currentField = record.Fields[i];
                string name = String.Empty;
                string valString;
                switch (currentField.BamlFieldType)
                {
                    case BamlFieldType.TypeId:
                    case BamlFieldType.ConverterTypeId:
                        valString = TypeIdToString((short)currentField.Value);
                        break;

                    case BamlFieldType.AttributeId:
                        valString = AttributeIdToString((short)currentField.Value);
                        break;

                    default:
                        valString = currentField.Value as String;
                        if (valString != null)
                        {
                            valString = ("\"" + valString + "\"");
                        }
                        else
                        {
                            valString = currentField.Value.ToString();
                        }
                        break;
                }
                sb.Append(", " + currentField.Name + "(" + valString + ")");
            }
            return sb.ToString();
        }

        private string TypeIdToString(short typeId)
        {
            string name = String.Empty;
            if (typeId < 0)
            {
                name = "<" + KnownElements.Elements[-typeId].ShortName + ">";
                Debug.Assert(KnownElements.Elements[-typeId].BamlNumber == -typeId);
            }
            else
            {
                name = "<" + _typeInfoTable[typeId] + ">";
            }
            return typeId + name;
        }

        private string AttributeIdToString(short attributeId)
        {
            string name = String.Empty;
            if (attributeId < 0)
            {
                name = "<" + KnownProperties.Properties[-attributeId].PropertyName + ">";
                Debug.Assert(KnownElements.Elements[-attributeId].BamlNumber == -attributeId);
            }
            else
            {
                name = "<" + _attributeInfoTable[attributeId] + ">";
            }
            return attributeId.ToString() + name;
        }


        private void ProcessBamlRecord(BamlRecord record)
        {
            _recordNumber += 1;
            if ((record.Flags & BamlRecordFlags.Start) != 0)
            {
                IndentLevel += 1;
            }
            if ((record.Flags & BamlRecordFlags.Table) != 0)
            {
                switch (record.Id)
                {
                    case BamlRecordType.AssemblyInfo:
                        _assemblyInfoTable.Add((short)record.Fields[0].Value, (string)record.Fields[1].Value);
                        break;

                    case BamlRecordType.TypeInfo:
                        _typeInfoTable.Add((short)record.Fields[0].Value, (string)record.Fields[2].Value);
                        break;

                    case BamlRecordType.AttributeInfo:
                        _attributeInfoTable.Add((short)record.Fields[0].Value, (string)record.Fields[3].Value);
                        break;

                    default:
                        //Debug.Assert(false, "Bad record in BamlRecordFlags.Table");
                        break;
                }
            }
        }

        private string Indent
        {
            get { return _indent; }
        }

        private int IndentLevel
        {
            get { return _indentLevel; }
            set
            {
                _indentLevel = value;
                _indent = String.Empty;
                for (int i = 0; i < _indentLevel; i++)
                {
                    _indent += " . ";
                }
            }
        }
    }
}
