// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MousePointer
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

        // raised when mouse cursor enters the area occupied by the element
        private void OnMouseEnterHandler(object sender, MouseEventArgs e)
        {
            border1.Background = Brushes.Red;
        }

        // raised when mouse cursor leaves the area occupied by the element
        private void OnMouseLeaveHandler(object sender, MouseEventArgs e)
        {
            border1.Background = Brushes.White;
        }
    }
}