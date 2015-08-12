// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace HostingAxInWpfWithXaml
{
    /// <summary>
    ///     Interaction logic for app.xaml
    /// </summary>
    public partial class app : Application
    {
        private void AppStartup(object sender, StartupEventArgs args)
        {
            Window1 mainWindow = new Window1();
            mainWindow.Show();
        }
    }
}