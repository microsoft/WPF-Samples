using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;


using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class ButtonPage : Page
    {
        public ButtonPageViewModel ViewModel { get; }

        public ButtonPage(ButtonPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
