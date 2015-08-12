// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Markup;

namespace DocumentMerge
{
    /// <summary>
    ///     Maintains data needed to generate a DocumentReference to be added to a RollUpDocument
    /// </summary>
    public class RollUpFixedDocument
    {
        public RollUpFixedDocument()
        {
            Pages = new List<RollUpFixedPage>();
        }

        public RollUpFixedDocument(FixedDocument fixedDocument)
            : this()
        {
            FixedDocument = fixedDocument;
        }

        public RollUpFixedDocument(Uri source, Uri baseUri)
            : this()
        {
            Source = source;
            BaseUri = baseUri;
        }

        public FixedDocument FixedDocument { get; set; }
        public List<RollUpFixedPage> Pages { get; set; }
        public Uri BaseUri { get; set; }
        public Uri Source { get; set; }

        internal void CreatePagesFromSource()
        {
            var documentReference = new DocumentReference {Source = Source};
            ((IUriContext) documentReference).BaseUri = BaseUri;
            var fixedDocument = documentReference.GetDocument(true);
            PrivateCreatePagesFromFixedDocument(fixedDocument);
        }

        internal void CreatePagesFromFixedDocument()
        {
            PrivateCreatePagesFromFixedDocument(FixedDocument);
        }

        private void PrivateCreatePagesFromFixedDocument(FixedDocument fixedDocument)
        {
            foreach (var pageContent in fixedDocument.Pages)
            {
                Pages.Add(((IUriContext) pageContent).BaseUri != null
                    ? new RollUpFixedPage(pageContent.Source, ((IUriContext) pageContent).BaseUri)
                    : new RollUpFixedPage(pageContent.Child));
            }
            Source = null;
            BaseUri = null;
        }
    }
}