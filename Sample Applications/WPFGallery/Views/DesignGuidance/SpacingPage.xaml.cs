using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for SpacingPage.xaml
    /// </summary>
    public partial class SpacingPage : Page
    {
        public SpacingPage(SpacingPageViewModel viewModel)
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
            if(IsLightTheme())
            {
                DialogImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Design/Dialog.light.png"));
                CardImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Design/Cards.light.png"));
            }
            else
            {
                DialogImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Design/Dialog.dark.png"));
                CardImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Design/Cards.dark.png"));

            }
        }

        public SpacingPageViewModel ViewModel { get; }

        public static bool IsLightTheme()
        {
            try
            {
                var themeMode = Application.Current.ThemeMode;

                if (themeMode == ThemeMode.Light)
                    return true;
                if (themeMode == ThemeMode.Dark)
                    return false;

                // For System theme, detect the actual effective theme
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    var backgroundResource = mainWindow.TryFindResource("SolidBackgroundFillColorBaseBrush");
                    if (backgroundResource is SolidColorBrush brush)
                    {
                        var color = brush.Color;
                        var luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
                        return luminance > 0.5;
                    }
                }

                return themeMode != ThemeMode.Dark;
            }
            catch
            {
                return true;
            }
        }
    }
}
