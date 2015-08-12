// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace SkinnedApplication
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add choices to combo box
            skinComboBox.Items.Add("Blue");
            skinComboBox.Items.Add("Yellow");
            skinComboBox.SelectedIndex = 0;

            // Set initial skin
            Application.Current.Resources = (ResourceDictionary) Application.Current.Properties["Blue"];

            // Detect when skin changes
            skinComboBox.SelectionChanged += skinComboBox_SelectionChanged;
        }

        private void newChildWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new skind child window
            var window = new ChildWindow();
            window.Show();
        }

        private void skinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change the skin
            var selectedValue = (string) e.AddedItems[0];
            Application.Current.Resources = (ResourceDictionary) Application.Current.Properties[selectedValue];
        }
    }
}