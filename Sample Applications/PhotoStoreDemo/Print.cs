// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Media.Imaging;

namespace PhotoStoreDemo
{
    public class Print : PrintBase
    {
        public Print(BitmapSource photo) : base(photo, "4x6 Print", 0.15)
        {
        }
    }
}