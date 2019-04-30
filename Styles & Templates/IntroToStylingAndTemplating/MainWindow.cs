// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Data;

namespace IntroToStylingAndTemplating
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhotoList _photos;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _photos = (PhotoList) (Resources["MyPhotos"] as ObjectDataProvider).Data;
            _photos.Path = "..\\..\\Images";
        }
    }
}