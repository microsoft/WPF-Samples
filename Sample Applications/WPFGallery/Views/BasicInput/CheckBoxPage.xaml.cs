
using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for CheckBox.xaml
    /// </summary>
    public partial class CheckBoxPage : Page
    {
        public CheckBoxPageViewModel ViewModel { get; }
        public CheckBoxPage(CheckBoxPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
