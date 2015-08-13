// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventOnGainAndLooseFocus
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

        private void OnGotFocusHandler(object sender, RoutedEventArgs e)
        {
            var tb = e.Source as Button;
            tb.Background = Brushes.Red;
        }

        // Raised when Button losses focus. 
        // Changes the color of the Button back to white.
        private void OnLostFocusHandler(object sender, RoutedEventArgs e)
        {
            var tb = e.Source as Button;
            tb.Background = Brushes.White;
        }
    }
}