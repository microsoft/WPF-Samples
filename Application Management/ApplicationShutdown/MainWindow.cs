// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationShutdown
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            shutdownModeListBox.Items.Add("OnLastWindowClose");
            shutdownModeListBox.Items.Add("OnExplicitShutdown");
            shutdownModeListBox.Items.Add("OnMainWindowClose");
            shutdownModeListBox.SelectedValue = "OnLastWindowClose";
            shutdownModeListBox.SelectionChanged +=
                shutdownModeListBox_SelectionChanged;
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        private void shutdownModeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.ShutdownMode =
                (ShutdownMode) Enum.Parse(typeof (ShutdownMode), shutdownModeListBox.SelectedValue.ToString());
        }

        private void newWindowButton_Click(object sender, RoutedEventArgs e)
        {
            (new ChildWindow()).Show();
        }

        private void explicitShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            var exitCode = 0;
            int.TryParse(appExitCodeTextBox.Text, out exitCode);
            Application.Current.Shutdown(exitCode);
        }
    }
}