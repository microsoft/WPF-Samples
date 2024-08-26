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
    public partial class DesignGuidancePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Design Guidance";

        [ObservableProperty]
        private string _pageDescription = "Design guidelines on how to use colors, typography, and icons in your app.";       

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetControlsInfo("Design Guidance");

        private readonly INavigationService _navigationService;

        public DesignGuidancePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Navigate(object pageType){
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }  
    }
}
