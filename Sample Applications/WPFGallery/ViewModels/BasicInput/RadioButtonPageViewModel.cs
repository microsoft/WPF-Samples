using System.Windows.Controls;

namespace WPFGallery.ViewModels;

public partial class RadioButtonPageViewModel : BasePageViewModel 
{
    public RadioButtonPageViewModel() : base("RadioButton") {}

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