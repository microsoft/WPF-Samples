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
    public partial class AllSamplesPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "All Controls";

        [ObservableProperty]
        private string _pageDescription = "";

        [ObservableProperty]
        private ICollection<ControlInfoDataItem> _navigationCards = ControlsInfoDataSource.Instance.GetAllControlsInfo();
        private readonly INavigationService _navigationService;

        public AllSamplesPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void Navigate(object pageType)
        {
            if (pageType is Type page)
            {
                _navigationService.NavigateTo(page);
            }
        }

    }
}
