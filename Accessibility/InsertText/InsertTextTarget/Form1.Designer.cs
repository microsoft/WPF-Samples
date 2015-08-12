namespace InsertTextW32Target
{
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
            this.tbTarget = new System.Windows.Forms.TextBox();
            this.rtbTarget = new System.Windows.Forms.RichTextBox();
            this.tbTargetMultiLine = new System.Windows.Forms.TextBox();
            this.lblSingleLineTextBox = new System.Windows.Forms.Label();
            this.lblMultilineTextBox = new System.Windows.Forms.Label();
            this.lblRichTextBox = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbTarget
            // 
            this.tbTarget.AccessibleDescription = "Target TextBox";
            this.tbTarget.AccessibleName = "tbTarget";
            this.tbTarget.AllowDrop = true;
            this.tbTarget.Location = new System.Drawing.Point(119, 223);
            this.tbTarget.MaxLength = 2;
            this.tbTarget.Name = "tbTarget";
            this.tbTarget.Size = new System.Drawing.Size(161, 20);
            this.tbTarget.TabIndex = 0;
            // 
            // rtbTarget
            // 
            this.rtbTarget.AcceptsTab = true;
            this.rtbTarget.AccessibleDescription = "Target RichTextBox";
            this.rtbTarget.AccessibleName = "rtbTarget";
            this.rtbTarget.Location = new System.Drawing.Point(119, 103);
            this.rtbTarget.Name = "rtbTarget";
            this.rtbTarget.Size = new System.Drawing.Size(161, 96);
            this.rtbTarget.TabIndex = 2;
            this.rtbTarget.Text = "";
            // 
            // tbTargetMultiLine
            // 
            this.tbTargetMultiLine.AcceptsReturn = true;
            this.tbTargetMultiLine.AcceptsTab = true;
            this.tbTargetMultiLine.AccessibleDescription = "Target multi-line TextBox";
            this.tbTargetMultiLine.AccessibleName = "mtbTarget";
            this.tbTargetMultiLine.AllowDrop = true;
            this.tbTargetMultiLine.Location = new System.Drawing.Point(119, 12);
            this.tbTargetMultiLine.Multiline = true;
            this.tbTargetMultiLine.Name = "tbTargetMultiLine";
            this.tbTargetMultiLine.Size = new System.Drawing.Size(161, 73);
            this.tbTargetMultiLine.TabIndex = 1;
            this.tbTargetMultiLine.Tag = "test";
            // 
            // lblSingleLineTextBox
            // 
            this.lblSingleLineTextBox.AccessibleDescription = "Label for single line TextBox";
            this.lblSingleLineTextBox.AccessibleName = "lblSingleLineTextBox";
            this.lblSingleLineTextBox.Location = new System.Drawing.Point(12, 223);
            this.lblSingleLineTextBox.Name = "lblSingleLineTextBox";
            this.lblSingleLineTextBox.Size = new System.Drawing.Size(101, 53);
            this.lblSingleLineTextBox.TabIndex = 3;
            this.lblSingleLineTextBox.Text = "Single Line TextBox (2 character limit) (tbTarget)";
            // 
            // lblMultilineTextBox
            // 
            this.lblMultilineTextBox.AccessibleDescription = "Label for multi-line TextBox";
            this.lblMultilineTextBox.AccessibleName = "lblMultiLineTextBox";
            this.lblMultilineTextBox.Location = new System.Drawing.Point(12, 12);
            this.lblMultilineTextBox.Name = "lblMultilineTextBox";
            this.lblMultilineTextBox.Size = new System.Drawing.Size(101, 50);
            this.lblMultilineTextBox.TabIndex = 4;
            this.lblMultilineTextBox.Text = "Multi-Line TextBox (tbTargetMultiLine)";
            // 
            // lblRichTextBox
            // 
            this.lblRichTextBox.AccessibleDescription = "Label for rich text TextBox";
            this.lblRichTextBox.AccessibleName = "lblRichTextBox";
            this.lblRichTextBox.Location = new System.Drawing.Point(12, 103);
            this.lblRichTextBox.Name = "lblRichTextBox";
            this.lblRichTextBox.Size = new System.Drawing.Size(94, 38);
            this.lblRichTextBox.TabIndex = 5;
            this.lblRichTextBox.Text = "RichTextBox (rtbTarget)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 40);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Read-only";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(15, 132);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(74, 17);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Read-only";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(15, 266);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(74, 17);
            this.checkBox3.TabIndex = 10;
            this.checkBox3.Text = "Read-only";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // Target
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 312);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lblRichTextBox);
            this.Controls.Add(this.lblMultilineTextBox);
            this.Controls.Add(this.lblSingleLineTextBox);
            this.Controls.Add(this.tbTargetMultiLine);
            this.Controls.Add(this.rtbTarget);
            this.Controls.Add(this.tbTarget);
            this.Name = "Target";
            this.Text = "Target";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbTarget;
        private System.Windows.Forms.RichTextBox rtbTarget;
        private System.Windows.Forms.TextBox tbTargetMultiLine;
        private System.Windows.Forms.Label lblSingleLineTextBox;
        private System.Windows.Forms.Label lblMultilineTextBox;
        private System.Windows.Forms.Label lblRichTextBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}

