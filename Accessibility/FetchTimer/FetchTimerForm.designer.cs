/*************************************************************************************************
 *
 * File: FetchTimerForm.Designer.cs
 *
 * Description: Design-time code for the interface.
 * 
 * See FetchTimerForm.cs for a full description of the sample.
 *
 *     
 *  This file is part of the Microsoft WinfFX SDK Code Samples.
 * 
 *  Copyright (C) Microsoft Corporation.  All rights reserved.
 * 
 * This source code is intended only as a supplement to Microsoft
 * Development Tools and/or on-line documentation.  See these other
 * materials for detailed information regarding Microsoft code samples.
 * 
 * THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 * 
 *************************************************************************************************/
namespace FetchTimer
{
    partial class FetchTimerForm
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
            this.gbScope = new System.Windows.Forms.GroupBox();
            this.cbDescendants = new System.Windows.Forms.CheckBox();
            this.cbChildren = new System.Windows.Forms.CheckBox();
            this.comboElement = new System.Windows.Forms.CheckBox();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnProps = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbFull = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.gbScope.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbScope
            // 
            this.gbScope.Controls.Add(this.cbDescendants);
            this.gbScope.Controls.Add(this.cbChildren);
            this.gbScope.Controls.Add(this.comboElement);
            this.gbScope.Location = new System.Drawing.Point(239, 76);
            this.gbScope.Name = "gbScope";
            this.gbScope.Size = new System.Drawing.Size(97, 93);
            this.gbScope.TabIndex = 3;
            this.gbScope.TabStop = false;
            this.gbScope.Text = "TreeScope";
            // 
            // cbDescendants
            // 
            this.cbDescendants.AutoSize = true;
            this.cbDescendants.Location = new System.Drawing.Point(6, 68);
            this.cbDescendants.Name = "cbDescendants";
            this.cbDescendants.Size = new System.Drawing.Size(89, 17);
            this.cbDescendants.TabIndex = 2;
            this.cbDescendants.Text = "&Descendants";
            this.cbDescendants.UseVisualStyleBackColor = true;
            this.cbDescendants.CheckedChanged += new System.EventHandler(this.cbDescendants_CheckedChanged);
            // 
            // cbChildren
            // 
            this.cbChildren.AutoSize = true;
            this.cbChildren.Location = new System.Drawing.Point(6, 44);
            this.cbChildren.Name = "cbChildren";
            this.cbChildren.Size = new System.Drawing.Size(64, 17);
            this.cbChildren.TabIndex = 1;
            this.cbChildren.Text = "&Children";
            this.cbChildren.UseVisualStyleBackColor = true;
            // 
            // comboElement
            // 
            this.comboElement.AutoCheck = false;
            this.comboElement.AutoSize = true;
            this.comboElement.Checked = true;
            this.comboElement.CheckState = System.Windows.Forms.CheckState.Checked;
            this.comboElement.Location = new System.Drawing.Point(6, 20);
            this.comboElement.Name = "comboElement";
            this.comboElement.Size = new System.Drawing.Size(64, 17);
            this.comboElement.TabIndex = 0;
            this.comboElement.TabStop = false;
            this.comboElement.Text = "Element";
            this.comboElement.UseVisualStyleBackColor = true;
            // 
            // tbOutput
            // 
            this.tbOutput.BackColor = System.Drawing.SystemColors.Window;
            this.tbOutput.Location = new System.Drawing.Point(10, 180);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(385, 165);
            this.tbOutput.TabIndex = 6;
            this.tbOutput.TabStop = false;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(20, 156);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(42, 13);
            this.lblOutput.TabIndex = 5;
            this.lblOutput.Text = "Results";
            this.lblOutput.Visible = false;
            // 
            // btnProps
            // 
            this.btnProps.Location = new System.Drawing.Point(156, 39);
            this.btnProps.Name = "btnProps";
            this.btnProps.Size = new System.Drawing.Size(100, 23);
            this.btnProps.TabIndex = 1;
            this.btnProps.Text = "&Get Properties";
            this.btnProps.UseVisualStyleBackColor = true;
            this.btnProps.Click += new System.EventHandler(this.btnProps_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(319, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Activate this window, put the cursor over an element\nanywhere on the screen, and " +
                "invoke the button by pressing Alt+G.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(68, 151);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clea&r";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Controls.Add(this.rbFull);
            this.groupBox1.Location = new System.Drawing.Point(36, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 69);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AutomationElementMode";
            // 
            // rbFull
            // 
            this.rbFull.AutoSize = true;
            this.rbFull.Checked = true;
            this.rbFull.Location = new System.Drawing.Point(51, 20);
            this.rbFull.Name = "rbFull";
            this.rbFull.Size = new System.Drawing.Size(41, 17);
            this.rbFull.TabIndex = 0;
            this.rbFull.TabStop = true;
            this.rbFull.Text = "&Full";
            this.rbFull.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(51, 44);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 1;
            this.rbNone.Text = "&None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // FetchForm
            // 
            this.ClientSize = new System.Drawing.Size(405, 357);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProps);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.gbScope);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FetchForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "UI Automation Caching Sample";
            this.TopMost = true;
            this.gbScope.ResumeLayout(false);
            this.gbScope.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbScope;
        private System.Windows.Forms.CheckBox comboElement;
        private System.Windows.Forms.CheckBox cbDescendants;
        private System.Windows.Forms.CheckBox cbChildren;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnProps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbFull;
    }
}

