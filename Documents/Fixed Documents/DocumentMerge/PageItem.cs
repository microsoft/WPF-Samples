// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Documents;
using System.Windows.Media;

namespace DocumentMerge
{
    internal class PageItem
    {
        public PageItem(PageContent pageContent)
        {
            PageContent = pageContent;
            FixedPage = PageContent.GetPageRoot(false);
            PageBrush = new VisualBrush(FixedPage);
        }

        public PageItem(PageItem pageItem)
        {
            PageContent = pageItem.PageContent;
            FixedPage = pageItem.FixedPage;
            PageBrush = new VisualBrush(FixedPage);
        }

        public Brush PageBrush { get; }
        public PageContent PageContent { get; }
        public FixedPage FixedPage { get; }

        #region private data

        #endregion private data
    }
}