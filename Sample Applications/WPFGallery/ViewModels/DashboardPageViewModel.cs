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
    public partial class DashboardPageViewModel : BaseSectionPageViewModel
    {
        public DashboardPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationCards = ControlsInfoDataSource.Instance.GetGroupedControlsInfo();
        }
    }
}
