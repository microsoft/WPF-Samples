// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace ExpenseItDemo
{
    /// <summary>
    ///     Interaction logic for ViewChartWindow.xaml
    /// </summary>
    public partial class ViewChartWindow : Window
    {
        public ViewChartWindow()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}