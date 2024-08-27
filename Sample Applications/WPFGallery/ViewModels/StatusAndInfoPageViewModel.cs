using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
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
               // Icon = newSymbolIcon { Symbol = SymbolRegular.ArrowDownload24 },
                Description = "Shows the apps progress on a task, or that the app is performing ongoing work that doesn't block user interaction."
            },
            new NavigationCard
            {
                Name = "ToolTip",
                PageType = typeof(ToolTipPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/ToolTip.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.Comment24 },
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
