using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class AboutPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "About";
    }
}
