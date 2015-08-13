// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LocalAnimations
{
    // Create the demonstration.
    public class InteractiveAnimationExample : Page
    {
        private readonly Border _containerBorder;
        private readonly Ellipse _interactiveEllipse;
        private readonly TranslateTransform _interactiveTranslateTransform;

        public InteractiveAnimationExample()
        {
            WindowTitle = "Interactive Animation Example";
            var myPanel = new DockPanel {Margin = new Thickness(20.0)};

            _containerBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2.0),
                VerticalAlignment = VerticalAlignment.Stretch
            };

            _interactiveEllipse = new Ellipse
            {
                Fill = Brushes.Lime,
                Stroke = Brushes.Black,
                StrokeThickness = 2.0,
                Width = 25,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            _interactiveTranslateTransform = new TranslateTransform();
            _interactiveEllipse.RenderTransform =
                _interactiveTranslateTransform;

            _containerBorder.MouseLeftButtonDown +=
                border_mouseLeftButtonDown;
            _containerBorder.MouseRightButtonDown +=
                border_mouseRightButtonDown;

            _containerBorder.Child = _interactiveEllipse;
            myPanel.Children.Add(_containerBorder);
            Content = myPanel;
        }

        // When the user left-clicks, use the 
        // SnapshotAndReplace HandoffBehavior when applying the animation.        
        private void border_mouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = Mouse.GetPosition(_containerBorder);

            // Set the target point so the center of the ellipse
            // ends up at the clicked point.
            var targetPoint = new Point
            {
                X = clickPoint.X - _interactiveEllipse.Width/2,
                Y = clickPoint.Y - _interactiveEllipse.Height/2
            };

            // Animate to the target point.
            var xAnimation =
                new DoubleAnimation(targetPoint.X,
                    new Duration(TimeSpan.FromSeconds(4)));
            _interactiveTranslateTransform.BeginAnimation(
                TranslateTransform.XProperty, xAnimation, HandoffBehavior.SnapshotAndReplace);

            var yAnimation =
                new DoubleAnimation(targetPoint.Y,
                    new Duration(TimeSpan.FromSeconds(4)));
            _interactiveTranslateTransform.BeginAnimation(
                TranslateTransform.YProperty, yAnimation, HandoffBehavior.SnapshotAndReplace);

            // Change the color of the ellipse.
            _interactiveEllipse.Fill = Brushes.Lime;
        }

        // When the user right-clicks, use the 
        // Compose HandoffBehavior when applying the animation.
        private void border_mouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Find the point where the use clicked.
            var clickPoint = Mouse.GetPosition(_containerBorder);

            // Set the target point so the center of the ellipse
            // ends up at the clicked point.
            var targetPoint = new Point
            {
                X = clickPoint.X - _interactiveEllipse.Width/2,
                Y = clickPoint.Y - _interactiveEllipse.Height/2
            };


            // Animate to the target point.
            var xAnimation =
                new DoubleAnimation(targetPoint.X,
                    new Duration(TimeSpan.FromSeconds(4)));
            _interactiveTranslateTransform.BeginAnimation(
                TranslateTransform.XProperty, xAnimation, HandoffBehavior.Compose);

            var yAnimation =
                new DoubleAnimation(targetPoint.Y,
                    new Duration(TimeSpan.FromSeconds(4)));
            _interactiveTranslateTransform.BeginAnimation(
                TranslateTransform.YProperty, yAnimation, HandoffBehavior.Compose);

            // Change the color of the ellipse.
            _interactiveEllipse.Fill = Brushes.Orange;
        }
    }
}