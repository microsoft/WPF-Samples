// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LayoutTransitionsDemo
{
    public class LayoutToLayoutHost : Border
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof (LayoutToLayoutTarget), typeof (LayoutToLayoutHost), null);

        //Animation duration
        private readonly int _timeSpan = 500;
        //Is this a fade-out animation?
        private bool _disappearOnCompletion;
        //This keeps track of time for when all the animations finish
        private DispatcherTimer _refresher;
        //This is kept as a member object so that local animations can affect it
        private TranslateTransform _translation;
        //Should I animate my way to future layout changes?
        public bool AnimateChanges;
        //Am I currently in the middle of an animation?
        public bool Animating;

        public LayoutToLayoutHost()
        {
            Loaded += OnLoad;
        }

        public LayoutToLayoutTarget Target
        {
            get { return (LayoutToLayoutTarget) GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            if (Target != null)
            {
                Debug.WriteLine("Binding Target!!!");
                BindToTarget(Target);
            }
            else
                Debug.WriteLine("Target was NULL!");

            Unloaded += OnUnload; //this gets done here rather in the ctor to avoid a frame bug (Windows OS 1224171)
        }

        /*
         * If the host gets unloaded, break the cyclical reference to make sure the GC does its job
         * */

        private void OnUnload(object sender, RoutedEventArgs e)
        {
            Target.Host = null;
            Target = null;
        }

        /*
         * Link up the Host to the Target and initialize but do not start the timer
         * */

        public void BindToTarget(LayoutToLayoutTarget t)
        {
            Target = t;
            t.Host = this;
            _translation = new TranslateTransform(0, 0);
            RenderTransform = _translation;

            _refresher = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(_timeSpan)};
            _refresher.Tick += OnAnimStateInvalidated;

            UpdateFromTarget();
        }

        /*
         * Start a new animation
         * */

        public void BeginAnimating(bool disappear)
        {
            if (Animating)
            {
                EndAnimations();
                //numActive -= 4;
            }

            Animating = false;
            AnimateChanges = true;

            _disappearOnCompletion = disappear;

            if (Visibility != Visibility.Visible)
            {
                Visibility = Visibility.Visible;
                Opacity = 0.0;
            }
        }

        /*
         * Do an immediate update, as long as there is not an animation running
         * */

        public void UpdateFromTarget()
        {
            if ((Target == null) || Animating)
                return;

            if (AnimateChanges)
                AnimateFromTarget();
            else
                MatchLayout();
        }

        /*
         * The double-check might not be necessary anymore, but this fixed a layout infinite loop
         * */

        private void MatchLayout()
        {
            if (Width != Target.ActualWidth)
                Width = Target.ActualWidth;

            if (Height != Target.ActualHeight)
                Height = Target.ActualHeight;

            var pt = Target.TranslatePoint(new Point(0, 0), Parent as UIElement);

            var t = RenderTransform as TranslateTransform;

            if (Math.Abs(t.X - pt.X) > 1)
                t.X = pt.X;

            if (Math.Abs(t.Y - pt.Y) > 1)
                t.Y = pt.Y;
        }

        /*
         * Make a local animation for each animated property
         * Base the destination on the new layout position of the target
         * */

        private void AnimateFromTarget()
        {
            var time = _timeSpan;
            Animating = true;
            AnimateChanges = false;

            var pt = Target.TranslatePoint(new Point(0, 0), Parent as UIElement);
            var t = RenderTransform as TranslateTransform;

            BeginAnimation(WidthProperty, SetupDoubleAnimation(Width, Target.ActualWidth, time));
            BeginAnimation(HeightProperty, SetupDoubleAnimation(Height, Target.ActualHeight, time));
            _translation.BeginAnimation(TranslateTransform.XProperty, SetupDoubleAnimation(t.X, pt.X, time));
            _translation.BeginAnimation(TranslateTransform.YProperty, SetupDoubleAnimation(t.Y, pt.Y, time));

            BeginAnimation(OpacityProperty,
                _disappearOnCompletion
                    ? SetupDoubleAnimation(Opacity, 0.0, time)
                    : SetupDoubleAnimation(Opacity, 1.0, time));


            _refresher.IsEnabled = false; //this restarts the timer
            _refresher.IsEnabled = true;
            _refresher.Start();
        }

        /*
         * This gets called by the DispatcherTimer Refresher when it is done
         * */

        private void OnAnimStateInvalidated(object sender, EventArgs e)
        {
            Animating = false;
            _refresher.IsEnabled = false;
            _refresher.Stop();

            EndAnimations();

            if (_disappearOnCompletion)
                Visibility = Visibility.Hidden;
        }

        /*
         * Explicitly replace all of the local animations will null
         * */

        public void EndAnimations()
        {
            var xBuffer = _translation.X;
            var yBuffer = _translation.Y;
            var widthBuffer = Width;
            var heightBuffer = Height;
            var opacityBuffer = Opacity;

            BeginAnimation(WidthProperty, null);
            BeginAnimation(HeightProperty, null);
            _translation.BeginAnimation(TranslateTransform.XProperty, null);
            _translation.BeginAnimation(TranslateTransform.YProperty, null);
            BeginAnimation(OpacityProperty, null);

            _translation.X = xBuffer;
            _translation.Y = yBuffer;
            Height = heightBuffer;
            Width = widthBuffer;
            Opacity = opacityBuffer;
        }

        /*
         * Helper function to create a DoubleAnimation
         * */

        public DoubleAnimation SetupDoubleAnimation(double @from, double to, int time)
        {
            var myDoubleAnimation = new DoubleAnimation
            {
                From = @from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(time)),
                AutoReverse = false
            };

            return myDoubleAnimation;
        }

        /*
         * Break the cyclical reference
         * */

        public void ReleaseFromTarget()
        {
            Target.Host = null;
            Target = null;
        }
    }
}