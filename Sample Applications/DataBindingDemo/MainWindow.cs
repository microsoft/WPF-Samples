// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DataBindingDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CollectionViewSource _listingDataView;

        public MainWindow()
        {
            InitializeComponent();
            _listingDataView = (CollectionViewSource) (Resources["ListingDataView"]);
        }

        private void OpenAddProductWindow(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
        }

        private void ShowOnlyBargainsFilter(object sender, FilterEventArgs e)
        {
            var product = e.Item as AuctionItem;
            if (product != null)
            {
                // Filter out products with price 25 or above
                e.Accepted = product.CurrentPrice < 25;
            }
        }

        private void AddGrouping(object sender, RoutedEventArgs args)
        {
            // This groups the items in the view by the property "Category"
            var groupDescription = new PropertyGroupDescription {PropertyName = "Category"};
            _listingDataView.GroupDescriptions.Add(groupDescription);
        }

        private void RemoveGrouping(object sender, RoutedEventArgs args)
        {
            _listingDataView.GroupDescriptions.Clear();
        }

        private void AddSorting(object sender, RoutedEventArgs args)
        {
            // This sorts the items first by Category and within each Category,
            // by StartDate. Notice that because Category is an enumeration,
            // the order of the items is the same as in the enumeration declaration
            _listingDataView.SortDescriptions.Add(
                new SortDescription("Category", ListSortDirection.Ascending));
            _listingDataView.SortDescriptions.Add(
                new SortDescription("StartDate", ListSortDirection.Ascending));
        }

        private void RemoveSorting(object sender, RoutedEventArgs args)
        {
            _listingDataView.SortDescriptions.Clear();
        }

        private void AddFiltering(object sender, RoutedEventArgs args)
        {
            _listingDataView.Filter += ShowOnlyBargainsFilter;
        }

        private void RemoveFiltering(object sender, RoutedEventArgs args)
        {
            _listingDataView.Filter -= ShowOnlyBargainsFilter;
        }
    }
}