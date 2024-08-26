using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;


namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for ProgressBarPage.xaml
    /// </summary>
    public partial class ProgressBarPage : Page
    {
    public ProgressBarPageViewModel ViewModel { get; }

    public ProgressBarPage(ProgressBarPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
    }
}
