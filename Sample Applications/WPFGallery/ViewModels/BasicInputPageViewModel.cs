using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class BasicInputPageViewModel : BaseSectionPageViewModel
    {
        public BasicInputPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Basic Input";
            PageDescription = "Controls for getting user input";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Basic Input");
        }
    }
}
