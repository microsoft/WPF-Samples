// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ExceptionHandlingSecondaryWorkerThread
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

        private void startSecondaryWorkerThreadButton_Click(object sender, RoutedEventArgs e)
        {
            // Creates and starts a secondary thread in a single threaded apartment (STA)
            var thread = new Thread(MethodRunningOnSecondaryWorkerThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        // THIS METHOD RUNS ON A SECONDARY WORKER THREAD (THREAD WITHOUT A DISPATCHER)
        private void MethodRunningOnSecondaryWorkerThread()
        {
            try
            {
                WorkerMethod();
            }
            catch (Exception ex)
            {
                // Dispatch the exception back to the main UI thread. Then, reraise
                // the exception on the main UI thread and handle it from the handler 
                // the Application object's DispatcherUnhandledException event.
                var secondaryWorkerThreadId = Thread.CurrentThread.ManagedThreadId;
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Send,
                    (DispatcherOperationCallback) delegate
                    {
                        // THIS CODE RUNS BACK ON THE MAIN UI THREAD
                        string msg = $"Exception forwarded from secondary worker thread {secondaryWorkerThreadId}.";
                        throw new Exception(msg, ex);
                    }
                    , null);

                // NOTE - Application execution will only continue from this point
                //        onwards if the exception was handled on the main UI thread.
                //        by Application.DispatcherUnhandledException
            }
        }

        private void WorkerMethod()
        {
            // This method would do real processing on the secondary worker thread.
            // For the purposes of this sample, it throws an exception
            string msg =
                $"Exception raised secondary on worker thread {Dispatcher.CurrentDispatcher.Thread.ManagedThreadId}.";
            throw new Exception(msg);
        }
    }
}