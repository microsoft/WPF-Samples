// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace BamlTools
{
    // Some of the records are "Not Implemented" this means there really is no
    // such record.  Some of the records are "Unused" because the BamlWriter doesn't
    // actually write them.
    public enum BamlRecordType : byte
    {
        Unknown = 0,
        DocumentStart,              // 1
        DocumentEnd,                // 2
        ElementStart,               // 3
        ElementEnd,                 // 4
        Property,                   // 5
        PropertyCustom,             // 6
        PropertyComplexStart,       // 7
        PropertyComplexEnd,         // 8
        PropertyArrayStart,         // 9
        PropertyArrayEnd,           // 10
        PropertyIListStart,         // 11
        PropertyIListEnd,           // 12
        PropertyIDictionaryStart,   // 13
        PropertyIDictionaryEnd,     // 14
        LiteralContent,             // 15
        Text,                       // 16
        TextWithConverter,          // 17
        RoutedEvent,                // 18       UNUSED
        ClrEvent,                   // 19       NOT IMPLEMENTED in Avalon
        XmlnsProperty,              // 20
        XmlAttribute,               // 21       NOT IMPLEMENTED in Avalon
        ProcessingInstruction,      // 22       NOT IMPLEMENTED in Avalon
        Comment,                    // 23       NOT IMPLEMENTED in Avalon
        DefTag,                     // 24       NOT IMPLEMENTED in Avalon
        DefAttribute,               // 25
        EndAttributes,              // 26       NOT IMPLEMENTED in Avalon
        PIMapping,                  // 27
        AssemblyInfo,               // 28
        TypeInfo,                   // 29
        TypeSerializerInfo,         // 30
        AttributeInfo,              // 31
        StringInfo,                 // 32
        PropertyStringReference,    // 33       UNUSED
        PropertyTypeReference,      // 34
        PropertyWithExtension,      // 35
        PropertyWithConverter,      // 36
        DeferableContentStart,      // 37
        DefAttributeKeyString,      // 38
        DefAttributeKeyType,        // 39
        KeyElementStart,            // 40
        KeyElementEnd,              // 41
        ConstructorParametersStart, // 42
        ConstructorParametersEnd,   // 43
        ConstructorParameterType,   // 44
        ConnectionId,               // 45
        ContentProperty,            // 46
        NamedElementStart,          // 47
        StaticResourceStart,        // 48
        StaticResourceEnd,          // 49
        StaticResourceId,           // 50
        TextWithId,                 // 51
        PresentationOptionsAttribute,// 52
        LineNumberAndPosition,      // 53
        LinePosition,               // 54
        OptimizedStaticResource,     // 55,
        PropertyWithStaticResourceId,// 56,
        LastRecordType
    }


    public class MockFormatVersion
    {
        // the real format version is Window Internal.
        // But we don't need the real type, we just need a unique type to standin for it.
    }

    public class DataFormatVersion
    {
        public string name;
        public int readerVersion;
        public int updateVersion;
        public int writerVersion;
    }

    [Flags]
    public enum BamlRecordFlags
    {
        None = 0x0,
        Start = 0x01,
        End = 0x02,
        Table = 0x04,
        Debug = 0x08
    }

    public class BamlRecord
    {
        private BamlRecordType _recordId;
        private bool _isVariableSize;
        private BamlRecordFlags _flags;
        private BamlField[] _fields;

        public BamlRecord(BamlRecordType recordId, bool isVariableSize, BamlRecordFlags flags, BamlField[] fields)
        {
            if (null == fields)
            {
                throw new ArgumentNullException("fields");
            }
            if (recordId <= BamlRecordType.Unknown || recordId >= BamlRecordType.LastRecordType)
            {
                throw new ArgumentOutOfRangeException("BamlRecordType recordId");
            }
            _recordId = recordId;
            _flags = flags;
            _isVariableSize = isVariableSize;
            _fields = fields;
        }

        public BamlRecord Clone()
        {
            return new BamlRecord(_recordId, _isVariableSize, _flags, (BamlField[])_fields.Clone());
        }

        public BamlRecordType Id
        {
            get { return _recordId; }
        }

        public bool IsVariableSize
        {
            get { return _isVariableSize; }
        }

        public BamlRecordFlags Flags
        {
            get { return _flags; }
        }

        public BamlField[] Fields
        {
            get { return _fields; }
        }
    }

    class StaticBamlRecords
    {
        private static BamlRecord[] _records;

        public static BamlRecord GetRecord(BamlRecordType recordId)
        {
            BamlRecord rec = _records[(int)recordId];
            return rec.Clone();
        }

        static StaticBamlRecords()
        {
            BamlRecordType recordId;
            BamlField[] fields;

            _records = new BamlRecord[(int)BamlRecordType.LastRecordType];

            // DocumentStart = 1
            recordId = BamlRecordType.DocumentStart;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.LoadAsync),
                new BamlField(BamlFieldType.MaxAsyncRecords),
                new BamlField(BamlFieldType.DebugBamlStream),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // DocumentEnd = 2
            recordId = BamlRecordType.DocumentEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // ElementStart = 3
            recordId = BamlRecordType.ElementStart;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.FlagsByte),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // ElementEnd = 4
            recordId = BamlRecordType.ElementEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // Property = 5
            recordId = BamlRecordType.Property;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.Value),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // PropertyCustom = 6
            recordId = BamlRecordType.PropertyCustom;
            fields = new BamlField[]             // same interface as Property 
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.SerializerTypeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // PropertyComplexStart = 7
            recordId = BamlRecordType.PropertyComplexStart;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // PropertyComplexEnd = 8
            recordId = BamlRecordType.PropertyComplexEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // PropertyArrayStart = 9
            recordId = BamlRecordType.PropertyArrayStart;
            fields = new BamlField[]           // based on ComplexPropertyStart
            {
                new BamlField(BamlFieldType.AttributeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // PropertyArrayEnd = 10
            recordId = BamlRecordType.PropertyArrayEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // PropertyIListStart = 11
            recordId = BamlRecordType.PropertyIListStart;
            fields = new BamlField[]           // based on ComplexPropertyStart
            {
                new BamlField(BamlFieldType.AttributeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // PropertyIListEnd = 12
            recordId = BamlRecordType.PropertyIListEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // PropertyIDictionaryStart = 13
            recordId = BamlRecordType.PropertyIDictionaryStart;
            fields = new BamlField[]           // based on PropertyComplexStart
            {
                new BamlField(BamlFieldType.AttributeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // PropertyIDictionaryEnd = 14
            recordId = BamlRecordType.PropertyIDictionaryEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // LiteralContent = 15
            recordId = BamlRecordType.LiteralContent;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.Value),
                new BamlField(BamlFieldType.LineNumber),
                new BamlField(BamlFieldType.LinePosition),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // Text =16
            recordId = BamlRecordType.Text;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.Value),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // TextWithConverter = 17
            recordId = BamlRecordType.TextWithConverter;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.Value),
                new BamlField(BamlFieldType.ConverterTypeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // RoutedEvent = 18                     UNUSED
            recordId = BamlRecordType.RoutedEvent;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.Value),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // ClrEvent = 19                        NOT IMPLEMENTED in Avalon
            recordId = BamlRecordType.ClrEvent;
            fields = new BamlField[] {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // XmlnsProperty = 20
            recordId = BamlRecordType.XmlnsProperty;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.NamespacePrefix),
                new BamlField(BamlFieldType.Value),
                new BamlField(BamlFieldType.AssemblyIdList),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // XmlAttribute = 21                     NOT IMPLEMENTED in Avalon
            recordId = BamlRecordType.XmlAttribute;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // ProcessingInstruction = 22            NOT IMPLEMENTED in Avalon
            recordId = BamlRecordType.ProcessingInstruction;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // Comment = 23                          NOT IMPLEMENTED in Avalon
            recordId = BamlRecordType.Comment;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // DefTag = 24                           NOT IMPLEMENTED in Avalon
            recordId = BamlRecordType.DefTag;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // DefAttribute = 25
            recordId = BamlRecordType.DefAttribute;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.Value),
                new BamlField(BamlFieldType.NameId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // EndAttributes = 26                    NOT IMPLEMENTED in Avalon
            recordId = BamlRecordType.EndAttributes;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // PIMapping = 27
            recordId = BamlRecordType.PIMapping;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.XmlNamespace),
                new BamlField(BamlFieldType.ClrNamespace),
                new BamlField(BamlFieldType.AssemblyId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // AssemblyInfo = 28
            recordId = BamlRecordType.AssemblyInfo;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AssemblyId),
                new BamlField(BamlFieldType.AssemblyFullName),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // TypeInfo = 29
            recordId = BamlRecordType.TypeInfo;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.AssemblyId),
                new BamlField(BamlFieldType.TypeFullName),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // TypeSerializerInfo = 30
            recordId = BamlRecordType.TypeSerializerInfo;
            fields = new BamlField[]  //                       based on TypeInfo
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.AssemblyId),
                new BamlField(BamlFieldType.TypeFullName),
                new BamlField(BamlFieldType.SerializerTypeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // AttributeInfo = 31
            recordId = BamlRecordType.AttributeInfo;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId, "NewAttributeId"),
                new BamlField(BamlFieldType.TypeId, "OwnerTypeId"),
                new BamlField(BamlFieldType.AttributeUsage),
                new BamlField(BamlFieldType.Value, "NewAttributeValue"),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // StringInfo = 32
            recordId = BamlRecordType.StringInfo;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.StringId),
                new BamlField(BamlFieldType.Value),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Table, fields);

            // PropertyStringReference = 33                 UNUSED
            recordId = BamlRecordType.PropertyStringReference;
            fields = new BamlField[]           // based on PropertyComplexStart
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.StringId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // PropertyTypeReference = 34
            recordId = BamlRecordType.PropertyTypeReference;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.TypeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // PropertyWithExtension = 35
            recordId = BamlRecordType.PropertyWithExtension;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.ExtensionTypeId),
                new BamlField(BamlFieldType.ValueId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // PropertyWithConverter = 36
            recordId = BamlRecordType.PropertyWithConverter;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.Value),
                new BamlField(BamlFieldType.ConverterTypeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // DeferableContentStart = 37
            recordId = BamlRecordType.DeferableContentStart;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.ContentSize),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Table, fields);

            // DefAttributeKeyString = 38
            recordId = BamlRecordType.DefAttributeKeyString;
            fields = new BamlField[]
            {
                // the "value" is not serialized on this record.
                new BamlField(BamlFieldType.ValueId),
                new BamlField(BamlFieldType.ValuePosition),
                new BamlField(BamlFieldType.Shared),
                new BamlField(BamlFieldType.SharedSet),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // DefAttributeKeyType = 39
            recordId = BamlRecordType.DefAttributeKeyType;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.FlagsByte),
                new BamlField(BamlFieldType.ValuePosition),
                new BamlField(BamlFieldType.Shared),
                new BamlField(BamlFieldType.SharedSet),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // KeyElementStart = 40
            recordId = BamlRecordType.KeyElementStart;
            fields = new BamlField[]        // same as DefAttributeKeyType
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.ValuePosition),
                new BamlField(BamlFieldType.FlagsByte),
                new BamlField(BamlFieldType.Shared),
                new BamlField(BamlFieldType.SharedSet),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // KeyElementEnd = 41
            recordId = BamlRecordType.KeyElementEnd;
            fields = new BamlField[]         // same as ElementEnd
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // ConstructorParametersStart=  42
            recordId = BamlRecordType.ConstructorParametersStart;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // ConstructorParametersEnd = 43
            recordId = BamlRecordType.ConstructorParametersEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // ConstructorParameterType = 44
            recordId = BamlRecordType.ConstructorParameterType;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.TypeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // ConnectionId = 45
            recordId = BamlRecordType.ConnectionId;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.ConnectionId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // ContentProperty = 46
            recordId = BamlRecordType.ContentProperty;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // NamedElementStart = 47
            recordId = BamlRecordType.NamedElementStart;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.RuntimeName),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.Start, fields);

            // StaticResourceStart = 48
            recordId = BamlRecordType.StaticResourceStart;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.TypeId),
                new BamlField(BamlFieldType.FlagsByte),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Start, fields);

            // StaticResourceEnd = 49
            recordId = BamlRecordType.StaticResourceEnd;
            fields = new BamlField[]
            {
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.End, fields);

            // StaticResourceId = 50
            recordId = BamlRecordType.StaticResourceId;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.StaticResourceId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // TextWithId = 51
            recordId = BamlRecordType.TextWithId;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.ValueId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // PresentationOptionsAttribute = 52
            recordId = BamlRecordType.PresentationOptionsAttribute;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.Value),
                new BamlField(BamlFieldType.NameId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, true, BamlRecordFlags.None, fields);

            // LineNumber = 53
            recordId = BamlRecordType.LineNumberAndPosition;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.LineNumber),
                new BamlField(BamlFieldType.LinePosition),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Debug, fields);

            // LinePosition = 54
            recordId = BamlRecordType.LinePosition;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.LinePosition),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.Debug, fields);

            // PropertyWithStaticResourceId = 55
            recordId = BamlRecordType.PropertyWithStaticResourceId;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.AttributeId),
                new BamlField(BamlFieldType.StaticResourceId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);

            // OptimizedStaticResource = 56
            recordId = BamlRecordType.OptimizedStaticResource;
            fields = new BamlField[]
            {
                new BamlField(BamlFieldType.FlagsByte),
                new BamlField(BamlFieldType.ValueId),
            };
            _records[(int)recordId] = new BamlRecord(recordId, false, BamlRecordFlags.None, fields);
        }
    }
}
