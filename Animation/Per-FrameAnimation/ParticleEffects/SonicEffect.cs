// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PerFrameAnimation
{
    //uielement
    //visual operations

    public class SonicEffect : OverlayRenderDecorator
    {
        protected override void OnAttachChild(UIElement child)
        {
            child.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        protected override void OnDetachChild(UIElement child)
        {
            CompositionTarget.Rendering -= OnFrameCallback;

            child.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            _timeTracker = null;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_timeTracker != null)
            {
                _timeTracker.TimerFired -= OnTimerFired;
                _timeTracker = null;
            }

            CompositionTarget.Rendering += OnFrameCallback;
            _timeTracker = new ParticleEffectsTimeTracker {TimerInterval = _ringDelayInSeconds};
            _timeTracker.TimerFired += OnTimerFired;
            _lowerRing = _upperRing = 0;
            _clickPosition = e.GetPosition(this);
        }

        private void OnFrameCallback(object sender, EventArgs e)
        {
            if (_timeTracker != null)
            {
                _timeTracker.Update();
                InvalidateVisual();
            }
        }

        private void OnTimerFired(object sender, EventArgs e)
        {
            if (_upperRing < RingCount)
            {
                _upperRing++;
            }
            else
            {
                _lowerRing++;
                if (_lowerRing >= _upperRing)
                {
                    _timeTracker.TimerFired -= OnTimerFired;
                    _timeTracker = null;
                    CompositionTarget.Rendering -= OnFrameCallback;
                }
            }
        }

        protected override void OnOverlayRender(DrawingContext dc)
        {
            if (_timeTracker != null)
            {
                for (var i = _lowerRing; i < _upperRing; i++)
                {
                    var radius = RingRadius*(i + 1);
                    dc.DrawEllipse(Brushes.Transparent, new Pen(new SolidColorBrush(RingColor), RingThickness),
                        _clickPosition, radius, radius);
                }
            }
        }

        #region Private Members

        private ParticleEffectsTimeTracker _timeTracker;
        private double _ringDelayInSeconds = 0.1;
        private Point _clickPosition;
        private int _lowerRing, _upperRing;

        #endregion

        #region Properties

        public int RingCount { get; set; } = 5;

        public TimeSpan RingDelay
        {
            get { return TimeSpan.FromSeconds(_ringDelayInSeconds); }
            set { _ringDelayInSeconds = value.TotalSeconds; }
        }

        public double RingRadius { get; set; } = 7.0;

        public double RingThickness { get; set; } = 5.0;

        public Color RingColor { get; set; } = Color.FromArgb(128, 128, 128, 128);

        #endregion
    }
}