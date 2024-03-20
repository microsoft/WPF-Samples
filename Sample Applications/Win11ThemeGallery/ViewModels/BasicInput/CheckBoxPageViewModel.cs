using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Win11ThemeGallery.ViewModels;

public partial class CheckBoxPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "CheckBox";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private bool? _selectAllCheckBoxChecked = null;

    [ObservableProperty]
    private bool _optionOneCheckBoxChecked = false;

    [ObservableProperty]
    private bool _optionTwoCheckBoxChecked = true;

    [ObservableProperty]
    private bool _optionThreeCheckBoxChecked = false;

    [RelayCommand]
    private void OnSelectAllChecked(object sender)
    {
        if (sender is not CheckBox checkBox)
            return;

        if (checkBox.IsChecked == null)
            checkBox.IsChecked = !(
                OptionOneCheckBoxChecked && OptionTwoCheckBoxChecked && OptionThreeCheckBoxChecked
            );

        if (checkBox.IsChecked == true)
        {
            OptionOneCheckBoxChecked = true;
            OptionTwoCheckBoxChecked = true;
            OptionThreeCheckBoxChecked = true;
        }
        else if (checkBox.IsChecked == false)
        {
            OptionOneCheckBoxChecked = false;
            OptionTwoCheckBoxChecked = false;
            OptionThreeCheckBoxChecked = false;
        }
    }

    [RelayCommand]
    private void OnSingleChecked(string option)
    {
        if (OptionOneCheckBoxChecked && OptionTwoCheckBoxChecked && OptionThreeCheckBoxChecked)
            SelectAllCheckBoxChecked = true;
        else if (!OptionOneCheckBoxChecked && !OptionTwoCheckBoxChecked && !OptionThreeCheckBoxChecked)
            SelectAllCheckBoxChecked = false;
        else
            SelectAllCheckBoxChecked = null;
    }
}
