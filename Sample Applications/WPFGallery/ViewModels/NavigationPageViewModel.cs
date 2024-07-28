using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class NavigationPageViewModel : BaseSectionPageViewModel
    {
        public NavigationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Navigation";
            PageDescription = "Controls for navigation and actions";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Navigation");
        }
    }
}
