// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Documents;

namespace DocumentMerge
{
    /// <summary>
    ///     Maintains data needed to generate a PageConent to be added to a RollUpDocument
    /// </summary>
    public class RollUpFixedPage
    {
        public RollUpFixedPage(FixedPage page)
        {
            FixedPage = page;
        }

        public RollUpFixedPage(Uri source, Uri baseUri)
        {
            Source = source;
            BaseUri = baseUri;
        }

        public FixedPage FixedPage { get; set; }
        public Uri BaseUri { get; set; }
        public Uri Source { get; set; }
    }
}