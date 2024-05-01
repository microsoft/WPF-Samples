using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WPFGallery.Navigation;
using WPFGallery.Views;

namespace WPFGallery.ViewModels
{
    public partial class AboutPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "About";
    }
}
