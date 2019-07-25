// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xaml;

namespace BamlTools
{
    public class NullWriter : XamlWriter
    {
        public NullWriter(XamlSchemaContext xamlSchemaContext)
        {
            this._xamlSchemaContext = xamlSchemaContext;
        }

        public override void WriteStartObject(XamlType type)
        {
        }

        public override void WriteGetObject()
        {
        }

        public override void WriteEndObject()
        {
        }

        public override void WriteStartMember(XamlMember property)
        {
        }

        public override void WriteEndMember()
        {
        }

        public override void WriteValue(object value)
        {
        }

        public override void WriteNamespace(NamespaceDeclaration namespaceDeclaration)
        {
        }

        private XamlSchemaContext _xamlSchemaContext;
        public override XamlSchemaContext SchemaContext
        {
            get
            {
                return this._xamlSchemaContext;
            }
        }
    }
}
