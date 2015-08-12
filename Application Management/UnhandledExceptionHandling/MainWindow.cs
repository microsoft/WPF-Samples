// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace UnhandledExceptionHandling
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

        private void raiseRecoverableException_Click(object sender, RoutedEventArgs e)
        {
            throw new DivideByZeroException("Recoverable Exception");
        }

        private void raiseUnecoverableException_Click(object sender, RoutedEventArgs e)
        {
            throw new ArgumentNullException(@"Unrecoverable Exception");
        }
    }
}