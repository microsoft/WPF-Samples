// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationExamples
{
    /// <summary>
    ///     ExponentialDoubleAnimation - gets exponentially faster / slower
    /// </summary>
    public class ExponentialDoubleAnimation : DoubleAnimationBase
    {
        public enum EdgeBehaviorEnum
        {
            EaseIn,
            EaseOut,
            EaseInOut
        }

        public static readonly DependencyProperty EdgeBehaviorProperty =
            DependencyProperty.Register("EdgeBehavior", typeof (EdgeBehaviorEnum), typeof (ExponentialDoubleAnimation),
                new PropertyMetadata(EdgeBehaviorEnum.EaseIn));

        public static readonly DependencyProperty PowerProperty =
            DependencyProperty.Register("Power", typeof (double), typeof (ExponentialDoubleAnimation),
                new PropertyMetadata(2.0));

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From",
                typeof (double?),
                typeof (ExponentialDoubleAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To",
                typeof (double?),
                typeof (ExponentialDoubleAnimation),
                new PropertyMetadata(null));

        /// <summary>
        ///     which side gets the effect
        /// </summary>
        public EdgeBehaviorEnum EdgeBehavior
        {
            get { return (EdgeBehaviorEnum) GetValue(EdgeBehaviorProperty); }
            set { SetValue(EdgeBehaviorProperty, value); }
        }

        /// <summary>
        ///     exponential rate of growth
        /// </summary>
        public double Power
        {
            get { return (double) GetValue(PowerProperty); }
            set
            {
                if (value > 0.0)
                {
                    SetValue(PowerProperty, value);
                }
                else
                {
                    throw new ArgumentException("cannot set power to less than 0.0. Value: " + value);
                }
            }
        }

        /// <summary>
        ///     Specifies the starting value of the animation.
        /// </summary>
        public double? From
        {
            get { return (double?) GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        /// <summary>
        ///     Specifies the ending value of the animation.
        /// </summary>
        public double? To
        {
            get { return (double?) GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue,
            AnimationClock clock)
        {
            double returnValue;
            var start = From ?? defaultOriginValue;
            var delta = To - start ?? defaultOriginValue - start;

            switch (EdgeBehavior)
            {
                case EdgeBehaviorEnum.EaseIn:
                    returnValue = EaseIn(clock.CurrentProgress.Value, start, delta, Power);
                    break;
                case EdgeBehaviorEnum.EaseOut:
                    returnValue = EaseOut(clock.CurrentProgress.Value, start, delta, Power);
                    break;
                default:
                    returnValue = EaseInOut(clock.CurrentProgress.Value, start, delta, Power);
                    break;
            }
            return returnValue;
        }

        protected override Freezable CreateInstanceCore() => new ExponentialDoubleAnimation();

        private static double EaseIn(double timeFraction, double start, double delta, double power)
        {
            // math magic: simple exponential growth
            var returnValue = Math.Pow(timeFraction, power);
            returnValue *= delta;
            returnValue = returnValue + start;
            return returnValue;
        }

        private static double EaseOut(double timeFraction, double start, double delta, double power)
        {
            // math magic: simple exponential decay
            var returnValue = Math.Pow(timeFraction, 1/power);
            returnValue *= delta;
            returnValue = returnValue + start;
            return returnValue;
        }

        private static double EaseInOut(double timeFraction, double start, double delta, double power)
        {
            double returnValue;

            // we cut each effect in half by multiplying the time fraction by two and halving the distance.
            if (timeFraction <= 0.5)
            {
                returnValue = EaseOut(timeFraction*2, start, delta/2, power);
            }
            else
            {
                returnValue = EaseIn((timeFraction - 0.5)*2, start, delta/2, power);
                returnValue += (delta/2);
            }
            return returnValue;
        }
    }
}