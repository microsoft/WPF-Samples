// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace SingleInstanceDetection
{
    public class SingleInstanceApplication : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and show the application's main window
            var window = new MainWindow();
            window.Show();
        }

        public void Activate()
        {
            // Reactivate application's main window
            MainWindow.Activate();
        }
    }
}