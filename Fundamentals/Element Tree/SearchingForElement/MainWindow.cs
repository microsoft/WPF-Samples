// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchingForElement
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Find(object sender, RoutedEventArgs e)
        {
            var wantedNode = stackPanel.FindName("dog");
            if (wantedNode is TextBlock)
            {
                // Following executed if Text element was found.
                var wantedChild = wantedNode as TextBlock;
                wantedChild.Foreground = Brushes.Blue;
            }
        }
    }
}