// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Baml2006;
using System.Xaml;
using System.Xml;

namespace BamlTools
{
    public class BamlNodeStreamReader
    {
        private readonly Baml2006Reader _bamlReader;
        private readonly DiagnosticWriter _displayWriter;

        public BamlNodeStreamReader(Stream bamlStream, Assembly localAssembly)
        {
            XamlReaderSettings readerSettings = new XamlReaderSettings
            {
                LocalAssembly = localAssembly,
                ValuesMustBeString = true
            };
            _bamlReader = new Baml2006Reader(bamlStream, readerSettings);
            _displayWriter = new DiagnosticWriter(_bamlReader.SchemaContext);
        }

        public string OneBigString()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                while (_bamlReader.Read())
                {
                    _displayWriter.WriteNode(_bamlReader);
                    sb.AppendLine(_displayWriter.CurrentIndentedNodeText);
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }
            return sb.ToString();
        }
    }

    public class BamlToXamlReader
    {
        private readonly Baml2006Reader _bamlReader;
        XamlXmlWriter _xamlWriter;

        public BamlToXamlReader(Stream bamlStream, Assembly localAssembly)
        {
            XamlReaderSettings readerSettings = new XamlReaderSettings
            {
                LocalAssembly = localAssembly,
                ValuesMustBeString = true
            };
            _bamlReader = new Baml2006Reader(bamlStream, readerSettings);
        }

        public string OneBigString()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };
            XmlWriter xmlWriter = XmlWriter.Create(sb, xmlSettings);

            _xamlWriter = new XamlXmlWriter(xmlWriter, _bamlReader.SchemaContext);
            while (_bamlReader.Read())
            {
                _xamlWriter.WriteNode(_bamlReader);
            }
            xmlWriter.Flush();
            xmlWriter.Close();
            return sb.ToString();
        }
    }
}
