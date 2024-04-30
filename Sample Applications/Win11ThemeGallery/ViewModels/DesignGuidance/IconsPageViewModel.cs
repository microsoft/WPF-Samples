using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Win11ThemeGallery.Models;
using Win11ThemeGallery.Navigation;

namespace Win11ThemeGallery.ViewModels
{
    public partial class IconsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Icons";

        [ObservableProperty]
        private string _pageDescription = "Guide showing how to use icons in your application.";

        [ObservableProperty]
        private ICollection<IconData> _icons;

        [ObservableProperty]
        private IconData? _selectedIcon;

        public IconsPageViewModel()
        {
            // HACK to avoid delay in loading the icons, rest of the icons are loaded after the Page is loaded.
            // Find a better way to load the icons
            _icons = IconsDataSource.GetFirstIconsData(200);
            _selectedIcon = _icons.FirstOrDefault();
        }
    }
}
