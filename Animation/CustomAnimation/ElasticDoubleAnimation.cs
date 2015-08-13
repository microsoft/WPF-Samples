// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace CustomAnimation
{
    /// <summary>
    ///     ElasticDoubleAnimation - like something attached to a rubber band
    /// </summary>
    public class ElasticDoubleAnimation : DoubleAnimationBase
    {
        public enum EdgeBehaviorEnum
        {
            EaseIn,
            EaseOut,
            EaseInOut
        }

        public static readonly DependencyProperty EdgeBehaviorProperty =
            DependencyProperty.Register("EdgeBehavior", typeof (EdgeBehaviorEnum), typeof (ElasticDoubleAnimation),
                new PropertyMetadata(EdgeBehaviorEnum.EaseIn));

        public static readonly DependencyProperty SpringinessProperty =
            DependencyProperty.Register("Springiness", typeof (double), typeof (ElasticDoubleAnimation),
                new PropertyMetadata(3.0));

        public static readonly DependencyProperty OscillationsProperty =
            DependencyProperty.Register("Oscillations", typeof (double), typeof (ElasticDoubleAnimation),
                new PropertyMetadata(10.0));

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From",
                typeof (double?),
                typeof (ElasticDoubleAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To",
                typeof (double?),
                typeof (ElasticDoubleAnimation),
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
        ///     how much springiness is there in the effect
        /// </summary>
        public double Springiness
        {
            get { return (double) GetValue(SpringinessProperty); }
            set { SetValue(SpringinessProperty, value); }
        }

        /// <summary>
        ///     number of oscillations in the effect
        /// </summary>
        public double Oscillations
        {
            get { return (double) GetValue(OscillationsProperty); }
            set { SetValue(OscillationsProperty, value); }
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
                    returnValue = EaseIn(clock.CurrentProgress.Value, start, delta, Springiness, Oscillations);
                    break;
                case EdgeBehaviorEnum.EaseOut:
                    returnValue = EaseOut(clock.CurrentProgress.Value, start, delta, Springiness, Oscillations);
                    break;
                default:
                    returnValue = EaseInOut(clock.CurrentProgress.Value, start, delta, Springiness, Oscillations);
                    break;
            }
            return returnValue;
        }

        protected override Freezable CreateInstanceCore() => new ElasticDoubleAnimation();

        private static double EaseOut(double timeFraction, double start, double delta, double springiness,
            double oscillations)
        {
            // math magic: The cosine gives us the right wave, the timeFraction * the # of oscillations is the 
            // frequency of the wave, and the amplitude (the exponent) makes the wave get smaller at the end
            // by the "springiness" factor. This is extremely similar to the bounce equation.
            var returnValue = Math.Pow((1 - timeFraction), springiness)
                              *Math.Cos(2*Math.PI*timeFraction*oscillations);
            returnValue = delta - (returnValue*delta);
            returnValue += start;
            return returnValue;
        }

        private static double EaseIn(double timeFraction, double start, double delta, double springiness,
            double oscillations)
        {
            // math magic: The cosine gives us the right wave, the timeFraction * the # of oscillations is the 
            // frequency of the wave, and the amplitude (the exponent) makes the wave get smaller at the beginning
            // by the "springiness" factor. This is extremely similar to the bounce equation. 
            var returnValue = Math.Pow((timeFraction), springiness)
                              *Math.Cos(2*Math.PI*timeFraction*oscillations);
            returnValue = returnValue*delta;
            returnValue += start;
            return returnValue;
        }

        private static double EaseInOut(double timeFraction, double start, double delta, double springiness,
            double oscillations)
        {
            double returnValue;

            // we cut each effect in half by multiplying the time fraction by two and halving the distance.
            if (timeFraction <= 0.5)
            {
                return EaseIn(timeFraction*2, start, delta/2, springiness, oscillations);
            }
            returnValue = EaseOut((timeFraction - 0.5)*2, start, delta/2, springiness, oscillations);
            returnValue += (delta/2);
            return returnValue;
        }
    }
}