// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Windows;

namespace WindowHiding
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChildWindow _childWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowChildWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the window if it's not already created
            if (_childWindow == null)
                _childWindow = new ChildWindow();

            // Show the window
            _childWindow.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Close the child window only when this window closes
            _childWindow.Close();
        }
    }
}