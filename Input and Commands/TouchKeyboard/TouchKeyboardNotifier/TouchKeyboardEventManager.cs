// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Interop;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using static Microsoft.Windows.Input.TouchKeyboard.Rcw.InputPaneRcw;

namespace Microsoft.Windows.Input.TouchKeyboard
{
    // Alias for neatness
    using WinRTVisibilityHandler = TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>;

    /// <summary>
    /// A delegate for handlers to the showing and hiding events
    /// </summary>
    /// <param name="occludedWindow">The window that is being occluded</param>
    /// <param name="occludedRect">The rectangle that represents the occluded region</param>
    public delegate void TouchKeyboardVisibilityHandler(Window occludedWindow, System.Windows.Rect occludedRect);

    /// <summary>
    /// Provides access to registering a WPF window for notifications from the 
    /// touch keyboard.
    /// </summary>
    [CLSCompliant(true)]
    public static class TouchKeyboardEventManager
    {
        #region Helper Classes

        /// <summary>
        /// Hold event handlers for the WinRT events
        /// </summary>
        private class WinRTEventHandlers
        {
            public WinRTVisibilityHandler Showing;
            public WinRTVisibilityHandler Hiding;
        }

        #endregion

        #region Constants

        private const string ShowingEventName = "Showing";
        private const string HidingEventName = "Hiding";
        private const string InputPaneTypeName = "Windows.UI.ViewManagement.InputPane, Windows, ContentType=WindowsRuntime";

        #endregion

        #region Member Variables

        /// <summary>
        /// Holds an association from a window to the WinRT event handlers used to register for the showing/hiding events
        /// </summary>
        private static Dictionary<Window, List<WinRTEventHandlers>> _registrationTokens = new Dictionary<Window, List<WinRTEventHandlers>>();

        /// <summary>
        /// The WinRT InputPane type.
        /// </summary>
        private static Type _inputPaneType = null;

        /// <summary>
        /// Holds the event info queried from the InputPane type
        /// </summary>
        private static EventInfo _showingEvent = null;

        /// <summary>
        /// Holds the event info queried from the InputPane type
        /// </summary>
        private static EventInfo _hidingEvent = null;

        /// <summary>
        /// Indicates if calling the touch keyboard is supported
        /// </summary>
        private static bool _touchKeyboardSupported = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates and caches some reflection
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static TouchKeyboardEventManager()
        {
            _inputPaneType = Type.GetType(InputPaneTypeName);

            if (_inputPaneType != null)
            {
                _showingEvent = _inputPaneType.GetRuntimeEvent(ShowingEventName);
                _hidingEvent = _inputPaneType.GetRuntimeEvent(HidingEventName);
            }

            _touchKeyboardSupported = (WindowsRuntimeMarshal.GetActivationFactory(_inputPaneType) as IInputPaneInterop) != null;
        }

        #endregion

        #region Event Registration

        /// <summary>
        /// Adds handlers for the showing and hiding events from the touch keyboard for the specific window.
        /// </summary>
        /// <param name="window">The window to add handlers for</param>
        /// <param name="showingHandler">The handler for the showing event</param>
        /// <param name="hidingHandler">The handler for the hiding event</param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "String interoplation is CurrentCulture by default.")]
        public static void AddHandlers(Window window, TouchKeyboardVisibilityHandler showingHandler, TouchKeyboardVisibilityHandler hidingHandler)
        {
            VerifySupported();

            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            if (showingHandler == null)
            {
                throw new ArgumentNullException(nameof(showingHandler));
            }

            if (hidingHandler == null)
            {
                throw new ArgumentNullException(nameof(hidingHandler));
            }

            var winRtShowingHandler = AddShowingHandler(window, showingHandler);
            var winRtHidingHandler = AddHidingHandler(window, hidingHandler);

            // Register to unregister the handlers if needed
            window.Closing += Window_Closing;

            if (!_registrationTokens.ContainsKey(window))
            {
                _registrationTokens[window] = new List<WinRTEventHandlers>();
            }

            // Keep track of the WinRT handlers
            _registrationTokens[window].Add(new WinRTEventHandlers()
            {
                Showing = winRtShowingHandler,
                Hiding = winRtHidingHandler
            });
        }

        /// <summary>
        /// Removes all touch keyboad showing/hiding handlers for the specific window.
        /// </summary>
        /// <param name="window">The window to remove handlers for</param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Justification = "String interoplation is CurrentCulture by default.")]
        public static void RemoveHandlers(Window window)
        {
            VerifySupported();

            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            if (_registrationTokens.ContainsKey(window))
            {
                throw new ArgumentException(nameof(window), $"{nameof(window)}: {window.Title} is not registered for visibility events!");
            }

            RemoveShowingHandler(window);
            RemoveHidingHandler(window);

            window.Closing -= Window_Closing;
            _registrationTokens.Remove(window);
        }

        #endregion

        #region Window Handlers

        /// <summary>
        /// Subscribed on adding handlers for a window.  This function cleans up any references to that
        /// window and removes all event registrations as needed.
        /// </summary>
        /// <param name="sender">The window that is closing</param>
        /// <param name="e"></param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window w = sender as Window;

            if (w != null)
            {
                try
                {
                    RemoveHandlers(w);
                }
                catch
                {
                    // Don't want exceptions in the closing handler so eat them
                }
            }
        }

        #endregion

        #region Handler Utility Functions

