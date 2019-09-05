// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

// Type
// DependencyProperty, RoutedEventArgs, DependencyObject, FrameworkElement
// Control
// Debug
// AdornerLayer
// MouseEventArgs, MouseEventButtonArgs, KeyEventArgs, KeyboardFocusChangedEventArgs

// VisualTreeHelper

namespace EditBoxControlLibrary
{
    /// <summary>
    ///     EditBox is a custom cotrol that can switch between two modes:
    ///     editing and normal. When it is in editing mode, the content is
    ///     displayed in a TextBox that provides editing capbability. When
    ///     the EditBox is in normal, its content is displayed in a TextBlock
    ///     that is not editable.
    ///     This control is designed to be used in with a GridView View.
    /// </summary>
    public class EditBox : Control
    {
        #region Static Constructor

        /// <summary>
        ///     Static constructor
        /// </summary>
        static EditBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (EditBox),
                new FrameworkPropertyMetadata(typeof (EditBox)));
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Called when the tree for the EditBox has been generated.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var textBlock = GetTemplateChild("PART_TextBlockPart") as TextBlock;
            Debug.Assert(textBlock != null, "No TextBlock!");

            _textBox = new TextBox();
            _adorner = new EditBoxAdorner(textBlock, _textBox);
            var layer = AdornerLayer.GetAdornerLayer(textBlock);
            layer.Add(_adorner);

            _textBox.KeyDown += OnTextBoxKeyDown;
            _textBox.LostKeyboardFocus +=
                OnTextBoxLostKeyboardFocus;

            //Receive notification of the event to handle the column 
            //resize. 
            HookTemplateParentResizeEvent();

            //Capture the resize event to  handle ListView resize cases.
            HookItemsControlEvents();

            _listViewItem = GetDependencyObjectFromVisualTree(this,
                typeof (ListViewItem)) as ListViewItem;

