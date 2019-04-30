// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageView
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArrayList _imageFiles;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            _imageFiles = GetImageFileInfo();
            imageListBox.DataContext = _imageFiles;
        }

        private void ShowImage(object sender, SelectionChangedEventArgs args)
        {
            var list = ((ListBox) sender);
            var index = list?.SelectedIndex; //Save the selected index 
            if (index >= 0)
            {
                var selection = list.SelectedItem.ToString();

                if (!string.IsNullOrEmpty(selection))
                {
                    //Set currentImage to selected Image
                    var selLoc = new Uri(selection);
                    var id = new BitmapImage(selLoc);
                    var currFileInfo = new FileInfo(selection);
                    currentImage.Source = id;

                    //Setup Info Text
                    imageSize.Text = id.PixelWidth + " x " + id.PixelHeight;
                    imageFormat.Text = id.Format.ToString();
                    fileSize.Text = ((currFileInfo.Length + 512)/1024) + "k";
                }
            }
        }

        private ArrayList GetImageFileInfo()
        {
            var imageFiles = new ArrayList();

            //Get directory path of myData
            var currDir = Directory.GetCurrentDirectory();
            var temp = currDir + "\\myData";
            var files = Directory.GetFiles(temp, "*.jpg");

            foreach (var image in files)
            {
                var info = new FileInfo(image);
                imageFiles.Add(info);
            }

            //imageFiles.Sort();

            return imageFiles;
        }
    }
}