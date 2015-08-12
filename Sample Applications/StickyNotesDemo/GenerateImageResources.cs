using System;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Controls;

namespace StickyNotesDemo.GenApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourceWriter myResource = new ResourceWriter("Images.resources");
            myResource.AddResource("flash", new Bitmap("flashScreen.png"));
            Image simpleImage = new Image();
            simpleImage.Margin = new Thickness(0);

            BitmapImage bi = new BitmapImage();
            //BitmapImage.UriSource must be in a BeginInit/EndInit block
            bi.BeginInit();





            bi.UriSource = new Uri(@"pack://siteoforigin:,,,/alarm3.png");
            bi.EndInit();
            //set image source
            simpleImage.Source = bi;
            //        simpleImage.Stretch = Stretch.None;
            simpleImage.HorizontalAlignment = HorizontalAlignment.Center;
            simpleImage.Visibility = Visibility.Hidden;
            simpleImage.Name = "AlarmIndicator";
            simpleImage.Width = 13;


            myResource.AddResource("alarm", new Image("alarm3.png"));
            myResource.Close(); 


        }
    }
}
