using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class CollectionsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Collections";

        [ObservableProperty]
        private string _pageDescription = "Controls for collection presentation";


        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Data Grid",
                PageType = typeof(DataGridPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DataGrid.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.GridKanban20 },
                Description = "The DataGrid control presents data in a customizable table of rows and columns."
            },
            new NavigationCard
            {
                Name = "ListBox",
                PageType = typeof(ListBoxPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListBox.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.AppsListDetail24 },
                Description = "A control that presents an inline list of items that the user can select from."
            },
            new NavigationCard
            {
                Name = "ListView",
                PageType = typeof(ListViewPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListView.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.GroupList24 },
                Description = "A control that presents a collection of items in a vertical list."
            },
            new NavigationCard
            {
                Name = "TreeView",
                PageType = typeof(TreeViewPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TreeView.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.TextBulletListTree24 },
                Description = "The TreeView control is a hierarchical list pattern with expanding and collapsing nodes that contain nested items."
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
