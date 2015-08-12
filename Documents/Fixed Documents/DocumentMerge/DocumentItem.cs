// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows.Documents;
using System.Windows.Media;

namespace DocumentMerge
{
    public class DocumentItem
    {
        public DocumentItem(string documentPath, DocumentReference documentReference)
        {
            TooltipString = documentPath;
            DocumentReference = documentReference;
            FixedDocument = documentReference.GetDocument(true);
            var enumerator = FixedDocument.Pages.GetEnumerator();
            enumerator.MoveNext();
            _firstPage = enumerator.Current.GetPageRoot(true);
        }

        public DocumentItem(DocumentItem documentItem)
        {
            _firstPage = documentItem._firstPage;
            TooltipString = documentItem.DocumentPath;
            DocumentReference = documentItem.DocumentReference;
            FixedDocument = documentItem.FixedDocument;
        }

        public DocumentItem(FixedPage fixedPage)
        {
            _firstPage = fixedPage;
        }

        #region Public properties

        public string Info => Path.GetFileName(TooltipString);

        public string TooltipString { get; }

        public Brush PageBrush
        {
            get { return _pageBrush ?? (_pageBrush = new VisualBrush(_firstPage)); }
            set { _pageBrush = value; }
        }

        public DocumentReference DocumentReference { get; }

        public FixedDocument FixedDocument { get; }

        public string DocumentPath => TooltipString;

        #endregion

        #region priavate data

        private readonly FixedPage _firstPage;
        private Brush _pageBrush;

        #endregion private data
    }
}