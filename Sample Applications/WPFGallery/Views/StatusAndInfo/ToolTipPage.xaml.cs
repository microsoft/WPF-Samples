using System.Windows.Controls;

using WPFGallery.ViewModels;


namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for ToolTipPage.xaml
    /// </summary>
    public partial class ToolTipPage : Page
    {
    public ToolTipPageViewModel ViewModel { get; }

    public ToolTipPage(ToolTipPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
    }
}
