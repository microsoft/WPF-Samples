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
    }
}
