// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AnimationTiming
{
    public class ElapsedTimeControl : Control
    {
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(
                "CurrentTime",
                typeof (TimeSpan?),
                typeof (ElapsedTimeControl),
                new FrameworkPropertyMetadata(
                    null,
                    currentTime_Changed));

        public static readonly DependencyProperty CurrentTimeAsStringProperty =
            DependencyProperty.Register("CurrentTimeAsString", typeof (string),
                typeof (ElapsedTimeControl));

        private TimeSpan? _previousTime;
        private Clock _theClock;

        public ElapsedTimeControl(TimeSpan? previousTime)
        {
            _previousTime = previousTime;
        }

        public ElapsedTimeControl()
        {
        }

        public Clock Clock
        {
            get { return _theClock; }
            set
            {
                if (_theClock != null)
                {
                    _theClock.CurrentTimeInvalidated -= OnClockTimeInvalidated;
                }

                _theClock = value;

                if (_theClock != null)
                {
                    _theClock.CurrentTimeInvalidated += OnClockTimeInvalidated;
                }
            }
        }

        public string CurrentTimeAsString
        {
            get { return GetValue(CurrentTimeAsStringProperty) as string; }
            set { SetValue(CurrentTimeAsStringProperty, value); }
        }

        private void OnClockTimeInvalidated(object sender, EventArgs args)
        {
            SetValue(CurrentTimeProperty, _theClock.CurrentTime);
        }

        private static void currentTime_Changed(DependencyObject d,
            DependencyPropertyChangedEventArgs args)
        {
            ((ElapsedTimeControl) d).SetValue(CurrentTimeAsStringProperty, args.NewValue.ToString());
        }
    }
}