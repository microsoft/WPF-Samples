// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Media.Imaging;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     This class describes a single photo - its location, the image and
    ///     the metadata extracted from the image.
    /// </summary>
    public class Photo
    {
        private readonly Uri _source;

        public Photo(string path)
        {
            Source = path;
            _source = new Uri(path);
            Image = BitmapFrame.Create(_source);
            Metadata = new ExifMetadata(_source);
        }

        public string Source { get; }
        public BitmapFrame Image { get; set; }
        public ExifMetadata Metadata { get; }

        public override string ToString() => _source.ToString();
    }
}