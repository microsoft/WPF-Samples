// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace FragmentProvider
{
    public class ListProvider : IRawElementProviderFragmentRoot, ISelectionProvider
    {
        // Control that contains the list.
        private readonly CustomListControl _ownerListControl;
        // Window handle of the control.
        private readonly IntPtr _windowHandle;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="control">
        ///     The control for which this object is providing UI Automation functionality.
        /// </param>
        public ListProvider(CustomListControl control)
        {
            _ownerListControl = control;
            _windowHandle = control.Handle;
        }

        #region IRawElementProviderSimple Members

        /// <summary>
        ///     Retrieves the object that supports the specified control pattern.
        /// </summary>
        /// <param name="patternId">The pattern identifier</param>
        /// <returns>
        ///     The supporting object, or null if the pattern is not supported.
        /// </returns>
        object IRawElementProviderSimple.GetPatternProvider(int patternId)
        {
            if (patternId == SelectionPatternIdentifiers.Pattern.Id)
            {
                return this;
            }
            return null;
        }


        /// <summary>
        ///     Gets provider property values.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <returns>The value of the property.</returns>
        object IRawElementProviderSimple.GetPropertyValue(int propertyId)
        {
            if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id)
            {
                return ControlType.List.Id;
            }
            // It is necessary to supply a value for IsKeyboardFocusable in a Windows Forms control,        
            //  because this value cannot be discovered by the HWND host provider. This is not 
            //  necessary for a Win32 provider.
            if (propertyId == AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id)
            {
                return true;
            }
            if (propertyId == AutomationElementIdentifiers.FrameworkIdProperty.Id)
            {
                return "Custom";
            }

            return null;
        }

        /// <summary>
        ///     Gets the host provider.
        /// </summary>
        /// <remarks>
        ///     Fragment roots return their window providers; most others return null.
        /// </remarks>
        IRawElementProviderSimple IRawElementProviderSimple.HostRawElementProvider
            => AutomationInteropProvider.HostProviderFromHandle(_windowHandle);

        /// <summary>
        ///     Gets provider options.
        /// </summary>
        ProviderOptions IRawElementProviderSimple.ProviderOptions => ProviderOptions.ServerSideProvider;

        #endregion IRawElementProviderSimple Members

        #region IRawElementProviderFragment Members

        /// <summary>
        ///     Gets the bounding rectangle.
        /// </summary>
        /// <remarks>
        ///     Fragment roots should return an empty rectangle. UI Automation will get the rectangle
        ///     from the host control (the HWND in this case).
        /// </remarks>
        Rect IRawElementProviderFragment.BoundingRectangle => Rect.Empty;

        IRawElementProviderFragmentRoot IRawElementProviderFragment.FragmentRoot => this;

        /// <summary>
        ///     Gets any fragment roots that are embedded in this fragment.
        /// </summary>
        /// <returns>Null in this case.</returns>
        IRawElementProviderSimple[] IRawElementProviderFragment.GetEmbeddedFragmentRoots()
        {
            return null;
        }

        /// <summary>
        ///     Gets the runtime identifier of the UI Automation element.
        /// </summary>
        /// <returns>Fragment roots return null.</returns>
        int[] IRawElementProviderFragment.GetRuntimeId()
        {
            return null;
        }

        /// <summary>
        ///     Navigates to adjacent elements in the UI Automation tree.
        /// </summary>
        /// <param name="direction">Direction of navigation.</param>
        /// <returns>The element in that direction, or null.</returns>
        /// <remarks>
        ///     The provider only returns directions that it is responsible for.
        ///     UI Automation knows how to navigate between HWNDs, so only the custom item
        ///     navigation needs to be provided.
        /// </remarks>
        IRawElementProviderFragment IRawElementProviderFragment.Navigate(NavigateDirection direction)
        {
            if (direction == NavigateDirection.FirstChild)
            {
                return GetProviderForIndex(0);
            }
            if (direction == NavigateDirection.LastChild)
            {
                return GetProviderForIndex(_ownerListControl.ItemCount - 1);
            }
            return null;
        }

        /// <summary>
        ///     Responds to a client request to set the focus to this control.
        /// </summary>
        /// <remarks>Setting focus to the control is handled by the parent window.</remarks>
        void IRawElementProviderFragment.SetFocus()
        {
            throw new Exception("The method is not implemented.");
        }

        #endregion IRawElementProviderFragment Members

        #region IRawElementProviderFragmentRoot Members

        /// <summary>
        ///     Gets the child element at the specified point.
        /// </summary>
        /// <param name="x">Distance from the left of the application window.</param>
        /// <param name="y">Distance from the top of the application window.</param>
        /// <returns>The provider for the element at that point.</returns>
        IRawElementProviderFragment
            IRawElementProviderFragmentRoot.ElementProviderFromPoint(double x, double y)
        {
            var index = -1;
            var clientPoint = new Point((int) x, (int) y);

            // Invoke control method on separate thread to avoid clashing with UI.
            // Use anonymous method for simplicity.
            _ownerListControl.Invoke(
                new MethodInvoker(delegate { clientPoint = this._ownerListControl.PointToClient(clientPoint); }));

            index = _ownerListControl.ItemIndexFromPoint(clientPoint);

            if (index == -1)
            {
                return null;
            }
            return GetProviderForIndex(index);
        }


        /// <summary>
        ///     Returns the child element that is selected when the list gets focus.
        /// </summary>
        /// <returns>The selected item.</returns>
        IRawElementProviderFragment IRawElementProviderFragmentRoot.GetFocus()
        {
            var index = _ownerListControl.SelectedIndex;
            return GetProviderForIndex(index);
        }

        #endregion IRawElementProviderFragmentRoot

        #region ISelectionProvider Members

        /// <summary>
        ///     Specifies whether selection of more than one item at a time is supported.
        /// </summary>
        bool ISelectionProvider.CanSelectMultiple => false;

        /// <summary>
        ///     Returns the UI Automation provider for the selected list items.
        /// </summary>
        /// <returns>The selected items.</returns>
        /// <remarks>
        ///     Because this is a single-selection list box, only one item is returned.
        /// </remarks>
        IRawElementProviderSimple[] ISelectionProvider.GetSelection()
        {
            var index = _ownerListControl.SelectedIndex;
            return new IRawElementProviderSimple[] {GetProviderForIndex(index)};
        }

        /// <summary>
        ///     Specifies whether the list must have an item selected at all times.
        /// </summary>
        bool ISelectionProvider.IsSelectionRequired => true;

        #endregion ISelectionProvider Members

        #region Helper methods

        /// <summary>
        ///     Gets the UI Automation provider for the item at the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>The provider object, or null if the index is out of range.</returns>
        public IRawElementProviderFragment GetProviderForIndex(int index)
        {
            var ownerCustomListItemCount = _ownerListControl.ItemCount - 1;
            if ((index < 0) || (index > ownerCustomListItemCount))
            {
                return null;
            }
            return _ownerListControl.GetItem(index).Provider;
        }


        /// <summary>
        ///     Responds to a focus change by raising an event.
        /// </summary>
        /// <param name="listItem">The item that has received focus.</param>
        public static void OnFocusChange(CustomListItem listItem)
        {
            if (AutomationInteropProvider.ClientsAreListening)
            {
                var args = new AutomationEventArgs(
                    AutomationElementIdentifiers.AutomationFocusChangedEvent);
                AutomationInteropProvider.RaiseAutomationEvent(
                    AutomationElementIdentifiers.AutomationFocusChangedEvent,
                    listItem.Provider, args);
            }
        }


        /// <summary>
        ///     Responds to a selection change by raising an event.
        /// </summary>
        /// <param name="listItem">The item that has been selected.</param>
        public static void OnSelectionChange(CustomListItem listItem)
        {
            if (AutomationInteropProvider.ClientsAreListening)
            {
                var args = new AutomationEventArgs(SelectionItemPatternIdentifiers.ElementSelectedEvent);
                AutomationInteropProvider.RaiseAutomationEvent(SelectionItemPatternIdentifiers.ElementSelectedEvent,
                    listItem.Provider, args);
            }
        }

        /// <summary>
        ///     Responds to an addition to the UI Automation tree structure by raising an event.
        /// </summary>
        /// <param name="list">
        ///     The list to which the item was added.
        /// </param>
        /// <remarks>
        ///     For the runtime Id of the item, pass 0 because the provider cannot know
        ///     what its actual runtime Id is.
        /// </remarks>
        public static void OnStructureChangeAdd(CustomListControl list)
        {
            if (AutomationInteropProvider.ClientsAreListening)
            {
                int[] fakeRuntimeId = {0};
                var args =
                    new StructureChangedEventArgs(StructureChangeType.ChildrenBulkAdded,
                        fakeRuntimeId);
                AutomationInteropProvider.RaiseStructureChangedEvent(
                    list.Provider, args);
            }
        }

        /// <summary>
        ///     Responds to a removal from the UI Automation tree structure
        ///     by raising an event.
        /// </summary>
        /// <param name="list">
        ///     The list from which the item was removed.
        /// </param>
        /// <remarks>
        ///     For the runtime Id of the list, pass 0 because the provider cannot know
        ///     what its actual runtime ID is.
        /// </remarks>
        public static void OnStructureChangeRemove(CustomListControl list)
        {
            if (AutomationInteropProvider.ClientsAreListening)
            {
                int[] fakeRuntimeId = {0};
                var args =
                    new StructureChangedEventArgs(StructureChangeType.ChildrenBulkRemoved,
                        fakeRuntimeId);
                AutomationInteropProvider.RaiseStructureChangedEvent(
                    list.Provider, args);
            }
        }

        #endregion Helper methods
    } // ListProvider class
}