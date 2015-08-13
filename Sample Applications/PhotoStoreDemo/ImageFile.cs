// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Media.Imaging;

namespace PhotoStoreDemo
{
    public class ImageFile
    {
        public ImageFile(string path)
        {
            Path = path;
            Uri = new Uri(Path);
            Image = BitmapFrame.Create(Uri);
        }

        public string Path { get; }
        public Uri Uri { get; }
        public BitmapFrame Image { get; }

        public override string ToString() => Path;
    }
}