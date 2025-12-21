using System.Text.Json;
using WPFGallery.Models;
using System.IO;

namespace WPFGallery.ViewModels
{
    public partial class IconsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Icons";

        [ObservableProperty]
        private string _pageDescription = "Guide showing how to use icons in your application.";

        [ObservableProperty]
        private ICollection<IconData> _allIcons = [];

        [ObservableProperty]
        private IconData? _selectedIcon;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<IconData> _searchFilteredIcons = [];

        [RelayCommand]
        private async Task LoadData()
        {
            AllIcons = await ReadIconData();
            SelectedIcon = AllIcons.FirstOrDefault();
            SearchFilteredIcons = new ObservableCollection<IconData>(AllIcons);
        }

        private static async Task<IList<IconData>> ReadIconData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WPFGallery.Models.IconsData.json";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            return await JsonSerializer.DeserializeAsync<List<IconData>>(stream);
        }

        partial void OnSearchTextChanged(string searchText)
        {
            //cache the name here to set the selected item after clearing and repopulating the list
            var selectedIconName = SelectedIcon?.Name;
            SearchFilteredIcons.Clear();

            var comparison = StringComparison.OrdinalIgnoreCase;
            var filterText = searchText ?? string.Empty;

            var searchFilteredIconData = AllIcons.Where(icon =>
                icon.Name.IndexOf(filterText, comparison) >= 0 ||
                (icon.Tags?.Any(tag => tag.IndexOf(filterText, comparison) >= 0) ?? false));
            foreach (var item in searchFilteredIconData)
            {
                SearchFilteredIcons.Add(item);
            }

            //keep the selected icon the same if it exists in the search results, if not select the first one
            Func<IconData, bool> predicate =
              !string.IsNullOrWhiteSpace(selectedIconName) &&
              SearchFilteredIcons.Any(icon => icon.Name.Equals(selectedIconName)) ?
              icon => icon.Name.Equals(selectedIconName) :
              icon => true;

            SelectedIcon = SearchFilteredIcons.FirstOrDefault(predicate);
        }
    }
}
