// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace DialogBox
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _needsToBeSaved;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Closing
        private void mainWindow_Closing(object sender, CancelEventArgs e)
        {
            // If the document needs to be saved
            if (_needsToBeSaved)
            {
                // Configure the message box
                var messageBoxText =
                    "This document needs to be saved. Click Yes to save and exit, No to exit without saving, or Cancel to not exit.";
                var caption = "Word Processor";
                var button = MessageBoxButton.YesNoCancel;
                var icon = MessageBoxImage.Warning;

                // Display message box
                var messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes: // Save document and exit
                        SaveDocument();
                        break;
                    case MessageBoxResult.No: // Exit without saving
                        break;
                    case MessageBoxResult.Cancel: // Don't exit
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void fileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenDocument();
        }

        private void fileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveDocument();
        }

        private void filePrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument();
        }

        private void fileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void editFindMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new FindDialogBox(documentTextBox) {Owner = this};

            // Configure the dialog box
            dlg.TextFound += dlg_TextFound;

            // Open the dialog box modally
            dlg.Show();
        }

        private void formatMarginsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new MarginsDialogBox
            {
                Owner = this,
                DocumentMargin = documentTextBox.Margin
            };

            // Configure the dialog box

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Update fonts
                documentTextBox.Margin = dlg.DocumentMargin;
            }
        }

        private void formatFontMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the dialog box
            var dlg = new FontDialogBox
            {
                Owner = this,
                FontFamily = documentTextBox.FontFamily,
                FontSize = documentTextBox.FontSize,
                FontWeight = documentTextBox.FontWeight,
                FontStyle = documentTextBox.FontStyle
            };

            // Configure the dialog box

            // Open the dialog box modally 
            dlg.ShowDialog();

            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                // Update fonts
                documentTextBox.FontFamily = dlg.FontFamily;
                documentTextBox.FontSize = dlg.FontSize;
                documentTextBox.FontWeight = dlg.FontWeight;
                documentTextBox.FontStyle = dlg.FontStyle;
            }
        }

        // Detect when document has been altered
        private void documentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _needsToBeSaved = true;
        }

        private void OpenDocument()
        {
            // Instantiate the dialog box
            var dlg = new OpenFileDialog
            {
                FileName = "Document",
                DefaultExt = ".wpf",
                Filter = "Word Processor Files (.wpf)|*.wpf"
            };

            // Configure open file dialog box
            // Default file name
            // Default file extension
            // Filter files by extension

            // Open the dialog box modally
            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                var filename = dlg.FileName;
            }
        }

        private void SaveDocument()
        {
            // Configure save file dialog
            var dlg = new SaveFileDialog
            {
                FileName = "Document",
                DefaultExt = ".wpf",
                Filter = "Word Processor Files (.wpf)|*.wpf"
            };
            // Default file name
            // Default file extension
            // Filter files by extension

            // Show save file dialog
            var result = dlg.ShowDialog();

            // Process save file dialog results
            if (result == true)
            {
                // Save document
                var filename = dlg.FileName;
            }
        }

        private void PrintDocument()
        {
            // Configure printer dialog
            var dlg = new PrintDialog
            {
                PageRangeSelection = PageRangeSelection.AllPages,
                UserPageRangeEnabled = true
            };

            // Show save file dialog
            var result = dlg.ShowDialog();

            // Process save file dialog results
            if (result == true)
            {
                // Print document
            }
        }

        private void dlg_TextFound(object sender, EventArgs e)
        {
            // Get the find dialog box that raised the event
            var dlg = (FindDialogBox) sender;

            // Get find results and select found text
            documentTextBox.Select(dlg.Index, dlg.Length);
            documentTextBox.Focus();
        }
    }
}