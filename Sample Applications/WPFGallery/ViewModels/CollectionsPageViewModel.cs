using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class CollectionsPageViewModel : BaseSectionPageViewModel
    {
        public CollectionsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Collections";
            PageDescription = "Controls for collection presentation";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Collections");
        }
    }
}
