using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
               // Icon = newSymbolIcon { Symbol = SymbolRegular.ArrowDownload24 },
                Description = "Shows the app progress on a task"
            },
            new NavigationCard
            {
                Name = "ToolTip",
                PageType = typeof(ToolTipPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.Comment24 },
                Description = "Information in popup window"
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
