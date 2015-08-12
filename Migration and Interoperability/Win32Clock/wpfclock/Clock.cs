// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WPFClock
{
    /// <summary>
    ///     Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : Page
    {
        private DispatcherTimer _dayTimer;

        public Clock()
        {
            InitializeComponent();
            Loaded += Clock_Loaded;
        }

        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {
            // set the datacontext to be today's date
            var now = DateTime.Now;
            DataContext = now.Day.ToString();

            // then set up a timer to fire at the start of tomorrow, so that we can update
            // the datacontext
            _dayTimer = new DispatcherTimer {Interval = new TimeSpan(1, 0, 0, 0) - now.TimeOfDay};
            _dayTimer.Tick += OnDayChange;
            _dayTimer.Start();

            // finally, seek the timeline, which assumes a beginning at midnight, to the appropriate
            // offset
            var sb = (Storyboard) PodClock.FindResource("sb");
            sb.Begin(PodClock, HandoffBehavior.SnapshotAndReplace, true);
            sb.Seek(PodClock, now.TimeOfDay, TimeSeekOrigin.BeginTime);
        }

        private void OnDayChange(object sender, EventArgs e)
        {
            // date has changed, update the datacontext to reflect today's date
            var now = DateTime.Now;
            DataContext = now.Day.ToString();
            _dayTimer.Interval = new TimeSpan(1, 0, 0, 0);
        }
    }
}

//</SnippetAllClockCS>