using WPFGallery.Navigation;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class DateAndTimePageViewModel : BaseSectionPageViewModel
    {
        public DateAndTimePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Date & Calendar";
            PageDescription = "Controls for date and calendar";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Date & Calendar");
        }
    }
}
