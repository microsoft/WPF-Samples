// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PerFrameAnimation
{
    /// <summary>
    ///     Interaction logic for FollowExample.xaml
    /// </summary>
    public partial class FollowExample : Page
    {
        private Point _lastMousePosition = new Point(0, 0);
        private Vector _rectangleVelocity = new Vector(0, 0);

        public FollowExample()
        {
            CompositionTarget.Rendering += UpdateRectangle;
            PreviewMouseMove += UpdateLastMousePosition;
        }

        private void UpdateRectangle(object sender, EventArgs e)
        {
            var location = new Point(Canvas.GetLeft(followRectangle), Canvas.GetTop(followRectangle));

            //find vector toward mouse location
            var toMouse = _lastMousePosition - location;

            //add a force toward the mouse to the rectangles velocity
            var followForce = 0.01;
            _rectangleVelocity += toMouse*followForce;

            //dampen the velocity to add stability
            var drag = 0.8;
            _rectangleVelocity *= drag;

            //update the new location from the velocity
            location += _rectangleVelocity;

            //set new position
            Canvas.SetLeft(followRectangle, location.X);
            Canvas.SetTop(followRectangle, location.Y);
        }

        private void UpdateLastMousePosition(object sender, MouseEventArgs e)
        {
            _lastMousePosition = e.GetPosition(containerCanvas);
        }
    }
}