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
    public partial class CollectionsPageViewModel : BaseSectionPageViewModel
    {
        public CollectionsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Collections";
            PageDescription = "Controls for collection presentation";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Collections");
        }
    }
}
