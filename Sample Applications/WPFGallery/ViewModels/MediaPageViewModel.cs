using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class MediaPageViewModel : BaseSectionPageViewModel
    {
        public MediaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Media Controls";
            PageDescription = "Controls for media presentation";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Media");
        }
    }
}
