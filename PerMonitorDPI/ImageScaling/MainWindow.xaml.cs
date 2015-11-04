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
        private string _baseImagePath = "images\\MSFT_logo.scale-{0}.png";
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateImage(VisualTreeHelper.GetDpi(image));
            image.DpiChanged += Image_DpiChanged;
        }

        private void Image_DpiChanged(object sender, RoutedEventArgs e)
        {
            Image image = (Image)sender;
            UpdateImage(VisualTreeHelper.GetDpi(image));
        }

        private void UpdateImage(DpiScaleInfo dpiInfo)
        {
            string newImagePath;
            int scale = (int)(dpiInfo.PixelsPerDip * 100);
            BitmapImage src;
            if (scale <= 100)
            {
                scale = 100;
            }
            else if (scale <= 200)
            {
                scale = 200;
            }
            else
            {
                scale = 400;
            }
            newImagePath = string.Format(_baseImagePath, "" + scale);
            if (!string.Equals(image.Tag.ToString(), newImagePath))
            {
                src = new BitmapImage(new Uri(newImagePath, UriKind.Relative));
                image.Source = src;
                image.Tag = newImagePath;
                textLabel.Text = newImagePath;
            }
        }
    }
}