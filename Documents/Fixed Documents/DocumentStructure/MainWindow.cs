// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;

namespace DocumentStructure
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region constructor

        // ------------------------ Window1 constructor -----------------------
        public MainWindow()
        {
            InitializeComponent();

            ShowPrompt("Click 'File | Open...' and select the file " +
                       "'Spec_withoutStructure.xps'.");
            ShowDescription(DescriptionString[0]);
        }

        #endregion constructor

        #region File|Exit

        // ------------------------------ OnExit ------------------------------
        /// <summary>
        ///     Handles the user File|Exit menu selection to
        ///     shutdown and exit the application.
        /// </summary>
        private void OnExit(object sender, EventArgs e)
        {
            Close(); // invokes OnClosed()
        } // end:OnExit()

        #endregion File|Exit

        #region File|Open...

        // ------------------------------ OnOpen ------------------------------
        /// <summary>
        ///     Handles the user "File | Open" menu operation.
        /// </summary>
        private void OnOpen(object target, ExecutedRoutedEventArgs args)
        {
            // Create a "File Open" dialog positioned to the
            // "Content\" folder containing the sample fixed document.
            var dialog = new OpenFileDialog
            {
                InitialDirectory = GetContentFolder(),
                CheckFileExists = true,
                Filter = "XPS Document files (*.xps)|*.xps|All (*.*)|*.*"
            };

            // Show the "File Open" dialog.  If the user picks a file and
            // clicks "OK", load and display the specified XPS document.
            if (dialog.ShowDialog() != true) return;

            // If the file is an XPS document, open it in the DocumentViewer.
            if (dialog.FileName.EndsWith(".xps"))
                OpenDocument(dialog.FileName);

            // If the file is a text document, show it in the desciption block.
            else if (dialog.FileName.EndsWith(".xaml")
                     || dialog.FileName.EndsWith(".xml")
                     || dialog.FileName.EndsWith(".txt"))
            {
                using (var sr = new StreamReader(dialog.FileName))
                {
                    ShowDescriptionTitle(Filename(dialog.FileName));
                    ShowDescription(sr.ReadToEnd(), TextWrapping.NoWrap);
                } // end:using StreamReader(flush and close)
                ShowPrompt("");
            }
        } // end:OnOpen()


        // --------------------------- OpenDocument ---------------------------
        /// <summary>
        ///     Opens a specified XPS document in given access mode.
        /// </summary>
        /// <param name="xpsFile">
        ///     The path and file name of the XPS document to open.
        /// </param>
        private bool OpenDocument(string xpsFile)
        {
            // Close the XpsDocument if it is currently open.
            if (_xpsDocument != null) CloseDocument();

            // Load and create an in-memory instance of the XPS document.
            ShowStatus("Opening '" + Filename(xpsFile) + "'.");
            _xpsDocument = new XpsDocument(xpsFile, FileAccess.Read);

            // Get the FixedDocumentSequence from the XPS document.
            var fds = _xpsDocument.GetFixedDocumentSequence();
            if (fds == null)
            {
                var msg = xpsFile +
                          "\n\nThe document package within the specified " +
                          "file does not contain a FixedDocumentSequence.";
                MessageBox.Show(msg, "Package Error");
                return false;
            }

            // Load the FixedDocumentSequence to the DocumentViewer control.
            DocViewer.Document = fds;

            var filename = Filename(xpsFile).ToLower();
            if (filename.Equals("spec_withoutstructure.xps"))
            {
                ShowPrompt("Click 'File | Open...' and select the file " +
                           "'Spec_withStructure.xps'.");
                ShowDescription(DescriptionString[2]);
            }
            else if (filename.Equals("spec_withstructure.xps"))
            {
                ShowPrompt("Click 'File | Add Structure...'.");
                ShowDescription(DescriptionString[3] + DescriptionString[4]);
            }
            else
            {
                ShowPrompt("");
            }

            _openedXpsFile = xpsFile;
            menuFileClose.IsEnabled = true;
            menuFilePrint.IsEnabled = true;
            descriptionBlockTitle.Text = "Description";
            Title = "DocumentStructure Sample - " + Filename(xpsFile);
            return true;
        } // OpenDocument()

        #endregion File|Open...

        #region File|Close

        // ----------------------------- OnClosed -----------------------------
        /// <summary>
        ///     Performs clean up when the application is closed.
        /// </summary>
        private void OnClosed(object sender, EventArgs e)
        {
            CloseDocument();
        } // end:OnClosed()


        // ----------------------------- OnClose ------------------------------
        /// <summary>
        ///     Handles the user "File | Close" menu operation
        ///     to close the currently open document.
        /// </summary>
        private void OnClose(object target, ExecutedRoutedEventArgs args)
        {
            CloseDocument();
        } // end:OnClose()


        // -------------------------- CloseDocument ---------------------------
        /// <summary>
        ///     Closes the document if its open.
        /// </summary>
        private void CloseDocument()
        {
            if (_xpsDocument != null)
            {
                ShowStatus("Closing '" + Filename(_openedXpsFile) + "'.");
                _xpsDocument.Close();
                _xpsDocument = null;
                _openedXpsFile = null;
                DocViewer.Document = null;
            }

            ShowPrompt("Click 'File | Open...' and select the file " +
                       "'Spec_withoutStructure.xps'.");
            ShowDescription(DescriptionString[0]);
            menuFilePrint.IsEnabled = false;
            menuFileClose.IsEnabled = false;
            Title = "DocumentStructure Sample";
        } // end:CloseDocument

        #endregion File|Close

        #region File|Add Structure...

        // -------------------------- OnAddStructure --------------------------
        /// <summary>
        ///     Handles the user File|Add Structure... menu option to
        ///     add structure elements to an unstructured XPS document.
        /// </summary>
        private void OnAddStructure(object sender, EventArgs e)
        {
            /*-----
            // Create a "File Open" dialog positioned to the
            // "Content\" folder containing the sample XPS documents.
            WinForms.OpenFileDialog openDialog = new WinForms.OpenFileDialog();
            openDialog.InitialDirectory = GetContentFolder();
            openDialog.Title = "Source Unstructured XPS Document";
            openDialog.CheckFileExists = true;
            openDialog.Filter = "XPS Documents " +
                "(*_withoutStructure.xps)|*_withoutStructure.xps|All (*.*)|*.*";

            // Show the "File Open" dialog.  If the user picks a file and
            // clicks "OK", set it as the XPS unstructured file.
            if (openDialog.ShowDialog() != true)  return;
            string xpsUnstructuredFile = openDialog.FileName;
            -----*/
            // For the sample, always use "Spec_withoutStructure.xps"
            // as the source unstructured document file.
            var xpsUnstructuredFile = GetContentFolder() +
                                      "\\Spec_withoutStructure.xps";

            // Create a "File Save" dialog positioned to the
            // "Content\" folder to write the new structured document to.
            var saveDialog = new SaveFileDialog
            {
                OverwritePrompt = false,
                InitialDirectory = GetContentFolder(),
                Title = "Save New Structured XPS Document As",
                Filter = "XPS Documents (*.xps)|*.xps|All (*.*)|*.*",
                FileName = GetContentFolder() + @"\Structured.xps"
            };

            // Set a default XPS document filename.

            // Show the "File Save" dialog.  If the user picks a file and
            // clicks "OK", set it as the target ouput XPS structured file.
            if (saveDialog.ShowDialog() != true) return;
            var xpsTargetFile = saveDialog.FileName;

            // Add the document structure resource elements
            // to create a new structured XPS document.
            AddDocumentStructure(xpsUnstructuredFile, // source unstructured doc
                FixedPageStructures, // structure definitions
                xpsTargetFile); // target structured doc
        } // end:OnAddStructure()


        // ----------------------- AddDocumentStructure -----------------------
        /// <summary>
        ///     Writes a structured XPS document given an unstructured document
        ///     and a file list of XAML document structure elements to add.
        /// </summary>
        /// <param name="xpsUnstructuredFile">
        ///     The path and filename of the unstructured XPS document.
        /// </param>
        /// <param name="xamlStructureFile">
        ///     A file list of XAML document structures to add.
        /// </param>
        /// <param name="xpsTargetFile">
        ///     The path and filename for the new structured
        ///     XPS document to write.
        /// </param>
        /// <remarks>
        ///     If xpsTargetFile exists, it is first deleted and
        ///     then a new structured XPS document written.
        /// </remarks>
        private void AddDocumentStructure(
            string xpsUnstructuredFile, // source unstructured document
            string[] xamlStructureFile, // structure definition resources
            string xpsTargetFile) // target structured XPS document
        {
            // Close the current document if one is open.
            CloseDocument();

            ShowStatus("\nCreating new structured XPS\n       " +
                       "document '" + Filename(xpsTargetFile) + "'.");

            // If the new target XPS file exists, prompt to confirm ok to replace.
            if (File.Exists(xpsTargetFile))
            {
                var result = MessageBox.Show("'" + xpsTargetFile +
                                             "' already exists.\nDo you want to delete and replace it?",
                    "Ok to replace '" + Filename(xpsTargetFile) + "'?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;
                ShowStatus("   Deleting old '" +
                           Filename(xpsTargetFile) + "'.");
                File.Delete(xpsTargetFile);
            }

            ShowStatus("   Copying '" + Filename(xpsUnstructuredFile) +
                       "'\n       to '" + Filename(xpsTargetFile) + "'.");
            File.Copy(xpsUnstructuredFile, xpsTargetFile);

            ShowStatus("   Opening '" + Filename(xpsTargetFile) +
                       "'\n       (currently without structure).");
            XpsDocument xpsDocument = null;
            try
            {
                xpsDocument =
                    new XpsDocument(xpsTargetFile, FileAccess.ReadWrite);
            }
            catch (FileFormatException)
            {
                var msg = xpsUnstructuredFile + "\n\nThe specified file " +
                          "does not appear to be a valid XPS document.";
                MessageBox.Show(msg, "Invalid File Format",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowStatus("   Invalid XPS document format.");
                return;
            }

            ShowStatus("   Getting FixedDocumentSequenceReader.");
            var fixedDocSeqReader =
                xpsDocument.FixedDocumentSequenceReader;

            ShowStatus("   Getting FixedDocumentReaders.");
            ICollection<IXpsFixedDocumentReader> fixedDocuments =
                fixedDocSeqReader.FixedDocuments;

            ShowStatus("   Getting FixedPageReaders.");
            var enumerator =
                fixedDocuments.GetEnumerator();
            enumerator.MoveNext();
            ICollection<IXpsFixedPageReader> fixedPages =
                enumerator.Current.FixedPages;


            // Add a document structure to each fixed page.
            var i = 0;
            foreach (var fixedPageReader in fixedPages)
            {
                XpsResource pageStructure;
                ShowStatus("   Adding page structure resource:\n       '" +
                           Filename(FixedPageStructures[i]) + "'");
                try
                {
                    // Add a new StoryFragment to hold the page structure.
                    pageStructure = fixedPageReader.AddStoryFragment();
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show(xpsUnstructuredFile +
                                    "\n\nDocument structure cannot be added.\n\n" +
                                    Filename(xpsUnstructuredFile) + " might already " +
                                    "contain an existing document structure.",
                        "Cannot Add Document Structure",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }

                // Copy the page structure to the new StoryFragment.
                WriteResource(pageStructure, FixedPageStructures[i++]);
            }

            ShowStatus("   Saving and closing the new document.\n");
            xpsDocument.Close();

            // Open the new structured document.
            OpenDocument(xpsTargetFile);

            ShowDescription(DescriptionString[4] + DescriptionString[5]);
            ShowPrompt(DescriptionString[5]);
        } // end:AddDocumentStructure

        #endregion File|Add Structure...

        #region File|Print

        // ----------------------------- OnPrint ------------------------------
        /// <summary>
        ///     Handles the user "File | Print" menu operation.
        /// </summary>
        private void OnPrint(object target, ExecutedRoutedEventArgs args)
        {
            PrintDocument();
        } // end:OnClose()


        // -------------------------- PrintDocument ---------------------------
        /// <summary>
        ///     Prints the DocumentViewer's content and annotations.
        /// </summary>
        public void PrintDocument()
        {
            DocViewer?.Print();
        } // end:PrintDocument()

        #endregion File|Print

        #region Utilities

        // ------------------------- GetContentFolder -------------------------
        /// <summary>
        ///     Locates and returns the path to the "Content\" folder
        ///     containing the content files for the sample.
        /// </summary>
        /// <returns>
        ///     The path to the sample "Content\" folder.
        /// </returns>
        public string GetContentFolder()
        {
            // Get the path to the current directory and its length.
            var contentDir = Directory.GetCurrentDirectory();
            var dirLength = contentDir.Length;

            // If we're in "...\bin\debug", move up to the root.
            if (contentDir.ToLower().EndsWith(@"\bin\debug"))
                contentDir = contentDir.Remove(dirLength - 10, 10);

            // If we're in "...\bin\release", move up to the root.
            else if (contentDir.ToLower().EndsWith(@"\bin\release"))
                contentDir = contentDir.Remove(dirLength - 12, 12);

            // If there's a "Content" subfolder, that's what we want.
            if (Directory.Exists(contentDir + @"\Content"))
                contentDir = contentDir + @"\Content";

            // Return the "Content\" folder (or the "current"
            // directory if we're executing somewhere else).
            return contentDir;
        } // end:GetContentFolder()


        // -------------------------- WriteResource ---------------------------
        private void WriteResource(XpsResource resource, string filename)
        {
            WriteStream(resource.GetStream(), filename);
        }


        // --------------------------- WriteStream ----------------------------
        private void WriteStream(Stream stream, string filename)
        {
            var srcStream = new FileStream(
                filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            var buf = new byte[65536];
            int bytesRead;

            do
            {
                bytesRead = srcStream.Read(buf, 0, 65536);
                stream.Write(buf, 0, bytesRead);
            } while (bytesRead > 0);

            srcStream.Close();
        } // end:WriteStream()


        // ------------------------- ShowPrompt(string) -----------------------
        /// <summary>
        ///     Writes a line of text to the prompt bar.
        /// </summary>
        /// <param name="prompt">
        ///     The line of text to write in the prompt bar.
        /// </param>
        public void ShowPrompt(string prompt)
        {
            promptBlock.Text = prompt;
        }


        // ---------------------------- ShowStatus ----------------------------
        /// <summary>
        ///     Adds a line of text to the statusBlock.
        /// </summary>
        /// <param name="status">
        ///     A line of text to add to the status TextBlock.
        /// </param>
        public void ShowStatus(string status)
        {
            statusBlock.Text += status + "\n";
        }


        // ------------------------- ShowDescription -------------------------
        /// <summary>
        ///     Displays a string of text to the description block.
        /// </summary>
        public void ShowDescription(string description)
        {
            descriptionBlock.Text = description;
            descriptionBlock.TextWrapping = TextWrapping.Wrap;
        }


        // ------------------------- ShowDescription -------------------------
        /// <summary>
        ///     Displays a string of text to the description block.
        /// </summary>
        public void ShowDescription(string description, TextWrapping wrapStyle)
        {
            descriptionBlock.Text = description;
            descriptionBlock.TextWrapping = wrapStyle;
        }


        // ----------------------- ShowDescriptionTitle -----------------------
        /// <summary>
        ///     Displays a string of text in the description title block.
        /// </summary>
        public void ShowDescriptionTitle(string title)
        {
            descriptionBlockTitle.Text = title;
        }


        // ------------------------------ Filename ----------------------------
        /// <summary>
        ///     Returns just the filename from a given path and filename.
        /// </summary>
        /// <param name="filepath">
        ///     A path and filename.
        /// </param>
        /// <returns>
        ///     The filename with extension.
        /// </returns>
        private static string Filename(string filepath)
        {
            // Locate the index of the last backslash.
            var slashIndex = filepath.LastIndexOf('\\');

            // If there is no backslash, return the original string.
            if (slashIndex < 0)
                return filepath;

            // Remove all the characters through the last backslash.
            return filepath.Remove(0, slashIndex + 1);
        }


        // ----------------------- DocViewer attribute ------------------------
        /// <summary>
        ///     Gets the current DocumentViewer.
        /// </summary>
        public DocumentViewer DocViewer => docViewer;

        #endregion Utilities

        #region Private Fields

        private string _openedXpsFile; // XPS document path & filename.
        private XpsDocument _xpsDocument; // current XpsDocument.

        // Document structure resource files.
        private static readonly string[] FixedPageStructures =
        {
            "FixedPage1_structure.xaml", // document structure elements, page 1
            "FixedPage2_structure.xaml" // document structure elements, page 2
        };

        // Description block text strings.
        private static readonly string[] DescriptionString =
        {
            //Step 0
            "The DocumentStructure example includes two sample XPS documents:\n" +
            "    1.  Spec_withoutStructure.xps\n" +
            "    2.  Spec_withStructure.xps\n\n" +
            "When you open each file note that the visual layout, " +
            "quality, and print output of both documents is exactly " +
            "the same - both documents fully conform to the open XML " +
            "Paper Specification (XPS).  Cutting and pasting information " +
            "from each document, however, is quite different.\n\nClick " +
            "'File | Open...', select the file " +
            "'Spec_withoutStructure.xps', and then click OK.",
            //Step 1
            "Click 'File | Open...', select the file " +
            "'Spec_withoutStructure.xps', and then click OK.",
            //Step 2
            "Within the 'Spec_withoutStructure.xps' document select a portion " +
            "of Table 1-1 and paste it into a blank Word or WordPad " +
            "document.  Notice that an XPS document without structure " +
            "elements pastes as plain text not as a formatted table.\n\n" +
            "Next click 'File | Open...' and select " +
            "'Spec_withStructure.xps'.",
            //Step 3
            "Select a portion of " +
            "Table 1-1 and paste it into the Word or WordPad document.  " +
            "Notice that a document with structure elements uses rich " +
            "text to paste the selection as styled table elements.\n\n" +
            "Next click 'File | Add Structure...'\n\n",
            //Step 4
            "The Add Structure process copies 'Spec_withoutStructure.xps' " +
            "to a new file and then programatically adds to the new " +
            "document the two structure elements contained in:\n    -  " +
            "FixedPage1_structure.xaml\n    -  FixedPage2_structure.xaml\n\n" +
            "The resulting new XPS document is equivalent " +
            "to 'Spec_withStructure.xps'.\n\n",
            //Step 5
            "Click 'File | Open...' and set 'Files of Type' to 'All' " +
            "to select a .xaml document structure resource file to view."
        };

        #endregion Private Fields
    }
}