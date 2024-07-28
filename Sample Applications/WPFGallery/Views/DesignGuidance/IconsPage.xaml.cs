using System.Windows.Controls;
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

    }
}
