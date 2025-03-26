// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

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