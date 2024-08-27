using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class NavigationPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Navigation";

        [ObservableProperty]
        private string _pageDescription = "Controls for navigation and actions";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Menu",
                PageType = typeof(MenuPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Pivot.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.RowTriple24 },
                Description = "A classic menu, allowing the display of MenuItems containing MenuFlyoutItems."
            },
            new NavigationCard
            {
                Name = "TabControl",
                PageType = typeof(TabControlPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TabView.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TabDesktopBottom24 },
                Description = "A control that displays a collection of tabs."
            },
        };

        private readonly INavigationService _navigationService;

        public NavigationPageViewModel(INavigationService navigationService)
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
