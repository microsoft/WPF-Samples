using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class DashboardPageViewModel : BaseSectionPageViewModel
    {
        public DashboardPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationCards = ControlsInfoDataSource.Instance.GetGroupedControlsInfo();
        }
    }
}
