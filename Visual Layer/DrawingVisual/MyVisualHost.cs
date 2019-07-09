// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisual
{
    public class MyVisualHost : FrameworkElement
    {
        // Create a collection of child visual objects.
        private readonly VisualCollection _children;

        public MyVisualHost()
        {
            _children = new VisualCollection(this)
            {
                CreateDrawingVisualRectangle(),
                CreateDrawingVisualText(),
                CreateDrawingVisualEllipses()
            };

            // Add the event handler for MouseLeftButtonUp.
            MouseLeftButtonUp += MyVisualHost_MouseLeftButtonUp;
        }

        // Capture the mouse event and hit test the coordinate point value against
        // the child visual objects.
        private void MyVisualHost_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Retreive the coordinates of the mouse button event.
            Point pt = e.GetPosition((UIElement) sender);

            // Initiate the hit test by setting up a hit test result callback method.
            VisualTreeHelper.HitTest(this, null, MyCallback, new PointHitTestParameters(pt));
        }

        // If a child visual object is hit, toggle its opacity to visually indicate a hit.
        public HitTestResultBehavior MyCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof (System.Windows.Media.DrawingVisual))
            {
                ((System.Windows.Media.DrawingVisual) result.VisualHit).Opacity =
                    ((System.Windows.Media.DrawingVisual) result.VisualHit).Opacity == 1.0 ? 0.4 : 1.0;
            }

            // Stop the hit test enumeration of objects in the visual tree.
            return HitTestResultBehavior.Stop;
        }

        // Create a DrawingVisual that contains a rectangle.
        private System.Windows.Media.DrawingVisual CreateDrawingVisualRectangle()
        {
            System.Windows.Media.DrawingVisual drawingVisual = new System.Windows.Media.DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            Rect rect = new Rect(new Point(160, 100), new Size(320, 80));
            drawingContext.DrawRectangle(Brushes.LightBlue, null, rect);

            // Persist the drawing content.
            drawingContext.Close();

            return drawingVisual;
        }

        // Create a DrawingVisual that contains text.
        private System.Windows.Media.DrawingVisual CreateDrawingVisualText()
        {
            // Create an instance of a DrawingVisual.
            System.Windows.Media.DrawingVisual drawingVisual = new System.Windows.Media.DrawingVisual();

            // Retrieve the DrawingContext from the DrawingVisual.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

#pragma warning disable CS0618 // 'FormattedText.FormattedText(string, CultureInfo, FlowDirection, Typeface, double, Brush)' is obsolete: 'Use the PixelsPerDip override'
            // Draw a formatted text string into the DrawingContext.
            drawingContext.DrawText(
                new FormattedText("Click Me!",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    36, Brushes.Black),
                new Point(200, 116));
#pragma warning enable CS0618 // 'FormattedText.FormattedText(string, CultureInfo, FlowDirection, Typeface, double, Brush)' is obsolete: 'Use the PixelsPerDip override'

            // Close the DrawingContext to persist changes to the DrawingVisual.
            drawingContext.Close();

            return drawingVisual;
        }

        // Create a DrawingVisual that contains an ellipse.
        private System.Windows.Media.DrawingVisual CreateDrawingVisualEllipses()
        {
            System.Windows.Media.DrawingVisual drawingVisual = new System.Windows.Media.DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            drawingContext.DrawEllipse(Brushes.Maroon, null, new Point(430, 136), 20, 20);
            drawingContext.Close();

            return drawingVisual;
        }


        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount => _children.Count;

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }
    }
}