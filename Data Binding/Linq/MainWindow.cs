// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Linq
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Tasks tasks = new Tasks();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pri = int.Parse(((sender as ListBox).SelectedItem as ListBoxItem).Content.ToString());

            DataContext = from task in tasks
                where task.Priority == pri
                select task;
        }
    }
}