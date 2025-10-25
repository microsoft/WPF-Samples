namespace WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            radioBackgroundLightSalmon = new RadioButton();
            radioBackgroundLightGreen = new RadioButton();
            radioBackgroundOriginal = new RadioButton();
            radioForegroundYellow = new RadioButton();
            radioForegroundRed = new RadioButton();
            groupBox2 = new GroupBox();
            radioForegroundOriginal = new RadioButton();
            radioFamilyWingDings = new RadioButton();
            radioFamilyTimes = new RadioButton();
            groupBox3 = new GroupBox();
            radioFamilyOriginal = new RadioButton();
            radioSizeTwelve = new RadioButton();
            radioSizeTen = new RadioButton();
            groupBox4 = new GroupBox();
            radioSizeOriginal = new RadioButton();
            radioStyleItalic = new RadioButton();
            groupBox5 = new GroupBox();
            radioStyleOriginal = new RadioButton();
            radioWeightBold = new RadioButton();
            groupBox6 = new GroupBox();
            radioWeightOriginal = new RadioButton();
            lblName = new Label();
            lblAddress = new Label();
            lblState = new Label();
            lblZip = new Label();
            lblCity = new Label();
            groupBox7 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(345, 3);
            panel1.Name = "panel1";
            tableLayoutPanel1.SetRowSpan(panel1, 3);
            panel1.Size = new Size(336, 285);
            panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioBackgroundLightSalmon);
            groupBox1.Controls.Add(radioBackgroundLightGreen);
            groupBox1.Controls.Add(radioBackgroundOriginal);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(336, 91);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Background Color";
            // 
            // radioBackgroundLightSalmon
            // 
            radioBackgroundLightSalmon.AutoSize = true;
            radioBackgroundLightSalmon.Location = new Point(11, 66);
            radioBackgroundLightSalmon.Name = "radioBackgroundLightSalmon";
            radioBackgroundLightSalmon.Size = new Size(94, 17);
            radioBackgroundLightSalmon.TabIndex = 2;
            radioBackgroundLightSalmon.Text = "LightSalmon";
            radioBackgroundLightSalmon.CheckedChanged += radioBackgroundLightSalmon_CheckedChanged;
            // 
            // radioBackgroundLightGreen
            // 
            radioBackgroundLightGreen.AutoSize = true;
            radioBackgroundLightGreen.Location = new Point(11, 43);
            radioBackgroundLightGreen.Name = "radioBackgroundLightGreen";
            radioBackgroundLightGreen.Size = new Size(87, 17);
            radioBackgroundLightGreen.TabIndex = 1;
            radioBackgroundLightGreen.Text = "LightGreen";
            radioBackgroundLightGreen.CheckedChanged += radioBackgroundLightGreen_CheckedChanged;
            // 
            // radioBackgroundOriginal
            // 
            radioBackgroundOriginal.AutoSize = true;
            radioBackgroundOriginal.Location = new Point(11, 20);
            radioBackgroundOriginal.Name = "radioBackgroundOriginal";
            radioBackgroundOriginal.Size = new Size(68, 17);
            radioBackgroundOriginal.TabIndex = 0;
            radioBackgroundOriginal.Text = "Original";
            radioBackgroundOriginal.CheckedChanged += radioBackgroundOriginal_CheckedChanged;
            // 
            // radioForegroundYellow
            // 
            radioForegroundYellow.AutoSize = true;
            radioForegroundYellow.Location = new Point(11, 66);
            radioForegroundYellow.Name = "radioForegroundYellow";
            radioForegroundYellow.Size = new Size(62, 17);
            radioForegroundYellow.TabIndex = 2;
            radioForegroundYellow.Text = "Yellow";
            radioForegroundYellow.CheckedChanged += radioForegroundYellow_CheckedChanged;
            // 
            // radioForegroundRed
            // 
            radioForegroundRed.AutoSize = true;
            radioForegroundRed.Location = new Point(11, 43);
            radioForegroundRed.Name = "radioForegroundRed";
            radioForegroundRed.Size = new Size(48, 17);
            radioForegroundRed.TabIndex = 1;
            radioForegroundRed.Text = "Red";
            radioForegroundRed.CheckedChanged += radioForegroundRed_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioForegroundYellow);
            groupBox2.Controls.Add(radioForegroundRed);
            groupBox2.Controls.Add(radioForegroundOriginal);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 100);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(336, 91);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Foreground Color";
            // 
            // radioForegroundOriginal
            // 
            radioForegroundOriginal.AutoSize = true;
            radioForegroundOriginal.Location = new Point(11, 19);
            radioForegroundOriginal.Name = "radioForegroundOriginal";
            radioForegroundOriginal.Size = new Size(68, 17);
            radioForegroundOriginal.TabIndex = 0;
            radioForegroundOriginal.Text = "Original";
            radioForegroundOriginal.CheckedChanged += radioForegroundOriginal_CheckedChanged;
            // 
            // radioFamilyWingDings
            // 
            radioFamilyWingDings.AutoSize = true;
            radioFamilyWingDings.Location = new Point(11, 66);
            radioFamilyWingDings.Name = "radioFamilyWingDings";
            radioFamilyWingDings.Size = new Size(86, 17);
            radioFamilyWingDings.TabIndex = 2;
            radioFamilyWingDings.Text = "WingDings";
            radioFamilyWingDings.CheckedChanged += radioFamilyWingDings_CheckedChanged;
            // 
            // radioFamilyTimes
            // 
            radioFamilyTimes.AutoSize = true;
            radioFamilyTimes.Location = new Point(11, 43);
            radioFamilyTimes.Name = "radioFamilyTimes";
            radioFamilyTimes.Size = new Size(130, 17);
            radioFamilyTimes.TabIndex = 1;
            radioFamilyTimes.Text = "Times New Roman";
            radioFamilyTimes.CheckedChanged += radioFamilyTimes_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioFamilyWingDings);
            groupBox3.Controls.Add(radioFamilyTimes);
            groupBox3.Controls.Add(radioFamilyOriginal);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 294);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(336, 91);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Font Family";
            // 
            // radioFamilyOriginal
            // 
            radioFamilyOriginal.AutoSize = true;
            radioFamilyOriginal.Location = new Point(11, 20);
            radioFamilyOriginal.Name = "radioFamilyOriginal";
            radioFamilyOriginal.Size = new Size(68, 17);
            radioFamilyOriginal.TabIndex = 0;
            radioFamilyOriginal.Text = "Original";
            radioFamilyOriginal.CheckedChanged += radioFamilyOriginal_CheckedChanged;
            // 
            // radioSizeTwelve
            // 
            radioSizeTwelve.AutoSize = true;
            radioSizeTwelve.Location = new Point(11, 66);
            radioSizeTwelve.Name = "radioSizeTwelve";
            radioSizeTwelve.Size = new Size(39, 17);
            radioSizeTwelve.TabIndex = 2;
            radioSizeTwelve.Text = "12";
            radioSizeTwelve.CheckedChanged += radioSizeTwelve_CheckedChanged;
            // 
            // radioSizeTen
            // 
            radioSizeTen.AutoSize = true;
            radioSizeTen.Location = new Point(11, 43);
            radioSizeTen.Name = "radioSizeTen";
            radioSizeTen.Size = new Size(39, 17);
            radioSizeTen.TabIndex = 1;
            radioSizeTen.Text = "10";
            radioSizeTen.CheckedChanged += radioSizeTen_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(radioSizeTwelve);
            groupBox4.Controls.Add(radioSizeTen);
            groupBox4.Controls.Add(radioSizeOriginal);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 197);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(336, 91);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Font Size";
            // 
            // radioSizeOriginal
            // 
            radioSizeOriginal.AutoSize = true;
            radioSizeOriginal.Location = new Point(11, 20);
            radioSizeOriginal.Name = "radioSizeOriginal";
            radioSizeOriginal.Size = new Size(68, 17);
            radioSizeOriginal.TabIndex = 0;
            radioSizeOriginal.Text = "Original";
            radioSizeOriginal.CheckedChanged += radioSizeOriginal_CheckedChanged;
            // 
            // radioStyleItalic
            // 
            radioStyleItalic.AutoSize = true;
            radioStyleItalic.Location = new Point(11, 43);
            radioStyleItalic.Name = "radioStyleItalic";
            radioStyleItalic.Size = new Size(53, 17);
            radioStyleItalic.TabIndex = 1;
            radioStyleItalic.Text = "Italic";
            radioStyleItalic.CheckedChanged += radioStyleItalic_CheckedChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(radioStyleItalic);
            groupBox5.Controls.Add(radioStyleOriginal);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(3, 391);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(336, 66);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Font Style";
            // 
            // radioStyleOriginal
            // 
            radioStyleOriginal.AutoSize = true;
            radioStyleOriginal.Location = new Point(11, 20);
            radioStyleOriginal.Name = "radioStyleOriginal";
            radioStyleOriginal.Size = new Size(64, 17);
            radioStyleOriginal.TabIndex = 0;
            radioStyleOriginal.Text = "Normal";
            radioStyleOriginal.CheckedChanged += radioStyleOriginal_CheckedChanged;
            // 
            // radioWeightBold
            // 
            radioWeightBold.AutoSize = true;
            radioWeightBold.Location = new Point(11, 43);
            radioWeightBold.Name = "radioWeightBold";
            radioWeightBold.Size = new Size(50, 17);
            radioWeightBold.TabIndex = 1;
            radioWeightBold.Text = "Bold";
            radioWeightBold.CheckedChanged += radioWeightBold_CheckedChanged;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(radioWeightBold);
            groupBox6.Controls.Add(radioWeightOriginal);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(3, 463);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(336, 85);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Font Weight";
            // 
            // radioWeightOriginal
            // 
            radioWeightOriginal.AutoSize = true;
            radioWeightOriginal.Location = new Point(11, 20);
            radioWeightOriginal.Name = "radioWeightOriginal";
            radioWeightOriginal.Size = new Size(68, 17);
            radioWeightOriginal.TabIndex = 0;
            radioWeightOriginal.Text = "Original";
            radioWeightOriginal.CheckedChanged += radioWeightOriginal_CheckedChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(6, 24);
            lblName.Name = "lblName";
            lblName.Size = new Size(43, 13);
            lblName.TabIndex = 7;
            lblName.Text = "Name:";
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(6, 48);
            lblAddress.Margin = new Padding(3, 3, 3, 1);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(94, 13);
            lblAddress.TabIndex = 8;
            lblAddress.Text = "Street Address:";
            // 
            // lblState
            // 
            lblState.AutoSize = true;
            lblState.Location = new Point(6, 96);
            lblState.Margin = new Padding(3, 1, 3, 3);
            lblState.Name = "lblState";
            lblState.Size = new Size(41, 13);
            lblState.TabIndex = 10;
            lblState.Text = "State:";
            // 
            // lblZip
            // 
            lblZip.AutoSize = true;
            lblZip.Location = new Point(6, 120);
            lblZip.Margin = new Padding(3, 1, 3, 3);
            lblZip.Name = "lblZip";
            lblZip.Size = new Size(29, 13);
            lblZip.TabIndex = 11;
            lblZip.Text = "Zip:";
            // 
            // lblCity
            // 
            lblCity.AutoSize = true;
            lblCity.Location = new Point(6, 72);
            lblCity.Margin = new Padding(3, 1, 2, 1);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(32, 13);
            lblCity.TabIndex = 9;
            lblCity.Text = "City:";
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(lblZip);
            groupBox7.Controls.Add(lblState);
            groupBox7.Controls.Add(lblCity);
            groupBox7.Controls.Add(lblAddress);
            groupBox7.Controls.Add(lblName);
            groupBox7.Location = new Point(345, 294);
            groupBox7.Name = "groupBox7";
            tableLayoutPanel1.SetRowSpan(groupBox7, 3);
            groupBox7.Size = new Size(330, 150);
            groupBox7.TabIndex = 12;
            groupBox7.TabStop = false;
            groupBox7.Text = "Data from control";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(groupBox6, 0, 5);
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 3);
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 4);
            tableLayoutPanel1.Controls.Add(groupBox7, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(684, 551);
            tableLayoutPanel1.TabIndex = 13;
            // 
            // Form1
            // 
            AutoScaleBaseSize = new Size(6, 13);
            AutoSize = true;
            ClientSize = new Size(684, 551);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Name = "Form1";
            Text = "Windows Form Hosting Wpf Control";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBackgroundOriginal;
        private System.Windows.Forms.RadioButton radioBackgroundLightGreen;
        private System.Windows.Forms.RadioButton radioBackgroundLightSalmon;
        private System.Windows.Forms.RadioButton radioForegroundYellow;
        private System.Windows.Forms.RadioButton radioForegroundRed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioForegroundOriginal;
        private System.Windows.Forms.RadioButton radioFamilyWingDings;
        private System.Windows.Forms.RadioButton radioFamilyTimes;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioFamilyOriginal;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioSizeTwelve;
        private System.Windows.Forms.RadioButton radioSizeTen;
        private System.Windows.Forms.RadioButton radioSizeOriginal;
        private System.Windows.Forms.RadioButton radioStyleItalic;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioStyleOriginal;
        private System.Windows.Forms.RadioButton radioWeightBold;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioWeightOriginal;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}
