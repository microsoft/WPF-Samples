namespace SelectionPatternTarget
{
    /// <summary>
    /// The target application for the SelectionPattern Sample.
    /// </summary>
    partial class Target
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Item1");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Item2");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Item3");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Item4");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Item5");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Item6");
            this.ListBox = new System.Windows.Forms.ListBox();
            this.ListView = new System.Windows.Forms.ListView();
            this.ComboBox = new System.Windows.Forms.ComboBox();
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListBox
            // 
            this.ListBox.FormattingEnabled = true;
            this.ListBox.Items.AddRange(new object[] {
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5",
            "Item6"});
            this.ListBox.Location = new System.Drawing.Point(36, 25);
            this.ListBox.Name = "ListBox";
            this.ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.ListBox.Size = new System.Drawing.Size(121, 95);
            this.ListBox.TabIndex = 0;
            // 
            // ListView
            // 
            this.ListView.HideSelection = false;
            listViewItem1.Checked = true;
            listViewItem1.StateImageIndex = 1;
            listViewItem1.Tag = "";
            this.ListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.ListView.Location = new System.Drawing.Point(27, 28);
            this.ListView.Name = "ListView";
            this.ListView.Size = new System.Drawing.Size(150, 86);
            this.ListView.TabIndex = 1;
            this.ListView.UseCompatibleStateImageBehavior = false;
            // 
            // ComboBox
            // 
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Items.AddRange(new object[] {
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5",
            "Item6"});
            this.ComboBox.Location = new System.Drawing.Point(36, 28);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(121, 21);
            this.ComboBox.TabIndex = 2;
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.CheckOnClick = true;
            this.CheckedListBox.FormattingEnabled = true;
            this.CheckedListBox.Items.AddRange(new object[] {
            "CheckBoxItem1",
            "CheckBoxItem2",
            "CheckBoxItem3",
            "CheckBoxItem4",
            "CheckBoxItem5",
            "CheckBoxItem6"});
            this.CheckedListBox.Location = new System.Drawing.Point(27, 26);
            this.CheckedListBox.Name = "CheckedListBox";
            this.CheckedListBox.Size = new System.Drawing.Size(120, 94);
            this.CheckedListBox.TabIndex = 3;
            this.CheckedListBox.UseWaitCursor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ListView);
            this.groupBox1.Location = new System.Drawing.Point(242, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ListView";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CheckedListBox);
            this.groupBox2.Location = new System.Drawing.Point(23, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 134);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CheckedListBox";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ComboBox);
            this.groupBox3.Location = new System.Drawing.Point(23, 181);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 134);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ComboBox";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ListBox);
            this.groupBox4.Location = new System.Drawing.Point(242, 181);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 134);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ListBox";
            // 
            // Target
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 338);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Name = "Target";
            this.Text = "SelectionPatternTarget";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListBox;
        private System.Windows.Forms.ListView ListView;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.CheckedListBox CheckedListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}

