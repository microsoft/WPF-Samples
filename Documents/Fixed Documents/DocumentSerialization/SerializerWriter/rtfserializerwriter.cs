using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Documents.Serialization;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Printing;
using System.Windows;
using System.Xml;

namespace DocumentSerialization
{
    class RtfSerializerWriter : SerializerWriter
    {
        public RtfSerializerWriter(Stream stream)
        {
            _stream = stream;
        }
        /// <summary>
        /// Write a single Visual and close package
        /// </summary>
        public override void Write(Visual visual)
        {
            Write(visual, null);
        }

        /// <summary>
        /// Write a single Visual and close package
        /// </summary>
        public override void Write(Visual visual, PrintTicket printTicket)
        {
            SerializeObjectTree(visual);
        }

        /// <summary>
        /// Asynchronous Write a single Visual and close package
        /// </summary>
        public override void WriteAsync(Visual visual)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single Visual and close package
        /// </summary>
        public override void WriteAsync(Visual visual, object userState)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Asynchronous Write a single Visual and close package
        /// </summary>
        public override void WriteAsync(Visual visual, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Asynchronous Write a single Visual and close package
        /// </summary>
        public override void WriteAsync(Visual visual, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write a single DocumentPaginator and close package
        /// </summary>
        public override void Write(DocumentPaginator documentPaginator)
        {
            Write(documentPaginator, null);
        }

        /// <summary>
        /// Write a single DocumentPaginator and close package
        /// </summary>
        public override void Write(DocumentPaginator documentPaginator, PrintTicket printTicket)
        {
            SerializeObjectTree(documentPaginator.Source);
        }

        /// <summary>
        /// Asynchronous Write a single DocumentPaginator and close package
        /// </summary>
        public override void WriteAsync(DocumentPaginator documentPaginator)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single DocumentPaginator and close package
        /// </summary>
        public override void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single DocumentPaginator and close package
        /// </summary>
        public override void WriteAsync(DocumentPaginator documentPaginator, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single DocumentPaginator and close package
        /// </summary>
        public override void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write a single FixedPage and close package
        /// </summary>
        public override void Write(FixedPage fixedPage)
        {
            Write(fixedPage, null);
        }

        /// <summary>
        /// Write a single FixedPage and close package
        /// </summary>
        public override void Write(FixedPage fixedPage, PrintTicket printTicket)
        {
            SerializeObjectTree(fixedPage);
        }

        /// <summary>
        /// Asynchronous Write a single FixedPage and close package
        /// </summary>
        public override void WriteAsync(FixedPage fixedPage)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedPage and close package
        /// </summary>
        public override void WriteAsync(FixedPage fixedPage, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedPage and close package
        /// </summary>
        public override void WriteAsync(FixedPage fixedPage, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedPage and close package
        /// </summary>
        public override void WriteAsync(FixedPage fixedPage, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Write a single FixedDocument and close package
        /// </summary>
        public override void Write(FixedDocument fixedDocument)
        {
            Write(fixedDocument, null);
        }

        /// <summary>
        /// Write a single FixedDocument and close package
        /// </summary>
        public override void Write(FixedDocument fixedDocument, PrintTicket printTicket)
        {
            SerializeObjectTree(fixedDocument);
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocument and close package
        /// </summary>
        public override void WriteAsync(FixedDocument fixedDocument)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocument and close package
        /// </summary>
        public override void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocument and close package
        /// </summary>
        public override void WriteAsync(FixedDocument fixedDocument, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocument and close package
        /// </summary>
        public override void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Write a single FixedDocumentSequence and close package
        /// </summary>
        public override void Write(FixedDocumentSequence fixedDocumentSequence)
        {
            Write(fixedDocumentSequence, null);
        }

        /// <summary>
        /// Write a single FixedDocumentSequence and close package
        /// </summary>
        public override void Write(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket)
        {
            SerializeObjectTree(fixedDocumentSequence);
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocumentSequence and close package
        /// </summary>
        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocumentSequence and close package
        /// </summary>
        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocumentSequence and close package
        /// </summary>
        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single FixedDocumentSequence and close package
        /// </summary>
        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Cancel Asynchronous Write
        /// </summary>
        public override void CancelAsync()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Create a SerializerWriterCollator to gobble up multiple Visuals
        /// </summary>
        public override SerializerWriterCollator CreateVisualsCollator()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Create a SerializerWriterCollator to gobble up multiple Visuals
        /// </summary>
        public override SerializerWriterCollator CreateVisualsCollator(PrintTicket documentSequencePT, PrintTicket documentPT)
        {
            throw new NotSupportedException();
        }

#pragma warning disable 0067

        /// <summary>
        /// This event will be invoked if the writer wants a PrintTicker
        /// </summary>
        public override event WritingPrintTicketRequiredEventHandler WritingPrintTicketRequired;

        /// <summary>
        /// This event will be invoked if the writer progress changes
        /// </summary>
        public override event WritingProgressChangedEventHandler WritingProgressChanged;

        /// <summary>
        /// This event will be invoked if the writer is done
        /// </summary>
        public override event WritingCompletedEventHandler WritingCompleted;

        /// <summary>
        /// This event will be invoked if the writer has been cancelled
        /// </summary>
        public override event WritingCancelledEventHandler WritingCancelled;

#pragma warning restore 0067

        private void SerializeObjectTree(object objectTree)
        {
            TextWriter writer = new StreamWriter(_stream);

            try
            {
                string fileContent = XamlRtfConverter.ConvertXamlToRtf(
                                           XamlWriter.Save(objectTree));
                writer.Write(fileContent);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        private Stream _stream;

    }
}
