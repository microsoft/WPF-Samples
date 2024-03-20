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
using Win11ThemeGallery.ViewModels.Samples;

namespace Win11ThemeGallery.Views.Samples
{
    /// <summary>
    /// Interaction logic for UserDashboardPage.xaml
    /// </summary>
    public partial class UserDashboardPage : Page
    {
        public UserDashboardPageViewModel ViewModel { get; }

        public UserDashboardPage(UserDashboardPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
