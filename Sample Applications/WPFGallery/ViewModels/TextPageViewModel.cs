using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class TextPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Text";

        [ObservableProperty]
        private string _pageDescription = "Controls for displaying and editing text";

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Text");
        private readonly INavigationService _navigationService;

        public TextPageViewModel(INavigationService navigationService)
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
