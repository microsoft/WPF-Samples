// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace InsertTextW32Target
{
    public partial class Target : Form
    {
        public Target()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbTargetMultiLine.Enabled = (checkBox1.CheckState != CheckState.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            rtbTarget.Enabled = (checkBox2.CheckState != CheckState.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            tbTarget.Enabled = (checkBox3.CheckState != CheckState.Checked);
        }
    }
}