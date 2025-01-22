// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
            double epsilon = 0.0001; // Define a small value for precision
            // Get the current value of the property.
            var marginThickness = btn1.Margin;
            // If the current left length value of margin is set to 10 then change it to a new value.
            // Otherwise change it back to 10.
            btn1.Margin = Math.Abs(marginThickness.Left - 10) < epsilon ? new Thickness(60) : new Thickness(10);
        }
    }
}