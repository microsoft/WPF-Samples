using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGallery.Controls;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for IconsPage.xaml
    /// </summary>
    public partial class IconsPage : Page
    {
        static IconsPage()
        {
            CommandManager.RegisterClassCommandBinding(typeof(IconsPage), new CommandBinding(ApplicationCommands.Copy, Copy_Content));
        }
        public IconsPage(IconsPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        public IconsPageViewModel ViewModel { get; }

        public static void Copy_Content(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((ExecutedRoutedEventArgs)e).Parameter.ToString()))
            {
                try
                {
                    Clipboard.SetText(((ExecutedRoutedEventArgs)e).Parameter.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error copying to clipboard: " + ex.Message);
                }
            }
        }

        private void IconsSearchBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (IconsSearchBox.Text.Length > 0)
            {
                IconsSearchBoxPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                IconsSearchBoxPlaceholder.Visibility = Visibility.Visible;
            }
            //IconsSearchBoxPlaceholder.Visibility = Visibility.Hidden;
        }

        private void IconsSearchBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (IconsSearchBox.Text.Length > 0)
            {
                IconsSearchBoxPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                IconsSearchBoxPlaceholder.Visibility = Visibility.Visible;
            }
            //IconsSearchBoxPlaceholder.Visibility = Visibility.Visible;
        }

        private void IconsSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IconsSearchBox.Text.Length > 0)
            {
                IconsSearchBoxPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                IconsSearchBoxPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void Open_SegoeFontDownloadPage(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://learn.microsoft.com/windows/apps/design/downloads/#fonts") { UseShellExecute = true });
        }

        private void Open_IconDesignGuidelinesPage(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://learn.microsoft.com/windows/apps/design/style/segoe-fluent-icons-font#layering-and-mirroring") { UseShellExecute = true });
        }
    }
}
