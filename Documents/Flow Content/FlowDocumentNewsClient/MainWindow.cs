// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlowDocumentNewsClient
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Application _app;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void MenuExit(object sender, RoutedEventArgs args)
        {
            _app = Application.Current;
            _app.Shutdown();
        }

        public void PrintPage(object sender, RoutedEventArgs args)
        {
            var pDialog = new PrintDialog();
            pDialog.ShowDialog();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            todayDate.Text += DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        private void Nav1(object sender, MouseButtonEventArgs e)
        {
            frame1.Source = new Uri("document.xaml", UriKind.Relative);
        }

        private void Nav2(object sender, MouseButtonEventArgs e)
        {
            frame1.Source = new Uri("document1.xaml", UriKind.Relative);
        }

        private void Nav3(object sender, MouseButtonEventArgs e)
        {
            frame1.Source = new Uri("document2.xaml", UriKind.Relative);
        }
    }
}