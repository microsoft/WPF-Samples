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
    private string _differentButtonsXamlCode = "<Button Content=\"Show MessageBox\" Click=\"ShowMessageBoxButton_Click\" />";

    [ObservableProperty]
    private string _differentButtonsCSharpCode = string.Format(_differentButtonsMessageBoxSampleCSharpCodeString, "\tMessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK);");

    partial void OnSelectedButtonIndexChanged(int value)
    {
        UpdateButtonCodeSnippets(value);
    }

    private void UpdateButtonCodeSnippets(int index)
    {
        string content = (MessageBoxButton)index switch
        {
            MessageBoxButton.OK => "\tMessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK);",
            MessageBoxButton.OKCancel => "\tvar result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OKCancel);\n\tif (result == MessageBoxResult.OK)\n\t{\n\t    // User clicked OK\n\t}",
            MessageBoxButton.AbortRetryIgnore => "\tvar result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.AbortRetryIgnore);\n\tif (result == MessageBoxResult.Abort)\n\t{\n\t    // User clicked Abort\n\t}\n\telse if (result == MessageBoxResult.Retry)\n\t{\n\t    // User clicked Retry\n\t}\n\telse if (result == MessageBoxResult.Ignore)\n\t{\n\t    // User clicked Ignore\n\t}",
            MessageBoxButton.YesNoCancel => "\tvar result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.YesNoCancel);\n\tif (result == MessageBoxResult.Yes)\n\t{\n\t    // User clicked Yes\n\t}\n\telse if (result == MessageBoxResult.No)\n\t{\n\t    // User clicked No\n\t}",
            MessageBoxButton.YesNo => "\tvar result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.YesNo);\n\tif (result == MessageBoxResult.Yes)\n\t{\n\t    // User clicked Yes\n\t}\n\telse if (result == MessageBoxResult.No)\n\t{\n\t    // User clicked No\n\t}",
            MessageBoxButton.RetryCancel => "\tvar result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.RetryCancel);\n\tif (result == MessageBoxResult.Retry)\n\t{\n\t    // User clicked Retry\n\t}",
            MessageBoxButton.CancelTryContinue => "\tvar result = MessageBox.Show(\"Message\", \"Title\", MessageBoxButton.CancelTryContinue);\n\tif (result == MessageBoxResult.TryAgain)\n\t{\n\t    // User clicked Try Again\n\t}\n\telse if (result == MessageBoxResult.Continue)\n\t{\n\t    // User clicked Continue\n\t}",
            _ => "\tMessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK);"
        };

        DifferentButtonsCSharpCode = string.Format(_differentButtonsMessageBoxSampleCSharpCodeString, content);
    }

    [ObservableProperty]
    private int _selectedImageIndex = 0;

    [ObservableProperty]
    private string _differentImagesResult = "No image example shown yet";

    [ObservableProperty]
    private string _differentImagesXamlCode = "<Button Content=\"Show MessageBox\" Click=\"ShowMessageButton_Click\" />";

    [ObservableProperty]
    private string _differentImagesCSharpCode = string.Format(_differentImagesMessageBoxSampleCSharpCodeString, "\tMessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK, MessageBoxImage.None);");

    partial void OnSelectedImageIndexChanged(int value)
    {
        UpdateImageCodeSnippets(value);
    }

    private void UpdateImageCodeSnippets(int index)
    {
        string content = index switch
        {
            0 => "\tMessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK, MessageBoxImage.None);",
            1 => "\t// MessageBoxImage.Error (also Hand, Stop)\n\tMessageBox.Show(\"An error occurred!\", \"Error\", MessageBoxButton.OK, MessageBoxImage.Error);",
            2 => "\t// MessageBoxImage.Question\n\tvar result = MessageBox.Show(\"Do you want to continue?\", \"Question\", MessageBoxButton.YesNo, MessageBoxImage.Question);",
            3 => "\t// MessageBoxImage.Warning (also Exclamation)\n\tMessageBox.Show(\"Warning: This action may have consequences.\", \"Warning\", MessageBoxButton.OKCancel, MessageBoxImage.Warning);",
            4 => "\t// MessageBoxImage.Information (also Asterisk)\n\tMessageBox.Show(\"Operation completed successfully.\", \"Information\", MessageBoxButton.OK, MessageBoxImage.Information);",
            _ => "\tMessageBox.Show(\"Message\", \"Title\", MessageBoxButton.OK, MessageBoxImage.None);"
        };

        DifferentImagesCSharpCode = string.Format(_differentImagesMessageBoxSampleCSharpCodeString, content);
        
    }

    [ObservableProperty]
    private string _commonMessagesResult = "No common message shown yet";

    [ObservableProperty]
    private string _commonMessagesXamlCode = @"<WrapPanel Margin=""0,0,0,10"">
    <Button Content=""Information"" Click=""ShowInformationButton_Click"" />
    <Button Content=""Error"" Click=""ShowErrorButton_Click"" />
    <Button Content=""Warning"" Click=""ShowWarningButton_Click"" />
</WrapPanel>";

    [ObservableProperty]
    private string _commonMessagesCSharpCode = @"// Information
private void ShowInformationButton_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show(""Operation completed successfully."", ""Information"", MessageBoxButton.OK, MessageBoxImage.Information);
}

// Error
private void ShowErrorButton_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show(""An error occurred!"", ""Error"", MessageBoxButton.OK, MessageBoxImage.Error);
}

// Warning
private void ShowWarningButton_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show(""This action cannot be undone!"", ""Warning"", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
}";

    [ObservableProperty]
    private string _customDefaultResult = "No selection made";

    private const string _differentButtonsMessageBoxSampleCSharpCodeString = "private void ShowMessageBoxButton_Click(object sender, RoutedEventArgs e)\n{{\n{0}\n}}";                
    private const string _differentImagesMessageBoxSampleCSharpCodeString = "private void ShowMessageBoxButton_Click(object sender, RoutedEventArgs e)\n{{\n{0}\n}}";
}
