// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Data;

namespace Grouping
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionView _myView;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddGrouping(object sender, RoutedEventArgs e)
        {
            _myView = (CollectionView) CollectionViewSource.GetDefaultView(myItemsControl.ItemsSource);
            if (_myView.CanGroup)
            {
                var groupDescription
                    = new PropertyGroupDescription("@Type");
                _myView.GroupDescriptions.Add(groupDescription);
            }
        }

        private void RemoveGrouping(object sender, RoutedEventArgs e)
        {
            _myView = (CollectionView) CollectionViewSource.GetDefaultView(myItemsControl.ItemsSource);
            _myView.GroupDescriptions.Clear();
        }
    }
}