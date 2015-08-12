// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace WindowHiding
{
    /// <summary>
    ///     Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        private bool _close;

        public ChildWindow()
        {
            InitializeComponent();

            // Get date/time when window is first shown
            firstShownTextBlock.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        // Shadow Window.Close to make sure we bypass the Hide call in 
        // the Closing event handler
        public new void Close()
        {
            _close = true;
            base.Close();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Get date/time the window is shown now (ie when it becomes visible)
            if ((bool) e.NewValue)
            {
                shownThisTimeTextBlock.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // If Close() was called, close the window (instead of hiding it)
            if (_close) return;

            // Hide the window (instead of closing it)
            e.Cancel = true;
            Hide();
        }
    }
}