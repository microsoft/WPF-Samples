// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace LocalAnimations
{
    // Displays the sample.
    public class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateAndShowMainWindow();
        }

        private void CreateAndShowMainWindow()
        {
            // Create the application's main window.
            Window sViewer = new SampleViewer();
            MainWindow = sViewer;
            sViewer.Show();
        }
    }

    // Starts the application.
}