using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for ResizeGripPage.xaml
    /// </summary>
    public partial class ResizeGripPage : Page
    {
        public ResizeGripPage(ResizeGripPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public ResizeGripPageViewModel ViewModel { get; }

        private void OpenResizeGripWindow_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 300,
                ResizeMode = ResizeMode.CanResizeWithGrip,
                Content = new TextBlock
                {
                    Text = "ResizeGrip is present at the bottom right corner of the window",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16
                }
            };
            window.Show();
        }
    }
}
