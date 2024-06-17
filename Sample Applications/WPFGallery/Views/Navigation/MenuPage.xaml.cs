using System.Windows.Controls;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPageViewModel ViewModel { get; }

        public MenuPage(MenuPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                StatusMenuItem.Visibility = Visibility.Visible;
                StatusMenuItem.Text = (menuItem.Tag != null) ?  $"You pressed {menuItem.Tag}" : $"You pressed {menuItem.Header}";
            }
        }
    }
}
