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
        private string _pageDescription = "With the release of Windows 11, Segoe Fluent Icons is the recommended icon font.";

        [ObservableProperty]
        private ICollection<IconData> _icons;

        [ObservableProperty]
        private IconData? _selectedIcon;

        public IconsPageViewModel()
        {
            var jsonText = File.ReadAllText("Models/IconsData.json");
            _icons = JsonSerializer.Deserialize<List<IconData>>(jsonText);
            _selectedIcon = _icons.FirstOrDefault();
        }
    }
}
