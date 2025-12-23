using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;
using WPFGallery.ViewModels.Layout;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for BorderPage.xaml
    /// </summary>
    public partial class BorderPage : Page
    {
        public BorderPage(BorderPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public BorderPageViewModel ViewModel { get; }
    }
}
