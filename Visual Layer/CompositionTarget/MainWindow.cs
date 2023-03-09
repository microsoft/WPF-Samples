// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CompositionTarget
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Stopwatch _stopwatch = new();
        private double _frameCounter;
        private Point _pt;

        private long _frameRate;
        private byte _redColor;
        private byte _blueColor;

        public MainWindow()
        {
            InitializeComponent();

            // Add an event handler to update canvas background color just before it is rendered.
            System.Windows.Media.CompositionTarget.Rendering += UpdateColor;
        }

        // Called just before frame is rendered to allow custom drawing.
        protected void UpdateColor(object sender, EventArgs e)
        {
            if (_frameCounter++ == 0)
            {
                // Starting timing.
                _stopwatch.Start();
            }

            // Determine frame rate in fps (frames per second).
            _frameRate =  (long)(_frameCounter/_stopwatch.Elapsed.TotalSeconds);
            if (_frameRate > 0)
            {
                // Update elapsed time, number of frames, and frame rate.
                MyStopwatchLabel.Content = _stopwatch.Elapsed.ToString();
                MyFrameCounterLabel.Content = _frameCounter.ToString(CultureInfo.InvariantCulture);
                MyFrameRateLabel.Content = _frameRate.ToString();
            }

            // Update the background of the canvas by converting MouseMove info to RGB info.
            _redColor = (byte)(_pt.X/3.0);
            _blueColor = (byte)(_pt.Y/2.0);
            MyCanvas.Background = new SolidColorBrush(Color.FromRgb(_redColor, 0x0, _blueColor));
        }

        public void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            // Retreive the coordinates of the mouse button event.
            _pt = e.GetPosition((UIElement)sender);
        }
    }
}