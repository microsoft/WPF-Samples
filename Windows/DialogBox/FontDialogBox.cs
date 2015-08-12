// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DialogBox
{
    public partial class FontDialogBox : Window
    {
        public FontDialogBox()
        {
            InitializeComponent();

            fontFamilyListBox.ItemsSource = FontPropertyLists.FontFaces;
            fontStyleListBox.ItemsSource = FontPropertyLists.FontStyles;
            fontWeightListBox.ItemsSource = FontPropertyLists.FontWeights;
            fontSizeListBox.ItemsSource = FontPropertyLists.FontSizes;
        }

        public new FontFamily FontFamily
        {
            get { return (FontFamily) fontFamilyListBox.SelectedItem; }
            set
            {
                fontFamilyListBox.SelectedItem = value;
                fontFamilyListBox.ScrollIntoView(value);
            }
        }

        public new FontStyle FontStyle
        {
            get { return (FontStyle) fontStyleListBox.SelectedItem; }
            set
            {
                fontStyleListBox.SelectedItem = value;
                fontStyleListBox.ScrollIntoView(value);
            }
        }

        public new FontWeight FontWeight
        {
            get { return (FontWeight) fontWeightListBox.SelectedItem; }
            set
            {
                fontWeightListBox.SelectedItem = value;
                fontWeightListBox.ScrollIntoView(value);
            }
        }

        public new double FontSize
        {
            get { return (double) fontSizeListBox.SelectedItem; }
            set
            {
                fontSizeListBox.SelectedItem = value;
                fontSizeListBox.ScrollIntoView(value);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box accepted
            DialogResult = true;
        }

        private void fontFamilyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If user enters family text, select family in list if matching item found
            FontFamily = new FontFamily(fontFamilyTextBox.Text);
        }

        private void fontStyleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If user enters style text, select style in list if matching item found
            if (FontPropertyLists.CanParseFontStyle(fontStyleTextBox.Text))
            {
                FontStyle = FontPropertyLists.ParseFontStyle(fontStyleTextBox.Text);
            }
        }

        private void fontWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If user enters weight text, select weight in list if matching item found
            if (FontPropertyLists.CanParseFontWeight(fontWeightTextBox.Text))
            {
                FontWeight = FontPropertyLists.ParseFontWeight(fontWeightTextBox.Text);
            }
        }

        private void fontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If user enters size text, select size in list if matching item found
            double fontSize;
            if (double.TryParse(fontSizeTextBox.Text, out fontSize))
            {
                FontSize = fontSize;
            }
        }
    }
}