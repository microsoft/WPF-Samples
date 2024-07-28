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
    public partial class TextPageViewModel : BaseSectionPageViewModel
    {
        public TextPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Text";
            PageDescription = "Controls for displaying and editing text";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Text");
        }
    }
}
