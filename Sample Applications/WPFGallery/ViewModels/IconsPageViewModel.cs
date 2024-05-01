using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WPFGallery.Models;
using WPFGallery.Navigation;

namespace WPFGallery.ViewModels
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
            var jsonText = File.ReadAllText("Models/IconsData.json");
            _icons = JsonSerializer.Deserialize<List<IconData>>(jsonText);
            _selectedIcon = _icons.FirstOrDefault();
        }
    }
}
