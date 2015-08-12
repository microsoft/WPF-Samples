// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AnimationExamples
{
    public partial class SplineExample : Page
    {
        private Point _controlPoint1 = new Point(0, 100);
        private Point _controlPoint2 = new Point(0, 100);

        private void OnSliderChanged(object sender, RoutedEventArgs e)
        {
            // Retrieve the name of slider.
            var name = ((Slider) sender).Name;

            var args = e as RoutedPropertyChangedEventArgs<double>;

            switch (name)
            {
                case "SliderControlPoint1X":
                    mySplineKeyFrame.KeySpline.ControlPoint1 = new Point(args.NewValue,
                        mySplineKeyFrame.KeySpline.ControlPoint1.Y);
                    _controlPoint1.X = 100*args.NewValue;
                    break;
                case "SliderControlPoint1Y":
                    mySplineKeyFrame.KeySpline.ControlPoint1 = new Point(mySplineKeyFrame.KeySpline.ControlPoint1.X,
                        args.NewValue);
                    _controlPoint1.Y = 100 - (100*args.NewValue);
                    break;
                case "SliderControlPoint2X":
                    mySplineKeyFrame.KeySpline.ControlPoint2 = new Point(args.NewValue,
                        mySplineKeyFrame.KeySpline.ControlPoint2.Y);
                    _controlPoint2.X = 100*args.NewValue;
                    break;
                case "SliderControlPoint2Y":
                    mySplineKeyFrame.KeySpline.ControlPoint2 = new Point(mySplineKeyFrame.KeySpline.ControlPoint2.X,
                        args.NewValue);
                    _controlPoint2.Y = 100 - (100*args.NewValue);
                    break;
            }


            // Update the animations and illustrations.
            myVector3DSplineKeyFrame.KeySpline.ControlPoint1 = mySplineKeyFrame.KeySpline.ControlPoint1;
            myVector3DSplineKeyFrame.KeySpline.ControlPoint2 = mySplineKeyFrame.KeySpline.ControlPoint2;


            SplineIllustrationSegment.Point1 = _controlPoint1;
            SplineIllustrationSegment.Point2 = _controlPoint2;
            SplineControlPoint1Marker.Center = _controlPoint1;
            SplineControlPoint2Marker.Center = _controlPoint2;

            keySplineText.Text =
                "KeySpline=\"" + mySplineKeyFrame.KeySpline.ControlPoint1.X.ToString("N") + "," +
                mySplineKeyFrame.KeySpline.ControlPoint1.Y.ToString("N") + " " +
                mySplineKeyFrame.KeySpline.ControlPoint2.X.ToString("N") + "," +
                mySplineKeyFrame.KeySpline.ControlPoint2.Y.ToString("N") + "\"";

            // Determine the storyboard's current time.
            TimeSpan? oldTime = ExampleStoryboard.GetCurrentTime(this) ?? TimeSpan.FromSeconds(0);

            // Generate new clocks for the animations by calling 
            // the Begin method.
            ExampleStoryboard.Begin(this, true);

            // Because the storyboard was reset, advance it to its previous
            // position using the Seek method.
            ExampleStoryboard.Seek(this, (TimeSpan) oldTime, TimeSeekOrigin.BeginTime);
        }
    }
}