// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MouseState
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

        private void HandleButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Casting the source to a StackPanel
            var sourceStackPanel = e.Source as StackPanel;

            //If the button is pressed then make dimensions larger.
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                sourceStackPanel.Width = 200;
                sourceStackPanel.Height = 200;
            }

            //If the button is released then make dimensions smaller.
            else if (e.ButtonState == MouseButtonState.Released)
            {
                sourceStackPanel.Width = 100;
                sourceStackPanel.Height = 100;
            }
        }
    }
}