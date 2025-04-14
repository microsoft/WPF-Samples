// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFGallery.ViewModels;

public partial class ListBoxPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "ListBox";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private ObservableCollection<string> _listBoxItems;

    public ListBoxPageViewModel()
    {
        _listBoxItems = new ObservableCollection<string>
        {
            "Arial",
            "Comic Sans MS",
            "Courier New",
            "Segoe UI",
            "Times New Roman"
        };
    }
}