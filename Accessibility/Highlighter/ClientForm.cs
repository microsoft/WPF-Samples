// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Highlighter
{
    /// <summary>
    ///     The form for the client application.
    /// </summary>
    public partial class ClientForm : Form
    {
        private readonly Timer _eventTimer;
        private readonly HighlightRectangle _highlight;
        private AutomationElement _focusedElement;
        private Rect _focusedRect;
        private AutomationFocusChangedEventHandler _focusHandler;
        private int _timerInterval;
        private bool _useTimer = true;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ClientForm()
        {
            // Make the process DPI-aware. Doing this ensures that when the
            // screen is set to a nonstandard DPI, all coordinates are 
            // correctly scaled and the highlight rectangle is displayed
            // in the correct place.
            try
            {
                NativeMethods.SetProcessDPIAware();
            }
            catch (EntryPointNotFoundException)
            {
                // Not running under Vista.
            }
            InitializeComponent();

            // Create timer.
            _timerInterval = tbInterval.Value;
            _eventTimer = new Timer();
            _eventTimer.Elapsed += OnTimerTick;
            _eventTimer.Enabled = false;
            _eventTimer.AutoReset = false;
            _timerInterval = tbInterval.Value;
            _eventTimer.Interval = _timerInterval;

            // Create highlight rectangle.
            _highlight = new HighlightRectangle();

            // Start a new thread to subscribe to UI Automation events.
            var threadDelegate = new ThreadStart(StartListening);
            var uiAutoThread = new Thread(threadDelegate);
            uiAutoThread.Start();
        }

        /// <summary>
        ///     Subscribe to UI Automation events.
        /// </summary>
        /// <remarks>
        ///     Do not call from the UI thread.
        /// </remarks>
        private void StartListening()
        {
            _focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(_focusHandler);
        }

        /// <summary>
        ///     Unsubscribe to UI Automation events.
        /// </summary>
        private void StopListening()
        {
            _eventTimer.Stop();
            Automation.RemoveAutomationFocusChangedEventHandler(_focusHandler);
        }

        /// <summary>
        ///     Responds to focus changes. Starts the timer so that the highlight
        ///     will be updated, or updates the highlight immediately if the timer
        ///     interval is set to 0.
        /// </summary>
        /// <param name="src">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnFocusChanged(object src, AutomationFocusChangedEventArgs e)
        {
            _focusedElement = src as AutomationElement;
            _focusedRect = _focusedElement.Current.BoundingRectangle;

#if DEBUG
            Console.WriteLine("Focus moved to: " + _focusedElement.Current.Name);
#endif

            if (_useTimer)
            {
                _eventTimer.Interval = _timerInterval;
                _eventTimer.Start();
            }
            else
            {
                UpdateHighlight();
            }
        }

        /// <summary>
        ///     Hides the old rectangle and creates a new one.
        /// </summary>
        private void UpdateHighlight()
        {
            // Hide old rectangle.
            _highlight.Visible = false;

            // Show new rectangle.
            _highlight.Location = new Rectangle(
                (int) _focusedRect.Left, (int) _focusedRect.Top,
                (int) _focusedRect.Width, (int) _focusedRect.Height);
            _highlight.Visible = true;
        }

        /// <summary>
        ///     Updates the highlight rectangle.
        /// </summary>
        /// <param name="sender">Ojbect that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>The timer stops because AutoReset is false.</remarks>
        private void OnTimerTick(object sender, EventArgs e)
        {
            UpdateHighlight();
        }

        /// <summary>
        ///     Responds to a change in the trackbar.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     Setting the value to 0 is equivalent to turning off the timer.
        /// </remarks>
        private void tbInterval_ValueChanged(object sender, EventArgs e)
        {
            if (tbInterval.Value > 0)
            {
                _timerInterval = tbInterval.Value;
                _useTimer = true;
            }
            else
            {
                _useTimer = false;
            }
        }

        /// <summary>
        ///     Responds to Exit button click.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        ///     Responds to form closing. Unsubscribes to UI Automation events.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Start a new thread to unsubscribe to UI Automation events.
            var threadDelegate = new ThreadStart(StopListening);
            var uiAutoThread = new Thread(threadDelegate);
            uiAutoThread.Start();
        }

        /// <summary>
        ///     The entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }
    } // class
} // namespace