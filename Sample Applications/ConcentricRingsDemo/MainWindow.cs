// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ConcentricRingsDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random _rand;
        private int _lastTick;

        public MainWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            var frameTimer = new DispatcherTimer();
            frameTimer.Tick += OnFrame;
            frameTimer.Interval = TimeSpan.FromSeconds(1.0/60.0);
            frameTimer.Start();

            _lastTick = Environment.TickCount;

            _rand = new Random(GetHashCode());

            Show();

            KeyDown += Window1_KeyDown;

            CreateCircles();
        }

        private void Window1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void OnFrame(object sender, EventArgs e)
        {
        }

        private void CreateCircles()
        {
            var centerX = MainCanvas.ActualWidth/2.0;
            var centerY = MainCanvas.ActualHeight/2.0;

            Color[] colors = {Colors.White, Colors.Green, Colors.Green, Colors.Lime};

            for (var i = 0; i < 24; ++i)
            {
                var e = new Ellipse();
                var alpha = (byte) _rand.Next(96, 192);
                var colorIndex = _rand.Next(4);
                e.Stroke =
                    new SolidColorBrush(Color.FromArgb(alpha, colors[colorIndex].R, colors[colorIndex].G,
                        colors[colorIndex].B));
                e.StrokeThickness = _rand.Next(1, 4);
                e.Width = 0.0;
                e.Height = 0.0;
                double offsetX = 16 - _rand.Next(32);
                double offsetY = 16 - _rand.Next(32);

                MainCanvas.Children.Add(e);

                e.SetValue(Canvas.LeftProperty, centerX + offsetX);
                e.SetValue(Canvas.TopProperty, centerY + offsetY);

                var duration = 6.0 + 10.0*_rand.NextDouble();
                var delay = 16.0*_rand.NextDouble();

                var offsetTransform = new TranslateTransform();

                var offsetXAnimation = new DoubleAnimation(0.0, -256.0, new Duration(TimeSpan.FromSeconds(duration)))
                {
                    RepeatBehavior = RepeatBehavior.Forever,
                    BeginTime = TimeSpan.FromSeconds(delay)
                };
                offsetTransform.BeginAnimation(TranslateTransform.XProperty, offsetXAnimation);
                offsetTransform.BeginAnimation(TranslateTransform.YProperty, offsetXAnimation);

                e.RenderTransform = offsetTransform;


                var sizeAnimation = new DoubleAnimation(0.0, 512.0, new Duration(TimeSpan.FromSeconds(duration)))
                {
                    RepeatBehavior = RepeatBehavior.Forever,
                    BeginTime = TimeSpan.FromSeconds(delay)
                };
                e.BeginAnimation(WidthProperty, sizeAnimation);
                e.BeginAnimation(HeightProperty, sizeAnimation);

                var opacityAnimation = new DoubleAnimation(duration - 1.0, 0.0,
                    new Duration(TimeSpan.FromSeconds(duration)))
                {
                    BeginTime = TimeSpan.FromSeconds(delay),
                    RepeatBehavior = RepeatBehavior.Forever
                };
                e.BeginAnimation(OpacityProperty, opacityAnimation);
            }
        }
    }
}