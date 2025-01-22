using WPFGallery.Navigation;

namespace WPFGallery.ViewModels
{
    public partial class ColorsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Colors";

        [ObservableProperty]
        private string _pageDescription = "Guide showing how to use colors in your app";

    }
}
