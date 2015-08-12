// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Data;

namespace PhotoViewerDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public PhotoCollection Photos;

        public MainWindow()
        {
            InitializeComponent();
            Photos = (PhotoCollection) (Application.Current.Resources["Photos"] as ObjectDataProvider)?.Data;
            Photos.Path = Environment.CurrentDirectory + "\\images";
        }

        private void OnPhotoClick(object sender, RoutedEventArgs e)
        {
            var pvWindow = new PhotoViewer {SelectedPhoto = (Photo) PhotosListBox.SelectedItem};
            pvWindow.Show();
        }

        private void EditPhoto(object sender, RoutedEventArgs e)
        {
            var pvWindow = new PhotoViewer {SelectedPhoto = (Photo) PhotosListBox.SelectedItem};
            pvWindow.Show();
        }

        private void OnImagesDirChangeClick(object sender, RoutedEventArgs e)
        {
            Photos.Path = ImagesDir.Text;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ImagesDir.Text = Environment.CurrentDirectory + "\\images";
        }
    }
}