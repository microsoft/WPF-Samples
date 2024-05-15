using System.Text.Json;

namespace WPFGallery.Models
{
    public static class IconsDataSource
    {
        private static List<IconData> _iconsData = new List<IconData>();

        public static void InitializeData()
        {
            var jsonText = File.ReadAllText("Models/IconsData.json");
            _iconsData = JsonSerializer.Deserialize<List<IconData>>(jsonText);
            Debug.WriteLine($"Loaded {_iconsData.Count} icons.");
        }

        public static List<IconData> GetIconsData()
        {
            return _iconsData;
        }

        public static List<IconData> GetFirstIconsData(int count)
        {
            return _iconsData.Take(count).ToList();
        }
    }
}