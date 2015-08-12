// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;

namespace SimpleProvider
{
    internal class CustomButton : Control, IRawElementProviderSimple, IInvokeProvider
    {
        private readonly IntPtr _myHandle;
        private bool _buttonState;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="rect">Position and size of control.</param>
        public CustomButton()
        {
            _myHandle = Handle;

            // Add event handlers.
            MouseDown += CustomButton_MouseDown;
            KeyPress += CustomButton_KeyPress;
            GotFocus += CustomButton_ChangeFocus;
            LostFocus += CustomButton_ChangeFocus;
        }

        #region IInvokeProvider

        /// <summary>
        ///     Responds to an InvokePattern.Invoke by simulating a MouseDown event.
        /// </summary>
        void IInvokeProvider.Invoke()
        {
            // If the control is not enabled, we're responsible for letting UI Automation know.
            // It catches the exception and then throws it to the client.
            var provider = this as IRawElementProviderSimple;
            if (false == (bool) provider.GetPropertyValue(AutomationElementIdentifiers.IsEnabledProperty.Id))
            {
                throw new ElementNotEnabledException();
            }

            // Create arguments for the click event. The parameters aren't used.
            var mouseArgs = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

            // Simulate a mouse click. We cannot call RespondToClick directly, 
            // because it is illegal to update the UI from a different thread.
            MouseEventHandler handler = CustomButton_MouseDown;
            BeginInvoke(handler, this, mouseArgs);
        }

        #endregion InvokeProvider

        /// <summary>
        ///     Handles WM_GETOBJECT message; others are passed to base handler.
        /// </summary>
        /// <param name="m">Windows message.</param>
        /// <remarks>This method provides the link with UI Automation.</remarks>
        [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
        protected override void WndProc(ref Message m)
        {
            // 0x3D == WM_GETOBJECT
            if ((m.Msg == 0x3D) && (m.LParam.ToInt32() == AutomationInteropProvider.RootObjectId))
            {
                m.Result = AutomationInteropProvider.ReturnRawElementProvider(
                    Handle, m.WParam, m.LParam, this);
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        ///     Ensure that the focus rectangle is drawn or erased when focus changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomButton_ChangeFocus(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        ///     Handles Paint event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var buttonRect = new Rectangle(ClientRectangle.Left + 2,
                ClientRectangle.Top + 2,
                ClientRectangle.Width - 4,
                ClientRectangle.Height - 4);
            HatchBrush brush;
            if (_buttonState)
            {
                brush = new HatchBrush(
                    HatchStyle.DarkHorizontal, Color.Red, Color.White);
            }
            else
            {
                brush = new HatchBrush(
                    HatchStyle.DarkVertical, Color.Green, Color.White);
            }

            e.Graphics.FillRectangle(brush, buttonRect);
            if (Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
            }
        }

        /// <summary>
        ///     Responds to a button click, regardless of whether it was caused by a mouse or
        ///     keyboard click or by InvokePattern.Invoke.
        /// </summary>
        private void RespondToClick()
        {
            _buttonState = !_buttonState;
            Focus();
            Refresh();

            // Raise an event.
            if (AutomationInteropProvider.ClientsAreListening)
            {
                var args = new AutomationEventArgs(InvokePatternIdentifiers.InvokedEvent);
                AutomationInteropProvider.RaiseAutomationEvent(InvokePatternIdentifiers.InvokedEvent, this, args);
            }
        }

        /// <summary>
        ///     Handles MouseDown event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        public void CustomButton_MouseDown(object sender, MouseEventArgs e)
        {
            RespondToClick();
        }

        /// <summary>
        ///     Handles Keypress event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        public void CustomButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Space)
            {
                RespondToClick();
            }
        }

        #region IRawElementProviderSimple

        /// <summary>
        ///     Returns the object that supports the specified pattern.
        /// </summary>
        /// <param name="patternId">ID of the pattern.</param>
        /// <returns>Object that implements IInvokeProvider.</returns>
        object IRawElementProviderSimple.GetPatternProvider(int patternId)
        {
            if (patternId == InvokePatternIdentifiers.Pattern.Id)
            {
                return this;
            }
            return null;
        }

        /// <summary>
        ///     Returns property values.
        /// </summary>
        /// <param name="propId">Property identifier.</param>
        /// <returns>Property value.</returns>
        object IRawElementProviderSimple.GetPropertyValue(int propId)
        {
            if (propId == AutomationElementIdentifiers.ClassNameProperty.Id)
            {
                return "CustomButtonControlClass";
            }
            if (propId == AutomationElementIdentifiers.ControlTypeProperty.Id)
            {
                return ControlType.Button.Id;
            }
            if (propId == AutomationElementIdentifiers.HelpTextProperty.Id)
            {
                return "Change the button color and pattern.";
            }
            if (propId == AutomationElementIdentifiers.IsEnabledProperty.Id)
            {
                return true;
            }
            return null;
        }


        /// <summary>
        ///     Tells UI Automation that this control is hosted in an HWND, which has its own
        ///     provider.
        /// </summary>
        IRawElementProviderSimple IRawElementProviderSimple.HostRawElementProvider
            => AutomationInteropProvider.HostProviderFromHandle(_myHandle);

        /// <summary>
        ///     Retrieves provider options.
        /// </summary>
        ProviderOptions IRawElementProviderSimple.ProviderOptions => ProviderOptions.ServerSideProvider;

        #endregion IRawElementProviderSimple
    } // CustomButton class.
} // Namespace.