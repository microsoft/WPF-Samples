using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class DashboardPageViewModel : ObservableObject
    {

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetGroupedControlsInfo();

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _newControlStyles = ControlsInfoDataSource.Instance.GetNewControlsInfo();

        private readonly INavigationService _navigationService;

        public DashboardPageViewModel(INavigationService navigationService)
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
