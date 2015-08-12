// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualBrush
{
    public partial class MagnifyingGlassExample : Page
    {
        private static readonly double DistanceFromMouse = 5;

        private void UpdateMagnifyingGlass(object sender, MouseEventArgs args)
        {
            Mouse.SetCursor(Cursors.Cross);

            // Get the current position of the mouse pointer.
            var currentMousePosition = args.GetPosition(this);

            // Determine whether the magnifying glass should be shown to the
            // the left or right of the mouse pointer.
            if (ActualWidth - currentMousePosition.X > magnifyingGlassEllipse.Width + DistanceFromMouse)
            {
                Canvas.SetLeft(magnifyingGlassEllipse, currentMousePosition.X + DistanceFromMouse);
            }
            else
            {
                Canvas.SetLeft(magnifyingGlassEllipse,
                    currentMousePosition.X - DistanceFromMouse - magnifyingGlassEllipse.Width);
            }

            // Determine whether the magnifying glass should be shown 
            // above or below the mouse pointer.
            if (ActualHeight - currentMousePosition.Y > magnifyingGlassEllipse.Height + DistanceFromMouse)
            {
                Canvas.SetTop(magnifyingGlassEllipse, currentMousePosition.Y + DistanceFromMouse);
            }
            else
            {
                Canvas.SetTop(magnifyingGlassEllipse,
                    currentMousePosition.Y - DistanceFromMouse - magnifyingGlassEllipse.Height);
            }


            // Update the visual brush's Viewbox to magnify a 20 by 20 rectangle,
            // centered on the current mouse position.
            myVisualBrush.Viewbox =
                new Rect(currentMousePosition.X - 10, currentMousePosition.Y - 10, 20, 20);
        }
    }
}