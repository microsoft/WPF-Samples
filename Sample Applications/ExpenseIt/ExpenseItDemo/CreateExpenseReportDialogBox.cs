// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ExpenseItDemo
{
    /// <summary>
    ///     Interaction logic for CreateExpenseReportDialogBox.xaml
    /// </summary>
    public partial class CreateExpenseReportDialogBox : Window
    {
        public CreateExpenseReportDialogBox()
        {
            InitializeComponent();
        }

        private void addExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            var expenseReport = (ExpenseReport) app.FindResource("ExpenseData");
            expenseReport?.LineItems.Add(new LineItem());
            
            DataGridRow row =null;

            // Dispatching this at loaded priority so the new row has been added before our code runs
            // Grab the last row in the datagrid and search up the visual tree to get the DataGridCellsPresenter for the first cell in the last row
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
            {
                row = expenseDataGrid1.ItemContainerGenerator.ContainerFromIndex(expenseDataGrid1.Items.Count - 1) as DataGridRow;
                if (row != null)
                {
                    expenseDataGrid1.SelectedItem = row.DataContext;
                    DataGridCell cell = GetCell(expenseDataGrid1, row, 0);
                    if (cell != null)
                    {
                        expenseDataGrid1.CurrentCell = new DataGridCellInfo(cell);
                        Keyboard.Focus(cell);
                    }
                }
            }));
        }

        private static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = RecursiveVisualChildFinder(rowContainer);
                if (presenter != null)
                    return presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
            }
            return null;
        }

        private static DataGridCellsPresenter RecursiveVisualChildFinder(DependencyObject myObj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myObj); i++)
            {
                // Retrieve child visual at specified index value.
                DependencyObject childObj = VisualTreeHelper.GetChild(myObj, i);

                // Do processing of the child visual object.
                if(childObj is DataGridCellsPresenter)
                {
                    return (DataGridCellsPresenter)childObj;
                }
                // Enumerate children of the child visual object.
                return RecursiveVisualChildFinder(childObj);
            }
            return null;
        }

        private void viewChartButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ViewChartWindow {Owner = this};
            dlg.Show();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Expense Report Created!",
                "ExpenseIt Standalone",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}