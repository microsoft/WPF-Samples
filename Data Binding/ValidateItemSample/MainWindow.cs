// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace ValidateItemSample
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

        private void StackPanel1_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the DataContext to a PurchaseItem object.
            // The BindingGroup and Binding objects use this as
            // the source.
            stackPanel1.DataContext = new PurchaseItem();

            // Begin an edit transaction that enables
            // the object to accept or roll back changes.
            stackPanel1.BindingGroup.BeginEdit();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (stackPanel1.BindingGroup.CommitEdit())
            {
                MessageBox.Show("Item submitted");
                stackPanel1.BindingGroup.BeginEdit();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the pending changes and begin a new edit transaction.
            stackPanel1.BindingGroup.CancelEdit();
            stackPanel1.BindingGroup.BeginEdit();
        }

        // This event occurs when a ValidationRule in the BindingGroup
        // or in a Binding fails.
        private void ItemError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }
    }
}