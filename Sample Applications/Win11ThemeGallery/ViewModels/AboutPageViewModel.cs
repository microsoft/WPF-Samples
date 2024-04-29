using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;

namespace Win11ThemeGallery.ViewModels
{
    public partial class AboutPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "About";
    }
}
