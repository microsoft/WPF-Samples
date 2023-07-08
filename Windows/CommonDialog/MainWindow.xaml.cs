using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace CommonDialogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataModel();
        }

        private void Button_OpenFileDialog(object sender, RoutedEventArgs e)
        {
            // Instantiate the open file dialog box.
            // Setting the following properties are optional
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select any text file...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Text files (*.txt)|*.txt"
            };

            // Forces the preview pane to be shown irrespective of the system settings
            //openFileDialog.ForcePreviewPane = true;

            // Allows to select multiple files in the dialog
            openFileDialog.Multiselect = true;

            StringBuilder sb = new StringBuilder();

            // If the user clicks OK button on the dialog, the ShowDialog() method returns true
            // otherwise it returns false.
            if (openFileDialog.ShowDialog() == true)
            {
                var streams = openFileDialog.OpenFiles();
                foreach (var stream in streams)
                {
                    sb.AppendLine("--- START OF FILE --- ");
                    sb.AppendLine();

                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        sb.AppendLine(reader.ReadToEnd());
                    }
                    sb.AppendLine();
                    sb.AppendLine("--- END OF FILE ---");
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("No file selected");
                sb.AppendLine("Either cancel button was clicked or dialog was closed");
            }

            // Setting the data context of this application. Not related to dialog functionality.
            DataModel data = DataModel.GetDialogRelatedInfo(openFileDialog);
            data.ResultBody = sb.ToString();
            DataContext = data;
        }

        private void Button_SaveFileDialog(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Select file to save ...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Text files (*.txt)|*.txt"
            };

            // Setting this flag to true will prompt the user if the file is going to be created.
            saveFileDialog.CreatePrompt = true;

            // We can attach an event handler to the FileOk event of the dialog box.
            saveFileDialog.FileOk += SaveFileDialog_FileOk;

            StringBuilder sb = new StringBuilder();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, "Saving File Successful");

                sb.AppendLine($"Selected File Name : {saveFileDialog.FileName}");
                sb.AppendLine("Saved the file with a short text 'Saving File Successful'");
            }
            else
            {
                sb.AppendLine("No file selected.");
                sb.AppendLine("Either cancel button was clicked or dialog was closed");
            }

            // Setting the data context of this application. Not related to dialog functionality.
            DataModel data = DataModel.GetDialogRelatedInfo(saveFileDialog);
            data.ResultBody = sb.ToString();
            DataContext = data;
        }

        private void SaveFileDialog_FileOk(object? sender, CancelEventArgs e)
        {
            // This can be used to perform addtional validation on the selected file.
            if (sender is SaveFileDialog sfd)
            {
                string selectedFile = sfd.SafeFileName;
                if (selectedFile.Contains(" "))
                {
                    MessageBox.Show("File name cannot contain spaces. Please choose a different filename and try again.");
                    e.Cancel = true;
                }
            }
        }

        private void Button_OpenFolderDialog(object sender, RoutedEventArgs e)
        {
            //OpenFolderDialog openFolderDialog = new OpenFolderDialog()
            //{
            //    Title = "Select folder to open ...",
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            //};

            //openFolderDialog.Multiselect = true;

            //StringBuilder sb = new StringBuilder();
            //if (openFolderDialog.ShowDialog() == true)
            //{
            //    sb.AppendLine($"List of all subdirectories in the selected folders :");
            //    sb.AppendLine();

            //    foreach (var folder in openFolderDialog.FolderNames)
            //    {
            //        sb.AppendLine($"Folder Name : {folder}");

            //        DirectoryInfo d = new DirectoryInfo(folder);
            //        d.GetDirectories().ToList().ForEach(x => sb.AppendLine($"\t{x.Name}"));
            //    }
            //}
            //else
            //{
            //    sb.AppendLine("No folder selected.");
            //    sb.AppendLine("Either cancel button was clicked or dialog was closed");
            //}

            ////Setting the data context of this application.Not related to dialog functionality.
            //DataModel data = DataModel.GetDialogRelatedInfo(openFolderDialog);
            //data.ResultBody = sb.ToString();
            //DataContext = data;
        }
    }
}