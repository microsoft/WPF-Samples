using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageScaling
{
    /// <summary>
    /// Image that has built in Dpi awareness to load appropriate scale image, and sets default size appropriately.
    /// </summary>
    public class DpiAwareImage : Image
    {
        double bestScale; // used in calculating default size

        public DpiAwareImage() : base()
        {
            this.Initialized += DpiAwareImage_Initialized;
        }

        // on initial load, ensure we are using the right scaled image, based on the DPI.
        private void DpiAwareImage_Initialized(object sender, EventArgs e)
        {
            DpiScale newDpi = VisualTreeHelper.GetDpi(sender as Visual);
            ScaleRightImage(newDpi);
        }
        
        // when DPI changes, ensure we are using the right scaled image, based on the DPI.
        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            ScaleRightImage(newDpi);
        }

        private void ScaleRightImage(DpiScale newDpi)
        {
            // update bestScale
            bestScale = ImageDpiHelper.GetBestScale(newDpi.PixelsPerDip);

            string imageUrl = ImageDpiHelper.GetDesiredImageUrlForDpi(this);
            UpdateImageSource(this, imageUrl);
        }

        public void UpdateImageSource(Image image, string newImagePath)
        {
            Uri uri = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            BitmapImage bitmapImage = new BitmapImage();
            
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            image.Source = bitmapImage;

            if (!bitmapImage.IsDownloading)
            {
                SetScaledSizeForImageElement(bitmapImage);
            }
            else
            {
                bitmapImage.DownloadCompleted += BitmapImage_DownloadCompleted;
            }
        }

        private void BitmapImage_DownloadCompleted(object sender, EventArgs e)
        {
            BitmapImage bitmapImage = sender as BitmapImage;
            SetScaledSizeForImageElement(bitmapImage);
            bitmapImage.DownloadCompleted -= BitmapImage_DownloadCompleted;
        }

        // based on the scale of the image used, adjust the default value of width/height.
        private void SetScaledSizeForImageElement(BitmapImage bitmapImage)
        {
            this.SetCurrentValue(Image.WidthProperty, bitmapImage.PixelWidth * 100 / bestScale);
            this.SetCurrentValue(Image.HeightProperty, bitmapImage.PixelHeight * 100 / bestScale);
        }
    }
}

