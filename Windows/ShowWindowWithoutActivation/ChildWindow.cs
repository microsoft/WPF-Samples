// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace ShowWindowWithoutActivation
{
    /// <summary>
    ///     Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        public ChildWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            activationStatusTextBlock.Text = "Activated";
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            activationStatusTextBlock.Text = "Deactivated";
        }
    }
}