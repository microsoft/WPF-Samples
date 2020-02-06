// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

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

        private void expenseDataGrid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Type")
            {
                e.Column.Header = "Expense Type";
            }
            if (e.Column.Header.ToString() == "Cost")
            {
                e.Column.HeaderStyle = FindResource("ColumnHeaderStyle2") as Style;
                e.Column.CellStyle = FindResource("CostColumnStyle") as Style;
            }
        }
    }
}