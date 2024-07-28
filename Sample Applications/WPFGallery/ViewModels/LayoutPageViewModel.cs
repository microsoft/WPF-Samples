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
    public partial class LayoutPageViewModel : BaseSectionPageViewModel
    {
        public LayoutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Layout";
            PageDescription = "Controls for layout";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Layout");
        }
    }
}
