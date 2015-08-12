// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Windows;

namespace WPFHostingWin32Control
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("comctl32.dll", EntryPoint = "InitCommonControls", CharSet = CharSet.Auto)]
        public static extern void InitCommonControls();

        private void ApplicationStartup(object sender, StartupEventArgs args)
        {
        }
    }
}