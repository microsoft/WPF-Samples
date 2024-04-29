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
    public partial class LayoutPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Layout Controls";

        [ObservableProperty]
        private string _pageDescription = "Controls for layouting";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Expander",
                PageType = typeof(ExpanderPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/Expander.png"))},
                Description = "A container with a header that can be expanded to show a body with more content."
            },
        };

        private readonly INavigationService _navigationService;

        public LayoutPageViewModel(INavigationService navigationService)
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
