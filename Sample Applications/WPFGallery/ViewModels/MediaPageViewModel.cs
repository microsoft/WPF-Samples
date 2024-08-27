
using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class MediaPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Media Controls";

        [ObservableProperty]
        private string _pageDescription = "Controls for media presentation";
 
        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Canvas",
                PageType = typeof(CanvasPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.InkStroke24 },
                Description = "Canvas presenter"
            },
            new NavigationCard
            {
                Name = "Image",
                PageType = typeof(ImagePage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.ImageMultiple24 },
                Description = "Image presente"
            },
        };

        private readonly INavigationService _navigationService;

        public MediaPageViewModel(INavigationService navigationService)
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
