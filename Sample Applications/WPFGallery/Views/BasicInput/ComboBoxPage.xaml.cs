using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

    // Fluent's HC selection color is WindowColor, washing out editable text selected on focus; use the Highlight color instead.
    if (SystemParameters.HighContrast)
    {
        Resources["TextControlSelectionHighlightColor"] = new SolidColorBrush(SystemColors.HighlightColor);
    }
}
}
