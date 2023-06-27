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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
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
                InitialDirectory = @"C:\Windows",
                Filter = "Text files (*.txt)|*.txt"
            };

            // Forces the preview pane to be shown irrespective of the system settings
            //openFileDialog.ForcePreviewPane = true;

            // Allows to select multiple files in the dialog
            openFileDialog.Multiselect = true;


            string text = "";

            // If the user clicks OK button on the dialog, the ShowDialog() method returns true
            // otherwise it returns false.
            if (openFileDialog.ShowDialog() == true)
            {
                var streams = openFileDialog.OpenFiles();
                foreach (var stream in streams)
                {
                    using (stream)
                    {
                        using (var reader = new System.IO.StreamReader(stream))
                        {
                            text += reader.ReadToEnd();
                        }
                        text += "\n\n--- END OF FILE ---\n";
                    }
                }
            }
            else
            {
                text = "No file selected. \nEither cancel button was clicked or dialog was closed";
            }

            // Setting the data context of this application. Not related to dialog functionality.
            DataModel data = DataModel.GetDialogRelatedInfo(openFileDialog);
            data.ResultBody = text;
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

            // Setting this flag to true will prompt the user if the file is going to be overwritten.
            saveFileDialog.OverwritePrompt = true;

            // Setting this flag to true will restore the Application.CurrentDirectory.
            //saveFileDialog.RestoreDirectory = true;

            // We can attach an event handler to the FileOk event of the dialog box.
            saveFileDialog.FileOk += SaveFileDialog_FileOk;

            string text = "";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, "Saving File Successful");

                text = $" Selected File Name : {saveFileDialog.FileName}\nSaved the file with a small text \" Saving File Successful \"";
            }
            else
            {
                text = "No file selected. \nEither cancel button was clicked or dialog was closed";
            }

            // Setting the data context of this application. Not related to dialog functionality.
            DataModel data = DataModel.GetDialogRelatedInfo(saveFileDialog);
            data.ResultBody = text;
            DataContext = data;
        }

        private void SaveFileDialog_FileOk(object? sender, CancelEventArgs e)
        {
            // This can be used to perform addtional validation on the selected file.
            if(sender is SaveFileDialog sfd)
            {
                string selectedFile = sfd.FileName;
                if (selectedFile.Contains(" "))
                {
                    MessageBox.Show("File name cannot contain spaces. Please rename the file and try again.");
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

            //string text = "";
            //if (openFolderDialog.ShowDialog() == true)
            //{
            //    text = "List of all subdirectories in the selected folders : \n\n";
            //    foreach (var folder in openFolderDialog.FolderNames)
            //    {
            //        DirectoryInfo d = new DirectoryInfo(folder);
            //        text += folder + "\n---\n";

            //        d.GetDirectories().ToList().ForEach(x => text += x.FullName + "\n");
            //    }
            //}
            //else
            //{
            //    text = "No folder selected. \n Either cancel button was clicked or dialog was closed";
            //}

            // Setting the data context of this application. Not related to dialog functionality.
            //DataModel data = DataModel.GetDialogRelatedInfo(openFolderDialog);
            //data.ResultBody = text;
            //DataContext = data;
        }


    }
}