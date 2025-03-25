using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels
{
    public partial class ResizeGripPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "ResizeGrip";

        [ObservableProperty]
        private string _pageDescription = "";

        public ResizeGripPageViewModel() { }
    }
}
