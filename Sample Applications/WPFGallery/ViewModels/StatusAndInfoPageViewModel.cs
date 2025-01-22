using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class StatusAndInfoPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Status & Info";

        [ObservableProperty]
        private string _pageDescription = "Controls to show progress and extra information";

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Status & Info");

        private readonly INavigationService _navigationService;

        public StatusAndInfoPageViewModel(INavigationService navigationService)
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
