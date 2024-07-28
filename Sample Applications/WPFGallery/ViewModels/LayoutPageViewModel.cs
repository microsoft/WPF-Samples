using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class LayoutPageViewModel : BaseSectionPageViewModel
    {
        public LayoutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Layout";
            PageDescription = "Controls for layout";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Layout");
        }
    }
}
