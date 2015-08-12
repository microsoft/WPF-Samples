// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace DataBindingDemo
{
    /// <summary>
    ///     Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void OnInit(object sender, RoutedEventArgs e)
        {
            DataContext = new AuctionItem("Type your description here",
                ProductCategory.DvDs, 1, DateTime.Now, ((App) Application.Current).CurrentUser,
                SpecialFeatures.None);
        }

        private void SubmitProduct(object sender, RoutedEventArgs e)
        {
            var item = (AuctionItem) (DataContext);
            ((App) Application.Current).AuctionItems.Add(item);
            Close();
        }
    }
}