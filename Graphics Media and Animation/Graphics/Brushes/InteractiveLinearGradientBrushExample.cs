// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brushes
{
    /// <summary>
    ///     Enables the user to configure a LinearGradientBrush interactively.
    /// </summary>
    public partial class InteractiveLinearGradientBrushExample : Page
    {
        public static readonly DependencyProperty SelectedMarkerProperty =
            DependencyProperty.Register
                ("SelectedMarker", typeof (Shape), typeof (InteractiveLinearGradientBrushExample),
                    new PropertyMetadata(null));

        public InteractiveLinearGradientBrushExample()
        {
            InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs s)
        {
            MappingModeComboBox.SelectionChanged += MappingModeChanged;
            OnStartPointTextBoxKeyUp(StartPointTextBox, null);
            OnEndPointTextBoxKeyUp(EndPointTextBox, null);
        }

        // Update the StartPoint and EndPoint markers when the gradient display
        // element's size changes.
        private void GradientDisplaySizeChanged(object sender, SizeChangedEventArgs e)
        {
            // The marker positions only need recalcutated if the brush's MappingMode
            // is RelativeToBoundingBox.
            if (InteractiveLinearGradientBrush.MappingMode ==
                BrushMappingMode.RelativeToBoundingBox)
            {
                StartPointMarkerTranslateTransform.X =
                    InteractiveLinearGradientBrush.StartPoint.X*e.NewSize.Width;
                StartPointMarkerTranslateTransform.Y =
                    InteractiveLinearGradientBrush.StartPoint.Y*e.NewSize.Height;

                EndPointMarkerTranslateTransform.X =
                    InteractiveLinearGradientBrush.EndPoint.X*e.NewSize.Width;
                EndPointMarkerTranslateTransform.Y =
                    InteractiveLinearGradientBrush.EndPoint.Y*e.NewSize.Height;
            }
        }

        private void OnStartPointTextBoxKeyUp(object sender, KeyEventArgs args)
        {
            var t = (TextBox) sender;
            try
            {
                var p = Point.Parse(t.Text);
                if (InteractiveLinearGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                {
                    StartPointMarkerTranslateTransform.X = p.X*GradientDisplayElement.ActualWidth;
                    StartPointMarkerTranslateTransform.Y = p.Y*GradientDisplayElement.ActualHeight;
                }
                else
                {
                    StartPointMarkerTranslateTransform.X = p.X;
                    StartPointMarkerTranslateTransform.Y = p.Y;
                }
            }
            catch (InvalidOperationException)
            {
                // Ignore errors.
            }
            catch (FormatException)
            {
                // Ignore errors.
            }
        }

        private void OnEndPointTextBoxKeyUp(object sender, KeyEventArgs args)
        {
            var t = (TextBox) sender;
            try
            {
                var p = Point.Parse(t.Text);
                if (InteractiveLinearGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                {
                    EndPointMarkerTranslateTransform.X = p.X*GradientDisplayElement.ActualWidth;
                    EndPointMarkerTranslateTransform.Y = p.Y*GradientDisplayElement.ActualHeight;
                }
                else
                {
                    EndPointMarkerTranslateTransform.X = p.X;
                    EndPointMarkerTranslateTransform.Y = p.Y;
                }
            }
            catch (InvalidOperationException)
            {
                // Ignore errors.
            }
            catch (FormatException)
            {
                // Ignore errors.
            }
        }

        // Determine whether the user clicked a marker.
        private void GradientDisplayMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Shape)
            {
                SetValue(SelectedMarkerProperty, (Shape) e.OriginalSource);
            }
            else
                SetValue(SelectedMarkerProperty, null);
        }

        // Determines whether the user just finished dragging a marker. If so,
        // this method updates the brush's StartPoint or EndPoint property,
        // depending on which marker was dragged. 
        private void GradientDisplayMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition(GradientDisplayElement);
            var s = (Shape) GetValue(SelectedMarkerProperty);
            if (s == EndPointMarker || s == StartPointMarker)
            {
                var translation = (TranslateTransform) s.RenderTransform;
                translation.X = clickPoint.X;
                translation.Y = clickPoint.Y;
                SetValue(SelectedMarkerProperty, null);
                Mouse.Synchronize();

                Point p;
                if (InteractiveLinearGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                {
                    p = new Point(clickPoint.X/GradientDisplayElement.ActualWidth,
                        clickPoint.Y/GradientDisplayElement.ActualHeight);
                }
                else
                {
                    p = clickPoint;
                }

                if (s == StartPointMarker)
                {
                    InteractiveLinearGradientBrush.StartPoint = p;
                    StartPointTextBox.Text = p.X.ToString("F4") + "," + p.Y.ToString("F4");
                }
                else
                {
                    InteractiveLinearGradientBrush.EndPoint = p;
                    EndPointTextBox.Text = p.X.ToString("F4") + "," + p.Y.ToString("F4");
                }
            }
        }

        // Update the StartPoint or EndPoint when the user drags one of the
        // points with the mouse. 
        private void GradientDisplayMouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = e.GetPosition(GradientDisplayElement);
            var s = (Shape) GetValue(SelectedMarkerProperty);

            // Determine whether the user dragged a StartPoint or
            // EndPoint marker.
            if (s == EndPointMarker || s == StartPointMarker)
            {
                // Move the selected marker to the current mouse position.
                var translation = (TranslateTransform) s.RenderTransform;
                translation.X = currentPoint.X;
                translation.Y = currentPoint.Y;
                Mouse.Synchronize();

                Point p;

                // Calculate the StartPoint or EndPoint.
                if (InteractiveLinearGradientBrush.MappingMode ==
                    BrushMappingMode.RelativeToBoundingBox)
                {
                    // If the MappingMode is relative, compute the relative
                    // value of the new point.
                    p = new Point(currentPoint.X/GradientDisplayElement.ActualWidth,
                        currentPoint.Y/GradientDisplayElement.ActualHeight);
                }
                else
                {
                    // If the MappingMode is absolute, there's no more
                    // work to do.
                    p = currentPoint;
                }

                if (s == StartPointMarker)
                {
                    // If the selected marker is the StartPoint marker,
                    // update the brush's StartPoint.
                    InteractiveLinearGradientBrush.StartPoint = p;
                    StartPointTextBox.Text = p.X.ToString("F4") + "," + p.Y.ToString("F4");
                }
                else
                {
                    // Otherwise, update the brush's EndPoint.
                    InteractiveLinearGradientBrush.EndPoint = p;
                    EndPointTextBox.Text = p.X.ToString("F4") + "," + p.Y.ToString("F4");
                }
            }
        }

        // Updates the StartPoint and EndPoint and their markers when
        // the user changes the brush's MappingMode.
        private void MappingModeChanged(object sender, SelectionChangedEventArgs e)
        {
            var oldStartPoint = InteractiveLinearGradientBrush.StartPoint;
            var newStartPoint = new Point();
            var oldEndPoint = InteractiveLinearGradientBrush.EndPoint;
            var newEndPoint = new Point();


            if (InteractiveLinearGradientBrush.MappingMode ==
                BrushMappingMode.RelativeToBoundingBox)
            {
                // The MappingMode changed from absolute to relative.
                // To find the new relative point, divide the old absolute points
                // by the painted area's width and height. 
                newStartPoint.X = oldStartPoint.X/GradientDisplayElement.ActualWidth;
                newStartPoint.Y = oldStartPoint.Y/GradientDisplayElement.ActualHeight;
                InteractiveLinearGradientBrush.StartPoint = newStartPoint;

                newEndPoint.X = oldEndPoint.X/GradientDisplayElement.ActualWidth;
                newEndPoint.Y = oldEndPoint.Y/GradientDisplayElement.ActualHeight;
                InteractiveLinearGradientBrush.EndPoint = newEndPoint;
            }
            else
            {
                // The MappingMode changed from relative to absolute.
                // To find the new absolute point, multiply the old relative points
                // by the painted area's width and height. 
                newStartPoint.X = oldStartPoint.X*GradientDisplayElement.ActualWidth;
                newStartPoint.Y = oldStartPoint.Y*GradientDisplayElement.ActualHeight;
                InteractiveLinearGradientBrush.StartPoint = newStartPoint;

                newEndPoint.X = oldEndPoint.X*GradientDisplayElement.ActualWidth;
                newEndPoint.Y = oldEndPoint.Y*GradientDisplayElement.ActualHeight;

                InteractiveLinearGradientBrush.EndPoint = newEndPoint;
            }

            // Update the StartPoint and EndPoint display text.
            StartPointTextBox.Text = newStartPoint.X.ToString("F4") +
                                     "," + newStartPoint.Y.ToString("F4");
            EndPointTextBox.Text = newEndPoint.X.ToString("F4") +
                                   "," + newEndPoint.Y.ToString("F4");
        }

        // Update the markup display whenever the brush changes.
        private void OnInteractiveLinearGradientBrushChanged(object sender, EventArgs e)
        {
            if (GradientDisplayElement != null)
            {
                markupOutputTextBlock.Text =
                    GenerateLinearGradientBrushMarkup(InteractiveLinearGradientBrush);
            }
        }

        // Helper method that displays the markup of interest for
        // creating the specified brush.
        private static string GenerateLinearGradientBrushMarkup(LinearGradientBrush theBrush)
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("<" + theBrush.GetType().Name + "\n" +
                            "  StartPoint=\"" + theBrush.StartPoint + "\"" +
                            "  EndPoint=\"" + theBrush.EndPoint + "\" \n" +
                            "  MappingMode=\"" + theBrush.MappingMode + "\"" +
                            "  SpreadMethod=\"" + theBrush.SpreadMethod + "\"\n" +
                            "  ColorInterpolationMode=\"" + theBrush.ColorInterpolationMode + "\"" +
                            "  Opacity=\"" + theBrush.Opacity.ToString(CultureInfo.InvariantCulture) + "\"" + ">\n");

            foreach (var stop in theBrush.GradientStops)
            {
                sBuilder.Append
                    (
                        "  <GradientStop Offset=\"" + stop.Offset.ToString("F4")
                        + "\" Color=\"" + stop.Color + "\" />\n"
                    );
            }
            sBuilder.Append("</LinearGradientBrush>");
            return sBuilder.ToString();
        }
    }
}