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
    public partial class BaseNavigablePageViewModel : BasePageViewModel
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards;

        [RelayCommand]
        public void Navigate(object pageType){
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }

        public BaseNavigablePageViewModel(INavigationService navigationService) : base()
        {
            _navigationService = navigationService;
        }
    }
}