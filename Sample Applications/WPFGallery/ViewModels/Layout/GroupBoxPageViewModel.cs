using System;
using System.Collections.Generic;
using System.Text;

namespace WPFGallery.ViewModels
{
    public partial class GroupBoxPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "GroupBox";

        [ObservableProperty]
        private string _pageDescription = "";

        public GroupBoxPageViewModel() { }
    }
}
