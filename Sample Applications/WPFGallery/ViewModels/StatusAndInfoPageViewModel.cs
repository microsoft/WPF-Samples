using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class StatusAndInfoPageViewModel : BaseSectionPageViewModel
    {
        public StatusAndInfoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Status & Info";
            PageDescription = "Controls to show progress and extra information";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Status & Info");
        }
    }
}
