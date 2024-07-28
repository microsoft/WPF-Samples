using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class AllSamplesPageViewModel : BaseSectionPageViewModel
    {
        public AllSamplesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "All Controls";
            PageDescription = "This page contains all the controls available in the gallery.";
            NavigationCards = ControlsInfoDataSource.Instance.GetAllControlsInfo();
        }
    }
}