            Debug.Assert(_listViewItem != null, "No ListViewItem found");
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     If the ListViewItem that contains the EditBox is selected,
        ///     when the mouse pointer moves over the EditBox, the corresponding
        ///     MouseEnter event is the first of two events (MouseUp is the second)
        ///     that allow the EditBox to change to editing mode.
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!IsEditing && IsParentSelected)
            {
                _canBeEdit = true;
            }
        }

        /// <summary>
        ///     If the MouseLeave event occurs for an EditBox control that
        ///     is in normal mode, the mode cannot be changed to editing mode
        ///     until a MouseEnter event followed by a MouseUp event occurs.
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _isMouseWithinScope = false;
            _canBeEdit = false;
        }

        /// <summary>
        ///     An EditBox switches to editing mode when the MouseUp event occurs
        ///     for that EditBox and the following conditions are satisfied:
        ///     1. A MouseEnter event for the EditBox occurred before the
        ///     MouseUp event.
        ///     2. The mouse did not leave the EditBox between the
        ///     MouseEnter and MouseUp events.
        ///     3. The ListViewItem that contains the EditBox was selected
        ///     when the MouseEnter event occurred.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Right ||
                e.ChangedButton == MouseButton.Middle)
                return;

            if (!IsEditing)
            {
                if (!e.Handled && (_canBeEdit || _isMouseWithinScope))
                {
                    IsEditing = true;
                }

                //If the first MouseUp event selects the parent ListViewItem,
                //then the second MouseUp event puts the EditBox in editing 
                //mode
                if (IsParentSelected)
                    _isMouseWithinScope = true;
            }
        }

        #endregion

        #region Public Properties

        #region Value

        /// <summary>
        ///     ValueProperty DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof (object),
                typeof (EditBox),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the value of the EditBox
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region IsEditing

        /// <summary>
        ///     IsEditingProperty DependencyProperty
        /// </summary>
        public static DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                "IsEditing",
                typeof (bool),
                typeof (EditBox),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        ///     Returns true if the EditBox control is in editing mode.
        /// </summary>
        public bool IsEditing
        {
            get { return (bool) GetValue(IsEditingProperty); }
            private set
            {
                SetValue(IsEditingProperty, value);
                _adorner.UpdateVisibilty(value);
            }
        }

        #endregion

        #region IsParentSelected

        /// <summary>
        ///     Gets whether the ListViewItem that contains the
        ///     EditBox is selected.
        /// </summary>
        private bool IsParentSelected
        {
            get
            {
                if (_listViewItem == null)
                    return false;
                return _listViewItem.IsSelected;
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        ///     When an EditBox is in editing mode, pressing the ENTER or F2
        ///     keys switches the EditBox to normal mode.
        /// </summary>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (IsEditing && (e.Key == Key.Enter || e.Key == Key.F2))
            {
                IsEditing = false;
                _canBeEdit = false;
            }
        }

        /// <summary>
        ///     If an EditBox loses focus while it is in editing mode,
        ///     the EditBox mode switches to normal mode.
        /// </summary>
        private void OnTextBoxLostKeyboardFocus(object sender,
            KeyboardFocusChangedEventArgs e)
        {
            IsEditing = false;
        }

        /// <summary>
        ///     Sets IsEditing to false when the ListViewItem that contains an
        ///     EditBox changes its size
        /// </summary>
        private void OnCouldSwitchToNormalMode(object sender,
            RoutedEventArgs e)
        {
            IsEditing = false;
        }

        /// <summary>
        ///     Walk the visual tree to find the ItemsControl and
        ///     hook its some events on it.
        /// </summary>
        private void HookItemsControlEvents()
        {
            _itemsControl = GetDependencyObjectFromVisualTree(this,
                typeof (ItemsControl)) as ItemsControl;
            if (_itemsControl != null)
            {
                //Handle the Resize/ScrollChange/MouseWheel 
                //events to determine whether to switch to Normal mode
                _itemsControl.SizeChanged +=
                    OnCouldSwitchToNormalMode;
                _itemsControl.AddHandler(ScrollViewer.ScrollChangedEvent,
                    new RoutedEventHandler(OnScrollViewerChanged));
                _itemsControl.AddHandler(MouseWheelEvent,
                    new RoutedEventHandler(OnCouldSwitchToNormalMode), true);
            }
        }

        /// <summary>
        ///     If an EditBox is in editing mode and the content of a ListView is
        ///     scrolled, then the EditBox switches to normal mode.
        /// </summary>
        private void OnScrollViewerChanged(object sender, RoutedEventArgs args)
        {
            if (IsEditing && Mouse.PrimaryDevice.LeftButton ==
                MouseButtonState.Pressed)
            {
                IsEditing = false;
            }
        }

        /// <summary>
        ///     Walk visual tree to find the first DependencyObject
        ///     of the specific type.
        /// </summary>
        private DependencyObject
            GetDependencyObjectFromVisualTree(DependencyObject startObject,
                Type type)
        {
            //Walk the visual tree to get the parent(ItemsControl) 
            //of this control
            var parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        /// <summary>
        ///     When the size of the column containing the EditBox changes
        ///     and the EditBox is in editing mode, switch the mode to normal mode
        /// </summary>
        private void HookTemplateParentResizeEvent()
        {
            var parent = TemplatedParent as FrameworkElement;
            if (parent != null)
            {
                parent.SizeChanged +=
                    OnCouldSwitchToNormalMode;
            }
        }

        #endregion

        #region private variable

        private EditBoxAdorner _adorner;
        //A TextBox in the visual tree
        private FrameworkElement _textBox;
        //Specifies whether an EditBox can switch to editing mode.
        //Set to true if the ListViewItem that contains the EditBox is
        //selected, and the mouse pointer moves over the EditBox.
        private bool _canBeEdit;
        //Specifies whether an EditBox can switch to editing mode.
        //Set to true if the ListViewItem that contains the EditBox is
        //selected, the mouse pointer is over the EditBox, and MouseUp
        //event occurs for the left mouse button.
        private bool _isMouseWithinScope;
        //The ListView control that contains the EditBox
        private ItemsControl _itemsControl;
        //The ListViewItem control that contains the EditBox
        private ListViewItem _listViewItem;

        #endregion
    }
}
