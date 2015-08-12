// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Colors
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

        private void OnNewColorClicked(object sender, RoutedEventArgs args)
        {
            var button = (Button) sender;
            var colorList = (ColorItemList) button.DataContext;
            var cv = (CollectionView) CollectionViewSource.GetDefaultView(colorList);

            // add a new color based on the current one, then select the new one
            var newItem = new ColorItem((ColorItem) cv.CurrentItem);
            colorList.Add(newItem);
            cv.MoveCurrentTo(newItem);
        }

        // Event handler for the SortBy radio buttons
        private void OnSortByChanged(object sender, RoutedEventArgs args)
        {
            var rb = (RadioButton) sender;
            var sortBy = rb.Content.ToString();

            if (sortBy != null)
            {
                // sort by the user's chosen property
                var cv = (CollectionView) CollectionViewSource.GetDefaultView((IEnumerable) rb.DataContext);
                cv.SortDescriptions.Clear();
                cv.SortDescriptions.Add(new SortDescription(sortBy, ListSortDirection.Descending));
            }
        }
    }
}