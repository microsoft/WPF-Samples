// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyControls
{
    /// <summary>
    ///     Summary description for UserControl1.
    /// </summary>
    public class MyControl : UserControl
    {
        public delegate void MyControlEventHandler(object sender, MyControlEventArgs args);

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        private Label _label1;
        private Label _label2;
        private Button _btnCancel;
        private Button _btnOk;
        private Label _label3;
        private Label _label4;
        private Label _label5;
        private Label _label6;
        private TextBox _txtAddress;
        private TextBox _txtCity;
        private TextBox _txtName;
        private TextBox _txtState;
        private TextBox _txtZip;

        public MyControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitComponent call
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._label1 = new System.Windows.Forms.Label();
            this._label2 = new System.Windows.Forms.Label();
            this._label3 = new System.Windows.Forms.Label();
            this._label4 = new System.Windows.Forms.Label();
            this._label5 = new System.Windows.Forms.Label();
            this._txtName = new System.Windows.Forms.TextBox();
            this._txtAddress = new System.Windows.Forms.TextBox();
            this._txtCity = new System.Windows.Forms.TextBox();
            this._txtState = new System.Windows.Forms.TextBox();
            this._txtZip = new System.Windows.Forms.TextBox();
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this._label1.Location = new System.Drawing.Point(20, 46);
            this._label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(65, 16);
            this._label1.TabIndex = 8;
            this._label1.Text = "Name";
            this._label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this._label2.Location = new System.Drawing.Point(20, 88);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(94, 13);
            this._label2.TabIndex = 9;
            this._label2.Text = "Street Address";
            this._label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this._label3.Location = new System.Drawing.Point(20, 127);
            this._label3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(49, 13);
            this._label3.TabIndex = 10;
            this._label3.Text = "City";
            this._label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this._label4.Location = new System.Drawing.Point(246, 127);
            this._label4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(47, 13);
            this._label4.TabIndex = 11;
            this._label4.Text = "State";
            this._label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this._label5.Location = new System.Drawing.Point(23, 167);
            this._label5.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(46, 13);
            this._label5.TabIndex = 12;
            this._label5.Text = "Zip";
            this._label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this._txtName.Location = new System.Drawing.Point(135, 44);
            this._txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this._txtName.Name = "_txtName";
            this._txtName.Size = new System.Drawing.Size(199, 20);
            this._txtName.TabIndex = 0;
            // 
            // txtAddress
            // 
            this._txtAddress.Location = new System.Drawing.Point(136, 84);
            this._txtAddress.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this._txtAddress.Name = "_txtAddress";
            this._txtAddress.Size = new System.Drawing.Size(198, 20);
            this._txtAddress.TabIndex = 1;
            // 
            // txtCity
            // 
            this._txtCity.Location = new System.Drawing.Point(136, 123);
            this._txtCity.Name = "_txtCity";
            this._txtCity.TabIndex = 2;
            // 
            // txtState
            // 
            this._txtState.Location = new System.Drawing.Point(300, 123);
            this._txtState.Name = "_txtState";
            this._txtState.Size = new System.Drawing.Size(33, 20);
            this._txtState.TabIndex = 3;
            // 
            // txtZip
            // 
            this._txtZip.Location = new System.Drawing.Point(135, 163);
            this._txtZip.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this._txtZip.Name = "_txtZip";
            this._txtZip.TabIndex = 4;
            // 
            // btnOK
            // 
            this._btnOk.Location = new System.Drawing.Point(23, 207);
            this._btnOk.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this._btnOk.Name = "_btnOk";
            this._btnOk.TabIndex = 5;
            this._btnOk.Text = "OK";
            this._btnOk.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(157, 207);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.TabIndex = 6;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label6
            // 
            this._label6.Location = new System.Drawing.Point(66, 12);
            this._label6.Name = "_label6";
            this._label6.Size = new System.Drawing.Size(226, 23);
            this._label6.TabIndex = 13;
            this._label6.Text = "Simple Windows Forms Control";
            this._label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MyControl1
            // 
            this.Controls.Add(this._label6);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._txtZip);
            this.Controls.Add(this._txtState);
            this.Controls.Add(this._txtCity);
            this.Controls.Add(this._txtAddress);
            this.Controls.Add(this._txtName);
            this.Controls.Add(this._label5);
            this.Controls.Add(this._label4);
            this.Controls.Add(this._label3);
            this.Controls.Add(this._label2);
            this.Controls.Add(this._label1);
            this.Name = "MyControl1";
            this.Size = new System.Drawing.Size(359, 244);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        public event MyControlEventHandler OnButtonClick;

        private void OKButton_Click(object sender, EventArgs e)
        {
            var retvals = new MyControlEventArgs(true,
                _txtName.Text,
                _txtAddress.Text,
                _txtCity.Text,
                _txtState.Text,
                _txtZip.Text);
            OnButtonClick(this, retvals);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            var retvals = new MyControlEventArgs(false,
                _txtName.Text,
                _txtAddress.Text,
                _txtCity.Text,
                _txtState.Text,
                _txtZip.Text);
            OnButtonClick(this, retvals);
        }
    }
}

// </snippet1>