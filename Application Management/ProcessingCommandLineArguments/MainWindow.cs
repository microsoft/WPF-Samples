// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace ProcessingCommandLineArguments
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (string key in App.CommandLineArgs.Keys)
            {
                commandLineArgsListBox.Items.Add(key + ": " + App.CommandLineArgs[key]);
            }
        }
    }
}