using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class CollectionsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Collections";

        [ObservableProperty]
        private string _pageDescription = "Controls for collection presentation";


        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Collections");

        private readonly INavigationService _navigationService;

        public CollectionsPageViewModel(INavigationService navigationService)
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
