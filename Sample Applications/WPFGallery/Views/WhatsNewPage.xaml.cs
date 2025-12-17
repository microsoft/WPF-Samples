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
    /// Interaction logic for WhatsNewPage.xaml
    /// </summary>
    public partial class WhatsNewPage : Page
    {
        public WhatsNewPageViewModel ViewModel { get; }
        public WhatsNewPage(WhatsNewPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        private void Open_WhatsNewPageNET10(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://learn.microsoft.com/en-in/dotnet/desktop/wpf/whats-new/net100") { UseShellExecute = true });
        }

        private void Open_WhatsNewPageNET9(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://learn.microsoft.com/en-in/dotnet/desktop/wpf/whats-new/net90") { UseShellExecute = true });
        }

        private void NavigateToMessageBoxSample(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigate(typeof(MessageBoxPage));
        }

        private void Open_UsingFluentInWPFPage(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://aka.ms/wpf-fluentdoc") { UseShellExecute = true });
        }
    }
}
