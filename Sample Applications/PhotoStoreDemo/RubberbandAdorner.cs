// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PhotoStoreDemo
{
    public class RubberbandAdorner : Adorner
    {
        private readonly UIElement _adornedElement;
        private readonly RectangleGeometry _geometry;
        private Point _anchorPoint;
        private Rect _selectRect;
        private MainWindow _window;

        public RubberbandAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _adornedElement = adornedElement;
            _selectRect = new Rect();
            _geometry = new RectangleGeometry();
            Rubberband = new Path
            {
                Data = _geometry,
                StrokeThickness = 2,
                Stroke = Brushes.Yellow,
                Opacity = .6,
                Visibility = Visibility.Hidden
            };
            AddVisualChild(Rubberband);
            MouseMove += DrawSelection;
            MouseUp += EndSelection;
        }

        public Rect SelectRect => _selectRect;
        public Path Rubberband { get; }
        protected override int VisualChildrenCount => 1;

        public MainWindow Window
        {
            set { _window = value; }
        }

        protected override Size ArrangeOverride(Size size)
        {
            var finalSize = base.ArrangeOverride(size);
            ((UIElement) GetVisualChild(0))?.Arrange(new Rect(new Point(), finalSize));
            return finalSize;
        }

        public void StartSelection(Point anchorPoint)
        {
            _anchorPoint = anchorPoint;
            _selectRect.Size = new Size(10, 10);
            _selectRect.Location = _anchorPoint;
            _geometry.Rect = _selectRect;
            if (Visibility.Visible != Rubberband.Visibility)
                Rubberband.Visibility = Visibility.Visible;
        }

        private void DrawSelection(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = e.GetPosition(_adornedElement);
                _selectRect.X = mousePosition.X < _anchorPoint.X ? mousePosition.X : _anchorPoint.X;
                _selectRect.Y = mousePosition.Y < _anchorPoint.Y ? mousePosition.Y : _anchorPoint.Y;
                _selectRect.Width = Math.Abs(mousePosition.X - _anchorPoint.X);
                _selectRect.Height = Math.Abs(mousePosition.Y - _anchorPoint.Y);
                _geometry.Rect = _selectRect;
                var layer = AdornerLayer.GetAdornerLayer(_adornedElement);
                layer.InvalidateArrange();
            }
        }

        private void EndSelection(object sender, MouseButtonEventArgs e)
        {
            if (3 >= _selectRect.Width || 3 >= _selectRect.Height)
                Rubberband.Visibility = Visibility.Hidden;
            else
                _window.CropButton.IsEnabled = true;
            ReleaseMouseCapture();
        }

        protected override Visual GetVisualChild(int index) => Rubberband;
    }


#if NoVISUALCHILD


    public class RubberbandAdorner : Adorner
    {

        private UIElement _adornedElement;
        private bool _showRect;
        private Window1 _window;
        SolidColorBrush _brush;
        Pen _pen;
        private Rect _selectRect;
        public Rect SelectRect { get { return _selectRect; } set { _selectRect = value; } }
        public bool ShowRect { get { return _showRect; } set { _showRect = value; } }
        public Window1 Window { set { _window = value; } }

        public RubberbandAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _adornedElement = adornedElement;
            _selectRect = new Rect();
            _brush = new SolidColorBrush();
            _brush.Color = Colors.Yellow;
            _brush.Opacity = .6;
            _pen = new Pen();
            _pen.Thickness = 2;
            _pen.Brush = _brush;
            _showRect = false;
            MouseMove += new MouseEventHandler(DrawSelection);
            MouseUp += new MouseButtonEventHandler(EndSelection);
        }

        public void StartSelection(Point anchorPoint)
        {
            _anchorPoint = anchorPoint;
            _selectRect.Size = new Size(0, 0);
            _selectRect.Location = _anchorPoint;
            if (!_showRect)
                _showRect = true;
        }

        private void DrawSelection(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(_adornedElement);
                if (mousePosition.X < _anchorPoint.X)
                    _selectRect.X = mousePosition.X;
                else
                    _selectRect.X = _anchorPoint.X;
                if (mousePosition.Y < _anchorPoint.Y)
                    _selectRect.Y = mousePosition.Y;
                else
                    _selectRect.Y = _anchorPoint.Y;
                _selectRect.Width = Math.Abs(mousePosition.X - _anchorPoint.X);
                _selectRect.Height = Math.Abs(mousePosition.Y - _anchorPoint.Y);
                InvalidateArrange();
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(_adornedElement);
                layer.InvalidateArrange();
            }
        }

        private void EndSelection(object sender, MouseButtonEventArgs e)
        {
            if (3 >= _selectRect.Width || 3 >= _selectRect.Height)
                ShowRect = false;
            else
                _window.CropButton.IsEnabled = true;
            ReleaseMouseCapture();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_showRect)
            {
                base.OnRender(drawingContext);
                drawingContext.DrawRectangle(Brushes.Transparent, _pen, _selectRect);
            }
            else
                return;
        }
    }
#endif
}