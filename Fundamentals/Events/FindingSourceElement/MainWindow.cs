// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace FindingSourceElement
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

        private void HandleClick(object sender, RoutedEventArgs e)
        {
            // You must cast the sender object as a Button element, or at least as FrameworkElement, to set Width
            var srcButton = e.Source as Button;
            srcButton.Width = 200;
        }
    }
}