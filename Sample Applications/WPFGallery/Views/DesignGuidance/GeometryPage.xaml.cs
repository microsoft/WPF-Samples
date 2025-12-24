using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WPFGallery.Helpers;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for GeometryPage.xaml
    /// </summary>
    public partial class GeometryPage : Page
    {
        public GeometryPageViewModel ViewModel { get; }

        public GeometryPage(GeometryPageViewModel viewModel)
        {
            InitializeComponent();
            UpdateImageResources();
            ViewModel = viewModel;
            DataContext = this;
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateImageResources();
            });
        }

        private void UpdateImageResources()
        {
            if (Utility.IsLightTheme())
            {
                GeometryImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Design/Geometry.light.png"));
            }
            else
            {
                GeometryImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Design/Geometry.dark.png"));
            }
        }
    }
}
