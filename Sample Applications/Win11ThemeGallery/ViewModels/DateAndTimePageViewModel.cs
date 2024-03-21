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
    public partial class DateAndTimePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Date and Calendar Controls";

        [ObservableProperty]
        private string _pageDescription = "Controls for date and calendar";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Calendar",
                PageType = typeof(CalendarPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CalendarLtr24 },
                Description = "Presents a calendar to the user"
            },
            new NavigationCard
            {
                Name = "DatePicker",
                PageType = typeof(DatePickerPage),
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CalendarSearch20 },
                Description = "Control that lets pick a date"
            },
        };

        private readonly INavigationService _navigationService;

        public DateAndTimePageViewModel(INavigationService navigationService)
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
