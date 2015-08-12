// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;

namespace DocumentMerge
{
    public class RollUpDocument
    {
        private readonly List<RollUpFixedDocument> _documents;
        private FixedDocumentSequence _fixedDocumentSequence;

        public RollUpDocument()
        {
            _documents = new List<RollUpFixedDocument>();
        }

        public Uri Uri { get; set; }

        public int DocumentCount => _documents.Count;

        public FixedDocumentSequence FixedDocumentSequence
        {
            get
            {
                if (_fixedDocumentSequence == null)
                {
                    ReCreateFixedDocumentSequence();
                }
                return _fixedDocumentSequence;
            }
        }

        public int AddDocument()
        {
            _documents.Add(new RollUpFixedDocument());
            _fixedDocumentSequence = null;
            return DocumentCount - 1;
        }

        public int AddDocument(FixedDocument fixedDocument)
        {
            _documents.Add(new RollUpFixedDocument(fixedDocument));
            _fixedDocumentSequence = null;
            return DocumentCount - 1;
        }

        public int AddDocument(Uri source, Uri uri)
        {
            _documents.Add(new RollUpFixedDocument(source, uri));
            _fixedDocumentSequence = null;
            return DocumentCount - 1;
        }

        public void InsertDocument(int insertAfterDocIndex)
        {
            _documents.Insert(insertAfterDocIndex, new RollUpFixedDocument());
            _fixedDocumentSequence = null;
        }

        public void InsertDocument(int insertAfterDocIndex, FixedDocument fixedDocument)
        {
            _documents.Insert(insertAfterDocIndex, new RollUpFixedDocument(fixedDocument));
            _fixedDocumentSequence = null;
        }

        public void InsertDocument(int insertAfterDocIndex, Uri source, Uri uri)
        {
            _documents.Insert(insertAfterDocIndex, new RollUpFixedDocument(source, uri));
            _fixedDocumentSequence = null;
        }

        public void RemoveDocument(int docIndex)
        {
            _documents.RemoveAt(docIndex);
            _fixedDocumentSequence = null;
        }

        public void AddPage(int docIndex, FixedPage page)
        {
            var rollUpFixedDocument = _documents[docIndex];
            TestForExistingPages(rollUpFixedDocument);
            rollUpFixedDocument.Pages.Add(new RollUpFixedPage(page));
            _fixedDocumentSequence = null;
        }

        private void TestForExistingPages(RollUpFixedDocument rollUpFixedDocument)
        {
            if (rollUpFixedDocument.BaseUri != null)
            {
                rollUpFixedDocument.CreatePagesFromSource();
            }
            else if (rollUpFixedDocument.FixedDocument != null)
            {
                rollUpFixedDocument.CreatePagesFromFixedDocument();
            }
        }

        public void AddPage(int docIndex, Uri source, Uri uri)
        {
            var rollUpFixedDocument = _documents[docIndex];
            TestForExistingPages(rollUpFixedDocument);
            rollUpFixedDocument.Pages.Add(new RollUpFixedPage(source, uri));
            _fixedDocumentSequence = null;
        }

        public void InsertPage(int docIndex, int insertAfterPageIndex, FixedPage page)
        {
            var rollUpFixedDocument = _documents[docIndex];
            TestForExistingPages(rollUpFixedDocument);
            rollUpFixedDocument.Pages.Insert(docIndex, new RollUpFixedPage(page));
            _fixedDocumentSequence = null;
        }

        public void InsertPage(int docIndex, int insertAfterPageIndex, Uri source, Uri uri)
        {
            var rollUpFixedDocument = _documents[docIndex];
            TestForExistingPages(rollUpFixedDocument);
            rollUpFixedDocument.Pages.Insert(insertAfterPageIndex, new RollUpFixedPage(source, uri));
            _fixedDocumentSequence = null;
        }

        public void RemovePage(int docIndex, int pageIndex)
        {
            var rollUpFixedDocument = _documents[docIndex];
            TestForExistingPages(rollUpFixedDocument);
            rollUpFixedDocument.Pages.RemoveAt(pageIndex);
            _fixedDocumentSequence = null;
        }

        public int GetPageCount(int docIndex)
        {
            var fixedDocument = _documents[docIndex];
            if (fixedDocument.BaseUri != null)
            {
                fixedDocument.CreatePagesFromSource();
            }
            return fixedDocument.Pages.Count;
        }

        public PageContent GetPage(int docIndex, int pageIndex)
        {
            var fixedDocument = _documents[docIndex];
            if (fixedDocument.BaseUri != null)
            {
                fixedDocument.CreatePagesFromSource();
            }

            var pageContent = new PageContent();
            var fixedPage = fixedDocument.Pages[pageIndex];
            pageContent.Source = fixedPage.Source;
            ((IUriContext) pageContent).BaseUri = fixedPage.BaseUri;
            return pageContent;
        }

        public void Save()
        {
            if (Uri == null)
            {
                throw new ArgumentException("Uri has not been specified");
            }

            var xpsDocument = new XpsDocument(Uri.OriginalString, FileAccess.ReadWrite);
            var xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            xpsDocumentWriter.Write(FixedDocumentSequence);
            xpsDocument.Close();
        }

        private void ReCreateFixedDocumentSequence()
        {
            _fixedDocumentSequence = new FixedDocumentSequence();
            foreach (var document in _documents)
            {
                var documentReference = new DocumentReference();
                FillDocumentReference(documentReference, document);
                var fixedDocument = documentReference.GetDocument(true);
                _fixedDocumentSequence.References.Add(documentReference);
            }
        }

        private void FillDocumentReference(DocumentReference documentReference, RollUpFixedDocument document)
        {
            if (document.BaseUri != null)
            {
                documentReference.Source = document.Source;
                (documentReference as IUriContext).BaseUri = document.BaseUri;
            }
            else if (document.FixedDocument != null)
            {
                documentReference.SetDocument(document.FixedDocument);
            }
            else
            {
                AddPages(documentReference, document);
            }
        }

        private void AddPages(DocumentReference documentReference, RollUpFixedDocument document)
        {
            var fixedDocument = new FixedDocument();
            documentReference.SetDocument(fixedDocument);
            foreach (var page in document.Pages)
            {
                var pageContent = new PageContent();
                if (page.BaseUri == null)
                {
                    ((IAddChild) pageContent).AddChild(page.FixedPage);
                }
                else
                {
                    pageContent.Source = page.Source;
                    ((IUriContext) pageContent).BaseUri = page.BaseUri;
                }
                pageContent.GetPageRoot(true);
                fixedDocument.Pages.Add(pageContent);
            }
        }
    }
}