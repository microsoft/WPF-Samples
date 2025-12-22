using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;
using WPFGallery.ViewModels.Layout;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for StackPanelPage.xaml
    /// </summary>
    public partial class StackPanelPage : Page
    {
        public StackPanelPage(StackPanelPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        public StackPanelPageViewModel ViewModel { get; }
    }
}
