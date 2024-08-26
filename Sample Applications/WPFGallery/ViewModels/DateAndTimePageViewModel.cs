using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class DateAndTimePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Date & Calendar";

        [ObservableProperty]
        private string _pageDescription = "Controls for date and calendar";

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Date & Calendar");

        private readonly INavigationService _navigationService;

        public DateAndTimePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Navigate(object pageType){
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }

        
    }
}
