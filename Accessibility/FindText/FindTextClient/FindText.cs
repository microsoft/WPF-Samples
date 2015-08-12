// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace FindTextClient
{
    public class FindText : Application
    {
        /// <summary>
        ///     Handles our application startup.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialize the sample
            new SearchWindow();
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        // Launch the sample application.
        internal sealed class TestMain
        {
            [STAThread]
            private static void Main()
            {
                // Create an instance of the sample class and call its Run() method to start it.
                var app = new FindText();
                app.Run();
            }
        }
    }
}