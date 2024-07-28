using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class SamplesPageViewModel : BaseSectionPageViewModel
    {
        public SamplesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Samples";
            PageDescription = "Sample pages for common scenarios";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Samples");
        }
    }
}
