// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using WPFWithWFAndDatabinding.NorthwindDataSetTableAdapters;

namespace WPFWithWFAndDatabinding
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomersTableAdapter _customersTableAdapter =
            new CustomersTableAdapter();

        private readonly BindingSource _nwBindingSource;
        private readonly NorthwindDataSet _nwDataSet;

        private readonly OrdersTableAdapter _ordersTableAdapter =
            new OrdersTableAdapter();

        public MainWindow()
        {
            InitializeComponent();

            // Create a DataSet for the Customers data.
            _nwDataSet = new NorthwindDataSet {DataSetName = "nwDataSet"};

            // Create a BindingSource for the Customers data.
            _nwBindingSource = new BindingSource
            {
                DataMember = "Customers",
                DataSource = _nwDataSet
            };
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Fill the Customers table adapter with data.
            _customersTableAdapter.ClearBeforeFill = true;
            _customersTableAdapter.Fill(_nwDataSet.Customers);

            // Fill the Orders table adapter with data.
            _ordersTableAdapter.Fill(_nwDataSet.Orders);

            // Assign the BindingSource to 
            // the data context of the main grid.
            mainGrid.DataContext = _nwBindingSource;

            // Assign the BindingSource to 
            // the data source of the list box.
            listBox1.ItemsSource = _nwBindingSource;

            // Because this is a master/details form, the DataGridView
            // requires the foreign key relating the tables.
            dataGridView1.DataSource = _nwBindingSource;
            dataGridView1.DataMember = "FK_Orders_Customers";

            // Handle the currency management aspect of the data models.
            // Attach an event handler to detect when the current item 
            // changes via the WPF ListBox. This event handler synchronizes
            // the list collection with the BindingSource.
            //

            var cv = CollectionViewSource.GetDefaultView(
                _nwBindingSource) as BindingListCollectionView;

            cv.CurrentChanged += WPF_CurrentChanged;
        }

        // This event handler updates the current item 
        // of the data binding.
        private void WPF_CurrentChanged(object sender, EventArgs e)
        {
            var cv = sender as BindingListCollectionView;
            _nwBindingSource.Position = cv.CurrentPosition;
        }
    }
}