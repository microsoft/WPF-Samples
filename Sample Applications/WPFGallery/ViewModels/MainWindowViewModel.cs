using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPFGallery.Navigation;
using WPFGallery.Views;
using WPFGallery.Views.Samples;

namespace WPFGallery.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "WPF Gallery";

    private readonly DispatcherTimer _timer;

    private string _searchText = string.Empty;

    [ObservableProperty]
    private ICollection<NavigationItem> _controls = new ObservableCollection<NavigationItem>
    {
        new NavigationItem
        {
            Name = "Home",
            PageType = typeof(DashboardPage),
            Icon = "\xE80F"
        },
        new NavigationItem
        {

            Name = "Design Guidance",
            PageType = typeof(DesignGuidancePage),
            Icon = "\xE8FD",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem
                {
                    Name = "Colors",
                    PageType = typeof(ColorsPage),
                    Icon = "\xE790"
                },
                new NavigationItem
                {
                    Name = "Typography",
                    PageType = typeof(TypographyPage),
                    Icon = "\xE8D2"
                },
                new NavigationItem
                {
                    Name = "Icons",
                    PageType = typeof(IconsPage),
                    Icon = "\xED58"
                },
            }
        },
        new NavigationItem
        {
            Name = "Samples",
            PageType = typeof(SamplesPage),
            Icon = "\xEF58",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("User Dashboard", typeof(UserDashboardPage)),
            }
        },
        new NavigationItem
        {
            Name = "All Samples",
            PageType = typeof(AllSamplesPage),
            Icon = "\xE71D",
        },
        new NavigationItem
        {
            Name = "Basic Input",
            PageType = typeof(BasicInputPage),
            Icon = "\xE73A",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Button", typeof(ButtonPage)),
                new NavigationItem("CheckBox", typeof(CheckBoxPage)),
                new NavigationItem("ComboBox", typeof(ComboBoxPage)),
                new NavigationItem("RadioButton", typeof(RadioButtonPage)),
                new NavigationItem("Slider", typeof(SliderPage)),
            }
        },
        new NavigationItem
        {
            Name="Collections",
            PageType = typeof(CollectionsPage),
            Icon = "\xE80A",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("DataGrid", typeof(DataGridPage)),
                new NavigationItem("ListBox", typeof(ListBoxPage)),
                new NavigationItem("ListView", typeof(ListViewPage)),
                new NavigationItem("TreeView", typeof(TreeViewPage)),
            }
        },
        new NavigationItem
        {
            Name="Date & Calendar",
            PageType = typeof(DateAndTimePage),
            Icon = "\xEC92",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Calendar", typeof(CalendarPage)),
                new NavigationItem("DatePicker", typeof(DatePickerPage)),
            }
        },
        new NavigationItem
        {
            Name = "Layout",
            PageType = typeof(LayoutPage),
            Icon = "\xF246",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Expander", typeof(ExpanderPage)),
            }
        },
        // new NavigationItem
        // {
        //     Name = "Media",
        //     PageType = typeof(MediaPage),
        //     Children = new ObservableCollection<NavigationItem>
        //     {
        //         new NavigationItem("Canvas", typeof(CanvasPage)),
        //         new NavigationItem("Image", typeof(ImagePage)),
        //     }
        // },
        new NavigationItem
        {
            Name = "Navigation",
            PageType = typeof(NavigationPage),
            Icon = "\xE700",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Menu", typeof(MenuPage)),
                new NavigationItem("TabControl", typeof(TabControlPage)),
            }
        },
        new NavigationItem
        {
            Name = "Status & Info",
            PageType = typeof(StatusAndInfoPage),
            Icon = "\xE8F2",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("ProgressBar", typeof(ProgressBarPage)),
                new NavigationItem("ToolTip", typeof(ToolTipPage)),
            }
        },
        new NavigationItem
        {
            Name = "Text",
            PageType = typeof(TextPage),
            Icon = "\xE8D2",
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Label", typeof(LabelPage)),
                new NavigationItem("TextBox", typeof(TextBoxPage)),
                new NavigationItem("TextBlock", typeof(TextBlockPage)),
                new NavigationItem("RichTextEdit", typeof(RichTextEditPage)),
                new NavigationItem("PasswordBox", typeof(PasswordBoxPage)),
            }
        },
    };

    [ObservableProperty]
    private NavigationItem? _selectedControl;
    private readonly INavigationService _navigationService;

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

    private Type? GetNavigationPageTypeFromName(string name, ICollection<NavigationItem> pages)
    {
        Type? type = null;

        if(pages == null)
        {
            return null;
        }

        foreach(var item in pages)
        {
            if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return item.PageType!;
            }

            type = GetNavigationPageTypeFromName(name, item.Children);

            if(type != null)
            {
                return type;
            }
        }
        return null;
    }

    internal List<NavigationItem> GetNavigationItemHierarchyFromPageType(Type? pageType)
    {
        List<NavigationItem> list = new List<NavigationItem>();
        Stack<NavigationItem> _stack = new Stack<NavigationItem>();
        Stack<NavigationItem> _revStack = new Stack<NavigationItem>();
        
        if(pageType == null)
        {
            return list;
        }

        bool found = false;

        foreach(var item in Controls)
        {
            _stack.Push(item);
            found = FindNavigationItemsHierarchyFromPageType(pageType, item.Children, ref _stack);
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

    private bool FindNavigationItemsHierarchyFromPageType(Type pageType, ICollection<NavigationItem> pages, ref Stack<NavigationItem> stack)
    {
        var item = stack.Peek();
        bool found = false;

        if(pageType == item.PageType)
        {
            return true;
        }

        foreach(var child in item.Children)
        {
            stack.Push(child);
            found = FindNavigationItemsHierarchyFromPageType(pageType, child.Children, ref stack);
            if(found) { return true; }
            stack.Pop();
        }

        return false;
    }
}
