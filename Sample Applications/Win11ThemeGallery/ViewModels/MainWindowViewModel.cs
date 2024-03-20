using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Win11ThemeGallery.Navigation;
using Win11ThemeGallery.Views;
using Win11ThemeGallery.Views.Samples;

namespace Win11ThemeGallery.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "WPF Win11 Theme Gallery";

    private DispatcherTimer _timer;

    private string _searchText = string.Empty;

    [ObservableProperty]
    private ICollection<NavigationItem> _controls = new ObservableCollection<NavigationItem>
    {
        new NavigationItem("Home", typeof(DashboardPage)),
        new NavigationItem
        {
            Name = "Samples",
            PageType = typeof(SamplesPage),
            Children = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("User Dashboard", typeof(UserDashboardPage)),
            }
        },
        new NavigationItem
        {
            Name = "Basic Input",
            PageType = typeof(BasicInputPage),
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
    private INavigationService _navigationService;

    [RelayCommand]
    public void Settings()
    {
        _navigationService.NavigateTo(typeof(SettingsPage));
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
}
