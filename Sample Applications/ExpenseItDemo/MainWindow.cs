// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ExpenseItDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow()
        {
            // Define CreateExpenseReportCommand
            CreateExpenseReportCommand = new RoutedUICommand("_Create Expense Report...", "CreateExpenseReport",
                typeof (MainWindow));
            CreateExpenseReportCommand.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift));

            // Define ExitCommand
            ExitCommand = new RoutedUICommand("E_xit", "Exit", typeof (MainWindow));

            // Define AboutCommand
            AboutCommand = new RoutedUICommand("_About ExpenseIt Standalone", "About", typeof (MainWindow));
        }

        public MainWindow()
        {
            Initialized += MainWindow_Initialized;

            InitializeComponent();

            employeeTypeRadioButtons.SelectionChanged += employeeTypeRadioButtons_SelectionChanged;

            // Bind CreateExpenseReportCommand
            var commandBindingCreateExpenseReport = new CommandBinding(CreateExpenseReportCommand);
            commandBindingCreateExpenseReport.Executed += commandBindingCreateExpenseReport_Executed;
            CommandBindings.Add(commandBindingCreateExpenseReport);

            // Bind ExitCommand
            var commandBindingExitCommand = new CommandBinding(ExitCommand);
            commandBindingExitCommand.Executed += commandBindingExitCommand_Executed;
            CommandBindings.Add(commandBindingExitCommand);

            // Bind AboutCommand
            var commandBindingAboutCommand = new CommandBinding(AboutCommand);
            commandBindingAboutCommand.Executed += commandBindingAboutCommand_Executed;
            CommandBindings.Add(commandBindingAboutCommand);
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            // Select the first employee type radio button
            employeeTypeRadioButtons.SelectedIndex = 0;
            RefreshEmployeeList();
        }

        private void commandBindingCreateExpenseReport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new CreateExpenseReportDialogBox {Owner = this};
            dlg.ShowDialog();
        }

        private void commandBindingExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void commandBindingAboutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(
                "ExpenseIt Standalone Sample Application, by the WPF SDK",
                "ExpenseIt Standalone",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void employeeTypeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshEmployeeList();
        }

        /// <summary>
        ///     Select the employees who have the employment type that is specified
        ///     by the currently checked employee type radio button
        /// </summary>
        private void RefreshEmployeeList()
        {
            var selectedItem = (ListBoxItem) employeeTypeRadioButtons.SelectedItem;

            // Get employees data source
            var employeesDataSrc = (XmlDataProvider) FindResource("Employees");

            // Select the employees who have of the specified employment type
            var query = string.Format(CultureInfo.InvariantCulture, "/Employees/Employee[@Type='{0}']",
                selectedItem.Content);
            employeesDataSrc.XPath = query;

            // Apply the selection
            employeesDataSrc.Refresh();
        }

        #region Commands

        public static RoutedUICommand CreateExpenseReportCommand;
        public static RoutedUICommand ExitCommand;
        public static RoutedUICommand AboutCommand;

        #endregion
    }
}