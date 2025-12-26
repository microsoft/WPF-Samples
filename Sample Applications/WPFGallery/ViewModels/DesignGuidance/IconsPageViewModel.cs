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

        [ObservableProperty]
        private ObservableCollection<IconData> _displayedIcons = [];

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private int _totalPages = 1;

        [ObservableProperty]
        private int _selectedPageSizeIndex = 1; // Default to 250

        public List<string> PageSizeOptions { get; } = ["100", "250", "500", "1000", "All"];

        private int PageSize => SelectedPageSizeIndex == 4 ? int.MaxValue : int.Parse(PageSizeOptions[SelectedPageSizeIndex]);

        [RelayCommand]
        private async Task LoadData()
        {
            AllIcons = await ReadIconData();
            SelectedIcon = AllIcons.FirstOrDefault();
            SearchFilteredIcons = new ObservableCollection<IconData>(AllIcons);
            UpdatePagination();
        }

        private static async Task<IList<IconData>> ReadIconData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WPFGallery.Models.IconsData.json";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            return await JsonSerializer.DeserializeAsync<List<IconData>>(stream);
        }

        partial void OnSearchTextChanged(string value)
        {
            var previousSelectedIcon = SelectedIcon;

            //cache the name here to set the selected item after clearing and repopulating the list
            var selectedIconName = previousSelectedIcon?.Name;
            var comparison = StringComparison.OrdinalIgnoreCase;
            var filterText = value ?? string.Empty;
            SearchFilteredIcons.Clear();

            var searchFilteredIconData = AllIcons.Where(icon =>
                icon.Name.IndexOf(filterText, comparison) >= 0 ||
                (icon.Tags?.Any(tag => tag.IndexOf(filterText, comparison) >= 0) ?? false));
            foreach (var item in searchFilteredIconData)
            {
                SearchFilteredIcons.Add(item);
            }

            // Reset to page 1 and update pagination
            CurrentPage = 1;
            UpdatePagination(false);

            if (SearchFilteredIcons.Count == 0)
            {
                SelectedIcon = previousSelectedIcon;
                return;
            }

            if (string.IsNullOrWhiteSpace(filterText))
            {
                SelectedIcon = DisplayedIcons.FirstOrDefault();
                return;
            }

            //keep the selected icon the same if it exists in the search results, if not select the first one
            Func<IconData, bool> predicate =
              !string.IsNullOrWhiteSpace(selectedIconName) &&
              DisplayedIcons.Any(icon => icon.Name.Equals(selectedIconName)) ?
              icon => icon.Name.Equals(selectedIconName) :
              icon => true;

            SelectedIcon = DisplayedIcons.FirstOrDefault(predicate);
        }

        [RelayCommand]
        private void ApplyTagFilter(string? tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return;
            }

            var trimmedTag = tag.Trim();
            if (string.Equals(trimmedTag, SearchText, StringComparison.Ordinal))
            {
                return;
            }

            SearchText = trimmedTag;
        }

        partial void OnSelectedPageSizeIndexChanged(int value)
        {
            CurrentPage = 1;
            UpdatePagination();
        }

        [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateDisplayedIcons();
            }
        }

        private bool CanGoToPreviousPage() => CurrentPage > 1;

        [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdateDisplayedIcons();
            }
        }

        private bool CanGoToNextPage() => CurrentPage < TotalPages;

        partial void OnCurrentPageChanged(int value)
        {
            PreviousPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }

        partial void OnTotalPagesChanged(int value)
        {
            PreviousPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }

        private void UpdatePagination(bool resetSelectedIcon = true)
        {
            var pageSize = PageSize;
            TotalPages = pageSize == int.MaxValue ? 1 : (int)Math.Ceiling((double)SearchFilteredIcons.Count / pageSize);
            if (TotalPages == 0) TotalPages = 1;
            
            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }
            
            UpdateDisplayedIcons(resetSelectedIcon);
        }

        private void UpdateDisplayedIcons(bool resetSelectedIcon = true)
        {
            DisplayedIcons.Clear();
            
            var pageSize = PageSize;
            var skip = (CurrentPage - 1) * pageSize;
            var iconsToDisplay = pageSize == int.MaxValue ? SearchFilteredIcons : SearchFilteredIcons.Skip(skip).Take(pageSize);
            
            foreach (var icon in iconsToDisplay)
            {
                DisplayedIcons.Add(icon);
            }

            if(resetSelectedIcon)
            {
                SelectedIcon = DisplayedIcons.FirstOrDefault();
            }
        }
    }
}
