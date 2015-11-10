using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageScaling
{
    /// <summary>
    /// This sample works with the Per Monitor DPI 11/2015 Preview of WPF.
    /// It shows how a developer can use a set of 100, 200, and 400 DPI images to cover the range 
    /// of DPIs that your users may have. It adjusts the images used as the App's window is moved 
    /// to monitors with different DPIs.
    /// This shows how this can work for a normal image element.
    /// One could also build an image aware WPF control (perhaps overriding Visual.OnDpiChanged)
    /// The final version of WPF that has Per Monitor DPI support may do some of this more automatically.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Ensure image of appropriate DPI is loaded for first load.
        private void PickRightScaledImage(object sender, EventArgs e)
        {
            Image image = (Image)sender;
            UpdateImageSourceForDpi(image);

            image.DpiChanged += Image_DpiChanged;
        }

        // Ensure image of appropriate DPI is loaded when DPI changes
        private void Image_DpiChanged(object sender, RoutedEventArgs e)
        {
            Image image = (Image)sender;
            UpdateImageSourceForDpi(image);
        }

        private void UpdateImageSourceForDpi(Image image)
        {
            string newImageUrl = ImageDpiHelper.GetDesiredImageUrlForDpi(image);
            ImageDpiHelper.UpdateImageSource(image, newImageUrl);
            textLabel.Text = newImageUrl + " for DPI of " + (VisualTreeHelper.GetDpi(image).PixelsPerDip * 100);
        }
    }
}