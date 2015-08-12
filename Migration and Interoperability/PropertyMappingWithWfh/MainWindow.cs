// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using Application = System.Windows.Forms.Application;
using FlowDirection = System.Windows.FlowDirection;

namespace PropertyMappingWithWfh
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // The WindowLoaded method handles the Loaded event.
        // It enables Windows Forms visual styles, creates 
        // a Windows Forms checkbox control, and assigns the
        // control as the child of the WindowsFormsHost element. 
        // This method also modifies property mappings on the 
        // WindowsFormsHost element.
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Application.EnableVisualStyles();

            // Create a Windows Forms checkbox control and assign 
            // it as the WindowsFormsHost element's child.
            var cb = new CheckBox
            {
                Text = "Windows Forms checkbox",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            cb.CheckedChanged += cb_CheckedChanged;
            wfHost.Child = cb;

            // Replace the default mapping for the FlowDirection property.
            ReplaceFlowDirectionMapping();

            // Remove the mapping for the Cursor property.
            RemoveCursorMapping();

            // Add the mapping for the Clip property.
            AddClipMapping();

            // Add another mapping for the Background property.
            ExtendBackgroundMapping();

            // Cause the OnFlowDirectionChange delegate to be called.
            wfHost.FlowDirection = FlowDirection.LeftToRight;

            // Cause the OnClipChange delegate to be called.
            wfHost.Clip = new RectangleGeometry();

            // Cause the OnBackgroundChange delegate to be called.
            wfHost.Background = new ImageBrush();
        }

        // The ReplaceFlowDirectionMapping method replaces the  
        // default mapping for the FlowDirection property.
        private void ReplaceFlowDirectionMapping()
        {
            wfHost.PropertyMap.Remove("FlowDirection");

            wfHost.PropertyMap.Add(
                "FlowDirection",
                OnFlowDirectionChange);
        }

        // The OnFlowDirectionChange method translates a 
        // Windows Presentation Foundation FlowDirection value 
        // to a Windows Forms RightToLeft value and assigns
        // the result to the hosted control's RightToLeft property.
        private void OnFlowDirectionChange(object h, string propertyName, object value)
        {
            var host = h as WindowsFormsHost;
            var fd = (FlowDirection) value;
            var cb = host.Child as CheckBox;

            cb.RightToLeft = (fd == FlowDirection.RightToLeft)
                ? RightToLeft.Yes
                : RightToLeft.No;
        }

        // The cb_CheckedChanged method handles the hosted control's
        // CheckedChanged event. If the Checked property is true,
        // the flow direction is set to RightToLeft, otherwise it is
        // set to LeftToRight.
        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;

            wfHost.FlowDirection = (cb.CheckState == CheckState.Checked)
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;
        }

        // The RemoveCursorMapping method deletes the default
        // mapping for the Cursor property.
        private void RemoveCursorMapping()
        {
            wfHost.PropertyMap.Remove("Cursor");
        }

        // The AddClipMapping method adds a custom 
        // mapping for the Clip property.
        private void AddClipMapping()
        {
            wfHost.PropertyMap.Add(
                "Clip",
                OnClipChange);
        }

        // The OnClipChange method assigns an elliptical clipping 
        // region to the hosted control's Region property.
        private void OnClipChange(object h, string propertyName, object value)
        {
            var host = h as WindowsFormsHost;
            var cb = host.Child as CheckBox;

            if (cb != null)
            {
                cb.Region = CreateClipRegion();
            }
        }

        // The Window1_SizeChanged method handles the window's 
        // SizeChanged event. It calls the OnClipChange method explicitly 
        // to assign a new clipping region to the hosted control.
        private void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnClipChange(wfHost, "Clip", null);
        }

        // The CreateClipRegion method creates a Region from an
        // elliptical GraphicsPath.
        private Region CreateClipRegion()
        {
            var path = new GraphicsPath();

            path.StartFigure();

            path.AddEllipse(new Rectangle(
                0,
                0,
                (int) wfHost.ActualWidth,
                (int) wfHost.ActualHeight));

            path.CloseFigure();

            return (new Region(path));
        }

        // The ExtendBackgroundMapping method adds a property
        // translator if a mapping already exists.
        private void ExtendBackgroundMapping()
        {
            if (wfHost.PropertyMap["Background"] != null)
            {
                wfHost.PropertyMap["Background"] += OnBackgroundChange;
            }
        }

        // The OnBackgroundChange method assigns a specific image 
        // to the hosted control's BackgroundImage property.
        private void OnBackgroundChange(object h, string propertyName, object value)
        {
            var host = h as WindowsFormsHost;
            var cb = host.Child as CheckBox;
            var b = value as ImageBrush;

            if (b != null)
            {
                cb.BackgroundImage = new Bitmap(@"C:\WINDOWS\Santa Fe Stucco.bmp");
            }
        }
    }
}