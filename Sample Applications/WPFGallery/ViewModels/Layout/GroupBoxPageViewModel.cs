using System;
using System.Collections.Generic;
using System.Text;

namespace WPFGallery.ViewModels
{
    public partial class GroupBoxPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Hyperlink";

        [ObservableProperty]
        private string _pageDescription = "";

        public GroupBoxPageViewModel() { }
    }
}
