using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            ViewModel = viewModel;
            DataContext = this;
        }

        public SpacingPageViewModel ViewModel { get; }
    }
}
