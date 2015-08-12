// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Button = System.Windows.Controls.Button;
using Color = System.Drawing.Color;

namespace PropertyMappingWithElementHost
{
    public partial class Form1 : Form
    {
        private ElementHost _elemHost;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create the ElementHost control.
            _elemHost = new ElementHost {Dock = DockStyle.Fill};
            Controls.Add(_elemHost);

            // Create a Windows Presentation Foundation Button element 
            // and assign it as the ElementHost control's child. 
            var wpfButton = new Button {Content = "Windows Presentation Foundation Button"};
            _elemHost.Child = wpfButton;

            // Map the Margin property.
            AddMarginMapping();

            // Remove the mapping for the Cursor property.
            RemoveCursorMapping();

            // Add a mapping for the Region property.
            AddRegionMapping();

            // Add another mapping for the BackColor property.
            ExtendBackColorMapping();

            // Cause the OnMarginChange delegate to be called.
            _elemHost.Margin = new Padding(23, 23, 23, 23);

            // Cause the OnRegionChange delegate to be called.
            _elemHost.Region = new Region();

            // Cause the OnBackColorChange delegate to be called.
            _elemHost.BackColor = Color.AliceBlue;
        }

        // The AddMarginMapping method adds a new property mapping
        // for the Margin property.
        private void AddMarginMapping()
        {
            _elemHost.PropertyMap.Add(
                "Margin",
                OnMarginChange);
        }

        // The OnMarginChange method implements the mapping 
        // from the Windows Forms Margin property to the
        // Windows Presentation Foundation Margin property.
        //
        // The provided Padding value is used to construct 
        // a Thickness value for the hosted element's Margin
        // property.
        private void OnMarginChange(object h, string propertyName, object value)
        {
            var host = h as ElementHost;
            var p = (Padding) value;
            var wpfButton =
                host.Child as Button;

            var t = new Thickness(p.Left, p.Top, p.Right, p.Bottom);

            wpfButton.Margin = t;
        }

        // The RemoveCursorMapping method deletes the default
        // mapping for the Cursor property.
        private void RemoveCursorMapping()
        {
            _elemHost.PropertyMap.Remove("Cursor");
        }

        // The AddRegionMapping method assigns a custom 
        // mapping for the Region property.
        private void AddRegionMapping()
        {
            _elemHost.PropertyMap.Add(
                "Region",
                OnRegionChange);
        }

        // The OnRegionChange method assigns an EllipseGeometry to
        // the hosted element's Clip property.
        private void OnRegionChange(
            object h,
            string propertyName,
            object value)
        {
            var host = h as ElementHost;
            var wpfButton =
                host.Child as Button;

            wpfButton.Clip = new EllipseGeometry(new Rect(
                0,
                0,
                wpfButton.ActualWidth,
                wpfButton.ActualHeight));
        }

        // The Form1_Resize method handles the form's Resize event.
        // It calls the OnRegionChange method explicitly to 
        // assign a new clipping geometry to the hosted element.
        private void Form1_Resize(object sender, EventArgs e)
        {
            OnRegionChange(_elemHost, "Region", null);
        }

        // The ExtendBackColorMapping method adds a property
        // translator if a mapping already exists.
        private void ExtendBackColorMapping()
        {
            if (_elemHost.PropertyMap["BackColor"] != null)
            {
                _elemHost.PropertyMap["BackColor"] +=
                    OnBackColorChange;
            }
        }

        // The OnBackColorChange method assigns a specific image 
        // to the hosted element's Background property.
        private void OnBackColorChange(object h, string propertyName, object value)
        {
            var host = h as ElementHost;
            var wpfButton =
                host.Child as Button;

            var b = new ImageBrush(new BitmapImage(
                new Uri(@"file:///C:\WINDOWS\Santa Fe Stucco.bmp")));
            wpfButton.Background = b;
        }
    }
}

// </snippet1>