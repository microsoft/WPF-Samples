// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Media.Imaging;

namespace PhotoStoreDemo
{
    public class Shirt : PrintBase
    {
        public Shirt(BitmapSource photo) : base(photo, "T-Shirt", 14.99)
        {
        }
    }
}