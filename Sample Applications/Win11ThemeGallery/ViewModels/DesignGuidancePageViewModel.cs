using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;
using Win11ThemeGallery.Views.DesignGuidance;

namespace Win11ThemeGallery.ViewModels
{
    public partial class DesignGuidancePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Design Guidance";

        [ObservableProperty]
        private string _pageDescription = "";       

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Colors",
                PageType = typeof(ColorsPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ColorPaletteResources.png"))},
                Description = ""
            },
            // new NavigationCard
            // {
            //     Name = "ListBox",
            //     PageType = typeof(ListBoxPage),
            //     Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListBox.png"))},
            //     Description = "A control that presents an inline list of items that the user can select from."
            // },
            // new NavigationCard
            // {
            //     Name = "ListView",
            //     PageType = typeof(ListViewPage),
            //     Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ListView.png"))},
            //     Description = "A control that presents a collection of items in a vertical list."
            // },
            // new NavigationCard
            // {
            //     Name = "TreeView",
            //     PageType = typeof(TreeViewPage),
            //     Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TreeView.png"))},
            //     Description = "The TreeView control is a hierarchical list pattern with expanding and collapsing nodes that contain nested items."
            // },
        };

        private readonly INavigationService _navigationService;

        public DesignGuidancePageViewModel(INavigationService navigationService)
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
