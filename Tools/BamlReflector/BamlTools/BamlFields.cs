// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace BamlTools
{
    public enum BamlFieldType : byte
    {
        None = 0,
        BamlVersion,
        LoadAsync,
        MaxAsyncRecords,
        TypeId,
        AttributeId,
        Value,
        ExtensionTypeId,
        SerializerTypeId,
        LineNumber,
        LinePosition,
        ConverterTypeId,
        NamespacePrefix,
        NameId,
        XmlNamespace,
        ClrNamespace,
        AssemblyId,
        AssemblyFullName,
        TypeFullName,
        AttributeUsage,
        StringId,
        ValuePosition,
        ValueId,
        StaticResourceId,
        ContentSize,
        Shared,
        SharedSet,
        ConnectionId,
        RuntimeName,
        FlagsByte,
        DebugBamlStream,
        AssemblyIdList,
        LastFieldType
    }

    public struct BamlField
    {
        private static readonly Type[] _clrTypes;

        private readonly BamlFieldType _fieldType;
        private readonly string _name;
        private object _val;

        public BamlField(BamlFieldType ft, string n)
        {
            _fieldType = ft;
            _name = n;
            _val = null;
        }

        public BamlField(BamlFieldType ft)
        {
            _fieldType = ft;
            _name = null;
            _val = null;
        }

        static BamlField()
        {
            _clrTypes = new Type[(int)BamlFieldType.LastFieldType];

            _clrTypes[(int)BamlFieldType.BamlVersion] = typeof(MockFormatVersion);
            _clrTypes[(int)BamlFieldType.LoadAsync] = typeof(bool);
            _clrTypes[(int)BamlFieldType.MaxAsyncRecords] = typeof(Int32);
            _clrTypes[(int)BamlFieldType.TypeId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.AttributeId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.Value] = typeof(string);
            _clrTypes[(int)BamlFieldType.ExtensionTypeId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.SerializerTypeId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.LineNumber] = typeof(Int32);
            _clrTypes[(int)BamlFieldType.LinePosition] = typeof(Int32);
            _clrTypes[(int)BamlFieldType.ConverterTypeId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.NamespacePrefix] = typeof(string);
            _clrTypes[(int)BamlFieldType.NameId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.XmlNamespace] = typeof(string);
            _clrTypes[(int)BamlFieldType.ClrNamespace] = typeof(string);
            _clrTypes[(int)BamlFieldType.AssemblyId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.AssemblyFullName] = typeof(string);
            _clrTypes[(int)BamlFieldType.TypeFullName] = typeof(string);
            _clrTypes[(int)BamlFieldType.AttributeUsage] = typeof(byte);
            _clrTypes[(int)BamlFieldType.StringId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.ValuePosition] = typeof(Int32);
            _clrTypes[(int)BamlFieldType.ValueId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.StaticResourceId] = typeof(Int16);
            _clrTypes[(int)BamlFieldType.ContentSize] = typeof(Int32);
            _clrTypes[(int)BamlFieldType.Shared] = typeof(bool);
            _clrTypes[(int)BamlFieldType.SharedSet] = typeof(bool);
            _clrTypes[(int)BamlFieldType.ConnectionId] = typeof(Int32);
            _clrTypes[(int)BamlFieldType.RuntimeName] = typeof(string);
            _clrTypes[(int)BamlFieldType.FlagsByte] = typeof(byte);
            _clrTypes[(int)BamlFieldType.DebugBamlStream] = typeof(bool);
            _clrTypes[(int)BamlFieldType.AssemblyIdList] = typeof(Int16[]);

#if DEBUG
            for (int i = 1; i < (int)BamlFieldType.LastFieldType; i++)
            {
                Debug.Assert(_clrTypes[i] != null, "A CLR definition for BamlFieldType " + ((BamlFieldType)i).ToString() + " is missing.  ");
            }
#endif
        }

        public string Name
        {
            get { return _name ?? _fieldType.ToString(); }
        }

        public Type ClrType
        {
            get { return _clrTypes[(int)_fieldType]; }
        }

        public BamlFieldType BamlFieldType
        {
            get { return _fieldType; }
        }

        public object Value
        {
            set { _val = value; }
            get { return _val; }
        }
    }
}
