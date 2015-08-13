// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MediaGallery
{
    public partial class MediaTimelineExample : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;

        public MediaTimelineExample()
        {
            InitializeComponent();
        }

        public double MyProp
        {
            get { return ClickedBSB.Storyboard.GetCurrentTime(DocumentRoot).Value.TotalSeconds; }

            set
            {
                ClickedBSB.Storyboard.SeekAlignedToLastTick(DocumentRoot,
                    new TimeSpan((long) Math.Floor(value*TimeSpan.TicksPerSecond)), TimeSeekOrigin.BeginTime);
                OnPropertyChanged("MyProp");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var b2 = new Binding
            {
                Source = this,
                Path = new PropertyPath("MyProp")
            };

            // Bind to the slider and the textbox
            BindingOperations.SetBinding(ClockSlider, RangeBase.ValueProperty, b2);
            BindingOperations.SetBinding(PositionTextBox, TextBox.TextProperty, b2);

            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 100)};

            // Every tick, the timer_Tick event handler is fired.
            _timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged("MyProp");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            if (MediaElement1.Clock != null)
            {
                StatusBar.Text = MediaElement1.Clock.NaturalDuration.ToString();
                ClockSlider.Maximum = MediaElement1.Clock.NaturalDuration.TimeSpan.TotalSeconds + 10;
            }
            _timer.Start();
        }


        public object Convert(object o, Type type, object param, CultureInfo cul)
        {
            var currPosition = (TimeSpan) o;
            double numMSecs = currPosition.Milliseconds;

            var newValue = (numMSecs/MediaElement1.Clock.NaturalDuration.TimeSpan.TotalMilliseconds)*ClockSlider.Maximum;
            return newValue;
        }

        public object ConvertBack(object o, Type type, object param, CultureInfo cul) => null;
    }
}