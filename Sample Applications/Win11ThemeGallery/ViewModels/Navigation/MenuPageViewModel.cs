// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Win11ThemeGallery.ViewModels;

public partial class MenuPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "Menu";

	[ObservableProperty]
	private string _pageDescription = "";


}

