// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace VisibiltyChanges
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

        private void ContentVis(object sender, RoutedEventArgs e)
        {
            tb1.Visibility = Visibility.Visible;
            txt1.Text = "Visibility is now set to Visible.";
        }

        private void ContentHid(object sender, RoutedEventArgs e)
        {
            tb1.Visibility = Visibility.Hidden;
            txt1.Text = "Visibility is now set to Hidden. Notice that the TextBox still occupies layout space.";
        }

        private void ContentCol(object sender, RoutedEventArgs e)
        {
            tb1.Visibility = Visibility.Collapsed;
            txt1.Text = "Visibility is now set to Collapsed. Notice that the TextBox no longer occupies layout space.";
        }
    }
}