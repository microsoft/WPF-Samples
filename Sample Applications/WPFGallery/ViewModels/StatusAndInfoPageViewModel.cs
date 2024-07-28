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
    public partial class StatusAndInfoPageViewModel : BaseSectionPageViewModel
    {
        public StatusAndInfoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Status & Info";
            PageDescription = "Controls to show progress and extra information";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Status & Info");
        }
    }
}
