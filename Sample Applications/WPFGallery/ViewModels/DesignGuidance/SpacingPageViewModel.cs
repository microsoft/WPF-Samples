using WPFGallery.Navigation;

namespace WPFGallery.ViewModels
{
    public partial class SpacingPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Spacing";

        [ObservableProperty]
        private string _pageDescription = "Guide showing how to use spacing in your app";
    }
}
