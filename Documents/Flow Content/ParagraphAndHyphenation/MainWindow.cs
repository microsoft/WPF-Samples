// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace ParagraphAndHyphenation
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

        private void ToggleHyphenation(object sender, RoutedEventArgs args)
        {
            flowDoc.IsHyphenationEnabled = ((CheckBox) sender).IsChecked.Value;
        }

        private void ToggleOptimalParagraph(object sender, RoutedEventArgs args)
        {
            flowDoc.IsOptimalParagraphEnabled = ((CheckBox) sender).IsChecked.Value;
        }

        private void ToggleColumnFlex(object sender, RoutedEventArgs args)
        {
            flowDoc.IsColumnWidthFlexible = ((CheckBox) sender).IsChecked.Value;
        }

        private void ChangeColumnWidth(object sender, RoutedEventArgs args)
        {
            if (myGrid.Children.Contains(flowReader))
            {
                if (columnWidthSlider.Value == 0)
                {
                    flowDoc.ColumnWidth = 100;
                }
                else if (columnWidthSlider.Value == 1)
                {
                    flowDoc.ColumnWidth = 200;
                }
                else if (columnWidthSlider.Value == 2)
                {
                    flowDoc.ColumnWidth = 300;
                }
                else if (columnWidthSlider.Value == 3)
                {
                    flowDoc.ColumnWidth = 400;
                }
                else if (columnWidthSlider.Value == 4)
                {
                    flowDoc.ColumnWidth = 500;
                }
            }
        }

        private void ChangeColumnGap(object sender, RoutedEventArgs args)
        {
            if (myGrid.Children.Contains(flowReader))
            {
                if (columnGapSlider.Value == 0)
                {
                    flowDoc.ColumnGap = 5;
                }
                else if (columnGapSlider.Value == 1)
                {
                    flowDoc.ColumnGap = 10;
                }
                else if (columnGapSlider.Value == 2)
                {
                    flowDoc.ColumnGap = 15;
                }
                else if (columnGapSlider.Value == 3)
                {
                    flowDoc.ColumnGap = 20;
                }
                else if (columnGapSlider.Value == 4)
                {
                    flowDoc.ColumnGap = 25;
                }
            }
        }
    }
}