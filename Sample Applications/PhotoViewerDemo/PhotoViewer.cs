// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     Interaction logic for PhotoViewer.xaml
    /// </summary>
    public partial class PhotoViewer : Window
    {
        public PhotoViewer()
        {
            InitializeComponent();
        }

        public Photo SelectedPhoto { get; set; }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ViewedPhoto.Source = SelectedPhoto.Image;
            ViewedCaption.Content = SelectedPhoto.Source;
        }

        private void Rotate(object sender, RoutedEventArgs e)
        {
            BitmapSource img = SelectedPhoto.Image;

            var cache = new CachedBitmap(img, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            SelectedPhoto.Image = BitmapFrame.Create(new TransformedBitmap(cache, new RotateTransform(90.0)));

            ViewedPhoto.Source = SelectedPhoto.Image;
        }

        private void Crop(object sender, RoutedEventArgs e)
        {
            BitmapSource img = SelectedPhoto.Image;

            var halfWidth = img.PixelWidth/2;
            var halfHeight = img.PixelHeight/2;
            SelectedPhoto.Image =
                BitmapFrame.Create(new CroppedBitmap(img,
                    new Int32Rect((halfWidth - (halfWidth/2)), (halfHeight - (halfHeight/2)), halfWidth, halfHeight)));

            ViewedPhoto.Source = SelectedPhoto.Image;
        }

        private void BlackAndWhite(object sender, RoutedEventArgs e)
        {
            BitmapSource img = SelectedPhoto.Image;
            SelectedPhoto.Image =
                BitmapFrame.Create(new FormatConvertedBitmap(img, PixelFormats.Gray8, BitmapPalettes.Gray256, 1.0));

            ViewedPhoto.Source = SelectedPhoto.Image;
        }
    }
}