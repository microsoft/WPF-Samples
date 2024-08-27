using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;


namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for PasswordBoxPage.xaml
    /// </summary>
    public partial class PasswordBoxPage : Page
    {
        public PasswordBoxPageViewModel ViewModel { get; }
public PasswordBoxPage(PasswordBoxPageViewModel viewModel)
        {
ViewModel = viewModel;
DataContext = this;
            InitializeComponent();
        }
    }
}
