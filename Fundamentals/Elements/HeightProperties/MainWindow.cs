// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace HeightProperties
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

        private void ChangeHeight(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var sz1 = double.Parse(li.Content.ToString());
            rect1.Height = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualHeight is set to " + rect1.ActualHeight;
            txt2.Text = "Height is set to " + rect1.Height;
            txt3.Text = "MinHeight is set to " + rect1.MinHeight;
            txt4.Text = "MaxHeight is set to " + rect1.MaxHeight;
        }

        private void ChangeMinHeight(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var sz1 = double.Parse(li.Content.ToString());
            rect1.MinHeight = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualHeight is set to " + rect1.ActualHeight;
            txt2.Text = "Height is set to " + rect1.Height;
            txt3.Text = "MinHeight is set to " + rect1.MinHeight;
            txt4.Text = "MaxHeight is set to " + rect1.MaxHeight;
        }

        private void ChangeMaxHeight(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var sz1 = double.Parse(li.Content.ToString());
            rect1.MaxHeight = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualHeight is set to " + rect1.ActualHeight;
            txt2.Text = "Height is set to " + rect1.Height;
            txt3.Text = "MinHeight is set to " + rect1.MinHeight;
            txt4.Text = "MaxHeight is set to " + rect1.MaxHeight;
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