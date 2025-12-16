// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFGallery.ViewModels;

/// <summary>
/// Interaction logic for FileAndFolderDialogsPage.xaml
/// </summary>
public partial class FileAndFolderDialogsPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "File and Folder Dialogs";

	[ObservableProperty]
	private string _pageDescription = "Use the OpenFileDialog, SaveFileDialog, and OpenFolderDialog to let users select files and folders in a secure way.";

    [ObservableProperty]
    private string _singleFilePath = "No file selected";

    [ObservableProperty]
    private string _multipleFilesPath = "No files selected";

    [ObservableProperty]
    private string _fileContent = "Enter text here to save to a file...";

    [ObservableProperty]
    private string _savedFilePath = "No file saved";

    [ObservableProperty]
    private string _selectedFolderPath = "No folder selected";
}
