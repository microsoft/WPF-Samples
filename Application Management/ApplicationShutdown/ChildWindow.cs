// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows;

namespace ApplicationShutdown
{
    /// <summary>
    ///     Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        public ChildWindow()
        {
            InitializeComponent();
        }

        private void ChildWindow_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine(@"Closing");
            var result = MessageBox.Show("Allow Shutdown?", "Application Shutdown Sample",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            e.Cancel = (result == MessageBoxResult.No);
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {
            Console.WriteLine(@"Closed");
        }
    }
}