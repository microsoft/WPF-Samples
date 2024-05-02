using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGallery.Navigation;

namespace WPFGallery.ViewModels
{
    public partial class TypographyPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Typography";

        [ObservableProperty]
        private string _pageDescription = "Type helps provide structure and hierarchy to UI. The default font for Windows is Segoe UI Variable. Best practice is to use Regular weight for most text, use Semibold for titles. The minimum values should be 12px Regular, 14px Semibold.";
    }
}
