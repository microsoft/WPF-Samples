// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GeometryDesignerDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnUiReady(object sender, EventArgs e)
        {
            LinePane.Width = ((StackPanel) LinePane.Parent).ActualWidth;
            LinePane.Height = ((StackPanel) LinePane.Parent).ActualHeight;
            DesignerPane.MouseLeave += DesignerPane_MouseLeave;
            SizeChanged += Window1_SizeChanged;
        }

        private void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            XAMLPane.BeginAnimation(Canvas.TopProperty, null);
            if (_isShow)
            {
                XAMLPane.SetValue(Canvas.TopProperty, e.NewSize.Height - 70);
            }
            else
            {
                XAMLPane.SetValue(Canvas.TopProperty, e.NewSize.Height);
            }
            XAMLPane.Width = e.NewSize.Width - 205;
        }

        private void OnShowHideXaml(object sender, RoutedEventArgs e)
        {
            var xamlstring = XamlWriter.Save(DrawingPane);
            ((TextBox) XAMLPane.Children[0]).Text = xamlstring;

            if (!_isShow)
            {
                _isShow = !_isShow;
                XAMLPane.Width = ActualWidth - 205;
                XAMLPane.Height = 50;
                var yLocation = DesignerPane.ActualHeight;
                XAMLPane.SetValue(Canvas.LeftProperty, DesignerPane.GetValue(Canvas.LeftProperty));
                XAMLPane.SetValue(Canvas.TopProperty, yLocation);
                ((Label) ((Button) sender).Content).Content = "Hide XAML";

                //Start animation
                var db = new DoubleAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(500),
                    FillBehavior = FillBehavior.HoldEnd,
                    To = yLocation - 50
                };

                XAMLPane.BeginAnimation(Canvas.TopProperty, db);
            }
            else
            {
                _isShow = !_isShow;
                ((Label) ((Button) sender).Content).Content = "Show XAML";

                var dbShrink = new DoubleAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(500),
                    FillBehavior = FillBehavior.HoldEnd,
                    To = DesignerPane.ActualHeight,
                    From = DesignerPane.ActualHeight - 50
                };

                XAMLPane.BeginAnimation(Canvas.TopProperty, dbShrink);
            }
        }

        private void OnGeometrySelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;

            if (box.Name != "GeometryTypeCB")
                return;

            if (_initialSelection)
            {
                _initialSelection = !_initialSelection;
            }
            else
            {
                GeometryPaneChange(((ComboBoxItem) box.SelectedItem).Content, true);
            }
        }

        public abstract class GeometryBase
        {
            protected GeometryBase(FrameworkElement pane)
            {
                ParentPane = pane;
            }

            public abstract void Parse();
            public abstract Geometry CreateGeometry();

            protected double DoubleParser(string s) => double.Parse(s);

            protected Point PointParser(string o)
            {
                try
                {
                    return Point.Parse(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    throw new ApplicationException(
                        "Error: please enter two numeric values separated  by a comma or a space; for example, 10,30.");
                }
            }

            protected Size SizeParser(string o)
            {
                var retval = new Size();

                var sizeString = o.Split(' ', ',', ';');
                if (sizeString.Length == 0 || sizeString.Length != 2)
                    throw new ApplicationException(
                        "Error: a size should contain two double that seperated by a space or ',' or ';'");

                try
                {
                    var d1 = Convert.ToDouble(sizeString[0], CultureInfo.InvariantCulture);
                    var d2 = Convert.ToDouble(sizeString[1], CultureInfo.InvariantCulture);
                    retval.Width = d1;
                    retval.Height = d2;
                }
                catch (Exception)
                {
                    throw new ApplicationException("Error: please enter only two numeric values into the field");
                }

                return retval;
            }

            #region Data member

            protected FrameworkElement ParentPane;
            public ArrayList ControlPoints;
            public string GeometryType;

            #endregion
        }

        public class LineG : GeometryBase
        {
            public LineG(FrameworkElement pane)
                : base(pane)
            {
                ControlPoints = new ArrayList();
                GeometryType = "Line";
                Parse();
            }

            public override void Parse()
            {
                var tbStartPoint = LogicalTreeHelper.FindLogicalNode(ParentPane, "LineStartPoint") as TextBox;
                var tbEndPoint = LogicalTreeHelper.FindLogicalNode(ParentPane, "LineEndPoint") as TextBox;

                _startPoint = PointParser(tbStartPoint.Text);
                _endPoint = PointParser(tbEndPoint.Text);

                ControlPoints.Add(_startPoint);
                ControlPoints.Add(_endPoint);
            }

            public override Geometry CreateGeometry()
            {
                var lg = new LineGeometry(_startPoint, _endPoint);
                return lg;
            }

            #region Data members

            private Point _startPoint;
            private Point _endPoint;

            #endregion
        }

        public class EllipseG : GeometryBase
        {
            public EllipseG(FrameworkElement pane) : base(pane)
            {
                ControlPoints = new ArrayList();
                GeometryType = "Ellipse";
                Parse();
            }

            public override void Parse()
            {
                var tbCenter = LogicalTreeHelper.FindLogicalNode(ParentPane, "EllipseCenterPoint") as TextBox;
                var tbRadiusx = LogicalTreeHelper.FindLogicalNode(ParentPane, "EllipseRadiusX") as TextBox;
                var tbRadiusy = LogicalTreeHelper.FindLogicalNode(ParentPane, "EllipseRadiusY") as TextBox;

                _center = PointParser(tbCenter.Text);
                _radiusx = DoubleParser(tbRadiusx.Text);
                _radiusy = DoubleParser(tbRadiusy.Text);

                //Center point
                ControlPoints.Add(_center);

                //TopLeft
                ControlPoints.Add(new Point(_center.X - _radiusx, _center.Y));

                //TopMiddle
                ControlPoints.Add(new Point(_center.X, _center.Y - _radiusy));

                //TopRight
                ControlPoints.Add(new Point(_center.X + _radiusx, _center.Y));

                //BottomMiddle
                ControlPoints.Add(new Point(_center.X, _center.Y + _radiusy));
            }

            public override Geometry CreateGeometry()
            {
                var retval = new EllipseGeometry(_center, _radiusx, _radiusy);
                return retval;
            }

            #region Data Members

            private Point _center;
            private double _radiusx;
            private double _radiusy;

            #endregion
        }

        public class RectanlgeG : GeometryBase
        {
            public RectanlgeG(FrameworkElement pane)
                : base(pane)
            {
                ControlPoints = new ArrayList();
                GeometryType = "Rectangle";
                Parse();
            }

            public override Geometry CreateGeometry()
            {
                var retval = new RectangleGeometry(new Rect(_topleft.X, _topleft.Y, _width, _height), _radiusx, _radiusy);

                return retval;
            }

            public override void Parse()
            {
                var tbTopleft = LogicalTreeHelper.FindLogicalNode(ParentPane, "RectangleTopLeftPoint") as TextBox;
                var tbRadiusx = LogicalTreeHelper.FindLogicalNode(ParentPane, "RectangleRadiusX") as TextBox;
                var tbRadiusy = LogicalTreeHelper.FindLogicalNode(ParentPane, "RectangleRadiusY") as TextBox;
                var tbWidth = LogicalTreeHelper.FindLogicalNode(ParentPane, "RectangleWidth") as TextBox;
                var tbHeight = LogicalTreeHelper.FindLogicalNode(ParentPane, "RectangleHeight") as TextBox;

                _topleft = PointParser(tbTopleft.Text);
                _radiusx = DoubleParser(tbRadiusx.Text);
                _radiusy = DoubleParser(tbRadiusy.Text);
                _width = DoubleParser(tbWidth.Text);
                _height = DoubleParser(tbHeight.Text);

                //TopLeft point
                ControlPoints.Add(_topleft);

                //TopRight
                ControlPoints.Add(new Point(_topleft.X + _width, _topleft.Y));

                //BottomLeft
                ControlPoints.Add(new Point(_topleft.X, _topleft.Y + _height));

                //BottomRight
                ControlPoints.Add(new Point(_topleft.X + _width, _topleft.Y + _height));

                if (_radiusx != 0 && _radiusy != 0)
                {
                    ControlPoints.Add(new Point(_topleft.X + _radiusx, _topleft.Y + _radiusy));
                }
            }

            #region Data Members

            private Point _topleft;
            private double _width;
            private double _height;
            private double _radiusx;
            private double _radiusy;

            #endregion
        }

        public class BezierG : GeometryBase
        {
            #region Data Members

            private Point _startpoint, _p1, _p2, _p3;

            #endregion

            public BezierG(FrameworkElement pane)
                : base(pane)
            {
                ControlPoints = new ArrayList();
                GeometryType = "Bezier";
                Parse();
            }

            public override Geometry CreateGeometry()
            {
                var retval = new PathGeometry();
                var pf = new PathFigure {StartPoint = _startpoint};
                pf.Segments.Add(new BezierSegment(_p1, _p2, _p3, true));
                retval.Figures.Add(pf);
                return retval;
            }

            public override void Parse()
            {
                var tbStartpoint = LogicalTreeHelper.FindLogicalNode(ParentPane, "BezierStartPoint") as TextBox;
                var tbPoint1 = LogicalTreeHelper.FindLogicalNode(ParentPane, "BezierPoint1") as TextBox;
                var tbPoint2 = LogicalTreeHelper.FindLogicalNode(ParentPane, "BezierPoint2") as TextBox;
                var tbPoint3 = LogicalTreeHelper.FindLogicalNode(ParentPane, "BezierPoint3") as TextBox;

                _startpoint = PointParser(tbStartpoint.Text);
                _p1 = PointParser(tbPoint1.Text);
                _p2 = PointParser(tbPoint2.Text);
                _p3 = PointParser(tbPoint3.Text);

                ControlPoints.Add(_startpoint);
                ControlPoints.Add(_p1);
                ControlPoints.Add(_p2);
                ControlPoints.Add(_p3);
            }
        }

        public class ArcG : GeometryBase
        {
            public ArcG(FrameworkElement pane)
                : base(pane)
            {
                ControlPoints = new ArrayList();
                GeometryType = "Arc";
                Parse();
            }

            public override Geometry CreateGeometry()
            {
                var retval = new PathGeometry();
                var pf = new PathFigure {StartPoint = _startpoint};
                pf.Segments.Add(new ArcSegment(_point, _size, _xrotation, _largearc, _sweepArcDirection, true));
                retval.Figures.Add(pf);
                return retval;
            }

            public override void Parse()
            {
                var tbStartpoint = LogicalTreeHelper.FindLogicalNode(ParentPane, "ArcStartPoint") as TextBox;
                var tbPoint = LogicalTreeHelper.FindLogicalNode(ParentPane, "ArcPoint") as TextBox;
                var tbSize = LogicalTreeHelper.FindLogicalNode(ParentPane, "ArcSize") as TextBox;
                var tbXrotation = LogicalTreeHelper.FindLogicalNode(ParentPane, "ArcXRotation") as TextBox;
                var cbSweeparc = LogicalTreeHelper.FindLogicalNode(ParentPane, "ArcSweepArc") as ComboBox;
                var cbLargearc = LogicalTreeHelper.FindLogicalNode(ParentPane, "ArcLargeArc") as ComboBox;

                _startpoint = PointParser(tbStartpoint.Text);
                _point = PointParser(tbPoint.Text);
                _size = SizeParser(tbSize.Text);
                _xrotation = DoubleParser(tbXrotation.Text);
                _sweepArcDirection = (SweepDirection) Enum.Parse(
                    typeof (SweepDirection),
                    (
                        (string)
                            (
                                (ComboBoxItem) cbSweeparc.SelectedItem
                                ).Content
                        )
                    );

                _largearc = bool.Parse(((string) ((ComboBoxItem) cbLargearc.SelectedItem).Content));
                ControlPoints.Add(_startpoint);
                ControlPoints.Add(_point);
            }

            #region Data Members

            private Point _startpoint, _point;
            private Size _size;
            private bool _largearc;
            private SweepDirection _sweepArcDirection;
            private double _xrotation;

            #endregion
        }

        // ---------------------------------------------------------------------------------

        #region private action functions

        private void GeometryPaneChange(object geometryName, bool allowInsert)
        {
            foreach (Canvas c in ControlPointPane.Children)
            {
                if (string.Compare(c.Name, (string) geometryName + "Pane", false, CultureInfo.InvariantCulture) == 0)
                {
                    c.Width = ((StackPanel) c.Parent).ActualWidth;
                    c.Height = ((StackPanel) c.Parent).ActualHeight;
                    if (allowInsert)
                    {
                        foreach (var b in c.Children)
                        {
                            if (b is TextBox)
                            {
                                ((TextBox) b).Text = "";
                                continue;
                            }

                            if ((b as Button)?.Content is string &&
                                ((string) ((Button) b).Content == "Insert" || (string) ((Button) b).Content == "Update"))
                            {
                                ((Button) b).IsEnabled = true;
                                ((Button) b).Content = "Insert";
                                _isInsert = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var b in c.Children)
                        {
                            if ((b as Button)?.Content is string && (string) ((Button) b).Content == "Insert")
                            {
                                ((Button) b).IsEnabled = false;
                            }
                        }
                    }
                }
                else
                {
                    c.Width = 0;
                    c.Height = 0;
                }
            }
        }


        //If mouse leaves the DesignerPane, hide all of the control points
        private void DesignerPane_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (var o in DesignerPane.Children)
            {
                if (o is Ellipse)
                {
                    ((Ellipse) o).Visibility = Visibility.Hidden;
                }
            }
        }


        private void path_MouseEnter(object sender, MouseEventArgs e)
        {
            var pathId = ((Path) sender).Name;

            //Search and set visibility on all of the related control points
            foreach (var o in DesignerPane.Children)
            {
                //Search for the control point that contains the element's ID
                //e.g Line1_StartPoint for Line1 element
                if (o is Ellipse)
                {
                    ((Ellipse) o).Visibility = ((Ellipse) o).Name.Contains(pathId)
                        ? Visibility.Visible
                        : Visibility.Hidden;
                }
            }
        }


        private void OnInsertGeometry(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isInsert)
                {
                    var path = new Path
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    var gb = GeometryFactory((FrameworkElement) ((Button) sender).Parent);


                    switch (gb.GeometryType)
                    {
                        case "Line":
                            path.Name = "Line" + _lineCount;
                            AddControlPoints(gb.ControlPoints, "Line");
                            _lineCount++;
                            break;
                        case "Rectangle":
                            path.Name = "Rectangle" + _rectangleCount;
                            AddControlPoints(gb.ControlPoints, "Rectangle");
                            _rectangleCount++;
                            break;
                        case "Ellipse":
                            path.Name = "Ellipse" + _elliipseCount;
                            AddControlPoints(gb.ControlPoints, "Ellipse");
                            _elliipseCount++;
                            break;
                        case "Arc":
                            path.Name = "Arc" + _arcCount;
                            AddControlPoints(gb.ControlPoints, "Arc");
                            _arcCount++;
                            break;
                        case "Bezier":
                            path.Name = "Bezier" + _bezierCount;
                            AddControlPoints(gb.ControlPoints, "Bezier");
                            _bezierCount++;
                            break;
                        default:
                            throw new ApplicationException("Error:  Incorrect Geometry type");
                    }

                    path.Data = gb.CreateGeometry();
                    path.MouseEnter += path_MouseEnter;
                    _currentElement = path;
                    DrawingPane.Children.Add(path);
                    ((Button) sender).Content = "Update";
                    _isInsert = !_isInsert;
                }
                else
                {
                    var gbUpdate = GeometryFactory((FrameworkElement) ((Button) sender).Parent);
                    _currentElement.Data = gbUpdate.CreateGeometry();
                    UpdateControlPoints(gbUpdate.ControlPoints);
                }
            }
            catch (ApplicationException argExcept)
            {
                MessageBox.Show(argExcept.Message);
            }
        }

        #endregion

        // ---------------------------------------------------------------------------------

        #region Drag and Move

        //Special variable for Drag and move actions
        private bool _isMoving;
        private Point _movingPreviousLocation;


        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var el = (Ellipse) sender;
            el.Cursor = Cursors.Hand;
            _isMoving = true;
            _movingPreviousLocation = e.GetPosition(DrawingPane);
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var el = (Ellipse) sender;
            el.Cursor = Cursors.Arrow;
            _isMoving = false;
        }


        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            var el = (Ellipse) sender;
            Point movingEndLocation;
            if (_isMoving)
            {
                movingEndLocation = e.GetPosition(DrawingPane);


                Canvas.SetLeft(el, movingEndLocation.X - el.Width/2);
                Canvas.SetTop(el, movingEndLocation.Y - el.Height/2);

                UpdateGeometries(movingEndLocation, el.Name);
                _movingPreviousLocation = movingEndLocation;
            }
        }

        #endregion

        // ---------------------------------------------------------------------------------

        #region private helper functions

        private void UpdateControlPoints(ArrayList controlPoints)
        {
            var geometryType = GetGeometryTypeInId(_currentElement.Name);

            switch (geometryType)
            {
                case "Line":
                    UpdateLineGeometryControlPoints(controlPoints);
                    break;
                case "Ellipse":
                    UpdateEllipseGeometryControlPoints(controlPoints);
                    break;
                case "Rectangle":
                    UpdateRectangleGeometryControlPoints(controlPoints);
                    break;
                case "Arc":
                    UpdateArcGeometryControlPoints(controlPoints);
                    break;
                case "Bezier":
                    UpdateBezierGeometryControlPoints(controlPoints);
                    break;
                default:
                    throw new ApplicationException("Error: incorrect Element name");
            }
        }


        private void UpdateLineGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 2)
                throw new ApplicationException("Error:  incorrect # of control points for LineGeometry");

            for (var i = 0; i < controlPoints.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        var eStartPoint =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_StartPoint") as
                                Ellipse;
                        Canvas.SetLeft(eStartPoint, ((Point) controlPoints[i]).X - eStartPoint.Width/2);
                        Canvas.SetTop(eStartPoint, ((Point) controlPoints[i]).Y - eStartPoint.Height/2);
                        break;
                    case 1:
                        var eEndPoint =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_EndPoint") as
                                Ellipse;
                        Canvas.SetLeft(eEndPoint, ((Point) controlPoints[i]).X - eEndPoint.Width/2);
                        Canvas.SetTop(eEndPoint, ((Point) controlPoints[i]).Y - eEndPoint.Height/2);
                        break;
                    default:
                        throw new ApplicationException("Error: incorrect # of control points in LineG");
                }
            }
        }

        private void UpdateEllipseGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 5)
            {
                throw new ApplicationException("Error:  incorrect # of control points for EllipseGeometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        var eCenter =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_Center") as Ellipse;
                        Canvas.SetLeft(eCenter, ((Point) controlPoints[i]).X - eCenter.Width/2);
                        Canvas.SetTop(eCenter, ((Point) controlPoints[i]).Y - eCenter.Height/2);
                        break;
                    case 1:
                        var eTopLeft =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_TopLeft") as
                                Ellipse;
                        Canvas.SetLeft(eTopLeft, ((Point) controlPoints[i]).X - eTopLeft.Width/2);
                        Canvas.SetTop(eTopLeft, ((Point) controlPoints[i]).Y - eTopLeft.Height/2);
                        break;
                    case 2:
                        var eTopMiddle =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_TopMiddle") as
                                Ellipse;
                        Canvas.SetLeft(eTopMiddle, ((Point) controlPoints[i]).X - eTopMiddle.Width/2);
                        Canvas.SetTop(eTopMiddle, ((Point) controlPoints[i]).Y - eTopMiddle.Height/2);
                        break;
                    case 3:
                        var eTopRight =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_TopRight") as
                                Ellipse;
                        Canvas.SetLeft(eTopRight, ((Point) controlPoints[i]).X - eTopRight.Width/2);
                        Canvas.SetTop(eTopRight, ((Point) controlPoints[i]).Y - eTopRight.Height/2);
                        break;
                    case 4:
                        var eBottomMiddle =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_BottomMiddle") as
                                Ellipse;
                        Canvas.SetLeft(eBottomMiddle, ((Point) controlPoints[i]).X - eBottomMiddle.Width/2);
                        Canvas.SetTop(eBottomMiddle, ((Point) controlPoints[i]).Y - eBottomMiddle.Height/2);
                        break;
                    default:
                        throw new ApplicationException("Error: incorrect # of control points in EllipseG");
                }
            }
        }

        private void UpdateRectangleGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 5 && controlPoints.Count != 4)
            {
                throw new ApplicationException("Error:  incorrect # of control points for RectangleGeometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        var eTopLeft =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_TopLeft") as
                                Ellipse;
                        Canvas.SetLeft(eTopLeft, ((Point) controlPoints[i]).X - eTopLeft.Width/2);
                        Canvas.SetTop(eTopLeft, ((Point) controlPoints[i]).Y - eTopLeft.Height/2);
                        break;
                    case 1:
                        var eTopRight =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_TopRight") as
                                Ellipse;
                        Canvas.SetLeft(eTopRight, ((Point) controlPoints[i]).X - eTopRight.Width/2);
                        Canvas.SetTop(eTopRight, ((Point) controlPoints[i]).Y - eTopRight.Height/2);
                        break;
                    case 2:
                        var eBottomLeft =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_BottomLeft") as
                                Ellipse;
                        Canvas.SetLeft(eBottomLeft, ((Point) controlPoints[i]).X - eBottomLeft.Width/2);
                        Canvas.SetTop(eBottomLeft, ((Point) controlPoints[i]).Y - eBottomLeft.Height/2);
                        break;
                    case 3:
                        var eBottomRight =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_BottomRight") as
                                Ellipse;
                        Canvas.SetLeft(eBottomRight, ((Point) controlPoints[i]).X - eBottomRight.Width/2);
                        Canvas.SetTop(eBottomRight, ((Point) controlPoints[i]).Y - eBottomRight.Height/2);
                        break;
                    case 4:
                        var eCorner =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_Corner") as Ellipse;
                        Canvas.SetLeft(eCorner, ((Point) controlPoints[i]).X - eCorner.Width/2);
                        Canvas.SetTop(eCorner, ((Point) controlPoints[i]).Y - eCorner.Height/2);
                        break;
                    default:
                        throw new ApplicationException("Error: incorrect # of control points in RectangleG");
                }
            }
        }

        private void UpdateArcGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 2)
            {
                throw new ApplicationException("Error:  incorrect # of control points for Arc Geometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        var eStartPoint =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_StartPoint") as
                                Ellipse;
                        Canvas.SetLeft(eStartPoint, ((Point) controlPoints[i]).X - eStartPoint.Width/2);
                        Canvas.SetTop(eStartPoint, ((Point) controlPoints[i]).Y - eStartPoint.Height/2);
                        break;
                    case 1:
                        var ePoint1 =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_Point") as Ellipse;
                        Canvas.SetLeft(ePoint1, ((Point) controlPoints[i]).X - ePoint1.Width/2);
                        Canvas.SetTop(ePoint1, ((Point) controlPoints[i]).Y - ePoint1.Height/2);
                        break;
                    default:
                        throw new ApplicationException("Error: Incorrect # of control points in ArcG");
                }
            }
        }

        private void UpdateBezierGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 4)
            {
                throw new ApplicationException("Error:  incorrect # of control points for Bezier Geometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        var eStartPoint =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_StartPoint") as
                                Ellipse;
                        Canvas.SetLeft(eStartPoint, ((Point) controlPoints[i]).X - eStartPoint.Width/2);
                        Canvas.SetTop(eStartPoint, ((Point) controlPoints[i]).Y - eStartPoint.Height/2);
                        break;
                    case 1:
                        var ePoint1 =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_Point1") as Ellipse;
                        Canvas.SetLeft(ePoint1, ((Point) controlPoints[i]).X - ePoint1.Width/2);
                        Canvas.SetTop(ePoint1, ((Point) controlPoints[i]).Y - ePoint1.Height/2);
                        break;
                    case 2:
                        var ePoint2 =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_Point2") as Ellipse;
                        Canvas.SetLeft(ePoint2, ((Point) controlPoints[i]).X - ePoint2.Width/2);
                        Canvas.SetTop(ePoint2, ((Point) controlPoints[i]).Y - ePoint2.Height/2);
                        break;
                    case 3:
                        var ePoint3 =
                            LogicalTreeHelper.FindLogicalNode(DesignerPane, _currentElement.Name + "_Point3") as Ellipse;
                        Canvas.SetLeft(ePoint3, ((Point) controlPoints[i]).X - ePoint3.Width/2);
                        Canvas.SetTop(ePoint3, ((Point) controlPoints[i]).Y - ePoint3.Height/2);
                        break;
                    default:
                        throw new ApplicationException("Error: incorrect # of control points in BezierG");
                }
            }
        }

        private void UpdateGeometries(Point movingEndLocation, string id)
        {
            var geometryType = GetGeometryTypeInId(id);

            //Switch the controlpoint pane to the right panel
            GeometryPaneChange(geometryType, false);

            switch (geometryType)
            {
                case "Line":
                    UpdateLineGeometry(movingEndLocation, id);
                    break;
                case "Ellipse":
                    UpdateEllipseGeometry(movingEndLocation, id);
                    break;
                case "Rectangle":
                    UpdateRectangleGeometry(movingEndLocation, id);
                    break;
                case "Bezier":
                    UpdateBezierGeometry(movingEndLocation, id);
                    break;
                case "Arc":
                    UpdateArcGeometry(movingEndLocation, id);
                    break;
                default:
                    throw new ApplicationException("Error: incorrect GeometryType in UpdateGeometries()");
            }
        }

        private object SearchUpdatedElement(string p) => DrawingPane.Children.Cast<object>().FirstOrDefault(o => o is Path && ((Path)o).Name == p);

        private void UpdateArcGeometry(Point movingEndLocation, string id)
        {
            var s = id.Split('_');
            var controlPointType = GetContronPointTypeInId(id);
            var p = SearchUpdatedElement(s[0]) as Path;
            if (p == null)
            {
                throw new ApplicationException("Error: incorrect geometry ID");
            }
            _currentElement = p;
            var pg = p.Data as PathGeometry;
            var pf = pg.Figures[0];

            switch (controlPointType)
            {
                case "StartPoint":
                    pf.StartPoint = movingEndLocation;
                    ArcStartPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    break;
                case "Point":
                    ((ArcSegment) pf.Segments[0]).Point = movingEndLocation;
                    ArcPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    break;
                default:
                    throw new ApplicationException("Error: incorrect control point type");
            }

            p.Data = pg;
            var xamlstring = XamlWriter.Save(DrawingPane);
            ((TextBox) XAMLPane.Children[0]).Text = xamlstring;
        }

        private void UpdateLineGeometry(Point movingEndLocation, string id)
        {
            var s = id.Split('_');
            var controlPointType = GetContronPointTypeInId(id);
            var p = SearchUpdatedElement(s[0]) as Path;
            if (p == null)
            {
                throw new ApplicationException("Error: incorrect geometry ID");
            }
            _currentElement = p;
            var lg = p.Data as LineGeometry;

            switch (controlPointType)
            {
                case "StartPoint":
                    LineStartPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    lg.StartPoint = movingEndLocation;

                    break;
                case "EndPoint":
                    LineEndPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    lg.EndPoint = movingEndLocation;

                    break;
                default:
                    throw new ApplicationException("Error: Incorrect controlpoint type, '" + controlPointType +
                                                   "' in UpdateLineGeometry()");
            }
            var xamlstring = XamlWriter.Save(DrawingPane);
            ((TextBox) XAMLPane.Children[0]).Text = xamlstring;
        }

        private void UpdateEllipseGeometry(Point movingEndLocation, string id)
        {
            var s = id.Split('_');
            var controlPointType = GetContronPointTypeInId(id);
            var p = SearchUpdatedElement(s[0]) as Path;
            if (p == null)
            {
                throw new ApplicationException("Error: incorrect geometry ID");
            }
            _currentElement = p;
            var eg = p.Data.Clone() as EllipseGeometry;
            var diffX = movingEndLocation.X - eg.Center.X;
            var diffY = movingEndLocation.Y - eg.Center.Y;

            double radians, angle;
            radians = Math.Atan2(diffY, diffX);
            angle = radians*(180/Math.PI);

            switch (controlPointType)
            {
                case "Center":
                    eg.Center = movingEndLocation;

                    //Update the center in the RotateTransform when the ellipsegeometry's center is moved
                    if (eg.Transform is RotateTransform)
                    {
                        var rtWithNewCenter = eg.Transform as RotateTransform;
                        rtWithNewCenter.CenterX = eg.Center.X;
                        rtWithNewCenter.CenterY = eg.Center.Y;
                    }
                    foreach (var o in DesignerPane.Children)
                    {
                        if (o is Ellipse && ((Ellipse) o).Name.Contains(s[0]) && ((Ellipse) o).Name != s[0])
                        {
                            Canvas.SetLeft(((Ellipse) o), Canvas.GetLeft(((Ellipse) o)) + diffX);
                            Canvas.SetTop(((Ellipse) o), Canvas.GetTop(((Ellipse) o)) + diffY);
                        }
                    }
                    p.Data = eg;

                    EllipseCenterPoint.Text = eg.Center.X + " " + eg.Center.Y;
                    break;

                case "TopLeft":
                    var v0 = new Vector(-eg.RadiusX, 0);
                    var v1 = new Vector(diffX, diffY);

                    var rtTopLeft = new RotateTransform(angle + 180, eg.Center.X, eg.Center.Y);
                    eg.Transform = rtTopLeft;
                    eg.RadiusX = v1.Length;
                    EllipseRadiusX.Text = v1.Length.ToString(CultureInfo.InvariantCulture);
                    p.Data = eg;

                    //Update the TopRight control point for this EllipseGeometry
                    var eTopRight = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopRight");
                    Canvas.SetLeft(eTopRight, (eg.Center.X - diffX) - eTopRight.Width/2);
                    Canvas.SetTop(eTopRight, (eg.Center.Y - diffY) - eTopRight.Height/2);

                    var eTopMiddle = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopMiddle");
                    Canvas.SetLeft(eTopMiddle, (eg.Center.X - diffY*eg.RadiusY/v1.Length) - eTopMiddle.Width/2);
                    Canvas.SetTop(eTopMiddle, eg.Center.Y + diffX*eg.RadiusY/v1.Length - eTopMiddle.Height/2);

                    var eBottomMiddle =
                        (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_BottomMiddle");
                    Canvas.SetLeft(eBottomMiddle, (eg.Center.X + diffY*eg.RadiusY/v1.Length) - eBottomMiddle.Width/2);
                    Canvas.SetTop(eBottomMiddle, (eg.Center.Y - diffX*eg.RadiusY/v1.Length) - eBottomMiddle.Height/2);

                    break;
                case "TopMiddle":
                    var v1TopMiddle = new Vector(diffX, diffY);
                    var rtTopMiddle = new RotateTransform(angle + 90, eg.Center.X, eg.Center.Y);
                    eg.Transform = rtTopMiddle;
                    eg.RadiusY = v1TopMiddle.Length;
                    EllipseRadiusY.Text = v1TopMiddle.Length.ToString(CultureInfo.InvariantCulture);
                    p.Data = eg;

                    var eTopLeft2 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopLeft");
                    Canvas.SetLeft(eTopLeft2, (eg.Center.X + diffY*eg.RadiusX/v1TopMiddle.Length) - eTopLeft2.Width/2);
                    Canvas.SetTop(eTopLeft2, (eg.Center.Y - diffX*eg.RadiusX/v1TopMiddle.Length) - eTopLeft2.Height/2);

                    var eTopRight2 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopRight");
                    Canvas.SetLeft(eTopRight2, (eg.Center.X - diffY*eg.RadiusX/v1TopMiddle.Length) - eTopRight2.Width/2);
                    Canvas.SetTop(eTopRight2, (eg.Center.Y + diffX*eg.RadiusX/v1TopMiddle.Length) - eTopRight2.Height/2);

                    var eBottomMiddle2 =
                        (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_BottomMiddle");
                    Canvas.SetLeft(eBottomMiddle2, (eg.Center.X - diffX) - eBottomMiddle2.Width/2);
                    Canvas.SetTop(eBottomMiddle2, (eg.Center.Y - diffY) - eBottomMiddle2.Height/2);
                    break;

                case "TopRight":
                    var v1TopRight = new Vector(diffX, diffY);
                    var rtTopRight = new RotateTransform(angle, eg.Center.X, eg.Center.Y);
                    eg.Transform = rtTopRight;
                    eg.RadiusX = v1TopRight.Length;
                    EllipseRadiusX.Text = v1TopRight.Length.ToString(CultureInfo.InvariantCulture);
                    p.Data = eg;

                    var eTopLeft3 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopLeft");
                    Canvas.SetLeft(eTopLeft3, (eg.Center.X - diffX) - eTopLeft3.Width/2);
                    Canvas.SetTop(eTopLeft3, (eg.Center.Y - diffY) - eTopLeft3.Height/2);

                    var eTopMiddle3 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopMiddle");
                    Canvas.SetLeft(eTopMiddle3, (eg.Center.X + diffY*eg.RadiusY/v1TopRight.Length) - eTopMiddle3.Width/2);
                    Canvas.SetTop(eTopMiddle3, (eg.Center.Y - diffX*eg.RadiusY/v1TopRight.Length) - eTopMiddle3.Height/2);

                    var eBottomMiddle3 =
                        (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_BottomMiddle");
                    Canvas.SetLeft(eBottomMiddle3,
                        (eg.Center.X - diffY*eg.RadiusY/v1TopRight.Length) - eBottomMiddle3.Width/2);
                    Canvas.SetTop(eBottomMiddle3,
                        (eg.Center.Y + diffX*eg.RadiusY/v1TopRight.Length) - eBottomMiddle3.Height/2);
                    break;

                case "BottomMiddle":
                    var v1BottomMiddle = new Vector(diffX, diffY);
                    var rtBottomMiddle = new RotateTransform(angle - 90, eg.Center.X, eg.Center.Y);
                    eg.Transform = rtBottomMiddle;
                    eg.RadiusY = v1BottomMiddle.Length;
                    EllipseRadiusY.Text = v1BottomMiddle.Length.ToString(CultureInfo.InvariantCulture);
                    p.Data = eg;

                    var eTopLeft4 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopLeft");
                    Canvas.SetLeft(eTopLeft4, (eg.Center.X - diffY*eg.RadiusX/v1BottomMiddle.Length) - eTopLeft4.Width/2);
                    Canvas.SetTop(eTopLeft4, (eg.Center.Y + diffX*eg.RadiusX/v1BottomMiddle.Length) - eTopLeft4.Height/2);

                    var eTopRight4 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopRight");
                    Canvas.SetLeft(eTopRight4,
                        (eg.Center.X + diffY*eg.RadiusX/v1BottomMiddle.Length) - eTopRight4.Width/2);
                    Canvas.SetTop(eTopRight4,
                        (eg.Center.Y - diffX*eg.RadiusX/v1BottomMiddle.Length) - eTopRight4.Height/2);

                    var eTopMiddle4 = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopMiddle");
                    Canvas.SetLeft(eTopMiddle4, (eg.Center.X - diffX) - eTopMiddle4.Width/2);
                    Canvas.SetTop(eTopMiddle4, (eg.Center.Y - diffY) - eTopMiddle4.Height/2);
                    break;

                default:
                    throw new ApplicationException("Error: incorrect EllipseGeometry controlpoint type");
            }
            var xamlstring = XamlWriter.Save(DrawingPane);
            ((TextBox) XAMLPane.Children[0]).Text = xamlstring;
        }

        private void UpdateRectangleGeometry(Point movingEndLocation, string id)
        {
            double width, height;
            var s = id.Split('_');
            var controlPointType = GetContronPointTypeInId(id);
            var p = SearchUpdatedElement(s[0]) as Path;
            if (p == null)
            {
                throw new ApplicationException("Error:  incorrect geometry ID");
            }
            _currentElement = p;
            var rg = p.Data as RectangleGeometry;

            //Get all of the related control points in DesignerPane
            var rTopLeft = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopLeft");
            var rTopRight = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_TopRight");
            var rBottomLeft = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_BottomLeft");
            var rBottomRight = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_BottomRight");
            var rCorner = (Ellipse) LogicalTreeHelper.FindLogicalNode(DesignerPane, s[0] + "_Corner");


            switch (controlPointType)
            {
                case "TopLeft":
                    width = rg.Rect.Width + (rg.Rect.X - movingEndLocation.X);
                    height = rg.Rect.Height + (rg.Rect.Y - movingEndLocation.Y);
                    RectangleWidth.Text = width.ToString(CultureInfo.InvariantCulture);
                    RectangleHeight.Text = height.ToString(CultureInfo.InvariantCulture);

                    //Update the RectangleGeometry.
                    rg.Rect = new Rect(movingEndLocation.X, movingEndLocation.Y, width, height);

                    //Update the control points
                    Canvas.SetLeft(rTopRight, (movingEndLocation.X + width) - rTopRight.Width/2);
                    Canvas.SetTop(rTopRight, (movingEndLocation.Y) - rTopRight.Height/2);
                    Canvas.SetLeft(rBottomLeft, movingEndLocation.X - rBottomLeft.Width/2);

                    if (rCorner != null)
                    {
                        Canvas.SetLeft(rCorner, movingEndLocation.X + rg.RadiusX - rCorner.Width/2);
                        Canvas.SetTop(rCorner, movingEndLocation.Y + rg.RadiusY - rCorner.Height/2);
                    }

                    RectangleTopLeftPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    break;
                case "TopRight":
                    width = movingEndLocation.X - rg.Rect.X;
                    height = (rg.Rect.Y + rg.Rect.Height) - movingEndLocation.Y;
                    RectangleWidth.Text = width.ToString(CultureInfo.InvariantCulture);
                    RectangleHeight.Text = height.ToString(CultureInfo.InvariantCulture);

                    //Update the control points
                    Canvas.SetTop(rTopLeft, movingEndLocation.Y - rTopLeft.Height/2);
                    Canvas.SetLeft(rBottomRight, movingEndLocation.X - rBottomRight.Width/2);

                    if (rCorner != null)
                    {
                        Canvas.SetTop(rCorner,
                            Canvas.GetTop(rCorner) + (movingEndLocation.Y - rg.Rect.Y) - rCorner.Height/2);
                    }

                    //Update the RectangleGeometry
                    rg.Rect = new Rect(Canvas.GetLeft(rTopLeft) + rTopLeft.Width/2,
                        Canvas.GetTop(rTopLeft) + rTopLeft.Height/2,
                        width, height);

                    //Update the TopLeft control field
                    RectangleTopLeftPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;

                    break;
                case "BottomLeft":

                    width = rg.Rect.Width + (rg.Rect.X - movingEndLocation.X);
                    height = movingEndLocation.Y - rg.Rect.Y;
                    RectangleWidth.Text = width.ToString(CultureInfo.InvariantCulture);
                    RectangleHeight.Text = height.ToString(CultureInfo.InvariantCulture);

                    //Update the control points
                    Canvas.SetLeft(rTopLeft, movingEndLocation.X - rTopLeft.Width/2);
                    Canvas.SetLeft(rBottomRight, movingEndLocation.X + width - rBottomRight.Width/2);
                    Canvas.SetTop(rBottomRight, movingEndLocation.Y - rBottomRight.Height/2);

                    if (rCorner != null)
                    {
                        Canvas.SetLeft(rCorner, movingEndLocation.X + rg.RadiusX - rCorner.Width/2);
                    }

                    RectangleTopLeftPoint.Text = Canvas.GetLeft(rTopLeft) + rTopLeft.Width/2 + " " +
                                                 Canvas.GetTop(rTopLeft) +
                                                 rTopLeft.Height/2;

                    //Update the RectangleGeometry
                    rg.Rect = new Rect(Canvas.GetLeft(rTopLeft) + rTopLeft.Width/2,
                        Canvas.GetTop(rTopLeft) + rTopLeft.Height/2,
                        width, height);

                    break;

                case "BottomRight":
                    width = movingEndLocation.X - rg.Rect.X;
                    height = movingEndLocation.Y - rg.Rect.Y;
                    RectangleWidth.Text = width.ToString(CultureInfo.InvariantCulture);
                    RectangleHeight.Text = height.ToString(CultureInfo.InvariantCulture);

                    //Update the control points
                    Canvas.SetLeft(rTopRight, movingEndLocation.X - rTopRight.Width/2);
                    Canvas.SetTop(rBottomLeft, movingEndLocation.Y - rBottomLeft.Height/2);

                    //Update the RectangleGeometry
                    rg.Rect = new Rect(Canvas.GetLeft(rTopLeft) + rTopLeft.Width/2,
                        Canvas.GetTop(rTopLeft) + rTopLeft.Height/2,
                        width, height);

                    p.Data = rg;
                    break;
                case "Corner":
                    var radiusx = movingEndLocation.X - rg.Rect.X;
                    var radiusy = movingEndLocation.Y - rg.Rect.Y;

                    if (radiusx <= 0)
                    {
                        Canvas.SetLeft(rCorner, rg.Rect.X + 0.1 - rCorner.Width);
                        radiusx = 0.1;
                    }
                    if (radiusy <= 0)
                    {
                        Canvas.SetTop(rCorner, rg.Rect.Y + 0.1 - rCorner.Height/2);
                        radiusy = 0.1;
                    }

                    RectangleWidth.Text = radiusx.ToString(CultureInfo.InvariantCulture);
                    RectangleHeight.Text = radiusy.ToString(CultureInfo.InvariantCulture);

                    rg.RadiusX = radiusx;
                    rg.RadiusY = radiusy;

                    p.Data = rg;
                    break;
                default:
                    throw new ApplicationException("Error: Incorrect control point type");
            }
            var xamlstring = XamlWriter.Save(DrawingPane);
            ((TextBox) XAMLPane.Children[0]).Text = xamlstring;
        }

        private void UpdateBezierGeometry(Point movingEndLocation, string id)
        {
            var s = id.Split('_');
            var controlPointType = GetContronPointTypeInId(id);
            var p = SearchUpdatedElement(s[0]) as Path;
            if (p == null)
            {
                throw new ApplicationException("Error:  incorrect geometry ID");
            }
            _currentElement = p;
            var pg = p.Data.Clone() as PathGeometry;
            var pf = pg.Figures[0];
            switch (controlPointType)
            {
                case "StartPoint":
                    pf.StartPoint = movingEndLocation;
                    BezierStartPoint.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    break;
                case "Point1":
                    BezierPoint1.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    ((BezierSegment) pf.Segments[0]).Point1 = movingEndLocation;
                    break;
                case "Point2":
                    BezierPoint2.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    ((BezierSegment) pf.Segments[0]).Point2 = movingEndLocation;
                    break;
                case "Point3":
                    BezierPoint3.Text = movingEndLocation.X + " " + movingEndLocation.Y;
                    ((BezierSegment) pf.Segments[0]).Point3 = movingEndLocation;
                    break;
                default:
                    throw new ApplicationException("Error:  Incorrect control point type");
            }

            p.Data = pg;
            var xamlstring = XamlWriter.Save(DrawingPane);
            ((TextBox) XAMLPane.Children[0]).Text = xamlstring;
        }

        private string GetContronPointTypeInId(string id)
        {
            var s = id.Split('_');
            return s[1];
        }

        private string GetGeometryTypeInId(string id)
        {
            var s = id.Split('_');
            if (s[0].Contains("Line"))
            {
                return "Line";
            }
            if (s[0].Contains("Ellipse"))
            {
                return "Ellipse";
            }
            if (s[0].Contains("Rectangle"))
            {
                return "Rectangle";
            }
            if (s[0].Contains("Arc"))
            {
                return "Arc";
            }
            return "Bezier";
        }

        private string GetControlPointName(string id) => null;

        private void AddControlPoints(ArrayList controlPoints, string geometryType)
        {
            switch (geometryType)
            {
                case "Line":
                    AddLineGeometryControlPoints(controlPoints);
                    break;
                case "Ellipse":
                    AddEllipseGeometryControlPoints(controlPoints);
                    break;
                case "Rectangle":
                    AddRectangleGeometryControlPoints(controlPoints);
                    break;
                case "Arc":
                    AddArcGeometryControlPoints(controlPoints);
                    break;
                case "Bezier":
                    AddBezierGeometryControlPoints(controlPoints);
                    break;
                default:
                    throw new ApplicationException("Error:  incorrect Geometry type");
            }
        }


        private static readonly double ControlPointMarkerWidth = 20;
        private static readonly double ControlPointMarkerHeight = 20;

        private void AddLineGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 2)
                throw new ApplicationException("Error:  incorrect # of control points for LineGeometry");

            for (var i = 0; i < controlPoints.Count; i++)
            {
                var e = new Ellipse
                {
                    Visibility = Visibility.Hidden,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = Brushes.White,
                    Opacity = 0.5,
                    Width = 3,
                    Height = 3
                };

                if (i == 0)
                {
                    e.Name = "Line" + _lineCount + "_StartPoint";
                }
                else
                {
                    e.Name = "Line" + _lineCount + "_EndPoint";
                }

                e.Width = ControlPointMarkerWidth;
                e.Height = ControlPointMarkerHeight;
                Canvas.SetLeft(e, ((Point) controlPoints[i]).X - e.Width/2);
                Canvas.SetTop(e, ((Point) controlPoints[i]).Y - e.Height/2);


                e.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                e.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                e.MouseMove += Ellipse_MouseMove;

                //Add the control point to the Designer Pane
                //DesignerPane.Children.Add(e);
                DesignerPane.Children.Insert(DesignerPane.Children.Count - 1, e);
            }
        }

        private void AddEllipseGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 5)
            {
                throw new ApplicationException("Error:  incorrect # of control points for EllipseGeometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                var e = new Ellipse
                {
                    Visibility = Visibility.Hidden,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = Brushes.White,
                    Opacity = 0.5,
                    Width = 3,
                    Height = 3
                };

                switch (i)
                {
                    case 0:
                        e.Name = "Ellipse" + _elliipseCount + "_Center";
                        break;
                    case 1:
                        e.Name = "Ellipse" + _elliipseCount + "_TopLeft";
                        break;
                    case 2:
                        e.Name = "Ellipse" + _elliipseCount + "_TopMiddle";
                        break;
                    case 3:
                        e.Name = "Ellipse" + _elliipseCount + "_TopRight";
                        break;
                    case 4:
                        e.Name = "Ellipse" + _elliipseCount + "_BottomMiddle";
                        break;
                    default:
                        throw new ApplicationException("Error: Incorrect control point ");
                }

                e.Width = ControlPointMarkerWidth;
                e.Height = ControlPointMarkerHeight;
                Canvas.SetLeft(e, ((Point) controlPoints[i]).X - e.Width/2);
                Canvas.SetTop(e, ((Point) controlPoints[i]).Y - e.Height/2);

                e.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                e.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                e.MouseMove += Ellipse_MouseMove;

                //Add the control point to the Designer Pane
                DesignerPane.Children.Insert(DesignerPane.Children.Count - 1, e);
            }
        }

        private void AddRectangleGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 5 && controlPoints.Count != 4)
            {
                throw new ApplicationException("Error:  incorrect # of control points for RectangleGeometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                var e = new Ellipse
                {
                    Visibility = Visibility.Hidden,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = Brushes.White,
                    Opacity = 0.5,
                    Width = 3,
                    Height = 3
                };

                switch (i)
                {
                    case 0:
                        e.Name = "Rectangle" + _rectangleCount + "_TopLeft";
                        break;
                    case 1:
                        e.Name = "Rectangle" + _rectangleCount + "_TopRight";
                        break;
                    case 2:
                        e.Name = "Rectangle" + _rectangleCount + "_BottomLeft";
                        break;
                    case 3:
                        e.Name = "Rectangle" + _rectangleCount + "_BottomRight";
                        break;
                    case 4:
                        e.Name = "Rectangle" + _rectangleCount + "_Corner";
                        break;
                    default:
                        throw new ApplicationException("Error: Incorrect control point ");
                }


                e.Width = ControlPointMarkerWidth;
                e.Height = ControlPointMarkerHeight;
                Canvas.SetLeft(e, ((Point) controlPoints[i]).X - e.Width/2);
                Canvas.SetTop(e, ((Point) controlPoints[i]).Y - e.Height/2);
                e.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                e.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                e.MouseMove += Ellipse_MouseMove;

                //Add the control point to the Designer Pane
                DesignerPane.Children.Insert(DesignerPane.Children.Count - 1, e);
            }
        }

        private void AddArcGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 2)
            {
                throw new ApplicationException("Error:  incorrect # of control points for Arc Geometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                var e = new Ellipse
                {
                    Visibility = Visibility.Hidden,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = Brushes.White,
                    Opacity = 0.5,
                    Width = 3,
                    Height = 3
                };

                switch (i)
                {
                    case 0:
                        e.Name = "Arc" + _arcCount + "_StartPoint";
                        break;
                    case 1:
                        e.Name = "Arc" + _arcCount + "_Point";
                        break;
                    default:
                        throw new ApplicationException("Error: Incorrect control point ");
                }

                e.Width = ControlPointMarkerWidth;
                e.Height = ControlPointMarkerHeight;
                Canvas.SetLeft(e, ((Point) controlPoints[i]).X - e.Width/2);
                Canvas.SetTop(e, ((Point) controlPoints[i]).Y - e.Height/2);

                e.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                e.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                e.MouseMove += Ellipse_MouseMove;

                //Add the control point to the Designer Pane
                //DesignerPane.Children.Insert(DesignerPane.Children.Count - 2, e);
                DesignerPane.Children.Insert(DesignerPane.Children.Count - 1, e);
            }
        }

        private void AddBezierGeometryControlPoints(ArrayList controlPoints)
        {
            if (controlPoints.Count != 4)
            {
                throw new ApplicationException("Error:  incorrect # of control points for Bezier Geometry");
            }
            for (var i = 0; i < controlPoints.Count; i++)
            {
                var e = new Ellipse
                {
                    Visibility = Visibility.Hidden,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = Brushes.White,
                    Opacity = 0.5,
                    Width = 3,
                    Height = 3
                };

                switch (i)
                {
                    case 0:
                        e.Name = "Bezier" + _bezierCount + "_StartPoint";
                        break;
                    case 1:
                        e.Name = "Bezier" + _bezierCount + "_Point1";
                        break;
                    case 2:
                        e.Name = "Bezier" + _bezierCount + "_Point2";
                        break;
                    case 3:
                        e.Name = "Bezier" + _bezierCount + "_Point3";
                        break;
                    default:
                        throw new ApplicationException("Error: Incorrect control point ");
                }

                e.Width = ControlPointMarkerWidth;
                e.Height = ControlPointMarkerHeight;
                Canvas.SetLeft(e, ((Point) controlPoints[i]).X - e.Width/2);
                Canvas.SetTop(e, ((Point) controlPoints[i]).Y - e.Height/2);
                e.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                e.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;
                e.MouseMove += Ellipse_MouseMove;

                //Add the control point to the Designer Pane
                DesignerPane.Children.Insert(DesignerPane.Children.Count - 1, e);
            }
        }

        /// <summary>
        ///     The function takes the design pane element,e.g. LinePane, RectanglePane,
        ///     and extract the value from the different control point fields from the pane and construct
        ///     a Path with the correct Geometry
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        private GeometryBase GeometryFactory(FrameworkElement pane)
        {
            switch (pane.Name.Remove(pane.Name.LastIndexOf("Pane", StringComparison.Ordinal)))
            {
                case "Line":
                    return new LineG(pane);
                case "Ellipse":
                    return new EllipseG(pane);
                case "Rectangle":
                    return new RectanlgeG(pane);
                case "Arc":
                    return new ArcG(pane);
                case "Bezier":
                    return new BezierG(pane);
                default:
                    throw new ApplicationException("Error:  Unknow Geometry name?");
            }
        }

        #endregion

        #region Data members

        private bool _initialSelection = true;
        private int _lineCount = 1;
        private int _elliipseCount = 1;
        private int _rectangleCount = 1;
        private int _arcCount = 1;
        private int _bezierCount = 1;
        private bool _isShow;
        private Path _currentElement;
        private bool _isInsert = true;

        #endregion
    }
}