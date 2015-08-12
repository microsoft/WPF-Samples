// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace TextWrapProperty
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

        private void Wrap(object sender, RoutedEventArgs e)
        {
            txt2.TextWrapping = TextWrapping.Wrap;
            txt1.Text = "The TextWrap property is currently set to Wrap.";
        }

        private void NoWrap(object sender, RoutedEventArgs e)
        {
            txt2.TextWrapping = TextWrapping.NoWrap;
            txt1.Text = "The TextWrap property is currently set to NoWrap.";
        }

        private void WrapWithOverflow(object sender, RoutedEventArgs e)
        {
            txt2.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.Text = "The TextWrap property is currently set to WrapWithOverflow.";
        }
    }
}