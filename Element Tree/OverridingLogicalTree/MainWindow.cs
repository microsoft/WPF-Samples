// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;

namespace OverridingLogicalTree
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

        private void AddLogicalElement(object sender, RoutedEventArgs e)
        {
            var mybutton = new Button {Content = "Happy birthday! " + DateTime.Now.TimeOfDay};
            CustomElement.SetSingleChild(mybutton);
        }
    }
}