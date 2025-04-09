using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels
{
    public partial class NavigationWindowPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Navigation Window";

        [ObservableProperty]
        private string _pageDescription = "";

        public NavigationWindowPageViewModel() { }
    }
}
