
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for TreeViewPage.xaml
    /// </summary>
    public partial class TreeViewPage : Page
    {
        public TreeViewPageViewModel ViewModel { get; }
        public TreeViewPage(TreeViewPageViewModel viewModel)
        {
            ViewModel = viewModel;
DataContext = this;
InitializeComponent();
        }
    }
}
