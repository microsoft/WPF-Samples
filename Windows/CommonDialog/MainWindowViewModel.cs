using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace CommonDialogs
{
    class MainWindowViewModel
    {
        public DialogDataModel Data { get; }

        #region Commands

        public ICommand OpenFileCommand { get;}
        public ICommand SaveFileCommand { get;}
        public ICommand OpenFolderCommand { get;}

        #endregion

        public MainWindowViewModel()
        {
            Data = new DialogDataModel();
            OpenFileCommand = new RelayCommand<object>(OpenFileCommandExecute);
            SaveFileCommand = new RelayCommand<object>(SaveFileCommandExecute);
            OpenFolderCommand = new RelayCommand<object>(OpenFolderCommandExecute);
        }

        private void OpenFileCommandExecute(object obj)
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
            openFileDialog.ForcePreviewPane = true;

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

            Data.ResultBody = sb.ToString();
            GetDialogRelatedInfo(openFileDialog);
        }

        private void SaveFileCommandExecute(object obj)
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

            Data.ResultBody = sb.ToString();
            GetDialogRelatedInfo(saveFileDialog);
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

        private void OpenFolderCommandExecute(object obj)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog()
            {
               Title = "Select folder to open ...",
               InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            };

            openFolderDialog.Multiselect = true;

            StringBuilder sb = new StringBuilder();
            if (openFolderDialog.ShowDialog() == true)
            {
               sb.AppendLine($"List of all subdirectories in the selected folders :");
               sb.AppendLine();

               foreach (var folder in openFolderDialog.FolderNames)
               {
                   sb.AppendLine($"Folder Name : {folder}");
                   foreach (var subfolder in Directory.EnumerateDirectories(folder))
                   {
                       sb.AppendLine($"\t{subfolder}");
                   }
               }
            }
            else
            {
               sb.AppendLine("No folder selected.");
               sb.AppendLine("Either cancel button was clicked or dialog was closed");
            }
            Data.ResultBody = sb.ToString();
            GetDialogRelatedInfo(openFolderDialog);
        }

        public void GetDialogRelatedInfo(Microsoft.Win32.CommonDialog cd)
        {

            if (cd is OpenFileDialog ofd)
            {
                Data.ResultTitle = "Open File Dialog Results";
                Data.DialogDescription = "OpenFileDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve the name\\ names of file to open.";
                Data.ClickOperationDescription = "In this example, we have created an OpenFileDialog that can be used to select multiple text files.\nWe have also set the ForcePreviewPane property to true, which will display the preview pane in the dialog box.\n\nOnce the user clicks OK, we read and display the contents of the file.";
            }
            else if (cd is SaveFileDialog sfd)
            {
                Data.ResultTitle = "Save File Dialog Results";
                Data.DialogDescription = "SaveFileDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve the location and filename, where the user wants to save the file.";
                Data.ClickOperationDescription = "In this example, we have created an SaveFileDialog and write a small text in the selected file.\n" +
                    "We have set the CreatePrompt and OverwritePrompt property to true, which will display a prompt warning user that the file does not exist and will be created and if you are overwriting an already existing file respectively.\n\n" +
                    "Once the user clicks OK, we write the contents to the selected file.";
            }
            else if (cd is OpenFolderDialog fbd)
            {
               Data.ResultTitle = "Open Folder Dialog Results";
               Data.DialogDescription = "OpenFolderDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve\\select the path of a folder.";
               Data.ClickOperationDescription = "In this example, we have created an OpenFolderDialog that can be used to select multiple folders (Multiselect property).\n\n" +
                 "Once the user clicks OK, we print all the subdirectories in each selected folder.";
            }
        }
    }   
}
