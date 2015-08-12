// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace FragmentProvider
{
    public class CustomListItem
    {
        private int _itemIndex;
        private ListItemProvider _itemProvider;
        private Rectangle _itemRect;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="parent">The owning list.</param>
        /// <param name="text">The text of the item.</param>
        /// <param name="id">The unique identifier of the item within the list.</param>
        /// <param name="availability">The status (online or offline) of the item.</param>
        public CustomListItem(CustomListControl parent, string text, int id, Availability availability)
        {
            IsAlive = true;
            Container = parent;
            Text = text;
            Id = id;
            Status = availability;
        }

        /// <summary>
        ///     Gets and sets the status of the item (alive if it is still displayed).
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        ///     Gets the CustomListControl that contains this item.
        /// </summary>
        public CustomListControl Container { get; }

        /// <summary>
        ///     Gets and sets the selection status of the item.
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        ///     Gets the unique identifier of the item within the list.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets the location of the item on the screen.
        /// </summary>
        /// <remarks>
        ///     Uses delegation to avoid interacting with the UI on a different thread.
        /// </remarks>
        public Rectangle Location
        {
            get
            {
                _itemRect = Container.GetRectForItem(Index);
                // Invoke control method on separate thread to avoid clashing with UI.
                // Use anonymous method for simplicity.
                Container.Invoke(new MethodInvoker(delegate { _itemRect = this.Container.RectangleToScreen(_itemRect); }));
                return _itemRect;
            }
        }

        /// <summary>
        ///     Gets and sets the text of the item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Gets and sets the index of the item.
        /// </summary>
        public int Index
        {
            get
            {
                _itemIndex = Container.ItemIndex(this);
                return _itemIndex;
            }
            set { _itemIndex = value; }
        }

        /// <summary>
        ///     Gets and sets the status (online or offline) of the item.
        /// </summary>
        public Availability Status { get; set; }

        /// <summary>
        ///     Gets and sets the UI Automation provider for the item.
        /// </summary>
        public ListItemProvider Provider
        {
            get { return _itemProvider ?? (_itemProvider = new ListItemProvider(Container.Provider, this)); }
            set { _itemProvider = value; }
        }
    }
}