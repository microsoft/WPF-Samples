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
    ///     Interaction logic for FrameIndependentFollowExample.xaml
    /// </summary>
    public partial class FrameIndependentFollowExample : Page
    {
        private Point _lastMousePosition = new Point(0, 0);
        //timing variables
        private TimeSpan _lastRender;
        private Vector _rectangleVelocity = new Vector(0, 0);

        public FrameIndependentFollowExample()
        {
            InitializeComponent();
            _lastRender = TimeSpan.FromTicks(DateTime.Now.Ticks);
            CompositionTarget.Rendering += UpdateRectangle;
            PreviewMouseMove += UpdateLastMousePosition;
        }

        private void UpdateRectangle(object sender, EventArgs e)
        {
            var renderArgs = (RenderingEventArgs) e;
            var deltaTime = (renderArgs.RenderingTime - _lastRender).TotalSeconds;
            _lastRender = renderArgs.RenderingTime;


            var location = new Point(Canvas.GetLeft(followRectangle), Canvas.GetTop(followRectangle));

            //find vector toward mouse location
            var toMouse = _lastMousePosition - location;

            //add a force toward the mouse to the rectangles velocity
            var followForce = 1.00;
            _rectangleVelocity += toMouse*followForce;

            //dampen the velocity to add stability
            var drag = 0.9;
            _rectangleVelocity *= drag;

            //update the new location from the velocity
            location += _rectangleVelocity*deltaTime;

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