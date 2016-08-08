// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Microsoft.Windows.Input.TouchKeyboard
{
    /// <summary>
    /// A decorator that registers for and responds to touch keyboard events.
    /// 
    /// If the keyboard focused control is within the decorator and the control is occluded by the keyboard
    /// the decorator will appropriately shift the arrangement of the content such that the focused control
    /// is either snapped to the top of the decorator or snapped to the top of the occluded rectangle.
    /// 
    /// To use this, wrap either part or the entire content of a window in this decorator.  The content inside
    /// the decorator will then respond to the touch keyboard showing and hiding.
    /// </summary>
    /// <remarks>
    /// Known Issues:
    /// 
    ///     Popups will NOT move with their owning controls.  This includes tooltips, menus, dropdowns.  This is
    ///     an internal WPF issue and is beyond the scope of this class.
    ///     
    ///     Two decorators placed side by side in the visual tree might exhibit a strange look and feel.  This is
    ///     due to only the decorator with the focused element being shifted up.
    ///     
    ///     Partial trust applications cannot automatically invoke the touch keyboard or register for notifications.
    /// </remarks>
    public class TouchKeyboardAwareDecorator : Decorator
    {
        #region Member Variables

        /// <summary>
        /// If the touch keyboard is currently showing
        /// </summary>
        private bool _kbShowing = false;

        /// <summary>
        /// The rectangle occluded by the touch keyboard
        /// </summary>
        private static Rect _occludedRect = new Rect();

        /// <summary>
        /// The Y value that is being used to shift the decorator's content
        /// </summary>
        private double _currentShift = 0;

        /// <summary>
        /// The window being occluded by the touch keyboard
        /// </summary>
        private Window _occludedWindow = null;

        #endregion

        #region Constructor/Initialize

        public TouchKeyboardAwareDecorator()
            : base()
        {
        }

        /// <summary>
        /// When the decorator is initialized, subscribe to window load
        /// in order to have the HWND to subscribe to touch keyboard events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Window w = Window.GetWindow(this);

            if (w != null)
            {
                w.Loaded += ParentWindowLoaded;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Overrides arrange so that we can alter the rectangle used for arranging the
        /// decorator's content.
        /// </summary>
        /// <param name="finalSize">The arrange size</param>
        /// <returns>The final arrangement size</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement child = Child;

            if (child != null)
            {
                // Get the Y shift that must be done to properly display the occluded control (if any)
                _currentShift = CalculateContentShift();

                // We need to modify the size we arrange into in order to account for the current shift value.
                // If we don't, any shift up will result in a bottom control that is clipped to the original
                // arrange size, much like a translate RenderTransform would do.
                finalSize = new Size(finalSize.Width, finalSize.Height + Math.Abs(_currentShift));

                // Arrange the child to the desired size, but with a shifted rect
                child.Arrange(new Rect(new Point(0, _currentShift), finalSize));
            }

            // Return the calculated size
            return finalSize;
        }

        #endregion

        #region Touch Keyboard Handling

        /// <summary>
        /// Subscribe to touch keyboard notifications.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindowLoaded(object sender, RoutedEventArgs e)
        {
            Window w = sender as Window;

            if (w != null)
            {
                // Register the window for touch KB notifications
                TouchKeyboardEventManager.AddHandlers(w, HandleTouchKeyboardShowing, HandleTouchKeyboardHiding);

                // Also register for focus changes to detect situations where a shift has caused a clipped top/bottom control
                Keyboard.AddGotKeyboardFocusHandler(w, HandleKeyboardFocus);

                w.Loaded -= ParentWindowLoaded;
            }
        }

        /// <summary>
        /// This is used to detect scenarios where the KB is already showing and a shift has
        /// split a control either with the top of the decorator, or the top of the touch keyboard.
        /// If this control is focused, we then need to modify the shift to make it fully visible.
        /// 
        /// Marks arrange as dirty to trigger a new layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _occludedWindow = sender as Window;

            // On a focus, we only want to update if the KB is showing.  Otherwise any
            // current arrangement is correct.
            if (_kbShowing)
            {
                InvalidateArrange();
            }
        }

        /// <summary>
        /// Stores state information about the touch keyboard and marks arrange as dirty to trigger a
        /// new layout.
        /// </summary>
        /// <param name="occludedWindow">The window for the touch keyboard</param>
        /// <param name="occludedRect">The screen rectangle being occluded by the touch keyboard</param>
        private void HandleTouchKeyboardShowing(Window occludedWindow, Rect occludedRect)
        {
            _kbShowing = true;
            _occludedRect = occludedRect;
            _occludedWindow = occludedWindow;

            InvalidateArrange();
        }

        /// <summary>
        /// Stores state information about the touch keyboard and marks arrange as dirty to trigger a
        /// new layout.
        /// </summary>
        /// <param name="occludedWindow">The window for the touch keyboard</param>
        /// <param name="occludedRect">The screen rectangle being occluded by the touch keyboard</param>
        private void HandleTouchKeyboardHiding(Window occludedWindow, Rect occludedRect)
        {
            _kbShowing = false;
            _occludedRect = occludedRect;
            _occludedWindow = occludedWindow;

            InvalidateArrange();
        }
        #endregion

        #region Utility

        /// <summary>
        /// The main math behind shifting the content of the decorator in response to occlusion
        /// by the touch keyboard.
        /// 
        /// The algorithm is as follows:
        /// 
        ///     Given a current layout with a current shift value
        /// 
        ///     If we should evaluate occlusion for the focused element
        ///         Start with the current shift as basis for new shift
        ///         Test difference between top of element vs top of decorator
        ///             If negative we are hiding part of it, modify shift downward to make it visible
        ///         Test difference between bottom of element vs top of touch keyboard
        ///             If negative we are hiding part of it, modify shift upward to make it visible
        ///                 If shift upward would occlude element by top of decorator, snap to top of decorator
        ///     If we should not evaluate occlusion (touch keyboard is hidden) shift by zero
        /// </summary>
        /// <returns>The calculated Y shift value</returns>
        private double CalculateContentShift()
        {
            double newShift = 0;

            FrameworkElement focusedElement = Keyboard.FocusedElement as FrameworkElement;

            // We should only calculate the shift if the KB is showing and both this 
            // decorator and the occluded window is an ancestor of the focused element.
            // This accounts for focusing on items in popups.
            if (_kbShowing
                && focusedElement != null
                && _occludedWindow == Window.GetWindow(focusedElement)
                && IsAncestorOf(focusedElement))
            {
                // If no work is to be done, we just keep the current shift.
                newShift = _currentShift;

                // The offset to the top of the focused element in window coordinates.
                // We use the decorator coordinates here since this directly influences the shift.
                double offsetTopToDecorator = focusedElement.TransformToVisual(this).Transform(new Point(0, 0)).Y;

                if (offsetTopToDecorator < 0)
                {
                    // Shift down an increment due to the top of the decorator occluding the focused control
                    newShift = _currentShift - offsetTopToDecorator;
                }
                else
                {
                    // The offset to the bottom of the focused element in screen coords.
                    // We use screen coordinates here since this is compared against the KB occluded rect.
                    double offsetBottomToDecorator = PointToScreen(focusedElement.TransformToVisual(this).Transform(new Point(0, focusedElement.RenderSize.Height))).Y;

                    // The different from the bottom of the occluded element to the KB top converted from screen coords
                    // since this will be added to the shift
                    double kbDiff = PointFromScreen(new Point(0, _occludedRect.Top - offsetBottomToDecorator)).Y;

                    if (kbDiff < 0)
                    {
                        // Shift up an increment due to the KB occluding the focused control
                        // Need to take into account the current shift since this control could be occluded due to
                        // a prior shift.  Also ensure we are not shifting the focused element out of the window by
                        // ensuring it does not shift passed the top offset.  If it does, snap to the top.
                        newShift = (offsetTopToDecorator + kbDiff < 0) ? -offsetTopToDecorator : _currentShift + kbDiff;
                    }
                }
            }

            return newShift;
        }

        #endregion
    }
}
