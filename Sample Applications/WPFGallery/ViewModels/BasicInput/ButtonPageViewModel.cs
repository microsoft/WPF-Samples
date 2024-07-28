using System.Windows.Controls;

namespace WPFGallery.ViewModels;

/// <summary>
/// Interaction logic for Button.xaml
/// </summary>
public partial class ButtonPageViewModel : BasePageViewModel 
{
    public ButtonPageViewModel() : base("Button") {}

    [ObservableProperty]
    private string _message = "Hello World!";

    [ObservableProperty]
    private bool _isSimpleButtonEnabled = true;

    [ObservableProperty]
    private bool _isUiButtonEnabled = true;

    [RelayCommand]
    private void OnSimpleButtonCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsSimpleButtonEnabled = !(checkbox?.IsChecked ?? false);
    }

    [RelayCommand]
    private void OnUiButtonCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsUiButtonEnabled = !(checkbox?.IsChecked ?? false);
    }
}