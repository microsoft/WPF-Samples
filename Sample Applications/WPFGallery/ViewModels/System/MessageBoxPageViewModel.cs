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
    private string _defaultMessageResult = "No message shown yet";

    [ObservableProperty]
    private string _customTitleResult = "No message shown yet";

    [ObservableProperty]
    private int _selectedButtonIndex = 0;

    [ObservableProperty]
    private string _differentButtonsResult = "No button clicked yet";

    [ObservableProperty]
    private string _differentButtonsXamlCode = "<Button Content=\"Show message with OK button\" Click=\"ShowMessageButton_Click\" />";

    [ObservableProperty]
    private string _differentButtonsCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK);\n}";

    partial void OnSelectedButtonIndexChanged(int value)
    {
        UpdateButtonCodeSnippets(value);
    }

    private void UpdateButtonCodeSnippets(int index)
    {
        switch (index)
        {
            case 0: // OK
                DifferentButtonsXamlCode = "<Button Content=\"Show message with OK button\" Click=\"ShowMessageButton_Click\" />";
                DifferentButtonsCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK);\n}";
                break;
            case 1: // OK/Cancel
                DifferentButtonsXamlCode = "<Button Content=\"Show message with OK/Cancel buttons\" Click=\"ShowMessageButton_Click\" />";
                DifferentButtonsCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    var result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OKCancel);\n    if (result == MessageBoxResult.OK)\n    {\n        // User clicked OK\n    }\n}";
                break;
            case 2: // Yes/No
                DifferentButtonsXamlCode = "<Button Content=\"Show message with Yes/No buttons\" Click=\"ShowMessageButton_Click\" />";
                DifferentButtonsCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    var result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.YesNo);\n    if (result == MessageBoxResult.Yes)\n    {\n        // User clicked Yes\n    }\n}";
                break;
            case 3: // Yes/No/Cancel
                DifferentButtonsXamlCode = "<Button Content=\"Show message with Yes/No/Cancel buttons\" Click=\"ShowMessageButton_Click\" />";
                DifferentButtonsCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    var result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.YesNoCancel);\n    if (result == MessageBoxResult.Yes)\n    {\n        // User clicked Yes\n    }\n    else if (result == MessageBoxResult.No)\n    {\n        // User clicked No\n    }\n}";
                break;
        }
    }

    [ObservableProperty]
    private int _selectedImageIndex = 0;

    [ObservableProperty]
    private string _differentImagesResult = "No image example shown yet";

    [ObservableProperty]
    private string _differentImagesXamlCode = "<Button Content=\"Show message with no icon\" Click=\"ShowMessageButton_Click\" />";

    [ObservableProperty]
    private string _differentImagesCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK, MessageBoxImage.None);\n}";

    partial void OnSelectedImageIndexChanged(int value)
    {
        UpdateImageCodeSnippets(value);
    }

    private void UpdateImageCodeSnippets(int index)
    {
        switch (index)
        {
            case 0: // None
                DifferentImagesXamlCode = "<Button Content=\"Show message with no icon\" Click=\"ShowMessageButton_Click\" />";
                DifferentImagesCSharpCode = "private void ShowMessageButton_Click(object sender, RoutedEventArgs e)\n{\n    MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK, MessageBoxImage.None);\n}";
                break;
            case 1: // Error
                DifferentImagesXamlCode = "<Button Content=\"Show error message\" Click=\"ShowErrorButton_Click\" />";
                DifferentImagesCSharpCode = "private void ShowErrorButton_Click(object sender, RoutedEventArgs e)\n{\n    // MessageBoxImage.Error (also Hand, Stop)\n    MessageBox.Show(\"An error occurred!\", \"Error\", MessageBoxButton.OK, MessageBoxImage.Error);\n}";
                break;
            case 2: // Question
                DifferentImagesXamlCode = "<Button Content=\"Show question\" Click=\"ShowQuestionButton_Click\" />";
                DifferentImagesCSharpCode = "private void ShowQuestionButton_Click(object sender, RoutedEventArgs e)\n{\n    // MessageBoxImage.Question\n    var result = MessageBox.Show(\"Do you want to continue?\", \"Question\", MessageBoxButton.YesNo, MessageBoxImage.Question);\n}";
                break;
            case 3: // Warning
                DifferentImagesXamlCode = "<Button Content=\"Show warning\" Click=\"ShowWarningButton_Click\" />";
                DifferentImagesCSharpCode = "private void ShowWarningButton_Click(object sender, RoutedEventArgs e)\n{\n    // MessageBoxImage.Warning (also Exclamation)\n    MessageBox.Show(\"Warning: This action may have consequences.\", \"Warning\", MessageBoxButton.OKCancel, MessageBoxImage.Warning);\n}";
                break;
            case 4: // Information
                DifferentImagesXamlCode = "<Button Content=\"Show information\" Click=\"ShowInformationButton_Click\" />";
                DifferentImagesCSharpCode = "private void ShowInformationButton_Click(object sender, RoutedEventArgs e)\n{\n    // MessageBoxImage.Information (also Asterisk)\n    MessageBox.Show(\"Operation completed successfully.\", \"Information\", MessageBoxButton.OK, MessageBoxImage.Information);\n}";
                break;
        }
    }

    [ObservableProperty]
    private string _commonMessagesResult = "No common message shown yet";

    [ObservableProperty]
    private string _customDefaultResult = "No selection made";
}
