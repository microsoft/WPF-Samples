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

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select any text file...",
                InitialDirectory = @"C:\Window",
                Filter = "Text files (*.txt)|*.txt"
            };

            //openFileDialog.ForcePreviewPane = true;
            openFileDialog.Multiselect = true;

            var dialogInfos = GetDialogRelatedInfo(openFileDialog);

            var data = (DataModel)DataContext;

            data.ResultBody = string.Empty;
            data.ResultTitle = dialogInfos[0];
            data.DialogDescription = dialogInfos[1];
            data.ClickOperationDescription = dialogInfos[2];


            if (openFileDialog.ShowDialog() == true)
            {
                var streams = openFileDialog.OpenFiles();
                foreach (var stream in streams)
                {
                    using (stream)
                    {
                        using (var reader = new System.IO.StreamReader(stream))
                        {
                            data.ResultBody += reader.ReadToEnd();
                        }
                        data.ResultBody += "\n\n--- END OF FILE ---\n";
                    }
                }
            }
            else
            {
                data.ResultBody = "No file selected. \n Either cancel button was clicked or dialog was closed";
            }

        }

        private void Button_SaveFileDialog(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = "Select file to save ...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Text files (*.txt)|*.txt"
            };

            saveFileDialog.CreatePrompt = true;
            saveFileDialog.OverwritePrompt = true;
            //saveFileDialog.RestoreDirectory = true;

            var dialogInfos = GetDialogRelatedInfo(saveFileDialog);

            var data = (DataModel)DataContext;

            data.ResultBody = string.Empty;
            data.ResultTitle = dialogInfos[0];
            data.DialogDescription = dialogInfos[1];
            data.ClickOperationDescription = dialogInfos[2];

            //Debug.WriteLine(Directory.GetCurrentDirectory());

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, "Saving File Successful");
                //Debug.WriteLine(Directory.GetCurrentDirectory());

                data.ResultBody = $" Selected File Name : {saveFileDialog.FileName}\nSaved the file with a small text \" Saving File Successful \"";
            }
            else
            {
                data.ResultBody = "No file selected. \n Either cancel button was clicked or dialog was closed";
            }
            //Debug.WriteLine(Directory.GetCurrentDirectory());
        }

        private void Button_OpenFolderDialog(object sender, RoutedEventArgs e)
        {
            //OpenFolderDialog openFolderDialog = new OpenFolderDialog()
            //{
            //    Title = "Select folder to open ...",
            //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            //};

            //openFolderDialog.Multiselect = true;

            //var dialogInfos = GetDialogRelatedInfo(openFolderDialog);

            //var data = (DataModel)DataContext;

            //data.ResultBody = string.Empty;
            //data.ResultTitle = dialogInfos[0];
            //data.DialogDescription = dialogInfos[1];
            //data.ClickOperationDescription = dialogInfos[2];

            //if (openFolderDialog.ShowDialog() == true)
            //{
            //    data.ResultBody = "List of all subdirectories in the selected folders : \n\n";
            //    foreach (var folder in openFolderDialog.FolderNames)
            //    {
            //        DirectoryInfo d = new DirectoryInfo(folder);
            //        data.ResultBody += folder + "\n---\n";

            //        d.GetDirectories().ToList().ForEach(x => data.ResultBody += x.FullName + "\n");
            //    }
            //}
            //else
            //{
            //    data.ResultBody = "No folder selected. \n Either cancel button was clicked or dialog was closed";
            //}
        }

        private List<string> GetDialogRelatedInfo(Microsoft.Win32.CommonDialog cd)
        {
            List<string> info;

            if (cd is OpenFileDialog ofd)
            {
                info = new List<string>()
                {
                    "Open File Dialog Results",
                    "OpenFileDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve the name\\names of file to open.",
                    "In this example, we have created an OpenFileDialog that can be used to select multiple text files.\nWe have also set the ForcePreviewPane property to true, which will display the preview pane in the dialog box.\n\nOnce the user clicks OK, we read and display the contents of the file."
                };
            }
            else if (cd is SaveFileDialog sfd)
            {
                info = new List<string>()
                {
                    "Save File Dialog Results",
                    "SaveFileDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve the location and filename, where the user wants to save the file.",
                    "In this example, we have created an SaveFileDialog and write a small text in the selected file.\n" +
                    "We have set the CreatePrompt and OverwritePrompt property to true, which will display a prompt warning user that the file does not exist and will be created and if you are overwriting an already existing file respectively.\n\n" +
                    "Once the user clicks OK, we write the contents to the selected file."
                };
            }
            //else if (cd is OpenFolderDialog fbd)
            //{
            //    info = new List<string>()
            //    {
            //        "Open Folder Dialog Results",
            //        "OpenFolderDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve\\select the path of a folder.",
            //        "In this example, we have created an OpenFolderDialog that can be used to select multiple folders (Multiselect property).\n\n" +
            //        "Once the user clicks OK, we print all the subdirectories in each selected folder."
            //    };
            //}
            else
            {
                info = new List<string>(3);
            }
            return info;

        }

        // this is for the purpose of cleaning up the saved files after the application is closed
        private List<string> savedFiles = new List<string>();

    }
}