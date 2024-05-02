using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views.Samples;

namespace WPFGallery.ViewModels
{
    public partial class SamplesPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Samples";

        [ObservableProperty]
        private string _pageDescription = "Sample pages for common scenarios";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "User Dashboard",
                PageType = typeof(UserDashboardPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/PersonPicture.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                Description = "User Dashboard Page"
            },
        };

        private readonly INavigationService _navigationService;

        public SamplesPageViewModel(INavigationService navigationService)
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
