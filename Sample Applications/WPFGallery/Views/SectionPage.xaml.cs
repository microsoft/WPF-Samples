using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    public partial class SectionPage : Page
    {
        public BaseSectionPageViewModel ViewModel { get; set; } = null; 
        public SectionPage(BaseSectionPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
       }
    }
}
