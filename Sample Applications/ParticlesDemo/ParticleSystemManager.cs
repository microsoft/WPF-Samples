// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ParticlesDemo
{
    public class ParticleSystemManager
    {
        private readonly Dictionary<Color, ParticleSystem> _particleSystems;

        public ParticleSystemManager()
        {
            _particleSystems = new Dictionary<Color, ParticleSystem>();
        }

        public int ActiveParticleCount => _particleSystems.Values.Sum(ps => ps.Count);

        public void Update(float elapsed)
        {
            foreach (var ps in _particleSystems.Values)
            {
                ps.Update(elapsed);
            }
        }

        public void SpawnParticle(Point3D position, double speed, Color color, double size, double life)
        {
            try
            {
                var ps = _particleSystems[color];
                ps.SpawnParticle(position, speed, size, life);
            }
            catch
            {
                // ignored
            }
        }

        public Model3D CreateParticleSystem(int maxCount, Color color)
        {
            var ps = new ParticleSystem(maxCount, color);
            _particleSystems.Add(color, ps);
            return ps.ParticleModel;
        }
    }
}