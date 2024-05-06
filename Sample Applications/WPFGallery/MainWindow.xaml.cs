using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPFGallery.Navigation;
using WPFGallery.ViewModels;
using WPFGallery.Views;

namespace WPFGallery;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider, INavigationService navigationService)
    {
        _serviceProvider = serviceProvider;
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();

        Toggle_TitleButtonVisibility();

        _navigationService = navigationService;
        _navigationService.Navigating += OnNavigating;
        _navigationService.SetFrame(this.RootContentFrame);
        _navigationService.Navigate(typeof(DashboardPage));

        WindowChrome.SetWindowChrome(
            this,
            new WindowChrome
            {
                CaptionHeight = 50,
                CornerRadius = default,
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = true
            }
        );

        this.StateChanged += MainWindow_StateChanged;
        SearchComboBox.ItemsSource = ViewModel.NavigationItemsForSearchBox;
    }

    private void MainWindow_StateChanged(object sender, EventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            MainGrid.Margin = new Thickness(8);
        }
        else
        {
            MainGrid.Margin = default;
        }
    }

    private IServiceProvider _serviceProvider;
    private INavigationService _navigationService;

    public MainWindowViewModel ViewModel { get; }

    private void ControlsList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (ControlsList.SelectedItem is NavigationItem navItem)
        {
            _navigationService.Navigate(navItem.PageType);

            var tvi = ControlsList.ItemContainerGenerator.ContainerFromItem(navItem) as TreeViewItem;
            if(tvi != null)
            {
                tvi.IsExpanded = true;
                tvi.BringIntoView();
            }
        }
    }

    private void Toggle_TitleButtonVisibility()
    {
        var appContextBackdropData = AppContext.GetData("Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop");
        bool disableFluentThemeWindowBackdrop = false;

        if (appContextBackdropData != null)
        {
            disableFluentThemeWindowBackdrop = bool.Parse(Convert.ToString(appContextBackdropData));
        }


        if (!disableFluentThemeWindowBackdrop)
        {
            foreach (ResourceDictionary mergedDictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (mergedDictionary.Source != null && mergedDictionary.Source.ToString().EndsWith("Fluent.xaml"))
                {
                    MinimizeButton.Visibility = Visibility.Collapsed;
                    MaximizeButton.Visibility = Visibility.Collapsed;
                    CloseButton.Visibility = Visibility.Collapsed;
                    break;
                }
            }
        }
    }

    /*private void SearchBox_KeyUp(object sender, KeyEventArgs e)
    {
        ViewModel.UpdateSearchText(SearchBox.Text);
    }

    private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        ViewModel.UpdateSearchText(SearchBox.Text);
    }*/

    private void MinimizeWindow(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow(object sender, RoutedEventArgs e)
    {
        if(this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            MaximizeIcon.Text = "\uE922";
        }
        else
        {
            this.WindowState = WindowState.Maximized;
            MaximizeIcon.Text = "\uE923";
        }
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnNavigating(object? sender, NavigatingEventArgs e)
    {
        List<NavigationItem> list = ViewModel.GetNavigationItemHierarchyFromPageType(e.PageType);
        
        if (list.Count > 0)
        {
            TreeViewItem selectedTreeViewItem = null;
            ItemsControl itemsControl = ControlsList;
            foreach(NavigationItem item in list)
            {
                var tvi = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if(tvi != null)
                {
                    tvi.IsExpanded = true;
                    tvi.UpdateLayout();
                    itemsControl = tvi;
                    selectedTreeViewItem = tvi;
                }
            }

            if(selectedTreeViewItem != null)
            {
                selectedTreeViewItem.IsSelected = true;
            }
        }
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        NavigationItem selectedItem = SearchComboBox.SelectedItem as NavigationItem;

        if (selectedItem is not null)
        {
            _navigationService.NavigateTo(selectedItem.PageType);
            var tvi = ControlsList.ItemContainerGenerator.ContainerFromItem(selectedItem) as TreeViewItem;
            if (tvi != null)
            {
                tvi.IsExpanded = true;
                tvi.BringIntoView();
            }

            SearchComboBox.ItemsSource = ViewModel.NavigationItemsForSearchBox;
            SearchComboBox.SelectedItem = null;
        }

        SearchComboBox.IsDropDownOpen = false;
        PlaceholderText.Visibility = Visibility.Visible;
        Keyboard.ClearFocus();
    }

    private void SearchComboBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        Task.Delay(300);
        string searchText = (sender as ComboBox).Text;
        SearchComboBox.IsDropDownOpen = true;
        SearchComboBox.ItemsSource = ViewModel.NavigationItemsForSearchBox.Where(item => item.Name.Contains(searchText));
    }

    private void SearchComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
        PlaceholderText.Visibility = Visibility.Hidden;
        SearchComboBox.IsDropDownOpen = true;
        SearchComboBox.ItemsSource = ViewModel.NavigationItemsForSearchBox;
    }

    private void SearchComboBox_LostFocus(object sender, RoutedEventArgs e)
    {
        SearchComboBox.ItemsSource = ViewModel.NavigationItemsForSearchBox;
        SearchComboBox.IsDropDownOpen = false;
        PlaceholderText.Visibility = Visibility.Visible;
    }
}