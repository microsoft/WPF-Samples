// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

[assembly: SecurityTransparent]

namespace BMPEncoderAndDecoder
{
    public class App : Application
    {
        private Window _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateAndShowMainWindow();
        }

        private void CreateAndShowMainWindow()
        {
            // Create the application's main window
            _mainWindow = new Window {Title = "BMP Imaging Sample"};
            var mySv = new ScrollViewer();

            var width = 128;
            var height = width;
            var stride = width/8;
            var pixels = new byte[height*stride];

            // Try creating a new image with a custom palette.
            var colors = new List<Color>
            {
                Colors.Red,
                Colors.Blue,
                Colors.Green
            };
            var myPalette = new BitmapPalette(colors);

            // Creates a new empty image with the pre-defined palette
            var image = BitmapSource.Create(
                width,
                height,
                96,
                96,
                PixelFormats.Indexed1,
                myPalette,
                pixels,
                stride);

            var stream = new FileStream("new.bmp", FileMode.Create);
            var encoder = new BmpBitmapEncoder();
            var myTextBlock = new TextBlock {Text = "Codec Author is: " + encoder.CodecInfo.Author};
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);


            // Open a Stream and decode a BMP image
            Stream imageStreamSource = new FileStream("tulipfarm.bmp", FileMode.Open, FileAccess.Read, FileShare.Read);
            var decoder = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            // Draw the Image
            var myImage = new Image
            {
                Source = bitmapSource,
                Stretch = Stretch.None,
                Margin = new Thickness(20)
            };


            // Open a Uri and decode a BMP image
            var myUri = new Uri("tulipfarm.bmp", UriKind.RelativeOrAbsolute);
            var decoder2 = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            // Draw the Image
            var myImage2 = new Image
            {
                Source = bitmapSource2,
                Stretch = Stretch.None,
                Margin = new Thickness(20)
            };

            // Define a StackPanel to host the decoded BMP images
            var myStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Add the Image and TextBlock to the parent Grid
            myStackPanel.Children.Add(myImage);
            myStackPanel.Children.Add(myImage2);
            myStackPanel.Children.Add(myTextBlock);

            // Add the StackPanel as the Content of the Parent Window Object
            mySv.Content = myStackPanel;
            _mainWindow.Content = mySv;
            _mainWindow.Show();
        }
    }

    // Define a static entry class
}