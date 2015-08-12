// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphingCalculatorDemo
{
    public class DrawAxisHelper
    {
        private readonly Canvas _screen;
        private Size _screenSize;
        private double _xmin, _xmax, _ymin, _ymax;

        public DrawAxisHelper(Canvas screen, Size screenSize)
        {
            _screen = screen;
            _screenSize = screenSize;
        }

        public void DrawAxes(double xmin, double xmax, double ymin, double ymax)
        {
            _xmin = xmin;
            _xmax = xmax;
            _ymin = ymin;
            _ymax = ymax;

            var canvasWidth = _screenSize.Width;
            var canvasHeight = _screenSize.Height;
            var graphWidth = xmax - xmin;
            var graphHeight = ymax - ymin;
            var xOffset = (xmin >= 0) ? 0.0 : (xmax <= 0) ? 1.0 : -xmin/graphWidth;
            var yOffset = (ymin >= 0) ? 1.0 : (ymax <= 0) ? 0.0 : ymax/graphHeight;
            xOffset = Math.Floor(xOffset*(canvasWidth - 0.0));
            yOffset = Math.Floor(yOffset*(canvasHeight - 0.0));

            // X axis
            DrawLine(xOffset, 0.0, xOffset, canvasHeight, false);
            // Y axis
            DrawLine(0.0, yOffset, canvasWidth, yOffset, false);
        }

        private void DrawLine(double x1, double y1, double x2, double y2, bool dotted)
        {
            var line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Red,
                StrokeThickness = 1.0
            };
            if (dotted)
            {
                var collection = new DoubleCollection {3, 3};
                line.StrokeDashArray = collection;
            }
            _screen.Children.Add(line);
        }
    }
}