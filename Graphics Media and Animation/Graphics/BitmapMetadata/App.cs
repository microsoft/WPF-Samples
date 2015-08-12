// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitmapMetadata
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
            _mainWindow = new Window {Title = "Image Metadata"};

            Stream pngStream = new FileStream("smiley.png", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            var pngDecoder = new PngBitmapDecoder(pngStream, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            var pngFrame = pngDecoder.Frames[0];
            var pngInplace = pngFrame.CreateInPlaceBitmapMetadataWriter();
            if (pngInplace.TrySave())
            {
                pngInplace.SetQuery("/Text/Description", "Have a nice day.");
            }
            pngStream.Close();

            // Draw the Image
            var myImage = new Image
            {
                Source = new BitmapImage(new Uri("smiley.png", UriKind.Relative)),
                Stretch = Stretch.None,
                Margin = new Thickness(20)
            };


            // Add the metadata of the bitmap image to the text block.
            var myTextBlock = new TextBlock
            {
                Text =
                    "The Description metadata of this image is: " + pngInplace.GetQuery("/Text/Description")
            };

            // Define a StackPanel to host Controls
            var myStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Height = 200,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Add the Image and TextBlock to the parent Grid
            myStackPanel.Children.Add(myImage);
            myStackPanel.Children.Add(myTextBlock);

            // Add the StackPanel as the Content of the Parent Window Object
            _mainWindow.Content = myStackPanel;
            _mainWindow.Show();
        }
    }

    // Define a static entry class
}