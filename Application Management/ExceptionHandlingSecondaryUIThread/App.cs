// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace ExceptionHandlingSecondaryUIThread
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Display exception message
            var sb = new StringBuilder();
            sb.AppendFormat("{0}\n", e.Exception.InnerException.Message);
            sb.AppendFormat("{0}\n", e.Exception.Message);
            sb.AppendFormat("Exception handled on main UI thread {0}.", e.Dispatcher.Thread.ManagedThreadId);
            MessageBox.Show(sb.ToString());

            // Keep application running in the face of this exception
            e.Handled = true;
        }
    }
}