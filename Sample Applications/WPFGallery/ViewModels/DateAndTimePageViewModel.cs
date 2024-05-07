using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class DateAndTimePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Date & Calendar";

        [ObservableProperty]
        private string _pageDescription = "Controls for date and calendar";

        [ObservableProperty]
        private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new NavigationCard
            {
                Name = "Calendar",
                PageType = typeof(CalendarPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/CalendarView.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CalendarLtr24 },
                Description = "A control that presents a calendar for a user to choose a date from."
            },
            new NavigationCard
            {
                Name = "DatePicker",
                PageType = typeof(DatePickerPage),
                Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DatePicker.png"))},
               // Icon = newSymbolIcon { Symbol = SymbolRegular.CalendarSearch20 },
                Description = "A control that lets a user pick a date value."
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
