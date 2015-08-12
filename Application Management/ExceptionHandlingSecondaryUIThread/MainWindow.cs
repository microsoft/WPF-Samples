// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ExceptionHandlingSecondaryUIThread
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = $"Running on Main UI Thread {Thread.CurrentThread.ManagedThreadId}";
        }

        // THIS EVENT HANDLER RUNS ON THE MAIN UI THREAD
        private void startSecondaryUIThreadButton_Click(object sender, RoutedEventArgs e)
        {
            // Creates and starts a secondary thread in a single threaded apartment (STA)
            var thread = new Thread(MethodRunningOnSecondaryUIThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        // THIS METHOD RUNS ON A SECONDARY UI THREAD (THREAD WITH A DISPATCHER)
        private void MethodRunningOnSecondaryUIThread()
        {
            var secondaryUiThreadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                // On secondary thread, show a new Window before starting a new Dispatcher
                // ie turn secondary thread into a UI thread
                var window = new SecondaryUIThreadWindow();
                window.Show();
                Dispatcher.Run();
            }
            catch (Exception ex)
            {
                // Dispatch the exception back to the main ui thread and reraise it
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Send,
                    (DispatcherOperationCallback) delegate
                    {
                        // THIS CODE RUNS BACK ON THE MAIN UI THREAD
                        string msg = $"Exception forwarded from secondary UI thread {secondaryUiThreadId}.";
                        throw new Exception(msg, ex);
                    }
                    , null);

                // NOTE - Application execution will only continue from this point
                //        onwards if the exception was handled on the main UI thread
                //        by Application.DispatcherUnhandledException
            }
        }
    }
}