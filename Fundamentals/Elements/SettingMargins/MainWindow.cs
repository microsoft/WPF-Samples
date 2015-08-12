// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace SettingMargins
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

        private void OnClick(object sender, RoutedEventArgs e)
        {
            // Get the current value of the property.
            var marginThickness = btn1.Margin;
            // If the current leftlength value of margin is set to 10 then change it to a new value.
            // Otherwise change it back to 10.
            btn1.Margin = marginThickness.Left == 10 ? new Thickness(60) : new Thickness(10);
        }
    }
}