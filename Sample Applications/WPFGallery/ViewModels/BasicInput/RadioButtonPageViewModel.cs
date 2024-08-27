
namespace WPFGallery.ViewModels;

public partial class RadioButtonPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "RadioButton";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private bool _isRadioButtonEnabled = true;

    [RelayCommand]
    private void OnRadioButtonCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsRadioButtonEnabled = !(checkbox?.IsChecked ?? false);
    }
}