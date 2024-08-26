using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Views.Samples;
using WPFGallery.Models;

namespace WPFGallery.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "WPF Gallery Preview";

    private readonly DispatcherTimer _timer;

    private string _searchText = string.Empty;

    [ObservableProperty]
    private ICollection<ControlInfoDataItem> _controls;
    [ObservableProperty]
    private ControlInfoDataItem? _selectedControl;
    private readonly INavigationService _navigationService;
    [ObservableProperty]
    private bool _canNavigateback;

    [RelayCommand]
    public void Settings()
    {
        _navigationService.NavigateTo(typeof(SettingsPage));
    }

    [RelayCommand]
    public void About()
    {
        _navigationService.Navigate(typeof(AboutPage));
    }

    [RelayCommand]
    public void Back()
    {
        _navigationService.NavigateBack();
    }

    [RelayCommand]
    public void Forward()
    {
        _navigationService.NavigateForward();
    }

    public MainWindowViewModel(INavigationService navigationService)
    {
        _controls = ControlsInfoDataSource.Instance.ControlsInfo;
        _navigationService = navigationService;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(400);
        _timer.Tick += PerformSearchNavigation;
    }

    public void UpdateSearchText(string searchText)
    {
        _searchText = searchText;
        _timer.Stop();
        _timer.Start();
    }

    private void PerformSearchNavigation(object? sender, EventArgs e)
    {
        _timer.Stop();
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            return;
        }

        _navigationService.NavigateTo(GetNavigationPageTypeFromName(_searchText, _controls));
    }

    private Type? GetNavigationPageTypeFromName(string name, ICollection<ControlInfoDataItem> pages)
    {
        Type? type = null;

        if(pages == null)
        {
            return null;
        }

        foreach(var item in pages)
        {
            if (item.Title.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return item.PageType!;
            }

            type = GetNavigationPageTypeFromName(name, item.Items);

            if(type != null)
            {
                return type;
            }
        }
        return null;
    }

    internal List<ControlInfoDataItem> GetNavigationItemHierarchyFromPageType(Type? pageType)
    {
        List<ControlInfoDataItem> list = new List<ControlInfoDataItem>();
        Stack<ControlInfoDataItem> _stack = new Stack<ControlInfoDataItem>();
        Stack<ControlInfoDataItem> _revStack = new Stack<ControlInfoDataItem>();
        
        if(pageType == null)
        {
            return list;
        }

        bool found = false;

        foreach(var item in Controls)
        {
            _stack.Push(item);
            found = FindNavigationItemsHierarchyFromPageType(pageType, item.Items, ref _stack);
            if(found)
            {
                break;
            }
            _stack.Pop();
        }

        while(_stack.Count > 0)
        {
            _revStack.Push(_stack.Pop());
        }

        foreach(var item in _revStack)
        {
            list.Add(item);
        }

        return list;
    }

    private bool FindNavigationItemsHierarchyFromPageType(Type pageType, ICollection<ControlInfoDataItem> pages, ref Stack<ControlInfoDataItem> stack)
    {
        var item = stack.Peek();
        bool found = false;

        if(pageType == item.PageType)
        {
            return true;
        }

        foreach(var child in item.Items)
        {
            stack.Push(child);
            found = FindNavigationItemsHierarchyFromPageType(pageType, child.Items, ref stack);
            if(found) { return true; }
            stack.Pop();
        }

        return false;
    }

    internal void UpdateCanNavigateBack()
    {
        CanNavigateback = _navigationService.IsBackHistoryNonEmpty();  
    }

}
