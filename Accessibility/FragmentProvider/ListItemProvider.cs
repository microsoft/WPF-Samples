// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;

namespace FragmentProvider
{
    public class ListItemProvider : IRawElementProviderFragment, ISelectionItemProvider
    {
        // Provider for the list that contains the item.
        private readonly ListProvider _containingListProvider;
        // Control that contains the list.
        private readonly CustomListItem _listItemControl;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="rootProvider">
        ///     UI Automation provider for the
        ///     fragment root (the containing list).
        /// </param>
        /// <param name="thisItem">The control that owns this provider.</param>
        public ListItemProvider(ListProvider rootProvider, CustomListItem thisItem)
        {
            _containingListProvider = rootProvider;
            _listItemControl = thisItem;
        }

        /// <summary>
        ///     Gets the index of the item within the list.
        /// </summary>
        public int Index => _listItemControl.Index;

        #region IRawElementProviderSimple Members

        /// <summary>
        ///     Retrieves the object that supports the specified control pattern.
        /// </summary>
        /// <param name="patternId">The pattern identifier</param>
        /// <returns>
        ///     The supporting object, or null if the pattern is not supported.
        /// </returns>
        public object GetPatternProvider(int patternId)
        {
            if (patternId == SelectionItemPatternIdentifiers.Pattern.Id)
            {
                return this;
            }
            return null;
        }

        /// <summary>
        ///     Returns UI Automation property values.
        /// </summary>
        /// <param name="propId">The identifier of the property.</param>
        /// <returns>The value of the property.</returns>
        public object GetPropertyValue(int propId)
        {
            if (_listItemControl.IsAlive == false)
            {
                throw new ElementNotAvailableException();
            }
            if (propId == AutomationElementIdentifiers.NameProperty.Id)
            {
                return _listItemControl.Text;
            }
            if (propId == AutomationElementIdentifiers.ControlTypeProperty.Id)
            {
                return ControlType.ListItem.Id;
            }
            if (propId == AutomationElementIdentifiers.AutomationIdProperty.Id)
            {
                return _listItemControl.Id.ToString();
            }
            if (propId == AutomationElementIdentifiers.HasKeyboardFocusProperty.Id)
            {
                return _listItemControl.IsSelected;
            }
            if (propId == AutomationElementIdentifiers.ItemStatusProperty.Id)
            {
                if (_listItemControl.Status == Availability.Online)
                {
                    return "Contact is online";
                }
                return "Contact is offline";
            }
            if (propId == AutomationElementIdentifiers.IsEnabledProperty.Id)
            {
                return true;
            }
            if (propId == AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id)
            {
                return true;
            }
            if (propId == AutomationElementIdentifiers.FrameworkIdProperty.Id)
            {
                return "Custom";
            }
            return null;
        }

        /// <summary>
        ///     Returns a host provider.
        /// </summary>
        /// <remarks>
        ///     In this case, because the element is not directly hosted in a
        ///     window, null is returned.
        /// </remarks>
        public IRawElementProviderSimple HostRawElementProvider => null;

        /// <summary>
        ///     Gets provider options.
        /// </summary>
        public ProviderOptions ProviderOptions => ProviderOptions.ServerSideProvider;

        #endregion IRawElementProviderSimple Members

        #region IRawElementProviderFragment Members

        /// <summary>
        ///     Gets the bounding rectangle, in screen coordinates.
        /// </summary>
        public Rect BoundingRectangle
        {
            get
            {
                var rc = _listItemControl.Location;
                return new Rect(rc.X, rc.Y, rc.Width, rc.Height);
            }
        }

        /// <summary>
        ///     Gets the root of this fragment.
        /// </summary>
        public IRawElementProviderFragmentRoot FragmentRoot => _containingListProvider;

        /// <summary>
        ///     Gets any fragment roots that are embedded in this fragment.
        /// </summary>
        /// <returns>Null in this case.</returns>
        public IRawElementProviderSimple[] GetEmbeddedFragmentRoots() => null;

        /// <summary>
        ///     Gets the runtime identifier of the UI Automation element.
        /// </summary>
        /// <returns>An array of integers.</returns>
        public int[] GetRuntimeId() => new[] { AutomationInteropProvider.AppendRuntimeId, _listItemControl.Id };

        /// <summary>
        ///     Navigate to adjacent elements in the UI Automation tree.
        /// </summary>
        /// <param name="direction">Direction to navigate.</param>
        /// <returns>The element in that direction, or null.</returns>
        public IRawElementProviderFragment Navigate(NavigateDirection direction)
        {
            if (direction == NavigateDirection.Parent)
            {
                return _containingListProvider;
            }
            if (direction == NavigateDirection.NextSibling)
            {
                return _containingListProvider.GetProviderForIndex(
                    _listItemControl.Index + 1);
            }
            if (direction == NavigateDirection.PreviousSibling)
            {
                return _containingListProvider.GetProviderForIndex(
                    _listItemControl.Index - 1);
            }
            return null;
        }


        /// <summary>
        ///     Responds to a client request to set the focus to this control.
        /// </summary>
        public void SetFocus()
        {
            if (_listItemControl.IsAlive == false)
            {
                throw new ElementNotAvailableException();
            }
            _listItemControl.Container.SelectedIndex = Index;
        }

        #endregion IRawElementProviderFragment Members

        #region ISelectionItemProvider Members

        /// <summary>
        ///     Adds an item to the selection in list boxes that support multiple
        ///     selection.
        /// </summary>
        public void AddToSelection()
        {
            throw new InvalidOperationException("Multiple selection is not supported.");
        }

        /// <summary>
        ///     Specifies whether the item is selected.
        /// </summary>
        public bool IsSelected => _listItemControl.IsSelected;

        /// <summary>
        ///     Removes the item from the selection in list boxes that support
        ///     multiple selection or no selection at all.
        /// </summary>
        public void RemoveFromSelection()
        {
            throw new InvalidOperationException("Selection is required for this control.");
        }

        /// <summary>
        ///     Selects the item.
        /// </summary>
        public void Select()
        {
            // For this list box, Focus and Selection are the same.
            SetFocus();
        }

        /// <summary>
        ///     Gets the list box that contains the item.
        /// </summary>
        public IRawElementProviderSimple SelectionContainer => _containingListProvider;

        #endregion ISelectionPatternProvider Members
    }
}