// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace FragmentProvider
{
    public partial class SampleApplicationForm : Form
    {
        private readonly CustomListControl _customList;

        /// <summary>
        ///     Constructor for the application window.
        /// </summary>
        public SampleApplicationForm()
        {
            InitializeComponent();

            // Create a label.  
            var listLabel = new Label
            {
                Location = new Point(20, 10),
                AutoSize = true,
                TabIndex = 0,
                Text = "&Contacts:"
            };
            Controls.Add(listLabel);

            // Create an instance of the custom control.
            _customList = new CustomListControl {Name = "customList"};

            // Add it to the form's controls. Among other things, this makes it possible for
            // UIAutomation to discover it, as it will become a child of the application window.
            Controls.Add(_customList);

            // Set some properties.
            _customList.Location = new Point(20, 30);
            // Text becomes the Name property for the custom control.
            _customList.Text = listLabel.Text;
            _customList.TabIndex = 3;

            // Add list items.
            _customList.Add("Prakash", Availability.Online);
            _customList.Add("James", Availability.Online);
            _customList.Add("Lisa", Availability.Offline);
            _customList.Add("Kim", Availability.Online);
            _customList.Add("Bailey", Availability.Offline);
        }

        /// <summary>
        ///     Handles Add button clicks.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtItem.Text.Length != 0)
            {
                _customList.Add(txtItem.Text, radioOn.Checked ? Availability.Online : Availability.Offline);
                txtItem.Clear();
            }
        }

        /// <summary>
        ///     Handles Remove button clicks.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            var success = _customList.Remove(_customList.SelectedIndex);
        }

        /// <summary>
        ///     Handles Exit button clicks.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}