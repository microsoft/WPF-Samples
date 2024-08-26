using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for ListBoxPage.xaml
    /// </summary>
    public partial class ListBoxPage : Page
    {
        public ListBoxPageViewModel ViewModel { get; }
        public ListBoxPage(ListBoxPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
