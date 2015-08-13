// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace WidthProperties
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

        private void ChangeWidth(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var sz1 = double.Parse(li.Content.ToString());
            rect1.Width = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualWidth is set to " + rect1.ActualWidth;
            txt2.Text = "Width is set to " + rect1.Width;
            txt3.Text = "MinWidth is set to " + rect1.MinWidth;
            txt4.Text = "MaxWidth is set to " + rect1.MaxWidth;
        }

        private void ChangeMinWidth(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var sz1 = double.Parse(li.Content.ToString());
            rect1.MinWidth = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualWidth is set to " + rect1.ActualWidth;
            txt2.Text = "Width is set to " + rect1.Width;
            txt3.Text = "MinWidth is set to " + rect1.MinWidth;
            txt4.Text = "MaxWidth is set to " + rect1.MaxWidth;
        }

        private void ChangeMaxWidth(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var sz1 = double.Parse(li.Content.ToString());
            rect1.MaxWidth = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualWidth is set to " + rect1.ActualWidth;
            txt2.Text = "Width is set to " + rect1.Width;
            txt3.Text = "MinWidth is set to " + rect1.MinWidth;
            txt4.Text = "MaxWidth is set to " + rect1.MaxWidth;
        }

        private void ClipRect(object sender, RoutedEventArgs args)
        {
            myCanvas.ClipToBounds = true;
            txt5.Text = "Canvas.ClipToBounds is set to " + myCanvas.ClipToBounds;
        }

        private void UnclipRect(object sender, RoutedEventArgs args)
        {
            myCanvas.ClipToBounds = false;
            txt5.Text = "Canvas.ClipToBounds is set to " + myCanvas.ClipToBounds;
        }
    }
}