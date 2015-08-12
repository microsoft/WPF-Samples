// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace HtmlToXamlDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void ConvertContent(object sender, RoutedEventArgs e)
        {
            myTextBox.Text = HtmlToXamlConverter.ConvertHtmlToXaml(myTextBox.Text, true);
            MessageBox.Show("Content Conversion Complete!");
        }

        public void CopyXaml(object sender, RoutedEventArgs e)
        {
            myTextBox.SelectAll();
            myTextBox.Copy();
        }

        public void ConvertContent2(object sender, RoutedEventArgs e)
        {
            myTextBox2.Text = HtmlFromXamlConverter.ConvertXamlToHtml(myTextBox2.Text);
            MessageBox.Show("Content Conversion Complete!");
        }

        public void CopyHtml(object sender, RoutedEventArgs e)
        {
            myTextBox2.SelectAll();
            myTextBox2.Copy();
        }
    }
}