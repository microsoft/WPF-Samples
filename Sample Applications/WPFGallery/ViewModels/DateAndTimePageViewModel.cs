using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class DateAndTimePageViewModel : BaseSectionPageViewModel
    {
        public DateAndTimePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Date & Calendar";
            PageDescription = "Controls for date and calendar";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Date & Calendar");
        }
    }
}
