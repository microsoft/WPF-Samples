using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class BasePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "";

        [ObservableProperty]
        private string _pageDescription = "";
    }
}