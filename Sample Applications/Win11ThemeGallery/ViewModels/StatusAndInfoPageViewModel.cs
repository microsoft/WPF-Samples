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
    public partial class StatusAndInfoPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Status & Info";

        [ObservableProperty]
        private string _pageDescription = "Controls to show progress and extra information";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "ProgressBar",
                PageType = typeof(ProgressBarPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ProgressBar.png"))},
                Description = "Shows the apps progress on a task, or that the app is performing ongoing work that doesn't block user interaction."
            },
            new NavigationCard
            {
                Name = "ToolTip",
                PageType = typeof(ToolTipPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ToolTip.png"))},
                Description = "Displays information for an element in a pop-up window."
            },
        };

        private readonly INavigationService _navigationService;

        public StatusAndInfoPageViewModel(INavigationService navigationService)
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
