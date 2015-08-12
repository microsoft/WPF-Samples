// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace ClipboardSpy
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDataObject _dataObject;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs args)
        {
            CheckStatus(null, null);
        }

        private void GetClipboard(object sender, RoutedEventArgs args)
        {
            _dataObject = Clipboard.GetDataObject();
            CheckCurrentDataObject();
        }

        private void CheckStatus(object sender, RoutedEventArgs args)
        {
            cbContainsAudio.IsChecked = Clipboard.ContainsAudio();
            cbContainsFileDropList.IsChecked = Clipboard.ContainsFileDropList();
            cbContainsImage.IsChecked = Clipboard.ContainsImage();
            cbContainsText.IsChecked = Clipboard.ContainsText();
        }

        private void ClearClipboard(object sender, RoutedEventArgs args)
        {
            Clipboard.Clear();
            CheckStatus(null, null);
            GetClipboard(null, null);
        }

        private void CheckCurrentDataObject()
        {
            CheckStatus(null, null);

            tbInfo.Clear();

            if (_dataObject == null)
            {
                tbInfo.Text = "DataObject is null.";
            }
            else
            {
                tbInfo.AppendText("Clipboard DataObject Type: ");
                tbInfo.AppendText(_dataObject.GetType().ToString());

                tbInfo.AppendText("\n\n****************************************************\n\n");

                var formats = _dataObject.GetFormats();

                tbInfo.AppendText(
                    "The following data formats are present in the data object obtained from the clipboard:\n");

                if (formats.Length > 0)
                {
                    foreach (var format in formats)
                    {
                        if (_dataObject.GetDataPresent(format, false))
                            tbInfo.AppendText("\t- " + format + " (native)\n");
                        else tbInfo.AppendText("\t- " + format + " (autoconvertable)\n");
                    }
                }
                else tbInfo.AppendText("\t- no data formats are present\n");

                foreach (var format in formats)
                {
                    tbInfo.AppendText("\n\n****************************************************\n");
                    tbInfo.AppendText(format + " data:\n");
                    tbInfo.AppendText("****************************************************\n\n");
                    tbInfo.AppendText(_dataObject.GetData(format, true).ToString());
                }
            }
        }
    }
}