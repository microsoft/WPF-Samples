// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;

namespace ExceptionHandlingSecondaryUIThread
{
    /// <summary>
    ///     Interaction logic for SecondaryUIThreadWindow.xaml
    /// </summary>
    public partial class SecondaryUIThreadWindow : Window
    {
        public SecondaryUIThreadWindow()
        {
            InitializeComponent();
            Title = $"Running on Secondary UI Thread {Thread.CurrentThread.ManagedThreadId}";
        }

        private void raiseExceptionOnSecondaryUIThreadButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise an exception on the secondary UI thread
            string msg = $"Exception raised on secondary UI thread {Dispatcher.Thread.ManagedThreadId}.";
            throw new Exception(msg);
        }

        private void SecondaryUIThreadWindow_OnClosed(object sender, EventArgs e)
        {
            // End this thread of execution
            Dispatcher.InvokeShutdown();
        }
    }
}