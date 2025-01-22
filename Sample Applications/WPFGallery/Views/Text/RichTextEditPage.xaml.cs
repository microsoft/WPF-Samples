using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for RichTextBoxPage.xaml
    /// </summary>
    public partial class RichTextEditPage : Page
    {
        public RichTextEditPageViewModel ViewModel { get; }
public RichTextEditPage(RichTextEditPageViewModel viewModel)
        {
ViewModel = viewModel;
DataContext = this;
            InitializeComponent();
        }
    }
}
