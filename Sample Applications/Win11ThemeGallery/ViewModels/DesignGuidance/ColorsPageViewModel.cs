using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win11ThemeGallery.Navigation;

namespace Win11ThemeGallery.ViewModels
{
    public partial class ColorsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Colors";

        [ObservableProperty]
        private string _pageDescription = "The Windows 11 color palette is designed to be bold and vibrant, with a focus on high contrast and accessibility. The palette includes a wide range of colors, from bright and saturated to soft and muted, allowing you to create a variety of visual styles and moods.";

    }
}
