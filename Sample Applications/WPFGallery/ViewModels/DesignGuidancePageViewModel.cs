using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class DesignGuidancePageViewModel : BaseSectionPageViewModel
    {
        public DesignGuidancePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Design Guidance";
            PageDescription = "Design guidelines on how to use colors, typography, and icons in your app.";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Design Guidance");
        }
    }
}
