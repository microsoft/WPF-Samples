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
    public partial class DateAndTimePageViewModel : BaseNavigablePageViewModel
    {
        public DateAndTimePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Date and Calendar Controls";
            PageDescription = "Controls for date and calendar";
            NavigationCards = new ObservableCollection<NavigationCard>
            {
                new NavigationCard
                {
                    Name = "Calendar",
                    PageType = typeof(CalendarPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/CalendarView.png"))},
                    Description = "A control that presents a calendar for a user to choose a date from."
                },
                new NavigationCard
                {
                    Name = "DatePicker",
                    PageType = typeof(DatePickerPage),
                    Icon = new Image {Source= new BitmapImage(new Uri("pack://application:,,,/Assets/ControlImages/DatePicker.png"))},
                    Description = "A control that lets a user pick a date value."
                },
            };
        }
    }
}
