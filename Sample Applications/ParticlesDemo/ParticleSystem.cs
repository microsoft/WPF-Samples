// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace ParticlesDemo
{
    public class ParticleSystem
    {
        private readonly List<Particle> _particleList;
        private readonly GeometryModel3D _particleModel;
        private readonly Random _rand;

        public ParticleSystem(int maxCount, Color color)
        {
            MaxParticleCount = maxCount;

            _particleList = new List<Particle>();

            _particleModel = new GeometryModel3D {Geometry = new MeshGeometry3D()};

            var e = new Ellipse
            {
                Width = 32.0,
                Height = 32.0
            };
            var b = new RadialGradientBrush();
            b.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, color.R, color.G, color.B), 0.25));
            b.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, color.R, color.G, color.B), 1.0));
            e.Fill = b;
            e.Measure(new Size(32, 32));
            e.Arrange(new Rect(0, 0, 32, 32));

            Brush brush = null;

#if USE_VISUALBRUSH
            brush = new VisualBrush(e);
#else
            var renderTarget = new RenderTargetBitmap(32, 32, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(e);
            renderTarget.Freeze();
            brush = new ImageBrush(renderTarget);
#endif

            var material = new DiffuseMaterial(brush);

            _particleModel.Material = material;

            _rand = new Random(brush.GetHashCode());
        }

        public int MaxParticleCount { get; set; }
        public int Count => _particleList.Count;
        public Model3D ParticleModel => _particleModel;

        public void Update(double elapsed)
        {
            var deadList = new List<Particle>();

            // Update all particles
            foreach (var p in _particleList)
            {
                p.Position += p.Velocity*elapsed;
                p.Life -= p.Decay*elapsed;
                p.Size = p.StartSize*(p.Life/p.StartLife);
                if (p.Life <= 0.0)
                    deadList.Add(p);
            }

            foreach (var p in deadList)
                _particleList.Remove(p);

            UpdateGeometry();
        }

        private void UpdateGeometry()
        {
            var positions = new Point3DCollection();
            var indices = new Int32Collection();
            var texcoords = new PointCollection();

            for (var i = 0; i < _particleList.Count; ++i)
            {
                var positionIndex = i*4;
                var indexIndex = i*6;
                var p = _particleList[i];

                var p1 = new Point3D(p.Position.X, p.Position.Y, p.Position.Z);
                var p2 = new Point3D(p.Position.X, p.Position.Y + p.Size, p.Position.Z);
                var p3 = new Point3D(p.Position.X + p.Size, p.Position.Y + p.Size, p.Position.Z);
                var p4 = new Point3D(p.Position.X + p.Size, p.Position.Y, p.Position.Z);

                positions.Add(p1);
                positions.Add(p2);
                positions.Add(p3);
                positions.Add(p4);

                var t1 = new Point(0.0, 0.0);
                var t2 = new Point(0.0, 1.0);
                var t3 = new Point(1.0, 1.0);
                var t4 = new Point(1.0, 0.0);

                texcoords.Add(t1);
                texcoords.Add(t2);
                texcoords.Add(t3);
                texcoords.Add(t4);

                indices.Add(positionIndex);
                indices.Add(positionIndex + 2);
                indices.Add(positionIndex + 1);
                indices.Add(positionIndex);
                indices.Add(positionIndex + 3);
                indices.Add(positionIndex + 2);
            }

            ((MeshGeometry3D) _particleModel.Geometry).Positions = positions;
            ((MeshGeometry3D) _particleModel.Geometry).TriangleIndices = indices;
            ((MeshGeometry3D) _particleModel.Geometry).TextureCoordinates = texcoords;
        }

        public void SpawnParticle(Point3D position, double speed, double size, double life)
        {
            if (_particleList.Count > MaxParticleCount)
                return;
            var p = new Particle
            {
                Position = position,
                StartLife = life,
                Life = life,
                StartSize = size,
                Size = size
            };

            var x = 1.0f - (float) _rand.NextDouble()*2.0f;
            var z = 1.0f - (float) _rand.NextDouble()*2.0f;

            var v = new Vector3D(x, z, 0.0);
            v.Normalize();
            v *= ((float) _rand.NextDouble() + 0.25f)*(float) speed;

            p.Velocity = new Vector3D(v.X, v.Y, v.Z);

            p.Decay = 1.0f; // 0.5 + rand.NextDouble();
            //if (p.Decay > 1.0)
            //    p.Decay = 1.0;

            _particleList.Add(p);
        }
    }
}