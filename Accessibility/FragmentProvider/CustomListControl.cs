// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Automation.Provider;
using System.Windows.Forms;

namespace FragmentProvider
{
    public class CustomListControl : Control
    {
        #region Private Members

        // Collection of strings that are items in the list.
        private readonly ArrayList _itemsArray;
        // Index of selected item, or -1 if no item is selected.
        private int _currentSelection = -1;
        // Constraints on list size.
        // Dimensions of list item.
        private const int ItemHeight = 15;
        private const int ListWidth = 70;
        // Dimensions of image that signifies item status.
        private const int ImageWidth = 10;
        private const int ImageHeight = 10;
        // Text indentation to allow room for image.
        private const int TextIndent = 15;

        // Unique identifier for each list item; never reused during the life of the control.
        private int _uniqueIdentifier;

        // The UI Automation provider for this instance of the control.

        #endregion Private Members

        #region Public Methods

        /// <summary>
        ///     Constructor.
        /// </summary>
        public CustomListControl()
        {
            Size = new Size(ListWidth, ItemHeight*MaximumNumberOfItems);

            // Initialize list item collection.
            _itemsArray = new ArrayList();
        }


        /// <summary>
        ///     Gets the maximum number of items the list can accommodate. For this example, this is constrained
        ///     by the size of the window so that we do not have to handle scrolling.
        /// </summary>
        public int MaximumNumberOfItems { get; } = 10;

        /// <summary>
        ///     Gets the minimum number of items the list can accommodate.
        /// </summary>
        public int MinimumNumberOfItems { get; } = 1;

        /// <summary>
        ///     Gets the unique identifier of a list item. This number is never reused within an instance
        ///     of the control.
        /// </summary>
        public int UniqueId
        {
            get
            {
                _uniqueIdentifier++;
                return _uniqueIdentifier;
            }
        }

        /// <summary>
        ///     Gets and sets the index of the selected item.
        /// </summary>
        public int SelectedIndex
        {
            get { return _currentSelection; }
            set { InternalSelect(value); }
        }

        /// <summary>
        ///     Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the item.</param>
        /// <returns>The control for the item.</returns>
        public CustomListItem GetItem(int i)
        {
            return (CustomListItem) _itemsArray[i];
        }

        /// <summary>
        ///     Gets the number of items in the list.
        /// </summary>
        public int ItemCount => _itemsArray.Count;

        /// <summary>
        ///     Gets the index of the specified item.
        /// </summary>
        /// <param name="listItem">The item.</param>
        /// <returns>The zero-based index.</returns>
        public int ItemIndex(CustomListItem listItem)
        {
            // Allows CustomListItem to requery its index, as it can change.
            return _itemsArray.IndexOf(listItem);
        }

        /// <summary>
        ///     Removes an item from the list.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns>true if successful.</returns>
        public bool Remove(int itemIndex)
        {
            if ((ItemCount == MinimumNumberOfItems) || (itemIndex <= -1) || (itemIndex >= ItemCount))
            {
                // If the number of items in the list is already at minimum,
                // no additional items can be removed.
                return false;
            }
            var itemToBeRemoved = (CustomListItem) _itemsArray[itemIndex];

            // Notify the provider that item it going to be destroyed.
            itemToBeRemoved.IsAlive = false;

            // Force refresh.
            Invalidate();

            // Raise notification.
            if (Provider != null)
            {
                ListProvider.OnStructureChangeRemove(itemToBeRemoved.Container);
            }
            _itemsArray.RemoveAt(itemIndex);
            _currentSelection = 0; // Reset selection to first item.

            return true;
        }

        /// <summary>
        ///     Adds an item to the list.
        /// </summary>
        /// <param name="item">Index at which to add the item.</param>
        /// <param name="a">Item to add.</param>
        /// <returns>true if successful.</returns>
        public bool Add(string item, Availability a)
        {
            if (_itemsArray.Count < MaximumNumberOfItems)
            {
                CustomListItem listItem;
                listItem = new CustomListItem(this, item, UniqueId, a);
                _itemsArray.Add(listItem);

                // Initialize the selection if necessary.
                if (_currentSelection < 0)
                {
                    _currentSelection = 0;
                    listItem.IsSelected = true;
                }
                Invalidate(); // update to draw new added item

                // Raise a UI Automation event.
                if (Provider != null)
                {
                    ListProvider.OnStructureChangeAdd(listItem.Container);
                }
                return true;
            }
            return false;
        }

        #endregion Public Methods

        #region Private/Internal methods

        /// <summary>
        ///     Responds to GotFocus event by repainting the list.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnGotFocus(EventArgs e)
        {
            OnPaint(new PaintEventArgs(CreateGraphics(), DisplayRectangle));
            base.OnGotFocus(e);
        }

        /// <summary>
        ///     Responds to LostFocus event by repainting the list.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            OnPaint(new PaintEventArgs(CreateGraphics(), DisplayRectangle));
            base.OnLostFocus(e);
        }

        /// <summary>
        ///     Responds to Paint event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Use SystemBrushes for colors of a control.
            var backgroundBrush = SystemBrushes.Window;
            Brush focusedBrush;
            focusedBrush = Focused ? SystemBrushes.Highlight : new SolidBrush(Color.DarkGray);
            e.Graphics.FillRectangle(backgroundBrush, DisplayRectangle);

