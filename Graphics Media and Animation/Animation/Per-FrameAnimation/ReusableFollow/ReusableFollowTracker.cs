// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace PerFrameAnimation
{
    //uielement
    //visual operations

    //animation effect stuff

    public class ReusableFollowTracker
    {
        #region Constructors

        public ReusableFollowTracker()
        {
            _timeline = new ParallelTimeline(null, Duration.Forever);
            _timeClock = _timeline.CreateClock();
            _timeClock.Controller?.Begin();
            _lastTime = TimeSpan.FromSeconds(0);
        }

        #endregion

        #region Events

        public event EventHandler TimerFired;

        #endregion

        public double Update()
        {
            var currentTime = _timeClock.CurrentTime;

            //we're working with nullables, so just to be safe we should check for values
            if (currentTime.HasValue && _lastTime.HasValue)
            {
                //get the difference in time
                var diffTime = currentTime.Value - _lastTime.Value;
                DeltaSeconds = diffTime.TotalSeconds;

                //does the user want a callback on regular intervals?
                if (TimerInterval > 0.0)
                {
                    //compute the intervals for this and previous update
                    var currInterval = (int) (currentTime.Value.TotalSeconds/TimerInterval);
                    var prevInterval = (int) (_lastTime.Value.TotalSeconds/TimerInterval);

                    //has the interval changed since last update?
                    if (currInterval != prevInterval)
                    {
                        //fire interval event
                        //note that this will only be called once per frame at most
                        // so if they interval is too small, you wont get 2+ fires per frame
                        TimerFired?.Invoke(this, null);
                    }
                }
            }

            //cycle old time
            _lastTime = currentTime;

            return DeltaSeconds;
        }

        #region Private Members

        private readonly Timeline _timeline;
        private readonly Clock _timeClock;
        private TimeSpan? _lastTime;

        #endregion

        #region Properties

        public double TimerInterval { get; set; } = -1;

        public TimeSpan ElapsedTime
        {
            get
            {
                if (_lastTime.HasValue)
                    return _lastTime.Value;
                return new TimeSpan(0);
            }
        }

        public double DeltaSeconds { get; private set; }

        #endregion
    }
}