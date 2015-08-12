// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace DragDropTextOps
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

        private void TextSelectionToDataObject(object sender, RoutedEventArgs e)
        {
            // Create a new data object using one of the overloaded constructors.  This particular
            // overload accepts a string specifying the data format (provided by the DataFormats class),
            // and an Object (in this case a string) that represents the data to be stored in the data object.
            var dataObject = new DataObject(DataFormats.Text, sourceTextBox.SelectedText);

            dataObjectInfoTextBox.Clear();

            // Get and display the native data formats (filtering out auto-convertable data formats).
            dataObjectInfoTextBox.Text = "\nNative data formats present:\n";
            foreach (var format in dataObject.GetFormats(false /*autoconvert*/))
                dataObjectInfoTextBox.Text += format + "\n";

            // Display the data in the data object.
            dataObjectInfoTextBox.Text += "\nData contents:\n";
            dataObjectInfoTextBox.Text += dataObject.GetData(DataFormats.Text, false /*autoconvert*/).ToString();
        }
    }
}