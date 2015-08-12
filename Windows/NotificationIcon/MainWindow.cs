// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace NotificationIcon
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            // Configure and show a notification icon in the system tray
            _notifyIcon = new NotifyIcon
            {
                BalloonTipText = @"Hello, NotifyIcon!",
                Text = @"Hello, NotifyIcon!",
                Icon = new Icon("NotifyIcon.ico"),
                Visible = true
            };
            _notifyIcon.ShowBalloonTip(1000);
        }
    }
}