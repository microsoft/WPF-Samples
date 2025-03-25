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
    /// Interaction logic for NavigationWindowPage.xaml
    /// </summary>
    public partial class NavigationWindowPage : Page
    {
        public NavigationWindowPage(NavigationWindowPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public NavigationWindowPageViewModel ViewModel { get; }

        private void OpenNavigationWindow_Click(object sender, RoutedEventArgs e)
        {
            NavigationWindow window = new NavigationWindow()
            {
                Width = 800,
                Height = 450,
                Source = new Uri("/Views/Navigation/Page1.xaml", UriKind.Relative)
            };
            window.Show();
        }
    }
}
