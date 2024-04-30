using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views.Samples;

namespace Win11ThemeGallery.ViewModels
{
    public partial class SamplesPageViewModel : BaseNavigablePageViewModel
    {
        public SamplesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Samples";
            PageDescription = "Sample pages for common scenarios";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "User Dashboard",
                    PageType = typeof(UserDashboardPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/PersonPicture.png"))},
                    Description = "User Dashboard Page"
                },
            };
        }
    }
}
