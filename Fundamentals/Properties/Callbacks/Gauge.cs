// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace Callbacks
{
    public class Gauge : Control
    {
        public static readonly DependencyProperty CurrentReadingProperty = DependencyProperty.Register(
            "CurrentReading",
            typeof (double),
            typeof (Gauge),
            new FrameworkPropertyMetadata(
                double.NaN,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnCurrentReadingChanged,
                CoerceCurrentReading
                ),
            IsValidReading
            );

        public static readonly DependencyProperty MaxReadingProperty = DependencyProperty.Register(
            "MaxReading",
            typeof (double),
            typeof (Gauge),
            new FrameworkPropertyMetadata(
                double.NaN,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnMaxReadingChanged,
                CoerceMaxReading
                ),
            IsValidReading
            );

        public static readonly DependencyProperty MinReadingProperty = DependencyProperty.Register(
            "MinReading",
            typeof (double),
            typeof (Gauge),
            new FrameworkPropertyMetadata(
                double.NaN,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnMinReadingChanged,
                CoerceMinReading
                ),
            IsValidReading);

        public double CurrentReading
        {
            get { return (double) GetValue(CurrentReadingProperty); }
            set { SetValue(CurrentReadingProperty, value); }
        }

        public double MaxReading
        {
            get { return (double) GetValue(MaxReadingProperty); }
            set { SetValue(MaxReadingProperty, value); }
        }

        public double MinReading
        {
            get { return (double) GetValue(MinReadingProperty); }
            set { SetValue(MinReadingProperty, value); }
        }

        public static bool IsValidReading(object value)
        {
            var v = (double) value;
            return (!v.Equals(double.NegativeInfinity) && !v.Equals(double.PositiveInfinity));
        }

        private static object CoerceCurrentReading(DependencyObject d, object value)
        {
            var g = (Gauge) d;
            var current = (double) value;
            if (current < g.MinReading) current = g.MinReading;
            if (current > g.MaxReading) current = g.MaxReading;
            return current;
        }

        private static void OnCurrentReadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MinReadingProperty);
            d.CoerceValue(MaxReadingProperty);
        }

        private static void OnMaxReadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MinReadingProperty);
            d.CoerceValue(CurrentReadingProperty);
        }

        private static object CoerceMaxReading(DependencyObject d, object value)
        {
            var g = (Gauge) d;
            var max = (double) value;
            if (max < g.MinReading) max = g.MinReading;
            return max;
        }

        private static void OnMinReadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MaxReadingProperty);
            d.CoerceValue(CurrentReadingProperty);
        }

        private static object CoerceMinReading(DependencyObject d, object value)
        {
            var g = (Gauge) d;
            var min = (double) value;
            if (min > g.MaxReading) min = g.MaxReading;
            return min;
        }
    }
}