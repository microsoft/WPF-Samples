using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using WPFGallery.ViewModels;

namespace WPFGallery.Views
{
    /// <summary>
    /// Interaction logic for FileAndFolderDialogsPage.xaml
    /// </summary>
    public partial class FileAndFolderDialogsPage : Page
    {
        public FileAndFolderDialogsPageViewModel ViewModel { get; }

        public FileAndFolderDialogsPage(FileAndFolderDialogsPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void PickSingleFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.SingleFilePath = openFileDialog.FileName;
            }
        }

        private void PickMultipleFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select multiple files",
                Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.MultipleFilesPath = $"Selected {openFileDialog.FileNames.Length} file(s): {string.Join(", ", openFileDialog.FileNames)}";
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save file",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, ViewModel.FileContent);
                    ViewModel.SavedFilePath = $"File saved successfully: {saveFileDialog.FileName}";
                }
                catch (Exception ex)
                {
                    ViewModel.SavedFilePath = $"Error saving file: {ex.Message}";
                }
            }
        }

        private void PickFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new OpenFolderDialog
            {
                Title = "Select a folder"
            };

            if (folderBrowserDialog.ShowDialog() == true)
            {
                ViewModel.SelectedFolderPath = folderBrowserDialog.FolderName;
            }
        }
    }
}
