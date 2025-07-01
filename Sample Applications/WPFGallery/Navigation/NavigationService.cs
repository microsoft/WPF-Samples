using System.Collections;
using System.Windows.Navigation;

namespace WPFGallery.Navigation;

/// <summary>
/// Interface for the NavigationService
/// </summary>
public interface INavigationService
{
    void Navigate(Type type, bool adjustFocus = true);

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


    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _history = new Stack<Type>();
        _future = new Stack<Type>();
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

    public void Navigate(Type type, bool adjustFocus)
    {
        if(type != null)
        {
            _history.Push(_currentPageType);
            _currentPageType = type;
            var page = _serviceProvider.GetRequiredService(type);
            
            if(adjustFocus && page is Page pg)
            {
                pg.Loaded += MoveFocusToPage;
            }

            _frame.Navigate(page);
        }
    }

    private void MoveFocusToPage(object sender, RoutedEventArgs e)
    {
        if(sender is Page page)
        {
            page.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            page.Loaded -= MoveFocusToPage;
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
