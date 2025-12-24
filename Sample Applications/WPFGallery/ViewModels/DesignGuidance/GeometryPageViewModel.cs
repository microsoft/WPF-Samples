using WPFGallery.Navigation;

namespace WPFGallery.ViewModels
{
    public partial class GeometryPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Geometry";

        [ObservableProperty]
        private string _pageDescription = "";
    }
}
