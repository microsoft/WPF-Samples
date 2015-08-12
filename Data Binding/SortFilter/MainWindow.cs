// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SortFilter
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ListCollectionView MyCollectionView;
        // Object o keeps the currency for the table
        public Order O;

        public MainWindow()
        {
            InitializeComponent();
        }

        public bool Contains(object de)
        {
            var order = de as Order;
            //Return members whose Orders have not been filled
            return (order?.Filled == "No");
        }

        public void StartHere(object sender, DependencyPropertyChangedEventArgs args)
        {
            MyCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(rootElement.DataContext);
        }

        private void OnClick(object sender, RoutedEventArgs args)
        {
            var button = sender as Button;
            //Sort the data based on the column selected
            MyCollectionView.SortDescriptions.Clear();
            switch (button.Name)
            {
                case "orderButton":
                    MyCollectionView.SortDescriptions.Add(new SortDescription("OrderItem",
                        ListSortDirection.Ascending));
                    break;
                case "customerButton":
                    MyCollectionView.SortDescriptions.Add(new SortDescription("Customer",
                        ListSortDirection.Ascending));
                    break;
                case "nameButton":
                    MyCollectionView.SortDescriptions.Add(new SortDescription("Name",
                        ListSortDirection.Ascending));
                    break;
                case "idButton":
                    MyCollectionView.SortDescriptions.Add(new SortDescription("Id",
                        ListSortDirection.Ascending));
                    break;
                case "filledButton":
                    MyCollectionView.SortDescriptions.Add(new SortDescription("Filled",
                        ListSortDirection.Ascending));
                    break;
            }
        }

        //OnBrowse is called whenever the Next or Previous buttons
        //are clicked to change the currency
        private void OnBrowse(object sender, RoutedEventArgs args)
        {
            var b = sender as Button;
            switch (b.Name)
            {
                case "Previous":
                    if (MyCollectionView.MoveCurrentToPrevious())
                    {
                        feedbackText.Text = "";
                        O = MyCollectionView.CurrentItem as Order;
                    }
                    else
                    {
                        MyCollectionView.MoveCurrentToFirst();
                        feedbackText.Text = "At first record";
                    }
                    break;
                case "Next":
                    if (MyCollectionView.MoveCurrentToNext())
                    {
                        feedbackText.Text = "";
                        O = MyCollectionView.CurrentItem as Order;
                    }
                    else
                    {
                        MyCollectionView.MoveCurrentToLast();
                        feedbackText.Text = "At last record";
                    }
                    break;
            }
        }

        //OnButton is called whenever the Next or Previous buttons
        //are clicked to change the currency
        private void OnFilter(object sender, RoutedEventArgs args)
        {
            var b = sender as Button;
            switch (b.Name)
            {
                case "Filter":
                    MyCollectionView.Filter = Contains;
                    break;
                case "Unfilter":
                    MyCollectionView.Filter = null;
                    break;
            }
        }
    }
}