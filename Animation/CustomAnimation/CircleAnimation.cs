// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace CustomAnimation
{
    /// <summary>
    ///     CircleAnimation: calculates polar coordinates as a function of time.
    ///     Use two of these (XDirection and YDirection) to move an element in an elliptical manner
    /// </summary>
    public class CircleAnimation : DoubleAnimationBase
    {
        public enum DirectionEnum
        {
            XDirection,
            YDirection
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof (DirectionEnum), typeof (CircleAnimation),
                new PropertyMetadata(DirectionEnum.XDirection));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof (double), typeof (CircleAnimation),
                new PropertyMetadata((double) 10));

        /// <summary>
        ///     distance from origin to polar coordinate
        /// </summary>
        public double Radius
        {
            get { return (double) GetValue(RadiusProperty); }
            set
            {
                if (value > 0.0)
                {
                    SetValue(RadiusProperty, value);
                }
                else
                {
                    throw new ArgumentException("a radius of " + value + " is not allowed!");
                }
            }
        }

        /// <summary>
        ///     are we measuring in the X or Y direction?
        /// </summary>
        public DirectionEnum Direction
        {
            get { return (DirectionEnum) GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue,
            AnimationClock clock)
        {
            var time = clock.CurrentProgress.Value;

            // math magic: calculate new coordinates using polar coordinate equations. This requires two 
            // animations to be wired up in order to move in a circle, since we don't make any assumptions
            // about what we're animating (e.g. a TranslateTransform). 
            var returnValue = Direction == DirectionEnum.XDirection
                ? Math.Cos(2*Math.PI*time)
                : Math.Sin(2*Math.PI*time);

            // Need to add the defaultOriginValue so that composition works.
            return returnValue*Radius + defaultOriginValue;
        }

        protected override Freezable CreateInstanceCore() => new CircleAnimation();
    }
}