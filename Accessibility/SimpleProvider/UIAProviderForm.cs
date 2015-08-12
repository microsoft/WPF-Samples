// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleProvider
{
    public partial class UIAProviderForm : Form
    {
        /// <summary>
        ///     Constructor for the application window.
        /// </summary>
        public UIAProviderForm()
        {
            InitializeComponent();

            // Create an instance of the custom control.
            var controlRect = new Rectangle(30, 15, 55, 40);
            var myCustomButton = new CustomButton
            {
                Text = "CustomControl",
                Location = new Point(controlRect.X, controlRect.Y),
                Size = new Size(controlRect.Width, controlRect.Bottom),
                TabIndex = 1
            };

            // This becomes the Name property for UI Automation.

            // Give the control a location and size so that it will trap mouse clicks
            // and will be repainted as necessary.

            // Add it to the form's controls. Among other things, this makes it possible for
            // UI Automation to discover it, as it will become a child of the application window.
            Controls.Add(myCustomButton);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}