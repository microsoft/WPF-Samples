using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class NavigationPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Navigation Controls";

        [ObservableProperty]
        private string _pageDescription = "Controls for navigation and actions";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Menu",
                PageType = typeof(MenuPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.RowTriple24 },
                Description = "Contains a collection of MenuItem elements"
            },
            new NavigationCard
            {
                Name = "TabControl",
                PageType = typeof(TabControlPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TabDesktopBottom24 },
                Description = "Tab control like in browser"
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
