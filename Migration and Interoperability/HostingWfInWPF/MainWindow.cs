// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace HostingWfInWPF
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
            // Create the interop host control.
            var host =
                new WindowsFormsHost();

            // Create the MaskedTextBox control.
            var mtbDate = new MaskedTextBox("00/00/0000");

            // Assign the MaskedTextBox control as the host control's child.
            host.Child = mtbDate;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            grid1.Children.Add(host);
        }
    }
}