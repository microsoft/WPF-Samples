using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win11ThemeGallery.Navigation;

namespace Win11ThemeGallery.ViewModels
{
    public partial class IconsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Icons";

        [ObservableProperty]
        private string _pageDescription = "With the release of Windows 11, Segoe Fluent Icons is the recommended icon font.";

        private readonly INavigationService _navigationService;

        public IconsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Navigate(object pageType)
        {
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }
    }
}
