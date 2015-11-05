using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageScaling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_Load(object sender, EventArgs e)
        {
            Image image = (Image)sender;
            UpdateImageSourceForDpi(image);

            image.DpiChanged += Image_DpiChanged;
        }

        private void Image_DpiChanged(object sender, RoutedEventArgs e)
        {
            Image image = (Image)sender;
            UpdateImageSourceForDpi(image);
        }

        private void UpdateImageSourceForDpi(Image image)
        {
            string imageUrl = ImageDpiHelper.GetDesiredImageUrlForDpi(image);
            ImageDpiHelper.UpdateImageSource(image, imageUrl);
            textLabel.Text = imageUrl + " for DPI of " + (VisualTreeHelper.GetDpi(image).PixelsPerDip * 100);
        }
    }
}