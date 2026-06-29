using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
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

    // Fluent editable ComboBox text is ~4.292:1 under Desert; use full-contrast window text in high contrast.
    if (SystemParameters.HighContrast)
    {
        Resources["ComboBoxForeground"] = SystemColors.WindowTextBrush;
        Resources["ComboBoxForegroundFocused"] = SystemColors.WindowTextBrush;
    }
}

// Place the caret at the end instead of selecting all text, so the value isn't highlighted on focus.
private void EditableComboBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
{
    if (((ComboBox)sender).Template.FindName("PART_EditableTextBox", (ComboBox)sender) is TextBox textBox)
    {
        textBox.Dispatcher.BeginInvoke(() => textBox.Select(textBox.Text.Length, 0),
            System.Windows.Threading.DispatcherPriority.Input);
    }
}
}
