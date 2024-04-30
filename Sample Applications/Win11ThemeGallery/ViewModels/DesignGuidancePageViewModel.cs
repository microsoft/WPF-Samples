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
    public partial class DesignGuidancePageViewModel : BaseNavigablePageViewModel
    {
        public DesignGuidancePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Design Guidance";
            PageDescription = "";
            NavigationCards = new ObservableCollection<NavigationCard>
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
        }
    }
}
