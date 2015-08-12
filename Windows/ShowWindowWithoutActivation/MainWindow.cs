// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace ShowWindowWithoutActivation
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

        private void ShowWindowActivateButton_Click(object sender, RoutedEventArgs e)
        {
            var tw = new ChildWindow();
            // tw.ShowActivated = true; // true is the default value
            tw.Show();
        }

        private void ShowWindowNoActivateButton_Click(object sender, RoutedEventArgs e)
        {
            var tw = new ChildWindow {ShowActivated = false};
            tw.Show();
        }

        // This option doesn't make sense
        private void ShowDialogBoxActivateButton_Click(object sender, RoutedEventArgs e)
        {
            var tw = new ChildWindow();
            // tw.ShowActivated = true; // true is the default value
            tw.ShowDialog();
        }

        // This option doesn't make sense
        private void ShowDialogBoxNoActivateButton_Click(object sender, RoutedEventArgs e)
        {
            var tw = new ChildWindow {ShowActivated = false};
            tw.ShowDialog();
        }
    }
}