using WPFGallery.Navigation;
using WPFGallery.Views.Samples;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class SamplesPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Samples";

        [ObservableProperty]
        private string _pageDescription = "Sample pages for common scenarios";

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Samples");

        private readonly INavigationService _navigationService;

        public SamplesPageViewModel(INavigationService navigationService)
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
