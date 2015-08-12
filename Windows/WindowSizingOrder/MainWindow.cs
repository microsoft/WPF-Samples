// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace WindowSizingOrder
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

        private void ShowWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var sw = new SizingWindow();
            if (setWSCB.IsChecked == true)
                sw.WindowState = (WindowState) Enum.Parse(typeof (WindowState), wsLB.Text);
            if (setMinWidthCB.IsChecked == true)
                sw.MinWidth = double.Parse(minWidthTB.Text);
            if (setMinHeightCB.IsChecked == true)
                sw.MinHeight = double.Parse(minHeightTB.Text);
            if (setMaxWidthCB.IsChecked == true)
                sw.MaxWidth = double.Parse(maxWidthTB.Text);
            if (setMaxHeightCB.IsChecked == true)
                sw.MaxHeight = double.Parse(maxHeightTB.Text);
            if (setWidthCB.IsChecked == true)
                sw.Width = double.Parse(widthTB.Text);
            if (setHeightCB.IsChecked == true)
                sw.Height = double.Parse(heightTB.Text);
            if (setSTCCB.IsChecked == true)
                sw.SizeToContent = (SizeToContent) Enum.Parse(typeof (SizeToContent), stcLB.Text);
            sw.Show();
        }
    }
}