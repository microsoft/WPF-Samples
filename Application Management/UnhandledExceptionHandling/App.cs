// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace UnhandledExceptionHandling
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            var shutdown = false;

            // Process exception
            if (e.Exception is DivideByZeroException)
            {
                // Recoverable - continue processing
                shutdown = false;
            }
            else if (e.Exception is ArgumentNullException)
            {
                // Unrecoverable - end processing
                shutdown = true;
            }

            if (shutdown)
            {
                // If unrecoverable, attempt to save data
                var result =
                    MessageBox.Show("Application must exit:\n\n" + e.Exception.Message + "\n\nSave before exit?", "app",
                        MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    // Save data
                }

                // Add entry to event log
                EventLog.WriteEntry("app", "Unrecoverable Exception: " + e.Exception.Message, EventLogEntryType.Error);

                // Return exit code
                Shutdown(-1);
            }

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}