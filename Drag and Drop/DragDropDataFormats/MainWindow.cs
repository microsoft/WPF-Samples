// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace DragDropDataFormats
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

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            tbDropTarget.Clear();
        }

        private void EhDragEnter(object sender, DragEventArgs args)
        {
            args.Effects = DragDropEffects.Copy;
        }

        private void EhPreviewDrop(object sender, DragEventArgs args)
        {
            // TextBox includes native drop handling.  This is to let the TextBox know that we're handling
            // the Drop event, and we don't want the native handler to execute.  
            args.Handled = true;

            ShowDataFormats(args);
        }

        private void ShowDataFormats(DragEventArgs args)
        {
            tbDropTarget.AppendText("The following data formats are present:\n");
            if (cbAutoConvert.IsChecked.Value)
            {
                foreach (var format in args.Data.GetFormats((bool) cbAutoConvert.IsChecked))
                {
                    if (args.Data.GetDataPresent(format, false))
                        tbDropTarget.AppendText("\t- " + format + " (native)\n");
                    else tbDropTarget.AppendText("\t- " + format + " (autoconvert)\n");
                }
            }
            else
            {
                foreach (var format in args.Data.GetFormats((bool) cbAutoConvert.IsChecked))
                {
                    tbDropTarget.AppendText("\t- " + format + " (native)\n");
                }
            }

            tbDropTarget.AppendText("\n");
            tbDropTarget.ScrollToEnd();
        }
    }
}