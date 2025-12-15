// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFGallery.ViewModels;

/// <summary>
/// Interaction logic for MessageBoxPage.xaml
/// </summary>
public partial class MessageBoxPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "MessageBox";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private string _simpleMessageResult = "No message shown yet";

    [ObservableProperty]
    private string _messageWithTitleResult = "No message shown yet";

    [ObservableProperty]
    private string _yesNoCancelResult = "No selection made";

    [ObservableProperty]
    private string _errorResult = "No error shown";

    [ObservableProperty]
    private string _warningResult = "No warning shown";

    [ObservableProperty]
    private string _informationResult = "No information shown";

    [ObservableProperty]
    private string _customDefaultResult = "No selection made";
}
