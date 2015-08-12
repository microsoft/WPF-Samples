// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;

namespace FetchTimer
{
    public partial class FetchTimerForm : Form
    {
        public delegate void OutputDelegate(string results);

        private TreeScope _cacheScope;
        private AutomationElementMode _elementMode;
        private UiAutomationFetcher _fetcher;
        // Member variables.
        private Point _targetPoint;
        private Thread _workerThread;
        public OutputDelegate OutputMethodInstance;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public FetchTimerForm()
        {
            InitializeComponent();
            OutputMethodInstance = OutputResults;
        }

        /// <summary>
        ///     Enables/disables Children check box when Descendants changed, since
        ///     Descendants includes Children.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDescendants_CheckedChanged(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            cbChildren.Enabled = !box.Checked;
        }

        /// <summary>
        ///     Prints information to the text box.
        /// </summary>
        /// <param name="output">String to print.</param>
        public void OutputResults(string output)
        {
            tbOutput.AppendText(output);
        }

        /// <summary>
        ///     Clears the output box.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbOutput.Clear();
        }

        /// <summary>
        ///     Responds to button click; saves options and starts
        ///     the UI Automation worker thread.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnProps_Click(object sender, EventArgs e)
        {
            // Convert Drawing.Point to Windows.Point.
            var drawingPoint = Cursor.Position;
            _targetPoint = new Point(drawingPoint.X, drawingPoint.Y);

            // Save caching settings in member variables so UI isn't accessed 
            // directly by the other thread.

            _elementMode = rbFull.Checked ? AutomationElementMode.Full : AutomationElementMode.None;

            // For simplicity, always include Element in scope.
            _cacheScope = TreeScope.Element;
            if (cbDescendants.Checked)
            {
                _cacheScope |= TreeScope.Descendants;
            }
            // Note: if descendants are specified, children 
            // are automatically included.
            else if (cbChildren.Checked)
            {
                _cacheScope |= TreeScope.Children;
            }

            _fetcher = new UiAutomationFetcher(this, _targetPoint,
                _cacheScope, _elementMode);

            // Start another thread to do the UI Automation work.
            var threadDelegate = new ThreadStart(StartWorkerThread);
            _workerThread = new Thread(threadDelegate) {Priority = ThreadPriority.Highest};
            _workerThread.Start();
            OutputResults("Wait..." + Environment.NewLine);
        }

        /// <summary>
        ///     Delegated method for ThreadStart.
        /// </summary>
        /// <remarks>
        ///     UI Automation must be called on a separate thread if the client application
        ///     itself might be a target.
        /// </remarks>
        private void StartWorkerThread()
        {
            _fetcher.DoWork();
        }
    } // Form class
} // Namespace