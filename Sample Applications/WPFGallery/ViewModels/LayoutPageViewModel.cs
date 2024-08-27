using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class LayoutPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Layout";

        [ObservableProperty]
        private string _pageDescription = "Controls for layouting";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Expander",
                PageType = typeof(ExpanderPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Expander.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                Description = "A container with a header that can be expanded to show a body with more content."
            },
        };

        private readonly INavigationService _navigationService;

        public LayoutPageViewModel(INavigationService navigationService)
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
