using WPFGallery.Models;

namespace WPFGallery.Navigation;

/// <summary>
/// Interface for the NavigationService
/// </summary>
public interface INavigationService
{
    void Navigate(Type type);

    void Navigate(string str);

    void NavigateTo(Type type);

    void SetFrame(Frame frame);

    void NavigateBack();

    void NavigateForward();

    bool IsBackHistoryNonEmpty();

    event EventHandler<NavigatingEventArgs> Navigating;
}

/// <summary>
/// Service for navigating between pages.
/// </summary>
public class NavigationService : INavigationService
{
    private Frame _frame;

    private Type _currentPageType = null;

    private readonly Stack<Type> _history;

    private readonly Stack<Type> _future;

    private readonly IServiceProvider _serviceProvider;

    public event EventHandler<NavigatingEventArgs> Navigating;

    private Dictionary<string, Type> _pageNameToTypeMapping;


    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _history = new Stack<Type>();
        _future = new Stack<Type>();

        InitializeMapping();
    }

    private void InitializeMapping()
    {
        ICollection<ControlInfoDataItem> allPages = ControlsInfoDataSource.Instance.ControlsInfo;

        _pageNameToTypeMapping = new();

        foreach (ControlInfoDataItem item in allPages)
        {
            _pageNameToTypeMapping[item.Title] = item.PageType;

            foreach (ControlInfoDataItem individualItem in item.Items)
            {
                _pageNameToTypeMapping[individualItem.Title] = individualItem.PageType;
            }
        }        
    }

    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    public void NavigateTo(Type type)
    {
        if (type != null)
        {
            _future.Clear();
            RaiseNavigatingEvent(type);
        }
    }

    public void Navigate(Type type)
    {
        if(type != null)
        {
            _history.Push(_currentPageType);
            _currentPageType = type;
            var page = _serviceProvider.GetRequiredService(type);
            _frame.Navigate(page);
        }
    }

    public void Navigate(string? pageName)
    {
        if (!string.IsNullOrEmpty(pageName) && _pageNameToTypeMapping.ContainsKey(pageName))
        {
            Type currentPage = _pageNameToTypeMapping[pageName];
            Navigate(currentPage);
        }
    }

    public void NavigateBack()
    {
        if(_history.Count > 0)
        {
            Type type = _history.Pop();
            if (type != null)
            {
                _future.Push(type);
                RaiseNavigatingEvent(type);
                _history.Pop();
            }
        }
    }

    public void NavigateForward()
    {
        if(_future.Count > 0)
        {
            Type type = _future.Pop();
            if (type != null)
            {
                _history.Push(type);
                RaiseNavigatingEvent(type);
            }
        }
    }

    public void RaiseNavigatingEvent(Type type)
    {
        Navigating?.Invoke(this, new NavigatingEventArgs(type));
    }

    public bool IsBackHistoryNonEmpty()
    {
        var item = _history.Peek();
        if (item == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
