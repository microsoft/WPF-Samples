using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class MediaPageViewModel : BaseSectionPageViewModel
    {
        public MediaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Media Controls";
            PageDescription = "Controls for media presentation";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Media");
        }
    }
}
