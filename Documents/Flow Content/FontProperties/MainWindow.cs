// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;

namespace FontProperties
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

        public void OnClick1(object sender, RoutedEventArgs e)
        {
            txt1.Text = "The FontFamily is set to Arial.";
            txt2.FontFamily = new FontFamily("Arial");
        }

        public void OnClick2(object sender, RoutedEventArgs e)
        {
            txt1.Text = "The FontFamily is set to Courier new.";
            txt2.FontFamily = new FontFamily("Courier new");
        }

        public void OnClick3(object sender, RoutedEventArgs e)
        {
            txt1.Text = "The FontFamily is set to Tahoma.";
            txt2.FontFamily = new FontFamily("Tahoma");
        }

        public void OnClick4(object sender, RoutedEventArgs e)
        {
            txt1.Text = "The FontFamily is set to Times new Roman.";
            txt2.FontFamily = new FontFamily("Times new Roman");
        }

        public void OnClick5(object sender, RoutedEventArgs e)
        {
            txt1.Text = "The FontFamily is set to Verdana.";
            txt2.FontFamily = new FontFamily("Verdana");
        }

        public void OnClick6(object sender, RoutedEventArgs e)
        {
            txt3.Text = "The FontSize is set to 8 point.";
            txt2.FontSize = 8;
        }

        public void OnClick7(object sender, RoutedEventArgs e)
        {
            txt3.Text = "The FontSize is set to 10 point.";
            txt2.FontSize = 10;
        }

        public void OnClick8(object sender, RoutedEventArgs e)
        {
            txt3.Text = "The FontSize is set to 12 point.";
            txt2.FontSize = 12;
        }

        public void OnClick9(object sender, RoutedEventArgs e)
        {
            txt3.Text = "The FontSize is set to 14 point.";
            txt2.FontSize = 14;
        }

        public void OnClick10(object sender, RoutedEventArgs e)
        {
            txt3.Text = "The FontSize is set to 16 point.";
            txt2.FontSize = 16;
        }

        public void OnClick11(object sender, RoutedEventArgs e)
        {
            txt4.Text = "The Foreground color is set to Black.";
            txt2.Foreground = Brushes.Black;
        }

        public void OnClick12(object sender, RoutedEventArgs e)
        {
            txt4.Text = "The Foreground color is set to Blue.";
            txt2.Foreground = Brushes.Blue;
        }

        public void OnClick13(object sender, RoutedEventArgs e)
        {
            txt4.Text = "The Foreground color is set to Green.";
            txt2.Foreground = Brushes.Green;
        }

        public void OnClick14(object sender, RoutedEventArgs e)
        {
            txt4.Text = "The Foreground color is set to Red.";
            txt2.Foreground = Brushes.Red;
        }

        public void OnClick15(object sender, RoutedEventArgs e)
        {
            txt4.Text = "The Foreground color is set to Yellow.";
            txt2.Foreground = Brushes.Yellow;
        }
    }
}