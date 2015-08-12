// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace EditingCollections
{
    /// <summary>
    ///     Interaction logic for ChangeItem.xaml
    /// </summary>
    public partial class ChangeItem : Window
    {
        public ChangeItem()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}