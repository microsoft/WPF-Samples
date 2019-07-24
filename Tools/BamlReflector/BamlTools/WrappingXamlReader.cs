// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xaml;

namespace BamlTools
{
    // Provides the 140 lines of boilerplate code that
    // will return infoset nodes from either a wrapped _reader
    // or from an XamlNodeQueue.

    // There is nothing in here the user could not write herself.

    abstract public class WrappingXamlReader : XamlReader
    {
        XamlNodeQueue _xamlNodeQueue;
        XamlReader _wrappedReader;
        bool _skipCurrentNode;

        public WrappingXamlReader(XamlSchemaContext xamlSchemaContext)
        {
            _xamlNodeQueue = new XamlNodeQueue(xamlSchemaContext);
        }

        public XamlNodeQueue XamlNodeQueue
        {
            get { return _xamlNodeQueue; }
        }

        protected void SetWrappedReader(XamlReader reader)
        {
            _wrappedReader = reader;
        }

        protected bool SkipCurrentNode
        {
            get { return _skipCurrentNode; }
            set { _skipCurrentNode = value; }
        }

        #region XamlReader Members

        // The Read method is left for the derived class to implement

        // abstract public bool Read();

        public override XamlNodeType NodeType
        {
            get
            {
                if (_skipCurrentNode)
                {
                    return _xamlNodeQueue.Reader.NodeType;
                }
                else
                {
                    return _wrappedReader.NodeType;
                }
            }
        }

        public override bool IsEof
        {
            get
            {
                if (_skipCurrentNode)
                {
                    return _xamlNodeQueue.Reader.IsEof;
                }
                else
                {
                    return _wrappedReader.IsEof;
                }
            }
        }


        public override NamespaceDeclaration Namespace
        {
            get
            {
                if (_skipCurrentNode)
                {
                    return _xamlNodeQueue.Reader.Namespace;
                }
                else
                {
                    return _wrappedReader.Namespace;
                }
            }
        }

        public override XamlType Type
        {
            get
            {
                if (_skipCurrentNode)
                {
                    return _xamlNodeQueue.Reader.Type;
                }
                else
                {
                    return _wrappedReader.Type;
                }
            }
        }

        public override object Value
        {
            get
            {
                if (_skipCurrentNode)
                {
                    return _xamlNodeQueue.Reader.Value;
                }
                else
                {
                    return _wrappedReader.Value;
                }
            }
        }

        public override XamlMember Member
        {
            get
            {
                if (_skipCurrentNode)
                {
                    return _xamlNodeQueue.Reader.Member;
                }
                else
                {
                    return _wrappedReader.Member;
                }
            }
        }

        public override XamlSchemaContext SchemaContext
        {
            get { return _wrappedReader.SchemaContext; }
        }

        #endregion
    }
}
