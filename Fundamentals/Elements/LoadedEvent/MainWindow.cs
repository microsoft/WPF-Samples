// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace LoadedEvent
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

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            var b1 = new Button {Content = "New Button"};
            root.Children.Add(b1);
            b1.Height = 25;
            b1.Width = 200;
            b1.HorizontalAlignment = HorizontalAlignment.Left;
        }
    }
}