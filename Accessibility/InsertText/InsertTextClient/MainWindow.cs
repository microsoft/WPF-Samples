// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Threading;
using Application = System.Windows.Forms.Application;
using Button = System.Windows.Controls.Button;

namespace InsertTextClient
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // InsertTextTarget.exe should be automatically copied to the 
        // InsertText client folder when you build the sample. 
        // You may have to manually copy this file if you receive an error 
        // stating the file cannot be found.
        private readonly string _filePath =
            Application.StartupPath
            + "\\InsertTextTarget.exe";

        private StringBuilder _feedbackText;
        private AutomationElement _targetWindow;
        private AutomationElementCollection _textControls;
        // Initialize both client and target applications.
        /// --------------------------------------------------------------------
        /// <summary>
        ///     The class constructor.
        /// </summary>
        /// --------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the click event for the Start App button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void btnStartApp_Click(object sender, RoutedEventArgs e)
        {
            _targetWindow = StartTargetApp(_filePath);

            if (_targetWindow == null)
            {
                return;
            }

            Feedback("Target started.");

            var clientLocationTop = Client.Top;
            var clientLocationRight = Client.Left + Client.Width + 100;
            var transformPattern =
                _targetWindow.GetCurrentPattern(TransformPattern.Pattern)
                    as TransformPattern;
            transformPattern?.Move(clientLocationRight, clientLocationTop);

            // Get required control patterns 
            //
            // Once you have an instance of an AutomationElement for the target 
            // obtain a WindowPattern object to handle the WindowClosed event.
            try
            {
                var windowPattern =
                    _targetWindow.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
            }
            catch (InvalidOperationException)
            {
                Feedback("Object does not support the Window Pattern");
                return;
            }

            // Register for an Event
            // 
            // The WindowPattern allows you to programmatically 
            // manipulate a window. 
            // It also exposes a window closed event. 
            // The following code shows an example of listening 
            // for the WindowClosed event.
            //
            // To intercept the WindowClosed event for our target application
            // you define an AutomationEventHandler delegate.
            var targetClosedHandler =
                new AutomationEventHandler(OnTargetClosed);

            // Use AddAutomationEventHandler() to add this event handler.
            // When listening for a WindowClosed event you must either scope 
            // the event to the automation element as done here, or cast 
            // the AutomationEventArgs in the handler to WindowClosedEventArgs 
            // and compare the RuntimeId of the automation element that raised 
            // the WindowClosed event to the automation element in the 
            // class member data.
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowClosedEvent,
                _targetWindow,
                TreeScope.Element,
                targetClosedHandler);

            SetClientControlProperties(false, true);

            // Get our collection of interesting controls.
            _textControls = FindTextControlsInTarget();
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Handles the click event for the Insert Text buttons.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// --------------------------------------------------------------------
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            _feedbackText = new StringBuilder();
            if (string.IsNullOrEmpty(tbInsert.Text))
            {
                Feedback("Please enter some text to insert.");
                return;
            }
            switch (((Button) sender).Content.ToString())
            {
                case "UIAutomation":
                    SetValueWithUiAutomation(tbInsert.Text);
                    break;
                case "Win32":
                    SetValueWithWin32(tbInsert.Text);
                    break;
                default:
                    Feedback("Insert failed.");
                    return;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Sets the values of the text controls using managed methods.
        /// </summary>
        /// <param name="s">The string to be inserted.</param>
        /// --------------------------------------------------------------------
        private void SetValueWithUiAutomation(string s)
        {
            foreach (AutomationElement control in _textControls)
            {
                InsertTextUsingUiAutomation(control, s);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Inserts a string into each text control of interest.
        /// </summary>
        /// <param name="element">A text control.</param>
        /// <param name="value">The string to be inserted.</param>
        /// --------------------------------------------------------------------
        private void InsertTextUsingUiAutomation(AutomationElement element,
            string value)
        {
            try
            {
                // Validate arguments / initial setup
                if (value == null)
                    throw new ArgumentNullException(
                        "String parameter must not be null.");

                if (element == null)
                    throw new ArgumentNullException(
                        "AutomationElement parameter must not be null");

                // A series of basic checks prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls 
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                if (!element.Current.IsEnabled)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId
                        + " is not enabled.\n\n");
                }

                // Check #2: Are there styles that prohibit us 
                //           from sending text to this control?
                if (!element.Current.IsKeyboardFocusable)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId
                        + "is read-only.\n\n");
                }


                // Once you have an instance of an AutomationElement,  
                // check if it supports the ValuePattern pattern.
                object valuePattern = null;

                // Control does not support the ValuePattern pattern 
                // so use keyboard input to insert content.
                //
                // NOTE: Elements that support TextPattern 
                //       do not support ValuePattern and TextPattern
                //       does not support setting the text of 
                //       multi-line edit or document controls.
                //       For this reason, text input must be simulated
                //       using one of the following methods.
                //       
                if (!element.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {
                    _feedbackText.Append("The control with an AutomationID of ")
                        .Append(element.Current.AutomationId)
                        .Append(" does not support ValuePattern.")
                        .AppendLine(" Using keyboard input.\n");

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    // Pause before sending keyboard input.
                    Thread.Sleep(100);

                    // Delete existing content in the control and insert new content.
                    SendKeys.SendWait("^{HOME}"); // Move to start of control
                    SendKeys.SendWait("^+{END}"); // Select everything
                    SendKeys.SendWait("{DEL}"); // Delete selection
                    SendKeys.SendWait(value);
                }
                // Control supports the ValuePattern pattern so we can 
                // use the SetValue method to insert content.
                else
                {
                    _feedbackText.Append("The control with an AutomationID of ")
                        .Append(element.Current.AutomationId)
                        .Append((" supports ValuePattern."))
                        .AppendLine(" Using ValuePattern.SetValue().\n");

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    ((ValuePattern) valuePattern).SetValue(value);
                }
            }
            catch (ArgumentNullException exc)
            {
                _feedbackText.Append(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                _feedbackText.Append(exc.Message);
            }
            finally
            {
                Feedback(_feedbackText.ToString());
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Sets the values of the text controls using unmanaged methods.
        /// </summary>
        /// <param name="s">The string to be inserted.</param>
        /// --------------------------------------------------------------------
        private void SetValueWithWin32(string s)
        {
            foreach (AutomationElement control in _textControls)
            {
                // An alternative to testing for static or read-only controls
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                InsertTextUsingWin32(control, s);
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Inserts the specified string into a text control.
        /// </summary>
        /// <param name="element">A text control.</param>
        /// <param name="value">The string to be inserted.</param>
        /// --------------------------------------------------------------------
        private void InsertTextUsingWin32(AutomationElement element, string value)
        {
            try
            {
                // Validate arguments / initial setup
                if (value == null)
                    throw new ArgumentNullException(
                        "String parameter 'value' must not be null.");

                if (element == null)
                    throw new ArgumentNullException(
                        "AutomationElement parameter 'element' must not be null");

                // Get hwnd
                var _hwnd = new IntPtr(element.Current.NativeWindowHandle);
                if (_hwnd == IntPtr.Zero)
                    throw new InvalidOperationException(
                        "Unable to get handle to object.");

                // A series of basic checks for the text control 
                // prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls 
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                if (!UnmanagedClass.IsWindowEnabled(_hwnd))
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId
                        + " is not enabled.\n");
                }

                // Check #2: Are there styles that prohibit us from 
                //           sending text to this control?
                var hwnd = UnmanagedClass.Hwnd.Cast(_hwnd);
                var controlStyle = UnmanagedClass.GetWindowLong(hwnd,
                    UnmanagedClass.GclStyle);

                if (IsBitSet(controlStyle, UnmanagedClass.EsReadonly))
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId +
                        " is read-only.");
                }

                // Check #3: Is the size of the text we want to set 
                //           greater than what the control accepts?
                IntPtr result;
                int resultInt;

                var resultSendMessage = UnmanagedClass.SendMessageTimeout(
                    hwnd,
                    UnmanagedClass.EmGetlimittext,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    1,
                    10000,
                    out result);
                var lastWin32Error =
                    Marshal.GetLastWin32Error();

                if (resultSendMessage == IntPtr.Zero)
                {
                    throw new InvalidOperationException(
                        "SendMessageTimeout() timed out.");
                }
                resultInt = unchecked((int) (long) result);

                // A result of -1 means that no limit is set.
                if (resultInt != -1 && resultInt < value.Length)
                {
                    throw new InvalidOperationException(
                        "The length of text entered ("
                        + value.Length
                        + ") is greater than the upper limit of the "
                        + "control with an AutomationID of "
                        + element.Current.AutomationId
                        + " (" + resultInt + ")"
                        + ".");
                }

                // Send the message...!
                result = UnmanagedClass.SendMessageTimeout(
                    hwnd,
                    UnmanagedClass.WmSettext,
                    IntPtr.Zero,
                    new StringBuilder(value),
                    1,
                    10000,
                    out result);
                resultInt = unchecked((int) (long) result);
                if (resultInt != 1)
                {
                    throw new InvalidOperationException(
                        "The text of the control with an AutomationID of "
                        + element.Current.AutomationId +
                        " cannot be changed.");
                }
                _feedbackText.Append(
                    "The text of the control with an AutomationID of ")
                    .Append(element.Current.AutomationId)
                    .AppendLine(" has been set.\n");
            }
            catch (EntryPointNotFoundException exc)
            {
                _feedbackText.AppendLine(exc.Message);
            }
            catch (ArgumentNullException exc)
            {
                _feedbackText.AppendLine(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                _feedbackText.AppendLine(exc.Message);
            }
            finally
            {
                Feedback(_feedbackText.ToString());
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Gets a pointer to an AutomationElement
        /// </summary>
        /// <param name="element">A text control.</param>
        /// <returns>A pointer to an AutomationElement</returns>
        /// --------------------------------------------------------------------
        private IntPtr GetWindowHandleFromAutomationElement
            (AutomationElement element)
        {
            var ptr = IntPtr.Zero;
            try
            {
                var objHwnd = element.GetCurrentPropertyValue(
                    AutomationElement.NativeWindowHandleProperty);

                if (objHwnd is IntPtr)
                    ptr = (IntPtr) objHwnd;
                else
                    ptr = new IntPtr(Convert.ToInt64(objHwnd));

                if (ptr == IntPtr.Zero)
                    throw new InvalidOperationException
                        ("Unable to get a handle for the element with an AutomationID of "
                         + element.Current.AutomationId + ".");
            }
            catch (InvalidOperationException exc)
            {
                Feedback(exc.Message);
            }
            return ptr;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Is bit set?
        /// </summary>
        /// <param name="flags">The flag(s) of interest.</param>
        /// <param name="bit">The bit value(s).</param>
        /// --------------------------------------------------------------------
        private bool IsBitSet(int flags, int bit) => (flags & bit) == bit;

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Finds the text controls of interest.
        /// </summary>
        /// <returns>
        ///     A collection of Automation Elements
        ///     that satisfy the specified conditions.
        /// </returns>
        /// --------------------------------------------------------------------
        // Find all 'text' controls that support TextPattern.
        private AutomationElementCollection FindTextControlsInTarget()
        {
            var condition = new AndCondition(
                new OrCondition(
                    new PropertyCondition(
                        AutomationElement.ControlTypeProperty,
                        ControlType.Edit),
                    new PropertyCondition(
                        AutomationElement.ControlTypeProperty,
                        ControlType.Document),
                    new PropertyCondition(
                        AutomationElement.ControlTypeProperty,
                        ControlType.Text)),
                new PropertyCondition(
                    AutomationElement.IsTextPatternAvailableProperty,
                    true)
                );
            return _targetWindow.FindAll(TreeScope.Descendants, condition);
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Starts the target app that contains the text controls of interest.
        /// </summary>
        /// <param name="sTarget">The target exe.</param>
        /// <returns>An Automation Element from our target window.</returns>
        /// --------------------------------------------------------------------
        // Start our target Win32 application
        private AutomationElement StartTargetApp(string target)
        {
            if (!File.Exists(target))
            {
                _feedbackText.Append(target).Append(" not found.");
                Feedback(_feedbackText.ToString());
                return null;
            }
            Process p;
            try
            {
                // Start application.
                p = Process.Start(target);
                if (p == null)
                    return null;

                // Give the target application some time to startup.
                // For Win32 applications, WaitForInputIdle can be used instead.
                // Another alternative is to listen for WindowOpened events.
                // Otherwise, an ArgumentException results when you try to
                // retrieve an automation element from the window handle.
                Thread.Sleep(2500);

                // Return the automation element
                return AutomationElement.FromHandle(p.MainWindowHandle);
            }
            catch (Win32Exception e)
            {
                Feedback(e.ToString());
                return null;
            }
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Intercepts the target window closed event and starts the client
        ///     window BackgroundWorker object.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        ///     It is not advisable to operate on your own UI within an
        ///     event handler. This is especially true in a multi-threaded
        ///     environment as the event handler is unlikely to be called on the
        ///     UI thread.
        /// </remarks>
        /// --------------------------------------------------------------------
        private void OnTargetClosed(object sender, AutomationEventArgs e)
        {
            // Schedule the update function in the UI thread.
            spInsert.Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new ControlsDelegate(SetClientControlProperties),
                true, false);
            txtFeedback.Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new FeedbackDelegate(Feedback),
                "Target closed.");
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Sets attributes of the controls in the client app.
        /// </summary>
        /// <param name="e">Enabled property.</param>
        /// <param name="v">Visibility property.</param>
        /// --------------------------------------------------------------------
        private void SetClientControlProperties(bool e1, bool e2)
        {
            btnStartApp.IsEnabled = e1;
            tbkInsert.IsEnabled = e2;
            spInsert.IsEnabled = e2;
        }

        /// --------------------------------------------------------------------
        /// <summary>
        ///     Outputs information to the client window.
        /// </summary>
        /// <param name="s">The string to display.</param>
        /// --------------------------------------------------------------------
        private void Feedback(string s)
        {
            txtFeedback.Text = s;
        }

        // Delegates to be used in placing jobs onto the Dispatcher.
        private delegate void ControlsDelegate(bool arg1, bool arg2);

        private delegate void FeedbackDelegate(string arg1);
    }
}