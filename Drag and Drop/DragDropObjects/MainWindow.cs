// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DragDropObjects
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isDown;
        private bool _isDragging;
        private UIElement _originalElement;
        private double _originalLeft;
        private double _originalTop;
        private SimpleCircleAdorner _overlayElement;
        private Point _startPoint;


        private void Window1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _isDragging)
            {
                DragFinished(true);
            }
        }

        private void MyCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDown)
            {
                DragFinished();
                e.Handled = true;
            }
        }

        private void DragFinished(bool cancelled = false)
        {
            Mouse.Capture(null);
            if (_isDragging)
            {
                AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);

                if (cancelled == false)
                {
                    Canvas.SetTop(_originalElement, _originalTop + _overlayElement.TopOffset);
                    Canvas.SetLeft(_originalElement, _originalLeft + _overlayElement.LeftOffset);
                }
                _overlayElement = null;
            }
            _isDragging = false;
            _isDown = false;
        }

        private void MyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(MyCanvas).X - _startPoint.X) >
                      SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(e.GetPosition(MyCanvas).Y - _startPoint.Y) >
                      SystemParameters.MinimumVerticalDragDistance)))
                {
                    DragStarted();
                }
                if (_isDragging)
                {
                    DragMoved();
                }
            }
        }

        private void DragStarted()
        {
            _isDragging = true;
            _originalLeft = Canvas.GetLeft(_originalElement);
            _originalTop = Canvas.GetTop(_originalElement);

            _overlayElement = new SimpleCircleAdorner(_originalElement);
            var layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);
        }

        private void DragMoved()
        {
            var currentPosition = Mouse.GetPosition(MyCanvas);

            _overlayElement.LeftOffset = currentPosition.X - _startPoint.X;
            _overlayElement.TopOffset = currentPosition.Y - _startPoint.Y;
        }

        private void MyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == MyCanvas)
            {
            }
            else
            {
                _isDown = true;
                _startPoint = e.GetPosition(MyCanvas);
                _originalElement = e.Source as UIElement;
                MyCanvas.CaptureMouse();
                e.Handled = true;
            }
        }
    }
}