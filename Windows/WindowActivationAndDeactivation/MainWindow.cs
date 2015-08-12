// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows;

namespace WindowActivationAndDeactivation
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isMediaElementPlaying;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Start media player
            mediaElement.Play();
            _isMediaElementPlaying = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop media player
            mediaElement.Stop();
            _isMediaElementPlaying = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // Recommence playing media if window is activated
            if (_isMediaElementPlaying) mediaElement.Play();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            // Pause playing if media is being played and window is deactivated
            if (_isMediaElementPlaying) mediaElement.Pause();
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Ask user if they want to close the window
            if (_isMediaElementPlaying)
            {
                var msg = "Media is playing. Really close?";
                var title = "Custom Media Player?";
                var buttons = MessageBoxButton.YesNo;
                var icon = MessageBoxImage.Warning;

                // Show message box and get user's answer
                var result =
                    MessageBox.Show(msg, title, buttons, icon);

                // Don't close window if user clicked No
                e.Cancel = (result == MessageBoxResult.No);
            }
        }
    }
}