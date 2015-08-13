// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MultiThreadingWebBrowser
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            placeHolder.Source = new Uri("http://www.msn.com");
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            placeHolder.Source = new Uri(newLocation.Text);
        }

        private void NewWindowHandler(object sender, RoutedEventArgs e)
        {
            var newWindowThread = new Thread(ThreadStartingPoint);
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private void ThreadStartingPoint()
        {
            var tempWindow = new MainWindow();
            tempWindow.Show();
            Dispatcher.Run();
        }
    }
}