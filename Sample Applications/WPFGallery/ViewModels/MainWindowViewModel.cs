using System.ComponentModel;
using System.Windows.Controls.Primitives;
using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Views.Samples;
using WPFGallery.Models;

namespace WPFGallery.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "WPF Gallery Preview";

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
        _navigationService.NavigateTo(ControlsInfoDataSource.Instance.GetControlInfo("Settings"));
    }

    [RelayCommand]
    public void About()
    {
        _navigationService.Navigate(ControlsInfoDataSource.Instance.GetControlInfo("About"));
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
    }

    internal List<ControlInfoDataItem> GetNavigationItemHierarchyFromPageType(ControlInfoDataItem? dataItem)
    {
        List<ControlInfoDataItem> list = new List<ControlInfoDataItem>();
        Stack<ControlInfoDataItem> _stack = new Stack<ControlInfoDataItem>();
        Stack<ControlInfoDataItem> _revStack = new Stack<ControlInfoDataItem>();
        
        if(dataItem == null)
        {
            return list;
        }

        bool found = false;

        foreach(var item in Controls)
        {
            _stack.Push(item);
            found = FindNavigationItemsHierarchyFromPageType(dataItem, item.Items, ref _stack);
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

    private bool FindNavigationItemsHierarchyFromPageType(ControlInfoDataItem dataItem, ICollection<ControlInfoDataItem> pages, ref Stack<ControlInfoDataItem> stack)
    {
        var item = stack.Peek();
        bool found = false;

        if(dataItem == item)
        {
            return true;
        }

        foreach(var child in item.Items)
        {
            stack.Push(child);
            found = FindNavigationItemsHierarchyFromPageType(dataItem, child.Items, ref stack);
            if(found) { return true; }
            stack.Pop();
        }

        return false;
    }

    internal void UpdateCanNavigateBack()
    {
        CanNavigateback = _navigationService.CanNavigateBack();  
    }

}