        /// <summary>
        /// Adds a handler for the showing event
        /// </summary>
        /// <param name="window">The window to add the handler for</param>
        /// <param name="handler">The handler</param>
        /// <returns>The WinRT handler</returns>
        private static WinRTVisibilityHandler AddShowingHandler(Window window, TouchKeyboardVisibilityHandler handler)
        {
            return AddVisibilityHandler(_showingEvent, window, handler);
        }

        /// <summary>
        /// Adds a handler for the hiding event
        /// </summary>
        /// <param name="window">The window to add the handler for</param>
        /// <param name="handler">The handler</param>
        /// <returns>The WinRT handler</returns>
        private static WinRTVisibilityHandler AddHidingHandler(Window window, TouchKeyboardVisibilityHandler handler)
        {
            return AddVisibilityHandler(_hidingEvent, window, handler);
        }

        /// <summary>
        /// Remove the handler for the showing event.
        /// </summary>
        /// <param name="window">The window to remove the handler for</param>
        private static void RemoveShowingHandler(Window window)
        {
            foreach (WinRTEventHandlers handlers in _registrationTokens[window])
            {
                RemoveVisibilityHandler(_showingEvent, window, handlers.Showing);
            }
        }

        /// <summary>
        /// Remove the handler for the hiding event.
        /// </summary>
        /// <param name="window">The window to remove the handler for</param>
        private static void RemoveHidingHandler(Window window)
        {
            foreach (WinRTEventHandlers handlers in _registrationTokens[window])
            {
                RemoveVisibilityHandler(_hidingEvent, window, handlers.Hiding);
            }
        }

        /// <summary>
        /// Adds a handler for the specific visibility event
        /// </summary>
        /// <param name="visibilityEvent">The event to add a handler for</param>
        /// <param name="window">The window to add the handler for</param>
        /// <param name="handler">The handler</param>
        /// <returns>The WinRT handler</returns>
        private static WinRTVisibilityHandler AddVisibilityHandler(EventInfo visibilityEvent, Window window, TouchKeyboardVisibilityHandler handler)
        {
            IInputPane2 inputPane = GetInputPane(window);

            // This adds the event handler to the visibility event
            Func<WinRTVisibilityHandler, EventRegistrationToken> add =
                (addHandler) =>
                {
                    return (EventRegistrationToken)visibilityEvent.AddMethod.Invoke(inputPane, new object[] { addHandler });
                };

            // This removes the handler from the visibility event
            Action<EventRegistrationToken> remove =
                (removeHandler) =>
                {
                    visibilityEvent.RemoveMethod.Invoke(inputPane, new object[] { removeHandler });
                };

            // This is the WinRT called handler to translate to the local WPF delegate
            WinRTVisibilityHandler localHandler =
                (inputPaneInstance, visArgs) =>
                {
                    // We cannot reference System.Runtime.WindowsRuntime directly, so only access the Rect type via
                    // reflection to ensure the proper assembly loads with no issues.
                    var rect = visArgs.GetType().GetProperty("OccludedRect").GetValue(visArgs);

                    // Invoke the WPF specific handler to pass the occluded rectangle information
                    handler?.Invoke(
                        window,
                        new System.Windows.Rect(
                            (double)rect.GetType().GetRuntimeProperty("X").GetValue(rect),
                            (double)rect.GetType().GetRuntimeProperty("Y").GetValue(rect),
                            (double)rect.GetType().GetRuntimeProperty("Width").GetValue(rect),
                            (double)rect.GetType().GetRuntimeProperty("Height").GetValue(rect)));
                };

            // Add the event handlers
            WindowsRuntimeMarshal.AddEventHandler<WinRTVisibilityHandler>(add, remove, localHandler);

            // Return the WinRT handler in order to use it to un-register later
            return localHandler;
        }

        /// <summary>
        /// Removes a handler for the specific visibility event
        /// </summary>
        /// <param name="visibilityEvent">The event to remove the handler for</param>
        /// <param name="window">The window to remove the handler for</param>
        /// <param name="winRtHandler">The WinRT handler</param>
        private static void RemoveVisibilityHandler(EventInfo visibilityEvent, Window window, WinRTVisibilityHandler winRtHandler)
        {
            IInputPane2 inputPane = GetInputPane(window);

            Action<EventRegistrationToken> remove =
                (removeHandler) =>
                {
                    visibilityEvent.RemoveMethod.Invoke(inputPane, new object[] { removeHandler });
                };

            WindowsRuntimeMarshal.RemoveEventHandler<WinRTVisibilityHandler>(remove, winRtHandler);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Returns an instance of the InputPane
        /// </summary>
        /// <param name="window">The window to get the InputPane for</param>
        /// <returns>The InputPane</returns>
        private static IInputPane2 GetInputPane(Window window)
        {
            // Need the HWND for the native interop call into IInputPaneInterop
            IntPtr handle = new WindowInteropHelper(window).Handle;

            // Get and cast an InputPane COM instance
            var inputPaneInterop = WindowsRuntimeMarshal.GetActivationFactory(_inputPaneType) as IInputPaneInterop;

            // Get the actual input pane for this HWND
            return inputPaneInterop.GetForWindow(handle, typeof(IInputPane2).GUID);
        }

        /// <summary>
        /// Determines if the calls to the touch keyboard APIs are supported.
        /// </summary>
        private static void VerifySupported()
        {
            if (!_touchKeyboardSupported)
            {
                throw new PlatformNotSupportedException("Native access to touch keyboard APIs not supported on this OS!");
            }
        }

        #endregion
    }
}
