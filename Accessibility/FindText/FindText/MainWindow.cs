// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using Microsoft.Win32;

namespace FindText
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

        private void LoadFile(object sender, RoutedEventArgs args)
        {
            var openFile = new OpenFileDialog
            {
                Filter = "FlowDocument Files (*.xaml)|*.xaml|All Files (*.*)|*.*",
                InitialDirectory = "Content"
            };

            if (openFile.ShowDialog().HasValue)
            {
                var xamlFile = openFile.OpenFile() as FileStream;
                if (xamlFile == null) return;
                FlowDocument content = null;
                try
                {
                    content = XamlReader.Load(xamlFile) as FlowDocument;
                    if (content == null)
                        throw (new XamlParseException("The specified file could not be loaded as a FlowDocument."));
                }
                catch (XamlParseException e)
                {
                    var error = "There was a problem parsing the specified file:\n\n";
                    error += openFile.FileName;
                    error += "\n\nException details:\n\n";
                    error += e.Message;
                    MessageBox.Show(error);
                    return;
                }
                catch (Exception e)
                {
                    var error = "There was a problem loading the specified file:\n\n";
                    error += openFile.FileName;
                    error += "\n\nException details:\n\n";
                    error += e.Message;
                    MessageBox.Show(error);
                    return;
                }

                // At this point, there is a non-null FlowDocument loaded into content.  
                FlowDocRdr.Document = content;
            }
        }

        private void SaveFile(object sender, RoutedEventArgs args)
        {
            var saveFile = new SaveFileDialog();
            FileStream xamlFile = null;
            saveFile.Filter = "FlowDocument Files (*.xaml)|*.xaml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog().HasValue)
            {
                try
                {
                    xamlFile = saveFile.OpenFile() as FileStream;
                }
                catch (Exception e)
                {
                    var error = "There was a opening the file:\n\n";
                    error += saveFile.FileName;
                    error += "\n\nException details:\n\n";
                    error += e.Message;
                    MessageBox.Show(error);
                    return;
                }
                if (xamlFile == null) return;
                XamlWriter.Save(FlowDocRdr.Document, xamlFile);
                xamlFile.Close();
            }
        }

        private void Clear(object sender, RoutedEventArgs args)
        {
            FlowDocRdr.Document = null;
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Close();
        }
    }
}