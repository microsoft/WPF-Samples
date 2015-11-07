using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageScaling
{
    public static class ImageDpiHelper
    {
        public static string GetDesiredImageUrlForDpi(Image image)
        {
            DpiScaleInfo imageScaleInfo = VisualTreeHelper.GetDpi(image);
            int bestScale = ImageDpiHelper.GetBestScale(imageScaleInfo.PixelsPerDip);

            var sourceUrl = image.Source.ToString();
            string imagePattern = Regex.Replace(sourceUrl, ".scale-[0-9]{3}.", ".scale-{0}.");
            //string imagePattern = sourceUrl.Replace(".scale-100.", ".scale-{0}.");

            string newImagePath = null;
            if (imagePattern != null)
            {
                newImagePath = string.Format(imagePattern, bestScale);
            }

            return newImagePath;
        }

        public static int GetBestScale(double currentPixelsPerDip)
        {
            int currentScale = (int)(currentPixelsPerDip * 100);
            int bestScale;

            if (currentScale > 200)
            {
                bestScale = 400;
            }
            else if (currentScale > 100)
            {
                bestScale = 200;
            }
            else
            {
                bestScale = 100;
            }

            return bestScale;
        }

        public static void UpdateImageSource(Image image, string newImagePath)
        {
            Uri uri = new Uri(newImagePath, UriKind.RelativeOrAbsolute);
            //TODO: need to make this be a relative URI so it works with F5...other wise the component in the path breaks with Foo.vshost.exe
            BitmapImage src = new BitmapImage(uri);

            image.Source = src;
        }
    }
}
