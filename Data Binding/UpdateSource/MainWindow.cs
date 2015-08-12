// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace UpdateSource
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

        private void Preview(object sender, RoutedEventArgs args)
        {
            // itemNameTextBox is an instance of a TextBox
            var be = itemNameTextBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();

            be = bidPriceTextBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();

            userdata.Opacity = 1;
            Finish.Opacity = 1;
        }

        private void Submit(object sender, RoutedEventArgs args)
        {
            var userProfile = RootElem.DataContext as UserProfile;
            MessageBox.Show("Thank you for your bid of " + userProfile?.BidPrice
                            + " on item " + userProfile?.ItemName);
            userdata.Opacity = 0;
            Finish.Opacity = 0;
        }
    }
}