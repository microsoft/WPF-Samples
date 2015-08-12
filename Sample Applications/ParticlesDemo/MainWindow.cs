// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace ParticlesDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ParticleSystemManager _pm;
        private readonly Random _rand;
        private int _currentTick;
        private double _elapsed;
        private int _frameCount;
        private double _frameCountTime;
        private int _frameRate;
        private int _lastTick;
        private Point3D _spawnPoint;
        private double _totalElapsed;

        public MainWindow()
        {
            InitializeComponent();

            var frameTimer = new DispatcherTimer();
            frameTimer.Tick += OnFrame;
            frameTimer.Interval = TimeSpan.FromSeconds(1.0/60.0);
            frameTimer.Start();

            _spawnPoint = new Point3D(0.0, 0.0, 0.0);
            _lastTick = Environment.TickCount;

            _pm = new ParticleSystemManager();

            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Gray));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Red));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Silver));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Orange));
            WorldModels.Children.Add(_pm.CreateParticleSystem(1000, Colors.Yellow));

            _rand = new Random(GetHashCode());

            KeyDown += Window1_KeyDown;
            Cursor = Cursors.None;
        }

        private void Window1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void OnFrame(object sender, EventArgs e)
        {
            // Calculate frame time;
            _currentTick = Environment.TickCount;
            _elapsed = (_currentTick - _lastTick)/1000.0;
            _totalElapsed += _elapsed;
            _lastTick = _currentTick;

            _frameCount++;
            _frameCountTime += _elapsed;
            if (_frameCountTime >= 1.0)
            {
                _frameCountTime -= 1.0;
                _frameRate = _frameCount;
                _frameCount = 0;
                FrameRateLabel.Content = "FPS: " + _frameRate + "  Particles: " + _pm.ActiveParticleCount;
            }

            _pm.Update((float) _elapsed);
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Red, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Orange, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Silver, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Gray, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Red, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Orange, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Silver, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Gray, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Red, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Yellow, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Silver, _rand.NextDouble(), 2.5*_rand.NextDouble());
            _pm.SpawnParticle(_spawnPoint, 10.0, Colors.Yellow, _rand.NextDouble(), 2.5*_rand.NextDouble());

            var c = Math.Cos(_totalElapsed);
            var s = Math.Sin(_totalElapsed);

            _spawnPoint = new Point3D(s*32.0, c*32.0, 0.0);
        }
    }
}