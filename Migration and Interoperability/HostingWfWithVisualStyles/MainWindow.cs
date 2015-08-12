// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Application = System.Windows.Forms.Application;

namespace HostingWfWithVisualStyles
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Comment out the following line to disable visual
            // styles for the hosted Windows Forms control.
            Application.EnableVisualStyles();

            // Create a WindowsFormsHost element to host
            // the Windows Forms control.
            var host =
                new WindowsFormsHost();

            // Create a Windows Forms tab control.
            var tc = new TabControl();
            tc.TabPages.Add("Tab1");
            tc.TabPages.Add("Tab2");

            // Assign the Windows Forms tab control as the hosted control.
            host.Child = tc;

            // Assign the host element to the parent Grid element.
            grid1.Children.Add(host);
        }
    }
}