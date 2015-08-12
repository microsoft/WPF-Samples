// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TableBuilder
{
    public class App : Application
    {
        private Window _mainWindow;
        private Table _table1;
        private FlowDocumentScrollViewer _tf1;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateAndShowMainWindow();
        }

        private void CreateAndShowMainWindow()
        {
            // Create the application's main window
            _mainWindow = new Window();

            // Create the parent viewer...
            _tf1 = new FlowDocumentScrollViewer {Document = new FlowDocument()};

            // Create the Table...
            _table1 = new Table();
            // ...and add it as a content element of the TextFlow.
            _tf1.Document.Blocks.Add(_table1);
            // tf1.ContentStart.InsertTextElement(table1);

            // Set some global formatting properties for the table.
            _table1.CellSpacing = 10;
            _table1.Background = Brushes.White;

            // Create 6 columns and add them to the table's Columns collection.
            var numberOfColumns = 6;
            for (var x = 0; x < numberOfColumns; x++) _table1.Columns.Add(new TableColumn());

            // Set alternating background colors for the middle colums.
            _table1.Columns[1].Background =
                _table1.Columns[3].Background =
                    Brushes.LightSteelBlue;
            _table1.Columns[2].Background =
                _table1.Columns[4].Background =
                    Brushes.Beige;

            // Create and add an empty TableRowGroup to hold the table's Rows.
            _table1.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            _table1.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            var currentRow = _table1.RowGroups[0].Rows[0];

            // Global formatting for the title row.
            currentRow.Background = Brushes.Silver;
            currentRow.FontSize = 40;
            currentRow.FontWeight = FontWeights.Bold;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("2004 Sales Project"))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 6;

            // Add the second (header) row.
            _table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = _table1.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Product"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 1"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 2"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 3"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 4"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TOTAL"))));

            _table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = _table1.RowGroups[0].Rows[2];

            // Global formatting for the row.
            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Normal;

            // Add cells with content to the third row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Widgets"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$50,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$55,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$60,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$65,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$230,000"))));

            // Bold the first cell.
            currentRow.Cells[0].FontWeight = FontWeights.Bold;

            // Add the fourth row.
            _table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = _table1.RowGroups[0].Rows[3];

            // Global formatting for the row.
            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Normal;

            // Add cells with content to the third row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Wickets"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$100,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$120,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$160,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$200,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$580,000"))));

            // Bold the first cell.
            currentRow.Cells[0].FontWeight = FontWeights.Bold;

            _table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = _table1.RowGroups[0].Rows[4];

            // Global formatting for the footer row.
            currentRow.Background = Brushes.LightGray;
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Normal;

            // Add the header row with content, 
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Projected 2004 Revenue: $810,000"))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 6;

            _mainWindow.Title = "Table Sample";
            _mainWindow.Content = _tf1;
            _mainWindow.Show();
        }
    }
}