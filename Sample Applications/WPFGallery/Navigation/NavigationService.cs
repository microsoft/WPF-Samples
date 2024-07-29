using WPFGallery.Models;
using WPFGallery.ViewModels;
using WPFGallery.Views;

namespace WPFGallery.Navigation;

public interface INavigationService
{
    void Navigate(ControlInfoDataItem item);
    void NavigateTo(ControlInfoDataItem item);
    void NavigateBack();
    void NavigateForward();
    void SetFrame(Frame frame);
    bool CanNavigateBack();
    bool CanNavigateForward();
    event EventHandler<NavigatingEventArgs> Navigating;
}

public class NavigationService : INavigationService
{
    private Frame _frame;
    private ControlInfoDataItem _currentPage;
    private readonly Stack<ControlInfoDataItem> _history;
    private readonly Stack<ControlInfoDataItem> _future;
    private readonly IServiceProvider _serviceProvider;

    public event EventHandler<NavigatingEventArgs> Navigating;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _history = new Stack<ControlInfoDataItem>();
        _future = new Stack<ControlInfoDataItem>();
    }

    public void SetFrame(Frame frame) => _frame = frame;

    public void Navigate(ControlInfoDataItem item)
    {
        if (item != null)
        {
            _history.Push(_currentPage);
            _currentPage = item;
            _frame.Navigate(GetPage(item));
        }
    }

    public void NavigateTo(ControlInfoDataItem item)
    {
        if (item != null)
        {
            _future.Clear();
            RaiseNavigatingEvent(item);
        }
    }

    public void NavigateBack()
    {
        if (_history.Count > 0)
        {
            ControlInfoDataItem item = _history.Pop();
            _future.Push(_currentPage);
            RaiseNavigatingEvent(_history.Peek());
        }
    }

    public void NavigateForward()
    {
        if (_future.Count > 0)
        {
            ControlInfoDataItem item = _future.Pop();
            _history.Push(item);
            RaiseNavigatingEvent(item);
        }
    }

    public bool CanNavigateBack()
    {
        return _history.Count > 0;
    }

    public bool CanNavigateForward()
    {
        return _future.Count > 0;
    }

    private void RaiseNavigatingEvent(ControlInfoDataItem item)
    {
        Navigating?.Invoke(this, new NavigatingEventArgs(item));
    }

    private object GetPage(ControlInfoDataItem item)
    {
        if(item.PageType != typeof(SectionPage))
        {
            return _serviceProvider.GetRequiredService(item.PageType);
        }

        SectionPage page = (SectionPage)_serviceProvider.GetRequiredService(item.PageType);
        page.ViewModel = GetSectionPageViewModel(item);
        return page;
    }

    private BaseSectionPageViewModel GetSectionPageViewModel(ControlInfoDataItem item)
    {
        var viewModel = _serviceProvider.GetRequiredService<BaseSectionPageViewModel>();
        viewModel.PageTitle = item.Title;
        viewModel.PageDescription = item.Description;
        viewModel.NavigationCards = ControlsInfoDataSource.Instance.GetControlsInfo(item.UniqueId);
        return viewModel;
    }
}