// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace StringLocalizationSample
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

        private void messageBoxButton_Click(object sender, RoutedEventArgs e)
        {
            // Programmatic use of string resource from StringResources.xaml resource dictionary
            var localizedMessage = (string) Application.Current.FindResource("LocalizedMessage");
            MessageBox.Show(localizedMessage);
        }
    }
}
