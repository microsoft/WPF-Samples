using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            tbTargetMultiLine.Enabled = (checkBox1.CheckState == CheckState.Checked) ? false : true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            rtbTarget.Enabled = (checkBox2.CheckState == CheckState.Checked) ? false : true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            tbTarget.Enabled = (checkBox3.CheckState == CheckState.Checked) ? false : true;
        }
    }
}