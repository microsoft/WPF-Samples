// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xaml;

namespace BamlTools
{
    public class NullWrappingXamlReader : XamlReader
    {
        XamlReader _wrappedReader;

        XamlNodeQueue _pipe;

        // cached from the PIPE.
        XamlReader _reader;
        XamlWriter _writer;

        bool _done;

        public NullWrappingXamlReader(XamlReader wrappedReader)
        {
            _wrappedReader = wrappedReader;

            _pipe = new XamlNodeQueue(wrappedReader.SchemaContext);
            _reader = _pipe.Reader;
            _writer = _pipe.Writer;
        }

        public override bool Read()
        {
            if (_done)
            {
                return false;
            }

            if (!_wrappedReader.Read())
            {
                _done = true;
            }

            _writer.WriteNode(_wrappedReader);
            _reader.Read();
            return true;
        }

        public override XamlNodeType NodeType
        {
            get { return _reader.NodeType; }
        }

        public override bool IsEof
        {
            get { return _reader.IsEof; }
        }

        public override NamespaceDeclaration Namespace
        {
            get { return _reader.Namespace; }
        }

        public override XamlType Type
        {
            get { return _reader.Type; }
        }

        public override object Value
        {
            get { return _reader.Value; }
        }

        public override XamlMember Member
        {
            get { return _reader.Member; }
        }

        public override XamlSchemaContext SchemaContext
        {
            get { return _reader.SchemaContext; }
        }
    }
}
