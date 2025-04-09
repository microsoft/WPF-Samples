using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels
{
    public partial class FramePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Frame";

        [ObservableProperty]
        private string _pageDescription = "";

        public FramePageViewModel() { }
    }
}
