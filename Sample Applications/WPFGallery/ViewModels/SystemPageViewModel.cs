using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class SystemPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "System";

        [ObservableProperty]
        private string _pageDescription = "System-level controls and dialogs";

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("System");

        private readonly INavigationService _navigationService;

        public SystemPageViewModel(INavigationService navigationService)
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
