// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Documents;

namespace TableRows
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

        public void AddRow(object sender, RoutedEventArgs e)
        {
            var row = new TableRow();
            trg1.Rows.Add(row);
            var para = new Paragraph();
            para.Inlines.Add("A new Row and Cell have been Added to the Table");
            var cell = new TableCell(para);
            row.Cells.Add(cell);
        }
    }
}