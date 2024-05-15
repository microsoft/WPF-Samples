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
using WPFGallery.Controls;
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for IconsPage.xaml
    /// </summary>
    public partial class IconsPage : Page
    {
        public IconsPage(IconsPageViewModel viewModel)
        {
            CommandManager.RegisterClassCommandBinding(typeof(IconsPage), new CommandBinding(ApplicationCommands.Copy, Copy_Content));
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        public IconsPageViewModel ViewModel { get; }

        public void Copy_Content(object sender, RoutedEventArgs e)
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
