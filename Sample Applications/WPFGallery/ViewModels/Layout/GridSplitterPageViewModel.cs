using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGallery.ViewModels.Layout
{
    public partial class GridSplitterPageViewModel  : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "GridSplitter";

        [ObservableProperty]
        private string _pageDescription = "";

        public GridSplitterPageViewModel() { }
    }
}
