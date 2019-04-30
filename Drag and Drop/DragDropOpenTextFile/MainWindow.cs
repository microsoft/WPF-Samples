// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows;

namespace DragDropOpenTextFile
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if ((bool) cbWrap.IsChecked)
                tbDisplayFileContents.TextWrapping = TextWrapping.Wrap;
            else
                tbDisplayFileContents.TextWrapping = TextWrapping.NoWrap;
        }

        private void ClickClear(object sender, RoutedEventArgs args)
        {
            tbDisplayFileContents.Clear();
        }

        private void ClickWrap(object sender, RoutedEventArgs args)
        {
            if ((bool) cbWrap.IsChecked)
                tbDisplayFileContents.TextWrapping = TextWrapping.Wrap;
            else
                tbDisplayFileContents.TextWrapping = TextWrapping.NoWrap;
        }

        private void EhDragOver(object sender, DragEventArgs args)
        {
            // As an arbitrary design decision, we only want to deal with a single file.
            args.Effects = IsSingleFile(args) != null ? DragDropEffects.Copy : DragDropEffects.None;

            // Mark the event as handled, so TextBox's native DragOver handler is not called.
            args.Handled = true;
        }

        private void EhDrop(object sender, DragEventArgs args)
        {
            // Mark the event as handled, so TextBox's native Drop handler is not called.
            args.Handled = true;

            var fileName = IsSingleFile(args);
            if (fileName == null) return;

            var fileToLoad = new StreamReader(fileName);
            tbDisplayFileContents.Text = fileToLoad.ReadToEnd();
            fileToLoad.Close();

            // Set the window title to the loaded file.
            Title = "File Loaded: " + fileName;
        }

        // If the data object in args is a single file, this method will return the filename.
        // Otherwise, it returns null.
        private string IsSingleFile(DragEventArgs args)
        {
            // Check for files in the hovering data object.
            if (args.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var fileNames = args.Data.GetData(DataFormats.FileDrop, true) as string[];
                // Check for a single file or folder.
                if (fileNames?.Length is 1)
                {
                    // Check for a file (a directory will return false).
                    if (File.Exists(fileNames[0]))
                    {
                        // At this point we know there is a single file.
                        return fileNames[0];
                    }
                }
            }
            return null;
        }
    }
}
