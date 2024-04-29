using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

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
                Description = "Guide showing how to use colors in your app"
            },
            new NavigationCard
            {
                Name = "Typography",
                PageType = typeof(TypographyPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TextBlock.png"))},
                Description = "Guide showing how to use typography in your app"
            },
            new NavigationCard
            {
                Name = "Icons",
                PageType = typeof(IconsPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/IconElement.png"))},
                Description = "Guide showing how to use icons in your app"
            },
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
