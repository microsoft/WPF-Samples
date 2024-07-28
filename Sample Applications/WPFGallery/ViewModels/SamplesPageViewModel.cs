using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views.Samples;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class SamplesPageViewModel : BaseSectionPageViewModel
    {
        public SamplesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Samples";
            PageDescription = "Sample pages for common scenarios";
            NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Samples");
        }
    }
}
