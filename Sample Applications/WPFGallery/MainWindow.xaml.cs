using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Shell;
using Microsoft.Win32;
using WPFGallery.Helpers;
using WPFGallery.Models;
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

        UpdateWindowBackground();
        UpdateMainWindowVisuals();

        _navigationService = navigationService;
        _navigationService.Navigating += OnNavigating;
        _navigationService.SetFrame(this.RootContentFrame);
        _navigationService.Navigate(typeof(DashboardPage));

        WindowChrome.SetWindowChrome(
            this,
            new WindowChrome
            {
                CaptionHeight = 50,
                CornerRadius = new CornerRadius(12),
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = true,
                NonClientFrameEdges = SystemParameters.HighContrast ? NonClientFrameEdges.None :
                    NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left
            }
        );

        SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        this.StateChanged += (s, e) => UpdateMainWindowVisuals();
        this.Activated += (s, e) => UpdateMainWindowVisuals();
        this.Deactivated += (s, e) => UpdateMainWindowVisuals();
    }

    private void UpdateWindowBackground()
    {
        if((!Utility.IsBackdropDisabled() && !Utility.IsBackdropSupported()))
        {
            this.SetResourceReference(BackgroundProperty, "WindowBackground");
        }
    }

    private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            UpdateMainWindowVisuals();
        });
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;

    public MainWindowViewModel ViewModel { get; }

    private void UpdateTitleBarButtonsVisibility()
    {
        if (Utility.IsBackdropDisabled() || !Utility.IsBackdropSupported() ||
                SystemParameters.HighContrast == true)
        {
            MinimizeButton.Visibility = Visibility.Visible;
            MaximizeButton.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Visible;
        }
        else
        {
            MinimizeButton.Visibility = Visibility.Collapsed;
            MaximizeButton.Visibility = Visibility.Collapsed;
            CloseButton.Visibility = Visibility.Collapsed;
        }
    }

    private void UpdateMainWindowVisuals()
    {
        MainGrid.Margin = default;
        if(WindowState == WindowState.Maximized)
        {
            MainGrid.Margin = SystemParameters.HighContrast ? new Thickness(0,8,0,0) : new Thickness(8);
        }

        UpdateTitleBarButtonsVisibility();
    
        if(SystemParameters.HighContrast == true)
        {
            HighContrastBorder.SetResourceReference(BorderBrushProperty, IsActive ? SystemColors.ActiveCaptionBrushKey : 
                                                                                    SystemColors.InactiveCaptionBrushKey);
            HighContrastBorder.BorderThickness = new Thickness(8, 1, 8, 8);
            
            WindowChrome wc = WindowChrome.GetWindowChrome(this);
            if(wc is not null)
            {
                wc.NonClientFrameEdges = NonClientFrameEdges.None;
            }
        }
        else
        {
            HighContrastBorder.BorderBrush = Brushes.Transparent;
            HighContrastBorder.BorderThickness = new Thickness(0);

            var wc = WindowChrome.GetWindowChrome(this);
            if(wc is not null)
            {
                wc.NonClientFrameEdges = NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left;
            }
        }
    }

    //private void SearchBox_KeyUp(object sender, KeyEventArgs e)
    //{
    //    ViewModel.UpdateSearchText(SearchBox.Text);
    //}

    //private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
    //{
    //    SearchBox.Text = "";
    //    ViewModel.UpdateSearchText(SearchBox.Text);
    //}

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
        List<ControlInfoDataItem> list = ViewModel.GetNavigationItemHierarchyFromPageType(e.PageType);
        
        if (list.Count > 0)
        {
            TreeViewItem selectedTreeViewItem = null;
            ItemsControl itemsControl = ControlsList;
            foreach(ControlInfoDataItem item in list)
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
                ControlsList_SelectedItemChanged();
            }
        }
    }

    private void RootContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        ViewModel.UpdateCanNavigateBack();
    }

    private void ControlsList_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) 
        {
            SelectedItemChanged(ControlsList.ItemContainerGenerator.ContainerFromItem((sender as TreeView).SelectedItem) as TreeViewItem);
        }
    }

    private void ControlsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is ToggleButton)
        {
            return;
        }
        SelectedItemChanged(ControlsList.ItemContainerGenerator.ContainerFromItem((sender as TreeView).SelectedItem) as TreeViewItem);
    }

    private void ControlsList_Loaded(object sender, RoutedEventArgs e)
    {
        if (ControlsList.Items.Count > 0)
        {
            TreeViewItem firstItem = (TreeViewItem)ControlsList.ItemContainerGenerator.ContainerFromItem(ControlsList.Items[0]);
            if (firstItem != null)
            {
                firstItem.IsSelected = true;
            }
        }
    }

    private void SelectedItemChanged(TreeViewItem? tvi)
    {
        ControlsList_SelectedItemChanged();
        if (tvi != null)
        {
            tvi.IsExpanded = !tvi.IsExpanded;
        }
    }

    private void ControlsList_SelectedItemChanged()
    {
        if (ControlsList.SelectedItem is ControlInfoDataItem navItem)
        {
            _navigationService.Navigate(navItem.PageType);
            var tvi = ControlsList.ItemContainerGenerator.ContainerFromItem(navItem) as TreeViewItem;
            if(tvi != null)
            {
                tvi.BringIntoView();
            }
        }
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement((Button)sender);
        peer.RaiseNotificationEvent(
           AutomationNotificationKind.Other,
            AutomationNotificationProcessing.ImportantMostRecent,
            "Settings Page Opened",
            "ButtonClickedActivity"
        );
    }
}