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
    public partial class NavigationPageViewModel : BaseNavigablePageViewModel
    {
        public NavigationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Navigation Controls";
            PageDescription = "Controls for navigation and actions";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "Menu",
                    PageType = typeof(MenuPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Pivot.png"))},
                    Description = "A classic menu, allowing the display of MenuItems containing MenuFlyoutItems."
                },
                new NavigationCard
                {
                    Name = "TabControl",
                    PageType = typeof(TabControlPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/TabView.png"))},
                    Description = "A control that displays a collection of tabs."
                },
            };
        }
    }
}
