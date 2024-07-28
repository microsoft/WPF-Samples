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
    public partial class NavigationPageViewModel : BaseSectionPageViewModel
    {
        public NavigationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Navigation";
            PageDescription = "Controls for navigation and actions";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Navigation");
        }
    }
}
