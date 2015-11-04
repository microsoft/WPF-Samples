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
            int scale = (int)(dpiInfo.DpiScaleX * 100);
            BitmapImage src;
            switch (scale)
            {
                case 100:
                    src = new BitmapImage(new Uri("scaled100.bmp", UriKind.Relative));
                    image.Source = src;
                    break;
                case 125:
                    src = new BitmapImage(new Uri("scaled125.bmp", UriKind.Relative));
                    image.Source = src;
                    break;
                case 150:
                    src = new BitmapImage(new Uri("scaled150.bmp", UriKind.Relative));
                    image.Source = src;
                    break;
                case 200:
                    src = new BitmapImage(new Uri("scaled200.bmp", UriKind.Relative));
                    image.Source = src;
                    break;
            }
        }
    }
}