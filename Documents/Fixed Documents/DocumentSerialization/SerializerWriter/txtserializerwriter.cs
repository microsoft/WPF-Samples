using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Documents.Serialization;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;

namespace DocumentSerialization
{
    class TxtSerializerWriter:SerializerWriter
    {
        public TxtSerializerWriter(Stream stream)
        {
            _stream = stream;
            _writer = new StreamWriter(_stream);
        }
        /// <summary>
        /// Write a single DependencyObject and close package
        /// </summary>
        public override void Write(Visual visual)
        {
            Write(visual, null);
        }

        /// <summary>
        /// Write a single DependencyObject and close package
        /// </summary>
        public override void Write(Visual visual, PrintTicket printTicket)
        {
            SerializeVisualTree(visual);
            _writer.Close();
        }

        /// <summary>
        /// Asynchronous Write a single DependencyObject and close package
        /// </summary>
        public override void WriteAsync(Visual visual)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Asynchronous Write a single DependencyObject and close package
        /// </summary>
        public override void WriteAsync(Visual visual, object userState)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Asynchronous Write a single DependencyObject and close package
        /// </summary>
        public override void WriteAsync(Visual visual, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Asynchronous Write a single DependencyObject and close package
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
            //SerializeVisualTree(documentPaginator.Source as DependencyObject);
            for( int i = 0; i < documentPaginator.PageCount; i++ )
            {
                DocumentPage page = documentPaginator.GetPage(i);
                 SerializeVisualTree(page.Visual);
            }
            _writer.Close();
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
            SerializeVisualTree(fixedPage);
            _writer.Close();
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
            Write(fixedDocument.DocumentPaginator, printTicket);
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
            Write(fixedDocumentSequence.DocumentPaginator, printTicket);
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

        private void SerializeVisualTree(DependencyObject objectTree)
        {
            List<GlyphRun> glyphrunList = new List<GlyphRun>();
            WalkVisualTree(glyphrunList, objectTree);

  
            try
            {
                // NOTE:  this is not gaurenteed to get the text in any reasonable order
                // To correct this the transforms associated with the parent drawing groups
                // can be collected and the glyph runs sorted into text blocks
                // This is a complex algorythem out side the scope of this sampel
                foreach (GlyphRun glyphRun in glyphrunList)
                {
                    StringBuilder builder = new StringBuilder(glyphRun.Characters.Count);
                    foreach (char ch in glyphRun.Characters)
                    {
                        builder.Append(ch);
                    }

                    _writer.Write(builder.ToString());
                    _writer.Write(" ");
                }
            }
            finally
            {
            }

        }

        private void WalkVisualTree(List<GlyphRun> textObjects, DependencyObject treeObject)
        {
            System.Windows.Media.Drawing content = VisualTreeHelper.GetDrawing(treeObject as Visual);
            BuildGlypeTree(textObjects, content);
            int childCount = VisualTreeHelper.GetChildrenCount( treeObject );
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(treeObject, i);
                WalkVisualTree(textObjects, child);
            }

        }
        private void BuildGlypeTree(List<GlyphRun> textObjects, System.Windows.Media.Drawing drawing)
        {
            if (drawing is GlyphRunDrawing)
            {
                textObjects.Add((drawing as GlyphRunDrawing).GlyphRun);
            }
            else if (drawing is DrawingGroup)
            {
                DrawingCollection children = (drawing as DrawingGroup).Children;
                for (int i = 0; i < children.Count; i++)
                {
                    BuildGlypeTree(textObjects, children[i]);
                }

            }
        }

 
        private string GetTextFromVisual(DependencyObject visual)
        {
            string result = "";

            if (visual is TextBlock)
            {
                result = (visual as TextBlock).Text;
            }
            else if (visual is TextBox)
            {
                result = (visual as TextBox).Text;
            }
            else if (visual is FlowDocument)
            {
                FlowDocument flowDocument = (visual as FlowDocument);
                result = new TextRange(flowDocument.ContentStart,
                                                flowDocument.ContentEnd).Text;;
            }
            else if (visual is Glyphs)
            {
                result = (visual as Glyphs).UnicodeString;
            }
            return result;
        }
 
            
        private Stream _stream;
        private TextWriter _writer;
    }

    class CompareByLocation : IComparer<GlyphRun>
    {
        public int Compare(GlyphRun a, GlyphRun b)
        {
            Point aPoint = a.BaselineOrigin;
            Point bPoint = b.BaselineOrigin;
            int result = 0;
            //Vertical takes priorty over horizontal
            if (aPoint.Y > bPoint.Y)
            {
                return -1;
            }
            else if (aPoint.Y > bPoint.Y)
            {
                result = 1;
            }
            else if (aPoint.X < bPoint.X)
            {
                result = -1;
            }
            else if (aPoint.X > bPoint.X)
            {
                result = 1;
            }
             return result;
        }
        
    }
}
