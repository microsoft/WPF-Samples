// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace MessageBox
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

        private void ShowMessageBoxButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure the message box
            Window owner = ((bool) ownerCheckBox.IsChecked ? this : null);
            var messageBoxText = this.messageBoxText.Text;
            var caption = this.caption.Text;
            var button = (MessageBoxButton) Enum.Parse(typeof (MessageBoxButton), buttonComboBox.Text);
            var icon = (MessageBoxImage) Enum.Parse(typeof (MessageBoxImage), imageComboBox.Text);
            var defaultResult =
                (MessageBoxResult) Enum.Parse(typeof (MessageBoxResult), defaultResultComboBox.Text);
            var options = (MessageBoxOptions) Enum.Parse(typeof (MessageBoxOptions), optionsComboBox.Text);

            // Show message box, passing the window owner if specified
            MessageBoxResult result;
            if (owner == null)
            {
                result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
            }
            else
            {
                result = System.Windows.MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult,
                    options);
            }

            // Show the result
            resultTextBlock.Text = "Result = " + result;
        }
    }
}