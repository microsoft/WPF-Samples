// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace PerFrameAnimation
{
    //uielement
    //visual operations

    //animation effect stuff

    public class ParticleEffectsTimeTracker
    {
        #region Constructors

        public ParticleEffectsTimeTracker()
        {
            ElapsedTime = DateTime.Now;
        }

        #endregion

        #region Events

        public event EventHandler TimerFired;

        #endregion

        public double Update()
        {
            var currentTime = DateTime.Now;


            //get the difference in time
            var diffTime = currentTime - ElapsedTime;
            DeltaSeconds = diffTime.TotalSeconds;


            //does the user want a callback on regular intervals?
            if (TimerInterval > 0.0)
            {
                /*
                    //compute the intervals for this and previous update
                    int currInterval = (int)(currentTime / TimeSpan.FromSeconds(_timerInterval));
                    int prevInterval = (int)(_lastTime / TimeSpan.FromSeconds(_timerInterval));

                    //has the interval changed since last update?
                    if (currInterval != prevInterval)
                    {
                        //fire interval event
                        //note that this will only be called once per frame at most
                        // so if they interval is too small, you wont get 2+ fires per frame
                        TimerFired(this, null);
                    } */

                if (currentTime != ElapsedTime)
                {
                    TimerFired?.Invoke(this, null);
                }
            }


            //cycle old time
            ElapsedTime = currentTime;

            return DeltaSeconds;
        }

        #region Private Memebers

        #endregion

        #region Properties

        public double TimerInterval { get; set; } = -1;

        public DateTime ElapsedTime { get; private set; }

        public double DeltaSeconds { get; private set; }

        #endregion
    }
}