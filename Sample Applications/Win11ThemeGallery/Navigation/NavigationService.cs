using System.Collections;
using System.Windows.Controls;

namespace Win11ThemeGallery.Navigation;

public interface INavigationService
{
    void NavigateTo(Type type);

    void SetFrame(Frame frame);

    void NavigateBack();

    void NavigateForward();

}


public class NavigationService : INavigationService
{
    private Frame _frame;

    private Type _currentPageType = null;

    private Stack<Type> _history;

    private Stack<Type> _future;

    private readonly IServiceProvider _serviceProvider;


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
            _history.Push(_currentPageType);
            _currentPageType = type;
            var page = _serviceProvider.GetRequiredService(type);
            _frame.Navigate(page);
        }
    }

    public void NavigateBack()
    {
        if(_history.Count > 0)
        {
            Type type = _history.Peek();
            if (type != null)
            {
                _history.Pop();
                _future.Push(_currentPageType);
                _currentPageType = type;
                var page = _serviceProvider.GetRequiredService(type);
                _frame.Navigate(page);
            }
        }
    }

    public void NavigateForward()
    {
        if(_future.Count > 0)
        {
            Type type = _future.Peek();
            if (type != null)
            {
                _future.Pop();
                _history.Push(_currentPageType);
                _currentPageType = type;
                var page = _serviceProvider.GetRequiredService(type);
                _frame.Navigate(page);
            }
        }
    }

}
