using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDialogs
{
    class DataModel : INotifyPropertyChanged
    {
        private string resultTitle = "";
        private string resultBody = "";
        private string dialogDescription = "";
        private string clickOperationDescription = "";



        public string ResultTitle
        {
            get => resultTitle;
            set
            {
                resultTitle = value;
                OnPropertyChanged(nameof(ResultTitle));
            }
        }

        public string ResultBody
        {
            get => resultBody;
            set
            {
                resultBody = value;
                OnPropertyChanged(nameof(ResultBody));
            }
        }

        public string DialogDescription
        {
            get => dialogDescription;
            set
            {
                dialogDescription = value;
                OnPropertyChanged(nameof(DialogDescription));
            }
        }

        public string ClickOperationDescription
        {
            get => clickOperationDescription;
            set
            {
                clickOperationDescription = value;
                OnPropertyChanged(nameof(ClickOperationDescription));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DataModel()
        {
        }

        public DataModel(string resultTitle, string resultBody, string dialogDescription, string clickOperationDescription)
        {
            ResultTitle = resultTitle;
            ResultBody = resultBody;
            DialogDescription = dialogDescription;
            ClickOperationDescription = clickOperationDescription;
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public static DataModel GetDialogRelatedInfo(Microsoft.Win32.CommonDialog cd)
        {
            DataModel model = new DataModel();

            if (cd is OpenFileDialog ofd)
            {
                model.ResultTitle = "Open File Dialog Results";
                model.DialogDescription = "OpenFileDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve the name\\ names of file to open.";
                model.ClickOperationDescription = "In this example, we have created an OpenFileDialog that can be used to select multiple text files.\nWe have also set the ForcePreviewPane property to true, which will display the preview pane in the dialog box.\n\nOnce the user clicks OK, we read and display the contents of the file.";
            }
            else if (cd is SaveFileDialog sfd)
            {
                model.ResultTitle = "Save File Dialog Results";
                model.DialogDescription = "SaveFileDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve the location and filename, where the user wants to save the file.";
                model.ClickOperationDescription = "In this example, we have created an SaveFileDialog and write a small text in the selected file.\n" +
                    "We have set the CreatePrompt and OverwritePrompt property to true, which will display a prompt warning user that the file does not exist and will be created and if you are overwriting an already existing file respectively.\n\n" +
                    "Once the user clicks OK, we write the contents to the selected file.";
            }
            //else if (cd is OpenFolderDialog fbd)
            //{
            //    model.ResultTitle = "Open Folder Dialog Results",
            //    model.DialogDescription = "OpenFolderDialog is a wrapper around the Win32 common dialog boxes.\n\nThis control allows developers to retrieve\\select the path of a folder.",
            //    model.ClickOperationDescription = "In this example, we have created an OpenFolderDialog that can be used to select multiple folders (Multiselect property).\n\n" +
            //      "Once the user clicks OK, we print all the subdirectories in each selected folder."
            //}

            return model;

        }
    }
}
