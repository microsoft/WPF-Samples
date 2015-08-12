// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HandlingEventOnCommandUsingCode
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Creating the main panel
            var mainStackPanel = new StackPanel();
            AddChild(mainStackPanel);

            // Button used to invoke the command
            var commandButton = new Button
            {
                Command = ApplicationCommands.Open,
                Content = "Open (KeyBindings: Ctrl-R, Ctrl-0)"
            };
            mainStackPanel.Children.Add(commandButton);

            // Creating CommandBinding and attaching an Executed and CanExecute handler
            var openCmdBinding = new CommandBinding(
                ApplicationCommands.Open,
                OpenCmdExecuted,
                OpenCmdCanExecute);

            CommandBindings.Add(openCmdBinding);

            // Creating a KeyBinding between the Open command and Ctrl-R
            var openCmdKeyBinding = new KeyBinding(
                ApplicationCommands.Open,
                Key.R,
                ModifierKeys.Control);

            InputBindings.Add(openCmdKeyBinding);
        }

        private void OpenCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("The command has been invoked.");
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}