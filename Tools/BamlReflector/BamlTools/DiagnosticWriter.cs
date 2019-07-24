// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using System.Xaml;

namespace BamlTools
{
    public class DiagnosticWriter : XamlWriter, IXamlLineInfoConsumer
    {
        const string _nullString = "*null*";

        XamlSchemaContext _schemaContext;
        String _nodeText;
        String _indentedNodeText;
        int _indent;
        string _indentUnit = " . ";
        StringBuilder _stringbuilder;
        XamlWriter _wrappedWriter = null;

        IXamlLineInfoConsumer _wrappedWriterLineInfoConsumer = null;
        int _lineNumber;
        int _linePosition;
        bool _lineInfoIsNew;

        public DiagnosticWriter(XamlSchemaContext schemaContext)
        {
            _schemaContext = schemaContext;
            _nodeText = String.Empty;
            _indent = 0;

            _stringbuilder = null;
            _wrappedWriter = null;
            _wrappedWriterLineInfoConsumer = null;
        }

        public DiagnosticWriter(StringBuilder stringbuilder, XamlSchemaContext schemaContext)
            : this(schemaContext)
        {
            _stringbuilder = stringbuilder;
        }

        public DiagnosticWriter(StringBuilder stringbuilder, XamlWriter wrappedWriter, XamlSchemaContext xamlSchemaContext)
            : this(stringbuilder, xamlSchemaContext)
        {
            _wrappedWriter = wrappedWriter;
            _wrappedWriterLineInfoConsumer = _wrappedWriter as IXamlLineInfoConsumer;
        }

        public String IndentUnit
        {
            get { return _indentUnit; }
            set { _indentUnit = value; }
        }

        // ------ XamlWriter implementation  -------------

        public override XamlSchemaContext SchemaContext
        {
            get { return _schemaContext; }
        }

        public override void WriteNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            CurrentNodeText = String.Format("NS prefix={0} Uri={1}", namespaceDeclaration.Prefix, namespaceDeclaration.Namespace);
            BuildString();

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteNamespace(namespaceDeclaration);
            }
        }

        public override void WriteStartObject(XamlType type)
        {
            CurrentNodeText = String.Format("SO {0}", type.Name);
            BuildString();
            _indent += 1;

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteStartObject(type);
            }
        }

        public override void WriteGetObject()
        {
            CurrentNodeText = "GO";
            BuildString();
            _indent += 1;

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteGetObject();
            }
        }

        public override void WriteEndObject()
        {
            _indent -= 1;
            CurrentNodeText = "EO";
            BuildString();

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteEndObject();
            }
        }

        public override void WriteStartMember(XamlMember xamlMember)
        {
            CurrentNodeText = String.Format("SM {0}", xamlMember.Name);
            BuildString();
            _indent += 1;

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteStartMember(xamlMember);
            }
        }

        public override void WriteEndMember()
        {
            _indent -= 1;
            CurrentNodeText = "EM";
            BuildString();

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteEndMember();
            }
        }

        public override void WriteValue(object value)
        {
            string valueString = value as string;
            if (value == null)  // null values
            {
                valueString = _nullString;
            }
            else if (valueString != null)  // string values
            {
                if (ShowLinebreaks)
                {
                    valueString = valueString.Replace("\n", "\\n");
                }
            }
            else  // Non-string Values
            {
                valueString = String.Format("({0}) [{1}]", value.ToString(), value.GetType().Name);
            }

            CurrentNodeText = String.Format("V '{0}'", valueString);
            BuildString();

            if (_wrappedWriter != null)
            {
                _wrappedWriter.WriteValue(value);
            }
        }

        // --------------------------

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && !IsDisposed)
                {
                    CurrentNodeText = "Closed.";
                    BuildString();

                    if (_wrappedWriter != null)
                    {
                        _wrappedWriter.Close();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public object Result
        {
            get
            {
                XamlObjectWriter objectWriter = _wrappedWriter as XamlObjectWriter;
                if (objectWriter != null)
                {
                    return objectWriter.Result;
                }
                else
                {
                    return null;
                }
            }
        }

        #region IXamlLineInfoConsumer Members

        public void SetLineInfo(int lineNumber, int linePosition)
        {
            {
                _lineNumber = lineNumber;
                _linePosition = linePosition;
                _lineInfoIsNew = true;

                if (_wrappedWriterLineInfoConsumer != null && _wrappedWriterLineInfoConsumer.ShouldProvideLineInfo)
                {
                    _wrappedWriterLineInfoConsumer.SetLineInfo(lineNumber, linePosition);
                }
            }
        }

        public bool ShouldProvideLineInfo
        {
            get { return true; }
        }

        #endregion

        public string LineInfoString
        {
            get
            {
                if (_lineInfoIsNew)
                {
                    _lineInfoIsNew = false;
                    return String.Format("   ({0},{1})", _lineNumber, _linePosition);
                }
                return String.Empty;
            }
        }

        // -----------------------------

        public String CurrentNodeText
        {
            get { return _nodeText; }

            private set
            {
                _nodeText = value;
                if (_lineInfoIsNew)
                {
                    _nodeText += LineInfoString;
                }
            }
        }

        public int IndentLevel
        {
            get { return _indent; }
        }

        public String CurrentIndentedNodeText
        {
            get
            {
                return _indentedNodeText;
            }
        }

        public bool ShowLinebreaks { set; get; }

        private string CurrentIndentString
        {
            get
            {
                string indent = String.Empty;
                for (int i = 0; i < _indent; i += 1)
                {
                    indent += _indentUnit;
                }
                return indent;
            }
        }

        private void BuildString()
        {
            _indentedNodeText = CurrentIndentString + CurrentNodeText;
            if (_stringbuilder != null)
            {
                _stringbuilder.AppendLine(_indentedNodeText);
            }
        }

    }
}
