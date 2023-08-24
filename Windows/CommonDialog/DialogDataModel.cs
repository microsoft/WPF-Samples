using System.ComponentModel;

namespace CommonDialogs
{
    class DialogDataModel : INotifyPropertyChanged
    {
        # region Data Fields and Properties

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

        #endregion

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
