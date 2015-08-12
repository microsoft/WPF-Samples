// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Automation;

namespace FocusTracker
{
    internal class Tracker
    {
        private AutomationElement _lastTopLevelWindow;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Tracker()
        {
            Startup();
        }

        /// <summary>
        ///     Entry point.
        /// </summary>
        /// <param name="args">Command-line arguments; not used.</param>
        public static void Main(string[] args)
        {
            Console.Title = "UI Automation Focus-tracking Sample";
            Console.WriteLine("Please wait while UI Automation initializes...");
            var reader = new Tracker();
            Console.WriteLine("Tracking focus. Press Enter to quit.");
            Console.ReadLine();
            Automation.RemoveAllEventHandlers();
        }

        /// <summary>
        ///     Initialization.
        /// </summary>
        private void Startup()
        {
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChanged);
        }

        /// <summary>
        ///     Retrieves the top-level window that contains the specified
        ///     UI Automation element.
        /// </summary>
        /// <param name="element">The contained element.</param>
        /// <returns>The  top-level window element.</returns>
        private AutomationElement GetTopLevelWindow(AutomationElement element)
        {
            var walker = TreeWalker.ControlViewWalker;
            AutomationElement elementParent;
            var node = element;
            try // In case the element disappears suddenly, as menu items are 
                // likely to do.
            {
                if (node == AutomationElement.RootElement)
                {
                    return node;
                }
                // Walk up the tree to the child of the root.
                while (true)
                {
                    elementParent = walker.GetParent(node);
                    if (elementParent == null)
                    {
                        return null;
                    }
                    if (elementParent == AutomationElement.RootElement)
                    {
                        break;
                    }
                    node = elementParent;
                }
            }
            catch (ElementNotAvailableException)
            {
                node = null;
            }
            catch (ArgumentNullException)
            {
                node = null;
            }
            return node;
        }

        /// <summary>
        ///     Handles focus-changed events. If the element that received focus is
        ///     in a different top-level window, announces that. If not, just
        ///     announces which element received focus.
        /// </summary>
        /// <param name="src">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnFocusChanged(object src, AutomationFocusChangedEventArgs e)
        {
            try
            {
                var elementFocused = src as AutomationElement;
                var topLevelWindow = GetTopLevelWindow(elementFocused);
                if (topLevelWindow == null)
                {
                    return;
                }

                // If top-level window has changed, announce it.
                if (topLevelWindow != _lastTopLevelWindow)
                {
                    _lastTopLevelWindow = topLevelWindow;
                    Console.WriteLine("Focus moved to top-level window:");
                    Console.WriteLine("  " + topLevelWindow.Current.Name);
                    Console.WriteLine();
                }
                else
                {
                    // Announce focused element.
                    Console.WriteLine("Focused element: ");
                    Console.WriteLine("  Type: " +
                                      elementFocused.Current.LocalizedControlType);
                    Console.WriteLine("  Name: " + elementFocused.Current.Name);
                    Console.WriteLine();
                }
            }
            catch (ElementNotAvailableException)
            {
            }
        }
    }
}