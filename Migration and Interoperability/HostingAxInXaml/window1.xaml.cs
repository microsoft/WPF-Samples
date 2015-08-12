// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace HostingAxInWpfWithXaml
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Get the AxHost wrapper from the WindowsFormsHost control.
            AxWMPLib.AxWindowsMediaPlayer axWmp =
                wfh.Child as AxWMPLib.AxWindowsMediaPlayer;

            // Play a .wav file with the ActiveX control.
            axWmp.URL = @"C:\WINDOWS\Media\Windows XP Startup.wav";
        }
    }
}

//</snippet10>