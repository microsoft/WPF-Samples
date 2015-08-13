// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace DrawingBrush
{
    public partial class SampleViewer : Window
    {
        public SampleViewer()
        {
            InitializeComponent();
        }

        private void SampleSelected(object sender, RoutedEventArgs args)
        {
            var rButton = sender as RadioButton;

            var buttonContent = (string) rButton?.Content;

            if (buttonContent != null)
            {
                if (buttonContent == "Using DrawingBrush Example")
                    mainFrame.Navigate(new DrawingBrushExample());
                else if (buttonContent == "Transform Example")
                    mainFrame.Navigate(new TransformExample());
                else if (buttonContent == "ImageDrawing Example")
                    mainFrame.Navigate(new ImageBrushExample());
                else
                    mainFrame.Navigate(new AnimateGeometryDrawingExample());
            }
        }

        private void ExitApplication(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }
    }
}