// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CustomWindowUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CustomWindowChrome : Window
    {
        public CustomWindowChrome()
        {
            InitializeComponent();
        }

        private void CloseButtonRectangle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            // Resize window width (honoring minimum width)
            var desiredWidth = (int) (ActualWidth + e.HorizontalChange);
            var minWidth = (int) (MinWidth + resizeThumb.Width + resizeThumb.Margin.Right);
            Width = Math.Max(desiredWidth, minWidth);

            // Resize window height (honoring minimum height)
            var desiredHeight = (int) (ActualHeight + e.VerticalChange);
            var minHeight = (int) (MinHeight + resizeThumb.Height + resizeThumb.Margin.Bottom);
            Height = Math.Max(desiredHeight, minHeight);
        }
    }
}