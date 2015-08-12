// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace FlowContentProperty
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

        public void Lr(object sender, RoutedEventArgs e)
        {
            tf1.FlowDirection = FlowDirection.LeftToRight;
            txt1.Text = "FlowDirection is now " + tf1.FlowDirection;
        }

        public void Rl(object sender, RoutedEventArgs e)
        {
            tf1.FlowDirection = FlowDirection.RightToLeft;
            txt1.Text = "FlowDirection is now " + tf1.FlowDirection;
        }
    }
}