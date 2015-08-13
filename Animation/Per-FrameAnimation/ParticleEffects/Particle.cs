// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media;

namespace PerFrameAnimation
{
    internal class Particle
    {
        public DateTime DeathTime;
        public double Diameter;
        public Color EndColor;
        public DateTime LifeTime;
        public Point Location;
        public Color StartColor;
        public Vector Velocity;
    }
}