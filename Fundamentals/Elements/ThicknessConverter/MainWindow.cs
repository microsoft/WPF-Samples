// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThicknessConverter
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

        private void ChangeThickness(object sender, SelectionChangedEventArgs args)
        {
            var li = ((sender as ListBox).SelectedItem as ListBoxItem);
            var myThicknessConverter = new System.Windows.ThicknessConverter();
            var th1 = (Thickness) myThicknessConverter.ConvertFromString(li.Content.ToString());
            border1.BorderThickness = th1;
            bThickness.Text = "Border.BorderThickness =" + li.Content;
        }

        private void ChangeColor(object sender, SelectionChangedEventArgs args)
        {
            var li2 = ((sender as ListBox).SelectedItem as ListBoxItem);
            var myBrushConverter = new BrushConverter();
            border1.BorderBrush = (Brush) myBrushConverter.ConvertFromString((string) li2.Content);
            bColor.Text = "Border.Borderbrush =" + li2.Content;
        }
    }
}