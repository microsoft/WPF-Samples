// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Geometry
{
    public partial class SampleViewer : Page
    {
        private readonly Page[] _examples;
        private int _sampleIndex;

        public SampleViewer()
        {
            InitializeComponent();
            _examples = new Page[5];

            _examples[0] = new GeometryUsageExample();
            _examples[1] = new ShapeGeometriesExample();
            _examples[2] = new PathGeometryExample();
            _examples[3] = new CombiningGeometriesExample();
            _examples[4] = new GeometryAttributeSyntaxExample();
        }

        private void PageLoaded(object sender, RoutedEventArgs args)
        {
            Example1RadioButton.IsChecked = true;
        }

        private void ZoomOutStoryboardCompleted(object sender, EventArgs args)
        {
            mainFrame.Navigate(_examples[_sampleIndex]);
        }

        private void FrameContentRendered(object sender, EventArgs args)
        {
            var s = (Storyboard) Resources["ZoomInStoryboard"];
            s.Begin(this);
        }

        private void ZoomInStoryboardCompleted(object sender, EventArgs e)
        {
            scrollViewerBorder.Visibility = Visibility.Visible;
        }

        private void SampleSelected(object sender, RoutedEventArgs args)
        {
            var points = new Point3DCollection();

            var ratio = myScrollViewer.ActualWidth/myScrollViewer.ActualHeight;

            points.Add(new Point3D(5, -5*ratio, 0));
            points.Add(new Point3D(5, 5*ratio, 0));
            points.Add(new Point3D(-5, 5*ratio, 0));

            points.Add(new Point3D(-5, 5*ratio, 0));
            points.Add(new Point3D(-5, -5*ratio, 0));
            points.Add(new Point3D(5, -5*ratio, 0));

            points.Add(new Point3D(-5, 5*ratio, 0));
            points.Add(new Point3D(-5, -5*ratio, 0));
            points.Add(new Point3D(5, -5*ratio, 0));

            points.Add(new Point3D(5, -5*ratio, 0));
            points.Add(new Point3D(5, 5*ratio, 0));
            points.Add(new Point3D(-5, 5*ratio, 0));

            myGeometry.Positions = points;
            myViewport3D.Width = 100;
            myViewport3D.Height = 100*ratio;

            scrollViewerBorder.Visibility = Visibility.Hidden;

            var button = sender as RadioButton;

            if (button != null)
            {
                if (button.Content.ToString() == "Geometry Usage")
                    _sampleIndex = 0;
                else if (button.Content.ToString() == "Shape Geometries")
                    _sampleIndex = 1;

                else if (button.Content.ToString() == "PathGeometry")
                    _sampleIndex = 2;

                else if (button.Content.ToString() == "Combining Geometries Example")
                    _sampleIndex = 3;
                else if (button.Content.ToString() == "Geometry Attribute Syntax Example")
                    _sampleIndex = 4;
            }
        }
    }
}