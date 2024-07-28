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
    public partial class DesignGuidancePageViewModel : BaseSectionPageViewModel
    {
        public DesignGuidancePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Design Guidance";
            PageDescription = "Design guidelines on how to use colors, typography, and icons in your app.";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Design Guidance");
        }
    }
}
