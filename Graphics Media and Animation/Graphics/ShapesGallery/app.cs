// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapesGallery
{
    public class App : Application
    {
        private Border _myBorder;
        private ColumnDefinition _myColDef1;
        private ColumnDefinition _myColDef2;
        private Ellipse _myEllipse;
        private Grid _myGrid;
        private Line _myLine;
        private Path _myPath;
        private Polygon _myPolygon;
        private Polyline _myPolyline;
        private Rectangle _myRect;
        private RowDefinition _myRowDef;
        private RowDefinition _myRowDef1;
        private RowDefinition _myRowDef2;
        private RowDefinition _myRowDef3;
        private RowDefinition _myRowDef4;
        private RowDefinition _myRowDef5;
        private RowDefinition _myRowDef6;
        private TextBlock _myTextBlock;
        private Window _myWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateAndShowMainWindow();
        }

        private void CreateAndShowMainWindow()
        {
            // Create the application's main window
            _myWindow = new Window();

            // Add a Border
            _myBorder = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Width = 400,
                Height = 600,
                Padding = new Thickness(15),
                Background = Brushes.White
            };

            // Create a Grid to host the Shapes
            _myGrid = new Grid {Margin = new Thickness(15)};
            _myColDef1 = new ColumnDefinition {Width = new GridLength(125)};
            _myColDef2 = new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)};
            _myGrid.ColumnDefinitions.Add(_myColDef1);
            _myGrid.ColumnDefinitions.Add(_myColDef2);
            _myRowDef = new RowDefinition();
            _myRowDef1 = new RowDefinition();
            _myRowDef2 = new RowDefinition();
            _myRowDef3 = new RowDefinition();
            _myRowDef4 = new RowDefinition();
            _myRowDef5 = new RowDefinition();
            _myRowDef6 = new RowDefinition();
            _myGrid.RowDefinitions.Add(_myRowDef);
            _myGrid.RowDefinitions.Add(_myRowDef1);
            _myGrid.RowDefinitions.Add(_myRowDef2);
            _myGrid.RowDefinitions.Add(_myRowDef3);
            _myGrid.RowDefinitions.Add(_myRowDef4);
            _myGrid.RowDefinitions.Add(_myRowDef5);
            _myGrid.RowDefinitions.Add(_myRowDef6);
            _myTextBlock = new TextBlock
            {
                FontSize = 20,
                Text = "WPF Shapes Gallery",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(_myTextBlock);
            Grid.SetRow(_myTextBlock, 0);
            Grid.SetColumnSpan(_myTextBlock, 2);


            // Add a Rectangle Element
            _myRect = new Rectangle
            {
                Stroke = Brushes.Black,
                Fill = Brushes.SkyBlue,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 50,
                Width = 50
            };
            _myGrid.Children.Add(_myRect);
            Grid.SetRow(_myRect, 1);
            Grid.SetColumn(_myRect, 0);
            var myTextBlock1 = new TextBlock
            {
                FontSize = 14,
                Text = "A Rectangle Element",
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(myTextBlock1);
            Grid.SetRow(myTextBlock1, 1);
            Grid.SetColumn(myTextBlock1, 1);


            // Add an Ellipse Element
            _myEllipse = new Ellipse
            {
                Stroke = Brushes.Black,
                Fill = Brushes.DarkBlue,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 50,
                Height = 75
            };
            _myGrid.Children.Add(_myEllipse);
            Grid.SetRow(_myEllipse, 2);
            Grid.SetColumn(_myEllipse, 0);
            var myTextBlock2 = new TextBlock
            {
                FontSize = 14,
                Text = "An Ellipse Element",
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(myTextBlock2);
            Grid.SetRow(myTextBlock2, 2);
            Grid.SetColumn(myTextBlock2, 1);


            // Add a Line Element
            _myLine = new Line
            {
                Stroke = Brushes.LightSteelBlue,
                X1 = 1,
                X2 = 50,
                Y1 = 1,
                Y2 = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 2
            };
            _myGrid.Children.Add(_myLine);
            Grid.SetRow(_myLine, 3);
            Grid.SetColumn(_myLine, 0);
            var myTextBlock3 = new TextBlock
            {
                FontSize = 14,
                Text = "A Line Element",
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(myTextBlock3);
            Grid.SetRow(myTextBlock3, 3);
            Grid.SetColumn(myTextBlock3, 1);


            //Add the Path Element
            _myPath = new Path
            {
                Stroke = Brushes.Black,
                Fill = Brushes.MediumSlateBlue,
                StrokeThickness = 4,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            var myEllipseGeometry = new EllipseGeometry
            {
                Center = new Point(50, 50),
                RadiusX = 25,
                RadiusY = 25
            };
            _myPath.Data = myEllipseGeometry;
            _myGrid.Children.Add(_myPath);
            Grid.SetRow(_myPath, 4);
            Grid.SetColumn(_myPath, 0);
            var myTextBlock4 = new TextBlock
            {
                FontSize = 14,
                Text = "A Path Element",
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(myTextBlock4);
            Grid.SetRow(myTextBlock4, 4);
            Grid.SetColumn(myTextBlock4, 1);


            //Add the Polygon Element
            _myPolygon = new Polygon
            {
                Stroke = Brushes.Black,
                Fill = Brushes.LightSeaGreen,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            var point1 = new Point(1, 50);
            var point2 = new Point(10, 80);
            var point3 = new Point(50, 50);
            var myPointCollection = new PointCollection {point1, point2, point3};
            _myPolygon.Points = myPointCollection;
            _myGrid.Children.Add(_myPolygon);
            Grid.SetRow(_myPolygon, 5);
            Grid.SetColumn(_myPolygon, 0);
            var myTextBlock5 = new TextBlock
            {
                Text = "A Polygon Element",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(myTextBlock5);
            Grid.SetRow(myTextBlock5, 5);
            Grid.SetColumn(myTextBlock5, 1);


            // Add the Polyline Element
            _myPolyline = new Polyline
            {
                Stroke = Brushes.SlateGray,
                StrokeThickness = 2,
                FillRule = FillRule.EvenOdd
            };
            var point4 = new Point(1, 50);
            var point5 = new Point(10, 80);
            var point6 = new Point(20, 40);
            var myPointCollection2 = new PointCollection {point4, point5, point6};
            _myPolyline.Points = myPointCollection2;
            _myGrid.Children.Add(_myPolyline);
            Grid.SetRow(_myPolyline, 6);
            Grid.SetColumn(_myPolyline, 0);
            var myTextBlock6 = new TextBlock
            {
                FontSize = 14,
                Text = "A Polyline Element",
                VerticalAlignment = VerticalAlignment.Center
            };
            _myGrid.Children.Add(myTextBlock6);
            Grid.SetRow(myTextBlock6, 6);
            Grid.SetColumn(myTextBlock6, 1);

            // Add the Grid to the Window as Content and show the Window
            _myBorder.Child = _myGrid;
            _myWindow.Content = _myBorder;
            _myWindow.Background = Brushes.LightSlateGray;
            _myWindow.Title = "Shapes Sample";
            _myWindow.Show();
        }
    }
}