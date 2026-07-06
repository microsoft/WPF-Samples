using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

using WPFGallery.ViewModels;

namespace WPFGallery.Views;

/// <summary>
/// Interaction logic for ComboBox.xaml
/// </summary>
public partial class ComboBoxPage : Page
{
public ComboBoxPageViewModel ViewModel { get; }

public ComboBoxPage(ComboBoxPageViewModel viewModel)
{
    ViewModel = viewModel;
    DataContext = this;

    InitializeComponent();

    SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
    this.Loaded += (s, e) => UpdatePageVisuals();
}

private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
{
    Dispatcher.Invoke(() =>
    {
        UpdatePageVisuals();
    });
}

private void UpdatePageVisuals()
{
    // Fluent's HC selection color is WindowColor, washing out editable text selected on focus; use the Highlight color instead.
    // Re-evaluated on theme changes so switching out of high contrast reverts to the theme default.
    if (SystemParameters.HighContrast)
    {
        Resources["TextControlSelectionHighlightColor"] = new SolidColorBrush(SystemColors.HighlightColor);
    }
    else
    {
        Resources.Remove("TextControlSelectionHighlightColor");
    }
}
}
