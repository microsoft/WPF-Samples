// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Input;

namespace HandlingEventOnCommand
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

        private void OpenCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            string command, targetobj;
            command = ((RoutedCommand) e.Command).Name;
            targetobj = ((FrameworkElement) target).Name;
            MessageBox.Show("The " + command + " command has been invoked on target object " + targetobj);
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}