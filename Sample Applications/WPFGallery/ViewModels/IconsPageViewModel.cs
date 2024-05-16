using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WPFGallery.Models;
using WPFGallery.Navigation;
using System.IO;
using System.Reflection;

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
            var jsonText = ReadIconData();
            _icons = JsonSerializer.Deserialize<List<IconData>>(jsonText);
            _selectedIcon = _icons.FirstOrDefault();
        }

        private string ReadIconData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WPFGallery.Models.IconsData.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
