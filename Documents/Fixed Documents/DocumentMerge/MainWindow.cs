// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;

namespace DocumentMerge
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, XpsDocument> _documentCache;
        private readonly RollUpDocument _documentRollUp;

        public MainWindow()
        {
            InitializeComponent();
            // Add the Open Command
            AddCommandBindings(ApplicationCommands.Open, OpenCommandHandler);
            // Add the Save Command
            //AddCommandBindings(ApplicationCommands.Save, SaveCommandHandler);
            AddCommandBindings(ApplicationCommands.Save, SaveCommandHandler);
            // Add the Exit/Close Command
            AddCommandBindings(ApplicationCommands.Close, CloseCommandHandler);

            _documentRollUp = new RollUpDocument();
            _documentCache = new Dictionary<string, XpsDocument>();
        }

        // ------------------------ OpenCommandHandler ------------------------
        /// <summary>
        ///     Opens an existing XPS document and displays it
        ///     in a DocumentViewer control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCommandHandler(object sender, RoutedEventArgs e)
        {
            //Display a file open dialog to select an existing document
            var dlg = new OpenFileDialog
            {
                Filter = "XPS Document (*.xps)|*.xps",
                InitialDirectory = GetContentFolder(),
                CheckFileExists = true,
                Multiselect = true
            };
            if (dlg.ShowDialog() == true)
            {
                foreach (var file in dlg.FileNames)
                {
                    AddDocumentsToSource(file);
                }
            }
            menuFileSave.IsEnabled = true;
        }

        // ------------------------- GetContentFolder -------------------------
        /// <summary>
        ///     Locates and returns the path to the "Content\" folder
        ///     containing the fixed document for the sample.
        /// </summary>
        /// <returns>
        ///     The path to the fixed document "Content\" folder.
        /// </returns>
        private string GetContentFolder()
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

        private void AddDocumentsToSource(string fileName)
        {
            if (!_documentCache.ContainsKey(fileName))
            {
                var xpsDocument = new XpsDocument(fileName, FileAccess.Read);
                _documentCache[fileName] = xpsDocument;
                var docSeq = xpsDocument.GetFixedDocumentSequence();
                foreach (var docRef in docSeq.References)
                {
                    var item = new DocumentItem(fileName, docRef);
                    source.Items.Add(item);
                }
            }
        }

        /// <summary>
        ///     Closes current Xps Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommandHandler(object sender, RoutedEventArgs e)
        {
            if (dest.Items.Count < 1)
            {
                MessageBox.Show("Please add document to list before saving",
                    "No document to be combined", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Xps Documents (*.xps)|*.xps",
                FilterIndex = 1
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var destFile = saveFileDialog.FileName;
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                _documentRollUp.Uri = new Uri(destFile);
                _documentRollUp.Save();
            }
        }

        /// <summary>
        ///     Closes current XPS document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// !!!private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        private void CloseCommandHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SourceSelected(object sender, SelectionChangedEventArgs args)
        {
            sourcePage.Items.Clear();
            foreach (var item in args.AddedItems)
            {
                if (item is DocumentItem)
                {
                    var docSrc = item as DocumentItem;
                    foreach (var pageContent in docSrc.FixedDocument.Pages)
                    {
                        sourcePage.Items.Add(new PageItem(pageContent));
                    }
                }
            }
        }

        private void DestSelected(object sender, SelectionChangedEventArgs args)
        {
            destPage.Items.Clear();
            var selectedIndex = dest.SelectedIndex;
            if (selectedIndex != -1)
            {
                var pageCount = _documentRollUp.GetPageCount(selectedIndex);
                for (var i = 0; i < pageCount; i++)
                {
                    var pageContent = _documentRollUp.GetPage(selectedIndex, i);
                    destPage.Items.Add(new PageItem(pageContent));
                }
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    AddDocumentsToSource(file);
                }
            }
        }

        /// <summary>
        ///     Helper function to find commands to handlers and register them
        /// </summary>
        /// <param name="command"></param>
        /// <param name="handler"></param>
        private void AddCommandBindings(ICommand command, ExecutedRoutedEventHandler handler)
        {
            var cmdBindings = new CommandBinding(command);
            cmdBindings.Executed += handler;
            CommandBindings.Add(cmdBindings);
        }

        protected void DestDocList_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof (List<DocumentItem>)))
            {
                var items = (List<DocumentItem>) e.Data.GetData(typeof (List<DocumentItem>));
                dest.BeginInit();
                dest.SelectedItems.Clear();
                DocumentItem firstItem = null;
                foreach (var item in items)
                {
                    var newItem = new DocumentItem(item);
                    dest.Items.Add(newItem);
                    // only select fist item
                    if (firstItem == null)
                    {
                        firstItem = newItem;
                    }
                    _documentRollUp.AddDocument(item.DocumentReference.Source,
                        (item.DocumentReference as IUriContext).BaseUri);
                }
                dest.SelectedItem = firstItem;
                dest.EndInit();
            }
            e.Handled = true;
        }

        protected void DestPageList_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof (List<PageItem>)))
            {
                var items = (List<PageItem>) e.Data.GetData(typeof (List<PageItem>));
                if (items.Count > 0)
                {
                    var selectedDoc = dest.SelectedIndex;
                    if (selectedDoc == -1)
                    {
                        selectedDoc = _documentRollUp.AddDocument();
                        var docItem = new DocumentItem(items[0].FixedPage);
                        dest.Items.Add(docItem);
                        dest.SelectedItem = docItem;
                    }
                    foreach (var item in items)
                    {
                        destPage.Items.Add(new PageItem(item));
                        _documentRollUp.AddPage(selectedDoc, item.PageContent.Source,
                            (item.PageContent as IUriContext).BaseUri);
                    }
                }
            }
            e.Handled = true;
        }

        protected void DestItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof (List<DocumentItem>)))
            {
                var items = (List<DocumentItem>) e.Data.GetData(typeof (List<DocumentItem>));
                var currItem = (ListBoxItem) sender;
                var currItemIndex = dest.Items.IndexOf(currItem.Content);
                var insertionIndex = currItemIndex;

                var currentDropPosition = e.GetPosition(dest);
                var itemPosition = currItem.TranslatePoint(new Point(0, 0), dest);
                if (currentDropPosition.X < (itemPosition.X + (currItem.ActualWidth/2)))
                {
                }
                else
                {
                    insertionIndex++;
                }
                foreach (var item in items)
                {
                    dest.Items.Insert(insertionIndex, new DocumentItem(item));
                    _documentRollUp.InsertDocument(insertionIndex, item.DocumentReference.Source,
                        (item.DocumentReference as IUriContext).BaseUri);
                }
            }
            e.Handled = true;
        }

        protected void DestPageItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof (List<PageItem>)))
            {
                var items = (List<PageItem>) e.Data.GetData(typeof (List<PageItem>));
                var currItem = (ListBoxItem) sender;
                var currItemIndex = destPage.Items.IndexOf(currItem.Content);
                var insertionIndex = currItemIndex;

                var currentDropPosition = e.GetPosition(dest);
                var itemPosition = currItem.TranslatePoint(new Point(0, 0), destPage);
                if (currentDropPosition.X < (itemPosition.X + (currItem.ActualWidth/2)))
                {
                }
                else
                {
                    insertionIndex++;
                }
                foreach (var item in items)
                {
                    destPage.Items.Insert(insertionIndex, new PageItem(item));
                    _documentRollUp.InsertPage(dest.SelectedIndex, insertionIndex, item.PageContent.Source,
                        (item.PageContent as IUriContext).BaseUri);
                }
            }
            e.Handled = true;
        }

        private void DestList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var index = -1;

                while ((index = dest.SelectedIndex) != -1)
                {
                    dest.Items.RemoveAt(index);
                    _documentRollUp.RemoveDocument(index);
                }
            }
        }

        private void DestPageList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var index = -1;

                while ((index = destPage.SelectedIndex) != -1)
                {
                    destPage.Items.RemoveAt(index);
                    _documentRollUp.RemovePage(dest.SelectedIndex, index);
                }
                // if we deleted all the pages, we will remove the parent document too
                if (destPage.Items.Count == 0)
                {
                    while ((index = dest.SelectedIndex) != -1)
                    {
                        dest.Items.RemoveAt(index);
                        _documentRollUp.RemoveDocument(index);
                    }
                }
            }
        }

        private void rect_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && !Keyboard.IsKeyDown(Key.RightCtrl) && !Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var items = new List<DocumentItem>();
                var clearAndSelect = false;

                var clikedItem = sender as ListBoxItem;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    items.AddRange(source.SelectedItems.Cast<DocumentItem>());
                }
                else
                {
                    clearAndSelect = true;
                    items.Add(clikedItem.Content as DocumentItem);
                }

                DragDrop.DoDragDrop(source, items, DragDropEffects.All);
                if (clearAndSelect)
                {
                    source.SelectedItems.Clear();
                }
                clikedItem.IsSelected = true;
                e.Handled = true;
            }
        }

        private void page_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && !Keyboard.IsKeyDown(Key.RightCtrl) && !Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var items = new List<PageItem>();
                var clearAndSelect = false;

                var clikedItem = sender as ListBoxItem;
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    items.AddRange(sourcePage.SelectedItems.Cast<PageItem>());
                }
                else
                {
                    clearAndSelect = true;
                    items.Add(clikedItem.Content as PageItem);
                }

                DragDrop.DoDragDrop(source, items, DragDropEffects.All);
                if (clearAndSelect)
                {
                    sourcePage.SelectedItems.Clear();
                }
                clikedItem.IsSelected = true;
                e.Handled = true;
            }
        }

        private void dest_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}