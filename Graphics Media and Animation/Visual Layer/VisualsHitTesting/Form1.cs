// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace SDKSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AddChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyWindow.CreateShape(Handle);
        }

        private void FillWithCirclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyWindow.FillWithCircles(Handle);
        }

        private void SmallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.Radius = 30.0d;
            SmallToolStripMenuItem.Checked = true;
            MediumToolStripMenuItem.Checked = false;
            LargeToolStripMenuItem.Checked = false;
            RandomToolStripMenuItem.Checked = false;
        }

        private void MediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.Radius = 50.0d;
            SmallToolStripMenuItem.Checked = false;
            MediumToolStripMenuItem.Checked = true;
            LargeToolStripMenuItem.Checked = false;
            RandomToolStripMenuItem.Checked = false;
        }

        private void LargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.Radius = 100.0d;
            SmallToolStripMenuItem.Checked = false;
            MediumToolStripMenuItem.Checked = false;
            LargeToolStripMenuItem.Checked = true;
            RandomToolStripMenuItem.Checked = false;
        }

        private void RandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.Radius = 0.0d;
            SmallToolStripMenuItem.Checked = false;
            MediumToolStripMenuItem.Checked = false;
            LargeToolStripMenuItem.Checked = false;
            RandomToolStripMenuItem.Checked = true;
        }

        private void ThreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.NumberCircles = 3;
            ThreeToolStripMenuItem.Checked = true;
            FiveToolStripMenuItem.Checked = false;
            EightToolStripMenuItem.Checked = false;
        }

        private void FiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.NumberCircles = 5;
            ThreeToolStripMenuItem.Checked = false;
            FiveToolStripMenuItem.Checked = true;
            EightToolStripMenuItem.Checked = false;
        }

        private void EightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyShape.NumberCircles = 8;
            ThreeToolStripMenuItem.Checked = false;
            FiveToolStripMenuItem.Checked = false;
            EightToolStripMenuItem.Checked = true;
        }

        private void TopmostLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyWindow.TopmostLayer = true;
            TopmostLayerToolStripMenuItem.Checked = true;
            AllLayersToolStripMenuItem.Checked = false;
        }

        private void AllLayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyWindow.TopmostLayer = false;
            AllLayersToolStripMenuItem.Checked = true;
            TopmostLayerToolStripMenuItem.Checked = false;
        }
    }
}