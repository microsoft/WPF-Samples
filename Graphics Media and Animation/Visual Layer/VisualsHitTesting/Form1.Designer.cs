namespace SDKSample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.CirclesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FillWithCirclesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddCircleChildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SmallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RandomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numberofRingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.behaviorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TopmostLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AllLayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CirclesToolStripMenuItem,
            this.SettingToolStripMenuItem,
            this.behaviorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(634, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // CirclesToolStripMenuItem
            // 
            this.CirclesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FillWithCirclesToolStripMenuItem,
            this.AddCircleChildToolStripMenuItem});
            this.CirclesToolStripMenuItem.Name = "CirclesToolStripMenuItem";
            this.CirclesToolStripMenuItem.Text = "Circles";
            // 
            // FillWithCirclesToolStripMenuItem
            // 
            this.FillWithCirclesToolStripMenuItem.Name = "FillWithCirclesToolStripMenuItem";
            this.FillWithCirclesToolStripMenuItem.Text = "Fill with Circles";
            this.FillWithCirclesToolStripMenuItem.Click += new System.EventHandler(this.FillWithCirclesToolStripMenuItem_Click);
            // 
            // AddCircleChildToolStripMenuItem
            // 
            this.AddCircleChildToolStripMenuItem.Name = "AddCircleChildToolStripMenuItem";
            this.AddCircleChildToolStripMenuItem.Text = "Add a Circle";
            this.AddCircleChildToolStripMenuItem.Click += new System.EventHandler(this.AddChildToolStripMenuItem_Click);
            // 
            // SettingToolStripMenuItem
            // 
            this.SettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.circleSizeToolStripMenuItem,
            this.numberofRingsToolStripMenuItem});
            this.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem";
            this.SettingToolStripMenuItem.Text = "Setting";
            // 
            // circleSizeToolStripMenuItem
            // 
            this.circleSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SmallToolStripMenuItem,
            this.MediumToolStripMenuItem,
            this.LargeToolStripMenuItem,
            this.RandomToolStripMenuItem});
            this.circleSizeToolStripMenuItem.Name = "circleSizeToolStripMenuItem";
            this.circleSizeToolStripMenuItem.Text = "Circle Size";
            // 
            // SmallToolStripMenuItem
            // 
            this.SmallToolStripMenuItem.Name = "SmallToolStripMenuItem";
            this.SmallToolStripMenuItem.Text = "Small";
            this.SmallToolStripMenuItem.Click += new System.EventHandler(this.SmallToolStripMenuItem_Click);
            // 
            // MediumToolStripMenuItem
            // 
            this.MediumToolStripMenuItem.Checked = true;
            this.MediumToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MediumToolStripMenuItem.Name = "MediumToolStripMenuItem";
            this.MediumToolStripMenuItem.Text = "Medium";
            this.MediumToolStripMenuItem.Click += new System.EventHandler(this.MediumToolStripMenuItem_Click);
            // 
            // LargeToolStripMenuItem
            // 
            this.LargeToolStripMenuItem.Name = "LargeToolStripMenuItem";
            this.LargeToolStripMenuItem.Text = "Large";
            this.LargeToolStripMenuItem.Click += new System.EventHandler(this.LargeToolStripMenuItem_Click);
            // 
            // RandomToolStripMenuItem
            // 
            this.RandomToolStripMenuItem.Name = "RandomToolStripMenuItem";
            this.RandomToolStripMenuItem.Text = "Random";
            this.RandomToolStripMenuItem.Click += new System.EventHandler(this.RandomToolStripMenuItem_Click);
            // 
            // numberofRingsToolStripMenuItem
            // 
            this.numberofRingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ThreeToolStripMenuItem,
            this.FiveToolStripMenuItem,
            this.EightToolStripMenuItem});
            this.numberofRingsToolStripMenuItem.Name = "numberofRingsToolStripMenuItem";
            this.numberofRingsToolStripMenuItem.Text = "Number of Rings";
            // 
            // ThreeToolStripMenuItem
            // 
            this.ThreeToolStripMenuItem.Name = "ThreeToolStripMenuItem";
            this.ThreeToolStripMenuItem.Text = "3";
            this.ThreeToolStripMenuItem.Click += new System.EventHandler(this.ThreeToolStripMenuItem_Click);
            // 
            // FiveToolStripMenuItem
            // 
            this.FiveToolStripMenuItem.Checked = true;
            this.FiveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FiveToolStripMenuItem.Name = "FiveToolStripMenuItem";
            this.FiveToolStripMenuItem.Text = "5";
            this.FiveToolStripMenuItem.Click += new System.EventHandler(this.FiveToolStripMenuItem_Click);
            // 
            // EightToolStripMenuItem
            // 
            this.EightToolStripMenuItem.Name = "EightToolStripMenuItem";
            this.EightToolStripMenuItem.Text = "8";
            this.EightToolStripMenuItem.Click += new System.EventHandler(this.EightToolStripMenuItem_Click);
            // 
            // behaviorToolStripMenuItem
            // 
            this.behaviorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TopmostLayerToolStripMenuItem,
            this.AllLayersToolStripMenuItem});
            this.behaviorToolStripMenuItem.Name = "behaviorToolStripMenuItem";
            this.behaviorToolStripMenuItem.Text = "Behavior";
            // 
            // TopmostLayerToolStripMenuItem
            // 
            this.TopmostLayerToolStripMenuItem.Checked = true;
            this.TopmostLayerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TopmostLayerToolStripMenuItem.Name = "TopmostLayerToolStripMenuItem";
            this.TopmostLayerToolStripMenuItem.Text = "Top-most Layer";
            this.TopmostLayerToolStripMenuItem.Click += new System.EventHandler(this.TopmostLayerToolStripMenuItem_Click);
            // 
            // AllLayersToolStripMenuItem
            // 
            this.AllLayersToolStripMenuItem.Name = "AllLayersToolStripMenuItem";
            this.AllLayersToolStripMenuItem.Text = "All Layers";
            this.AllLayersToolStripMenuItem.Click += new System.EventHandler(this.AllLayersToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(634, 448);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Visual Hit Test";
            this.menuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CirclesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddCircleChildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FillWithCirclesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SmallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MediumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LargeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem numberofRingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ThreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RandomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem behaviorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TopmostLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AllLayersToolStripMenuItem;
    }
}

