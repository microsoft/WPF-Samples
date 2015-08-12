// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PerFrameAnimation
{
    public class FollowMouseCanvas : Canvas
    {
        private TimeSpan _lastRender;
        private Canvas _parentCanvas;
        private Point _parentLastMousePosition = new Point(0, 0);
        private Vector _velocity = new Vector(0, 0);

        public FollowMouseCanvas()
        {
            _lastRender = TimeSpan.FromTicks(DateTime.Now.Ticks);
            CompositionTarget.Rendering += UpdatePosition;
        }

        private void UpdatePosition(object sender, EventArgs e)
        {
            var renderingArgs = (RenderingEventArgs) e;

            var deltaTime = (renderingArgs.RenderingTime - _lastRender).TotalSeconds;
            _lastRender = renderingArgs.RenderingTime;

            if (_parentCanvas == null)
            {
                _parentCanvas = VisualTreeHelper.GetParent(this) as Canvas;
                if (_parentCanvas == null)
                {
                    //parent isnt canvas so just abort trying to follow mouse
                    CompositionTarget.Rendering -= UpdatePosition;
                }
                else
                {
                    //parent is canvas, so track mouse position and time
                    _parentCanvas.PreviewMouseMove += UpdateLastMousePosition;
                }
            }


            //get location
            var location = new Point(GetLeft(this), GetTop(this));

            //check for NaN's and replace with 0,0
            if (double.IsNaN(location.X) || double.IsNaN(location.Y))
                location = new Point(0, 0);

            //find vector toward mouse location
            var toMouse = _parentLastMousePosition - location;

            //add a force toward the mouse to the rectangles velocity
            var followForce = 1.0;
            _velocity += toMouse*followForce;

            //dampen the velocity to add stability
            var drag = 0.95;
            _velocity *= drag;

            //update the new location from the velocity
            location += _velocity*deltaTime;

            //set new position
            SetLeft(this, location.X);
            SetTop(this, location.Y);
        }

        private void UpdateLastMousePosition(object sender, MouseEventArgs e)
        {
            if (_parentCanvas == null)
                return;

            _parentLastMousePosition = e.GetPosition(_parentCanvas);
        }
    }
}