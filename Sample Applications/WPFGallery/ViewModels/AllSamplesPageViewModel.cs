using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using System.Windows.Controls;
using WPFGallery.Views;
using WPFGallery.Models;

namespace WPFGallery.ViewModels
{
    public partial class AllSamplesPageViewModel : BaseSectionPageViewModel
    {
        public AllSamplesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "All Controls";
            PageDescription = "This page contains all the controls available in the gallery.";
            NavigationCards = ControlsInfoDataSource.Instance.GetAllControlsInfo();
        }
    }
}
