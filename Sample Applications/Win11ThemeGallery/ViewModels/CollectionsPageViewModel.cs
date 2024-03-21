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
    public partial class CollectionsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Collection Controls";

        [ObservableProperty]
        private string _pageDescription = "Controls for collection presentation";


        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Data Grid",
                PageType = typeof(DataGridPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.GridKanban20 },
                Description = "Complex data presenter"
            },
            new NavigationCard
            {
                Name = "ListBox",
                PageType = typeof(ListBoxPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.AppsListDetail24 },
                Description = "Selectable list"
            },
            new NavigationCard
            {
                Name = "ListView",
                PageType = typeof(ListViewPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.GroupList24 },
                Description = "Selectable list"
            },
            new NavigationCard
            {
                Name = "TreeView",
                PageType = typeof(TreeViewPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TextBulletListTree24 },
                Description = "Hierarchical list control"
            },
        };

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
