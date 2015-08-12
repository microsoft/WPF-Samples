// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Input;

namespace MoveObjectWithMouse
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

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            // Get the x and y coordinates of the mouse pointer.
            var position = e.GetPosition(this);
            var pX = position.X;
            var pY = position.Y;

            // Sets the Height/Width of the circle to the mouse coordinates.
            ellipse.Width = pX;
            ellipse.Height = pY;
        }
    }
}