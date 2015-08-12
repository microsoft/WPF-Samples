// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WindowMove
{
    /// <summary>
    ///     WindowMove application.
    /// </summary>
    public class WindowMove : Application
    {
        private readonly string _targetApplication = "Notepad.exe";
        private StringBuilder _feedbackText = new StringBuilder();
        private DockPanel _informationPanel;
        private Button _moveTarget;
        private Point _targetLocation;
        private AutomationElement _targetWindow;
        private TransformPattern _transformPattern;
        private WindowPattern _windowPattern;
        private TextBox _xCoordinate;
        private TextBox _yCoordinate;

        /// <summary>
        ///     The Startup handler.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Start the WindowMove client.
            CreateWindow();

            try
            {
                // Obtain an AutomationElement from the target window handle.
                _targetWindow = StartTargetApp(_targetApplication);

                // Does the automation element exist?
                if (_targetWindow == null)
                {
                    Feedback("No target.");
                    return;
                }
                Feedback("Found target.");

                // find current location of our window
                _targetLocation = _targetWindow.Current.BoundingRectangle.Location;

                // Obtain required control patterns from our automation element
                _windowPattern = GetControlPattern(_targetWindow,
                    WindowPattern.Pattern) as WindowPattern;

                if (_windowPattern == null) return;

                // Make sure our window is usable.
                // WaitForInputIdle will return before the specified time 
                // if the window is ready.
                if (false == _windowPattern.WaitForInputIdle(10000))
                {
                    Feedback("Object not responding in a timely manner.");
                    return;
                }
                Feedback("Window ready for user interaction");

                // Register for required events
                RegisterForEvents(
                    _targetWindow, WindowPattern.Pattern, TreeScope.Element);

                // Obtain required control patterns from our automation element
                _transformPattern =
                    GetControlPattern(_targetWindow, TransformPattern.Pattern)
                        as TransformPattern;

                if (_transformPattern == null) return;

                // Is the TransformPattern object moveable?
                if (_transformPattern.Current.CanMove)
                {
                    // Enable our WindowMove fields
                    _xCoordinate.IsEnabled = true;
                    _yCoordinate.IsEnabled = true;
                    _moveTarget.IsEnabled = true;

                    // Move element
                    _transformPattern.Move(0, 0);
                }
                else
                {
                    Feedback("Wndow is not moveable.");
                }
            }
            catch (ElementNotAvailableException)
            {
                Feedback("Client window no longer available.");
            }
            catch (InvalidOperationException)
            {
                Feedback("Client window cannot be moved.");
            }
            catch (Exception exc)
            {
                Feedback(exc.ToString());
            }
        }

        /// <summary>
        ///     Start the WindowMove client.
        /// </summary>
        private void CreateWindow()
        {
            var window = new Window();
            var sp = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var txtX = new TextBlock
            {
                Text = "X-coordinate: ",
                VerticalAlignment = VerticalAlignment.Center
            };

            _xCoordinate = new TextBox
            {
                Text = "0",
                IsEnabled = false,
                MaxLines = 1,
                MaxLength = 4,
                Margin = new Thickness(10, 0, 10, 0)
            };

            var txtY = new TextBlock
            {
                Text = "Y-coordinate: ",
                VerticalAlignment = VerticalAlignment.Center
            };

            _yCoordinate = new TextBox
            {
                Text = "0",
                IsEnabled = false,
                MaxLines = 1,
                MaxLength = 4,
                Margin = new Thickness(10, 0, 10, 0)
            };

            _moveTarget = new Button
            {
                IsEnabled = false,
                Width = 100,
                Content = "Move Window"
            };
            _moveTarget.Click += btnMove_Click;

            sp.Children.Add(txtX);
            sp.Children.Add(_xCoordinate);
            sp.Children.Add(txtY);
            sp.Children.Add(_yCoordinate);
            sp.Children.Add(_moveTarget);

            _informationPanel = new DockPanel {LastChildFill = false};
            DockPanel.SetDock(sp, Dock.Top);
            _informationPanel.Children.Add(sp);

            window.Content = _informationPanel;
            window.Show();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Provides feedback in the client.
        /// </summary>
        /// <param name="message">The string to display.</param>
        /// <remarks>
        ///     Since the events may happen on a different thread than the
        ///     client we need to use a Dispatcher delegate to handle them.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void Feedback(string message)
        {
            // Check if we need to call BeginInvoke.
            if (Dispatcher.CheckAccess() == false)
            {
                // Pass the same function to BeginInvoke,
                // but the call would come on the correct
                // thread and InvokeRequired will be false.
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Send,
                    new FeedbackDelegate(Feedback),
                    message);
                return;
            }
            var textElement = new TextBlock {Text = message};
            DockPanel.SetDock(textElement, Dock.Top);
            _informationPanel.Children.Add(textElement);
        }

        /// <summary>
        ///     Handles the 'Move' button invoked event.
        ///     By default, the Move method does not allow an object
        ///     to be moved completely off-screen.
        /// </summary>
        /// <param name="src">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnMove_Click(object src, RoutedEventArgs e)
        {
            try
            {
                // If coordinate left blank, substitute 0
                if (_xCoordinate.Text == "") _xCoordinate.Text = "0";
                if (_yCoordinate.Text == "") _yCoordinate.Text = "0";

                // Reset background colours
                _xCoordinate.Background = Brushes.White;
                _yCoordinate.Background = Brushes.White;

                if (_windowPattern.Current.WindowVisualState ==
                    WindowVisualState.Minimized)
                    _windowPattern.SetWindowVisualState(WindowVisualState.Normal);

                var x = double.Parse(_xCoordinate.Text);
                var y = double.Parse(_yCoordinate.Text);

                // Should validate the requested screen location
                if ((x < 0) ||
                    (x >= (SystemParameters.WorkArea.Width -
                           _targetWindow.Current.BoundingRectangle.Width)))
                {
                    Feedback("X-coordinate would place the window all or partially off-screen.");
                    _xCoordinate.Background = Brushes.Yellow;
                }

                if ((y < 0) ||
                    (y >= (SystemParameters.WorkArea.Height -
                           _targetWindow.Current.BoundingRectangle.Height)))
                {
                    Feedback("Y-coordinate would place the window all or partially off-screen.");
                    _yCoordinate.Background = Brushes.Yellow;
                }

                // transformPattern was obtained from the target window.
                _transformPattern.Move(x, y);
            }
            catch (ElementNotAvailableException)
            {
                Feedback("Client window no longer available.");
            }
            catch (InvalidOperationException)
            {
                Feedback("Client window cannot be moved.");
            }
        }

        /// <summary>
        ///     Update client controls based on target location changes.
        /// </summary>
        /// <param name="src">The object that raised the event.</param>
        private void UpdateClientControls(object src)
        {
            // If window is minimized, no need to report new screen coordinates
            if (_windowPattern.Current.WindowVisualState == WindowVisualState.Minimized)
                return;

            var ptCurrent =
                ((AutomationElement) src).Current.BoundingRectangle.Location;
            if (_targetLocation != ptCurrent)
            {
                Feedback("Window moved from " + _targetLocation +
                         " to " + ptCurrent);
                _targetLocation = ptCurrent;
            }
            if (_targetLocation.X != double.Parse(_xCoordinate.Text))
            {
                Feedback("Alert: Final X-coordinate not equal to requested X-coordinate.");
                _xCoordinate.Text = _targetLocation.X.ToString(CultureInfo.InvariantCulture);
            }
            if (_targetLocation.Y != double.Parse(_yCoordinate.Text))
            {
                Feedback("Alert: Final Y-coordinate not equal to requested Y-coordinate.");
                _yCoordinate.Text = _targetLocation.Y.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        ///     Window move event handler.
        /// </summary>
        /// <param name="src">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnWindowMove(object src, AutomationPropertyChangedEventArgs e)
        {
            // Pass the same function to BeginInvoke.
            Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new ClientControlsDelegate(UpdateClientControls), src);
        }

        /// <summary>
        ///     Starts the target application.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <returns>The target automation element.</returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private AutomationElement StartTargetApp(string app)
        {
            try
            {
                // Start application.
                var p = Process.Start(app);
                if (p.WaitForInputIdle(20000))
                    Feedback("Window ready for user interaction");
                else return null;

                // Give application a second to startup.
                Thread.Sleep(2000);

                // Return the automation element
                return AutomationElement.FromHandle(p.MainWindowHandle);
            }
            catch (ArgumentException e)
            {
                Feedback(e.ToString());
                return null;
            }
            catch (Win32Exception e)
            {
                Feedback(e.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Gets a specified control pattern.
        /// </summary>
        /// <param name="ae">
        ///     The automation element we want to obtain the control pattern from.
        /// </param>
        /// <param name="ap">The control pattern of interest.</param>
        /// <returns>A ControlPattern object.</returns>
        private object GetControlPattern(
            AutomationElement ae, AutomationPattern ap)
        {
            object oPattern = null;

            if (false == ae.TryGetCurrentPattern(ap, out oPattern))
            {
                Feedback("Object does not support the " +
                         ap.ProgrammaticName + " Pattern");
                return null;
            }

            Feedback("Object supports the " +
                     ap.ProgrammaticName + " Pattern.");

            return oPattern;
        }

        /// <summary>
        ///     Register for events of interest.
        /// </summary>
        /// <param name="ae">The automation element of interest.</param>
        /// <param name="ap">The control pattern of interest.</param>
        /// <param name="ts">The tree scope of interest.</param>
        private void RegisterForEvents(AutomationElement ae,
            AutomationPattern ap, TreeScope ts)
        {
            if (ap.Id == WindowPattern.Pattern.Id)
            {
                // The WindowPattern Exposes an element's ability 
                // to change its on-screen position or size.

                // The following code shows an example of listening for the 
                // BoundingRectangle property changed event on the window.
                Feedback("Start listening for WindowMove events for the control.");

                // Define an AutomationPropertyChangedEventHandler delegate to 
                // listen for window moved events.
                var moveHandler =
                    new AutomationPropertyChangedEventHandler(OnWindowMove);

                Automation.AddAutomationPropertyChangedEventHandler(
                    ae, ts, moveHandler,
                    AutomationElement.BoundingRectangleProperty);
            }
        }

        /// <summary>
        ///     Window shut down event handler.
        /// </summary>
        /// <param name="e">The exit event arguments.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            Automation.RemoveAllEventHandlers();
            base.OnExit(e);
        }

        // Delegates for updating the client UI based on target application events.
        private delegate void FeedbackDelegate(string message);

        private delegate void ClientControlsDelegate(object src);

        /// <summary>
        ///     Launch the sample application.
        /// </summary>
        internal sealed class TestMain
        {
            [STAThread]
            private static void Main()
            {
                // Create an instance of the sample class
                var app = new WindowMove();
                app.Run();
            }
        }
    }
}