            // Colors for online/offline image.
            var itemOnBrush = Brushes.Green;
            var itemOffBrush = Brushes.Red;

            var itemTextFont = SystemFonts.DefaultFont;

            var itemInk = SystemBrushes.ControlText;
            var selectedItemInk = SystemBrushes.HighlightText;

            // Loop through items to draw their text onto screen
            for (var i = 0; i < _itemsArray.Count; i++)
            {
                var pt = new Point(DisplayRectangle.Left + 2, DisplayRectangle.Top + (i*ItemHeight));
                var listItem = ((CustomListItem) _itemsArray[i]);
                var rc = GetRectForItem(i);
                rc = new Rectangle(rc.X, rc.Y + 2, ImageWidth, ImageHeight);
                e.Graphics.FillRectangle(listItem.Status == Availability.Online ? itemOnBrush : itemOffBrush, rc);
                rc = new Rectangle(rc.X + TextIndent, rc.Y - 2, ListWidth - TextIndent, ItemHeight);
                if (i == SelectedIndex)
                {
                    e.Graphics.FillRectangle(focusedBrush, rc);
                    e.Graphics.DrawString(listItem.Text, itemTextFont, selectedItemInk, rc);
                }
                else
                {
                    // Item not selected.
                    e.Graphics.DrawString(listItem.Text, itemTextFont, itemInk, rc);
                }
            }
            e.Dispose();
        }

        /// <summary>
        ///     Handles MouseDown event by selecting the item under the cursor.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            var index = ItemIndexFromPoint(new Point(e.X, e.Y));
            if (index != -1)
            {
                InternalSelect(index);
            }
        }

        /// <summary>
        ///     Gets the index at the specified screen coordinates.
        /// </summary>
        /// <param name="pt">The screen coordinates.</param>
        /// <returns>The index of the item, or -1 if pt is not within the control.</returns>
        /// <remarks>This logic is simple because the control does not support scrolling.</remarks>
        internal int ItemIndexFromPoint(Point pt)
        {
            // Determine whether the point is within the control. 
            if (!DisplayRectangle.Contains(pt))
            {
                return -1;
            }

            var index = (DisplayRectangle.Y + pt.Y)/ItemHeight;
            if (index >= _itemsArray.Count)
            {
                index = -1;
            }
            return index;
        }

        /// <summary>
        ///     Gets the screen coordinates of an item.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The screen coordinates.</returns>
        internal Rectangle GetRectForItem(int index)
        {
            var itemTop = DisplayRectangle.Top + (ItemHeight*index) + 1;
            return new Rectangle(DisplayRectangle.X, itemTop, DisplayRectangle.Width, ItemHeight);
        }

        /// <summary>
        ///     Processes up/down arrow keys.
        /// </summary>
        /// <param name="msg">The Windows message.</param>
        /// <param name="keyData">Information about the key press.</param>
        /// <returns>true if successful.</returns>
        [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down)
            {
                if (_currentSelection < _itemsArray.Count - 1)
                {
                    InternalSelect(_currentSelection + 1);
                }
                return true;
            }
            if (keyData == Keys.Up)
            {
                if (_currentSelection > 0)
                {
                    InternalSelect(_currentSelection - 1);
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        ///     Selects an item in the list.
        /// </summary>
        /// <param name="index">Index of the item to select.</param>
        private void InternalSelect(int index)
        {
            if ((index >= _itemsArray.Count) || (index < 0))
            {
                throw new ArgumentOutOfRangeException();
            }
            if (_currentSelection != index)
            {
                ((CustomListItem) _itemsArray[_currentSelection]).IsSelected = false;
                ((CustomListItem) _itemsArray[index]).IsSelected = true;
                _currentSelection = index;
                // Force refresh.
                Invalidate();
                ListProvider.OnSelectionChange((CustomListItem) _itemsArray[index]);
            }

            // Raise an event.
            ListProvider.OnFocusChange((CustomListItem) _itemsArray[index]);
        }

        #endregion Private/Internal methods

        #region UI Automation related methods

        /// <summary>
        ///     Gets the UI Automation provider for the control.
        /// </summary>
        public ListProvider Provider { get; private set; }

        /// <summary>
        ///     Gets a value that specifies whether the UI Automation provider is attached.
        /// </summary>
        protected bool AutomationIsActive => (Provider != null);

        /// <summary>
        ///     Handles WM_GETOBJECT message; others are passed to base handler.
        /// </summary>
        /// <param name="winMessage">Windows message.</param>
        /// <remarks>This method enables UI Automation to find the control.</remarks>
        [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
        protected override void WndProc(ref Message winMessage)
        {
            const int wmGetobject = 0x003D;

            if (winMessage.Msg == wmGetobject)
            {
                if (!AutomationIsActive)
                {
                    // If no provider has been created, then create one.
                    Provider = new ListProvider(this);

                    // Create providers for each existing item in the list.
                    foreach (CustomListItem listItem in _itemsArray)
                    {
                        listItem.Provider = new ListItemProvider(Provider, listItem);
                    }
                }

                winMessage.Result =
                    AutomationInteropProvider.ReturnRawElementProvider(
                        Handle, winMessage.WParam, winMessage.LParam, Provider);
                return;
            }
            base.WndProc(ref winMessage);
        }

        #endregion UI Automation related methods
    } // End class
}