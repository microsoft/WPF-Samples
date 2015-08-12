// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;

namespace ClipboardViewer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // This is called when UI is laid out, rendered and ready for interaction.
        private void WindowLoaded(object sender, RoutedEventArgs args)
        {
            // Check the current Clipboard data format status
            RefreshClipboardDataFormat();

            // Set the text copy information on RichTextBox
            SetTextOnRichTextBox("Please type clipboard copy data here!");
        }

        private void ClearClipboard(object sender, RoutedEventArgs args)
        {
            // Clear Clipboard
            Clipboard.Clear();

            // Update the current Clipboard data format status after clear Clipboard
            RefreshClipboardDataFormat();
        }

        private void RefreshClipboardDataFormatStatus(object sender, RoutedEventArgs args)
        {
            // Refresh the all clipboard data format into the information panel
            RefreshClipboardDataFormat();
        }

        private void DumpAllClipboardContents(object sender, RoutedEventArgs args)
        {
            // Dump all Clipboard contents on the Clipboard information panel
            DumpAllClipboardContentsInternal();
        }

        private void CopyToClipboard(object sender, RoutedEventArgs args)
        {
            var dataObject = new DataObject();

            // Copy data from RichTextBox/File content into DataObject
            if ((bool) rbCopyDataFromRichTextBox.IsChecked)
            {
                CopyDataFromRichTextBox(dataObject);
            }
            else
            {
                CopyDataFromFile(dataObject);
            }

            // Copy DataObject on the system Clipboard
            if (dataObject.GetFormats().Length > 0)
            {
                if ((bool) cbFlushOnCopy.IsChecked)
                {
                    // Copy data to the system clipboard with flush
                    Clipboard.SetDataObject(dataObject, true /*copy*/);
                }
                else
                {
                    // Copy data to the system clipboard without flush
                    Clipboard.SetDataObject(dataObject, false /*copy*/);
                }
            }

            // Dump the copied data contents on the information panel
            DumpAllClipboardContentsInternal();
        }

        private void PasteFromClipboard(object sender, RoutedEventArgs args)
        {
            var dataObject = GetDataObjectFromClipboard();

            if (dataObject == null)
            {
                MessageBox.Show("Couldn't get DataObject from system clipboard!\n", "Clipboard Error");
                return;
            }

            var autoConvert = (bool) cbAutoConvertibleData.IsChecked;
            var pasteDataFormat = lbPasteDataFormat.SelectedItems[0] as string;

            if ((bool) rbPasteDataToRichTextBox.IsChecked)
            {
                // Paste data from Clipboard to RichTextBox
                PasteClipboardDataToRichTextBox(pasteDataFormat, dataObject, autoConvert);
            }
            else
            {
                // Paste data from Clipboard to File
                PasteClipboardDataToFile(pasteDataFormat, dataObject, autoConvert);
            }
        }

        private void ClearRichTextBox(object sender, RoutedEventArgs args)
        {
            SetTextOnRichTextBox("");
        }

        // Update the data format status and describe all available data formats
        private void RefreshClipboardDataFormat()
        {
            clipboardInfo.Clear();

            lbPasteDataFormat.Items.Clear();

            var dataObject = GetDataObjectFromClipboard();

            if (dataObject == null)
            {
                clipboardInfo.Text = "Can't access clipboard now! Please click Refresh button again.";
            }
            else
            {
                // Check the data format whether it is on Clipboard or not
                cbAudio.IsChecked = dataObject.GetDataPresent(DataFormats.WaveAudio);
                cbFileDropList.IsChecked = dataObject.GetDataPresent(DataFormats.FileDrop);
                cbImage.IsChecked = dataObject.GetDataPresent(DataFormats.Bitmap);
                cbText.IsChecked = dataObject.GetDataPresent(DataFormats.Text);
                cbRtf.IsChecked = dataObject.GetDataPresent(DataFormats.Rtf);
                cbXaml.IsChecked = dataObject.GetDataPresent(DataFormats.Xaml);

                // Update the data format into the information panel
                UpdateAvailableDataFormats(dataObject);
            }
        }

        // Update and describe all available data formats
        private void UpdateAvailableDataFormats(IDataObject dataObject)
        {
            clipboardInfo.AppendText("Clipboard DataObject Type: ");
            clipboardInfo.AppendText(dataObject.GetType().ToString());

            clipboardInfo.AppendText("\n\n****************************************************\n\n");

            var formats = dataObject.GetFormats();

            clipboardInfo.AppendText(
                "The following data formats are present in the data object obtained from the clipboard:\n");

            if (formats.Length > 0)
            {
                foreach (var format in formats)
                {
                    bool nativeData;
                    if (dataObject.GetDataPresent(format, false))
                    {
                        nativeData = true;
                        clipboardInfo.AppendText("\t- " + format + " (native)\n");
                    }
                    else
                    {
                        nativeData = false;
                        clipboardInfo.AppendText("\t- " + format + " (autoconvertable)\n");
                    }

                    if (nativeData)
                    {
                        lbPasteDataFormat.Items.Add(format);
                    }
                    else if ((bool) cbAutoConvertibleData.IsChecked)
                    {
                        lbPasteDataFormat.Items.Add(format);
                    }
                }

                lbPasteDataFormat.SelectedIndex = 0;
            }
            else
            {
                clipboardInfo.AppendText("\t- no data formats are present\n");
            }
        }

        // Dump all available data contents
        private void DumpAllClipboardContentsInternal()
        {
            clipboardInfo.Clear();

            lbPasteDataFormat.Items.Clear();

            var dataObject = GetDataObjectFromClipboard();

            if (dataObject == null)
            {
                clipboardInfo.Text =
                    clipboardInfo.Text =
                        "Can't access clipboard now! \n\nPlease click Dump All Clipboard Contents button again.";
            }
            else
            {
                // Update the availabe data formats 
                UpdateAvailableDataFormats(dataObject);

                // Update the all available data contents
                var formats = dataObject.GetFormats();
                foreach (var format in formats)
                {
                    clipboardInfo.AppendText("\n\n****************************************************\n");
                    clipboardInfo.AppendText(format + " data:\n");
                    clipboardInfo.AppendText("****************************************************\n\n");

                    var data = GetDataFromDataObject(dataObject, format, true /*autoConvert*/);

                    clipboardInfo.AppendText(data?.ToString() ?? "null");
                }
            }
        }

        // Set the selected data format's data from RichTextBox's content 
        // into DataObject for copying data on the system clipboard
        private void CopyDataFromRichTextBox(IDataObject dataObject)
        {
            if ((bool) cbCopyTextDataFormat.IsChecked)
            {
                var textData = GetTextStringFromRichTextBox(DataFormats.Text);
                if (textData != string.Empty)
                {
                    dataObject.SetData(DataFormats.Text, textData);
                }
            }

            if ((bool) cbCopyXamlDataFormat.IsChecked)
            {
                var textData = GetTextStringFromRichTextBox(DataFormats.Xaml);
                if (textData != string.Empty)
                {
                    dataObject.SetData(DataFormats.Xaml, textData);
                }
            }

            if ((bool) cbCopyRtfDataFormat.IsChecked)
            {
                var textData = GetTextStringFromRichTextBox(DataFormats.Rtf);
                if (textData != string.Empty)
                {
                    dataObject.SetData(DataFormats.Rtf, textData);
                }
            }

            // Finally, consider a custom, application defined format.
            // We use an arbitrary encoding here, for demonstration purposes.
            if ((bool) cbCustomSampleDataFormat.IsChecked)
            {
                Stream customStream = new MemoryStream();

                var textData = "This is Custom Sample Data Start\n\n" +
                               GetTextStringFromRichTextBox(DataFormats.Text) +
                               "\nCustom Sample Data End.";

                var bytesUnicode = Encoding.Unicode.GetBytes(textData);
                var bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytesUnicode);

                if (bytes.Length > 0)
                {
                    customStream.Write(bytes, 0, bytes.Length);
                    dataObject.SetData("CustomSample", customStream);
                }
            }
        }

        // Set the selected data format's data from the file content 
        // into DataObject for copying data on the system clipboard
        private void CopyDataFromFile(IDataObject dataObject)
        {
            string fileName = null;

            var dialog = new OpenFileDialog {CheckFileExists = true};

            if ((bool) cbCopyTextDataFormat.IsChecked)
            {
                dialog.Filter = "Plain Text (*.txt)|*.txt";
                dialog.ShowDialog();
                fileName = dialog.FileName;

                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                var fileEncoding = Encoding.Default;
                var textData = GetTextStringFromFile(fileName, fileEncoding);
                if (!string.IsNullOrEmpty(textData))
                {
                    dataObject.SetData(DataFormats.Text, textData);
                }
            }

            if ((bool) cbCopyRtfDataFormat.IsChecked)
            {
                fileName = null;
                dialog.Filter = "RTF Documents (*.rtf)|*.rtf";
                dialog.ShowDialog();
                fileName = dialog.FileName;

                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                var fileEncoding = Encoding.ASCII;
                var textData = GetTextStringFromFile(fileName, fileEncoding);
                if (!string.IsNullOrEmpty(textData))
                {
                    dataObject.SetData(DataFormats.Rtf, textData);
                }
            }

            if ((bool) cbCopyXamlDataFormat.IsChecked)
            {
                fileName = null;
                dialog.Filter = "XAML Flow Documents (*.xaml)|*.xaml";
                dialog.ShowDialog();
                fileName = dialog.FileName;

                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                var fileEncoding = Encoding.UTF8;
                var textData = GetTextStringFromFile(fileName, fileEncoding);
                if (!string.IsNullOrEmpty(textData))
                {
                    dataObject.SetData(DataFormats.Xaml, textData);
                }
            }

            // Finally, consider a custom, application defined format.
            // We use an arbitrary encoding here, for demonstartion purposes.
            if ((bool) cbCustomSampleDataFormat.IsChecked)
            {
                fileName = null;
                dialog.Filter = "All Files (*.*)|*.*";
                dialog.ShowDialog();
                fileName = dialog.FileName;

                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }

                var fileEncoding = Encoding.UTF8;
                var textData = GetTextStringFromFile(fileName, fileEncoding);
                if (string.IsNullOrEmpty(textData)) return;
                var bytesUnicode = Encoding.Unicode.GetBytes(textData);
                var bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytesUnicode);

                if (bytes.Length > 0)
                {
                    var customStream = new MemoryStream();
                    customStream.Write(bytes, 0, bytes.Length);
                    dataObject.SetData("CustomSample", customStream);
                }
            }
        }

        // Paste a selected paste data format's data to RichTextBox
        private void PasteClipboardDataToRichTextBox(string dataFormat, IDataObject dataObject, bool autoConvert)
        {
            if (dataObject != null && dataObject.GetFormats().Length > 0)
            {
                var pasted = false;

                if (dataFormat == DataFormats.Xaml)
                {
                    var xamlData = dataObject.GetData(DataFormats.Xaml) as string;
                    if (!string.IsNullOrEmpty(xamlData))
                    {
                        pasted = PasteTextDataToRichTextBox(DataFormats.Xaml, xamlData);
                    }
                }
                else if (dataFormat == DataFormats.Rtf)
                {
                    var rtfData = dataObject.GetData(DataFormats.Rtf) as string;
                    if (!string.IsNullOrEmpty(rtfData))
                    {
                        pasted = PasteTextDataToRichTextBox(DataFormats.Rtf, rtfData);
                    }
                }
                else if (dataFormat == DataFormats.UnicodeText
                         || dataFormat == DataFormats.Text
                         || dataFormat == DataFormats.StringFormat)
                {
                    var textData = dataObject.GetData(dataFormat) as string;
                    if (textData != string.Empty)
                    {
                        SetTextOnRichTextBox(textData);
                        pasted = true;
                    }
                }
                else if (dataFormat == "CustomSample")
                {
                    // Paste the application defined custom data format's data to RichTextBox content
                    var customStream = dataObject.GetData(dataFormat, autoConvert) as Stream;
                    if (customStream.Length > 0)
                    {
                        var textRange = new TextRange(richTextBox.Document.ContentStart,
                            richTextBox.Document.ContentEnd);
                        textRange.Load(customStream, DataFormats.Text);
                        pasted = true;
                    }
                }

                if (!pasted)
                {
                    MessageBox.Show(
                        "Can't paste the selected data format into RichTextBox!\n\nPlease click Refresh button to update the current clipboard format Or select File RadioButton to paste data.",
                        "Paste Data Format Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }

        // Paste a selected paste data format's data to the fileSSS
        private void PasteClipboardDataToFile(string dataFormat, IDataObject dataObject, bool autoConvert)
        {
            var dialog = new SaveFileDialog {CheckFileExists = false};

            if (dataFormat == DataFormats.Text
                || dataFormat == DataFormats.UnicodeText
                || dataFormat == DataFormats.StringFormat)
            {
                dialog.Filter = "Plain Text (*.txt)|*.txt | All Files (*.*)|*.*";
            }
            else if (dataFormat == DataFormats.Xaml)
            {
                dialog.Filter = "XAML Flow Documents (*.xaml)|*.xaml | All Files (*.*)|*.*";
            }
            else if (dataFormat == DataFormats.Rtf)
            {
                dialog.Filter = "RTF Documents (*.rtf)|*.rtf | All Files (*.*)|*.*";
            }
            else
            {
                dialog.Filter = "All Files (*.*)|*.*";
            }

            if (!(bool) dialog.ShowDialog())
            {
                return;
            }

            var fileName = dialog.FileName;

            if (dataFormat == DataFormats.Xaml)
            {
                var xamlData = dataObject.GetData(DataFormats.Xaml) as string;
                if (!string.IsNullOrEmpty(xamlData))
                {
                    PasteTextDataToFile(dataFormat, xamlData, fileName, Encoding.UTF8);
                }
            }
            else if (dataFormat == DataFormats.Rtf)
            {
                var rtfData = dataObject.GetData(DataFormats.Rtf) as string;
                if (!string.IsNullOrEmpty(rtfData))
                {
                    PasteTextDataToFile(dataFormat, rtfData, fileName, Encoding.ASCII);
                }
            }
            else if (dataFormat == DataFormats.UnicodeText
                     || dataFormat == DataFormats.Text
                     || dataFormat == DataFormats.StringFormat)
            {
                var textData = dataObject.GetData(dataFormat, autoConvert) as string;
                if (!string.IsNullOrEmpty(textData))
                {
                    PasteTextDataToFile(dataFormat, textData, fileName,
                        dataFormat == DataFormats.Text ? Encoding.Default : Encoding.Unicode);
                }
            }
            else
            {
                // Paste the CustomSample data or others to the file
                //Stream customStream = dataObject.GetData(dataFormat, autoConvert) as Stream;
                var customStream = GetDataFromDataObject(dataObject, dataFormat, autoConvert) as Stream;
                if (customStream != null && customStream.Length > 0)
                {
                    PasteStreamDataToFile(customStream, fileName);
                }
            }
        }

        // Get DataObject from the system clipboard
        private IDataObject GetDataObjectFromClipboard()
        {
            IDataObject dataObject;

            try
            {
                dataObject = Clipboard.GetDataObject();
            }
            catch (COMException)
            {
                // Clipboard.GetDataObject can be failed by opening the system clipboard 
                // from other or processing clipboard operation like as setting data on clipboard
                dataObject = null;
            }

            return dataObject;
        }

        private object GetDataFromDataObject(IDataObject dataObject, string dataFormat, bool autoConvert)
        {
            object data = null;

            try
            {
                data = dataObject.GetData(dataFormat, autoConvert);
            }
            catch (COMException)
            {
                // Fail to get the data by the invalid value like tymed(DV_E_TYMED) 
                // or others(Aspect, Formatetc).
                // It depends on application's IDataObject::GetData implementation.
                clipboardInfo.AppendText("Fail to get data!!! ***COMException***");
            }
            catch (OutOfMemoryException)
            {
                // Fail by the out of memory from getting data on Clipboard. 
                // Occurs with the low memory.
                clipboardInfo.AppendText("Fail to get data!!! ***OutOfMemoryException***");
            }

            return data;
        }

        // Set the plain text on RichTextBox
        private void SetTextOnRichTextBox(string text)
        {
            var document = richTextBox.Document;
            var textRange = new TextRange(document.ContentStart, document.ContentEnd) {Text = text};
            richTextBox.Focus();
            richTextBox.SelectAll();
        }

        // Get text data string from RichTextBox content which encoded as UTF8
        private string GetTextStringFromRichTextBox(string dataFormat)
        {
            var document = richTextBox.Document;
            var textRange = new TextRange(document.ContentStart, document.ContentEnd);

            if (dataFormat == DataFormats.Text)
            {
                return textRange.Text;
            }
            Stream contentStream = new MemoryStream();
            textRange.Save(contentStream, dataFormat);

            if (contentStream.Length > 0)
            {
                var bytes = new byte[contentStream.Length];

                contentStream.Position = 0;
                contentStream.Read(bytes, 0, bytes.Length);

                var utf8Encoding = Encoding.UTF8;

                return utf8Encoding.GetString(bytes);
            }

            return null;
        }

        // Get text data string from file content with the proper encoding
        private string GetTextStringFromFile(string fileName, Encoding fileEncoding)
        {
            string textString = null;

            FileStream fileStream;

            try
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch (IOException)
            {
                MessageBox.Show("File is not acessible.\n", "File Open Error");
                return null;
            }

            fileStream.Position = 0;

            var bytes = new byte[fileStream.Length];

            fileStream.Read(bytes, 0, bytes.Length);

            if (bytes.Length > 0)
            {
                textString = fileEncoding.GetString(bytes);
            }

            fileStream.Close();

            return textString;
        }

        // Paste text data on RichTextBox as UTF8 encoding
        private bool PasteTextDataToRichTextBox(string dataFormat, string textData)
        {
            var pasted = false;

            var document = richTextBox.Document;
            var textRange = new TextRange(document.ContentStart, document.ContentEnd);

            Stream stream = new MemoryStream();

            var bytesUnicode = Encoding.Unicode.GetBytes(textData);
            var bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytesUnicode);

            if (bytes.Length > 0 && textRange.CanLoad(dataFormat))
            {
                stream.Write(bytes, 0, bytes.Length);
                textRange.Load(stream, dataFormat);
                pasted = true;
            }

            return pasted;
        }

        // Paste text data to the file with the file encoding
        private void PasteTextDataToFile(string dataFormat, string textData, string fileName, Encoding fileEncoding)
        {
            FileStream fileWriteStream;

            try
            {
                fileWriteStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch (IOException)
            {
                MessageBox.Show("File is not acessible.\n", "File Write Error");
                return;
            }

            fileWriteStream.SetLength(0);

            var bytesUnicode = Encoding.Unicode.GetBytes(textData);
            var bytes = Encoding.Convert(Encoding.Unicode, fileEncoding, bytesUnicode);

            if (bytes.Length > 0)
            {
                fileWriteStream.Write(bytes, 0, bytes.Length);
            }

            fileWriteStream.Close();
        }

        // Paste stream data to the file
        private void PasteStreamDataToFile(Stream stream, string fileName)
        {
            FileStream fileWriteStream = null;

            try
            {
                fileWriteStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch (IOException)
            {
                MessageBox.Show("File is not acessible.\n", "File Write Error");
            }

            fileWriteStream.SetLength(0);

            var bytes = new byte[stream.Length];

            stream.Position = 0;
            stream.Read(bytes, 0, bytes.Length);

            if (bytes.Length > 0)
            {
                fileWriteStream.Write(bytes, 0, bytes.Length);
            }

            fileWriteStream.Close();
        }
    }
}