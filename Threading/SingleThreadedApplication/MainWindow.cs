// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace SingleThreadedApplication
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void NextPrimeDelegate();

        private bool _continueCalculating;
        private bool _notAPrime;
        //Current number to check 
        private long _num = 3;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartOrStop(object sender, EventArgs e)
        {
            if (_continueCalculating)
            {
                _continueCalculating = false;
                startStopButton.Content = "Resume";
            }
            else
            {
                _continueCalculating = true;
                startStopButton.Content = "Stop";
                startStopButton.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new NextPrimeDelegate(CheckNextNumber));
            }
        }

        public void CheckNextNumber()
        {
            var x = new Stopwatch();
            x.Start();

            // Reset flag.
            _notAPrime = false;

            for (long i = 3; i <= Math.Sqrt(_num); i++)
            {
                if (_num%i == 0)
                {
                    // Set not a prime flag to true.
                    _notAPrime = true;
                    break;
                }
            }

            // If a prime number.
            if (!_notAPrime)
            {
                x.Stop();
                elapsed.Text = x.ElapsedMilliseconds.ToString();
                bigPrime.Text = _num.ToString();
            }

            _num += 2;
            if (_continueCalculating)
            {
                startStopButton.Dispatcher.BeginInvoke(
                    DispatcherPriority.SystemIdle,
                    new NextPrimeDelegate(CheckNextNumber));
            }
        }
    }
}
