namespace FormsApp
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
        this.panel1 = new System.Windows.Forms.Panel();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.radioBackgroundLightSalmon = new System.Windows.Forms.RadioButton();
        this.radioBackgroundLightGreen = new System.Windows.Forms.RadioButton();
        this.radioBackgroundOriginal = new System.Windows.Forms.RadioButton();
        this.radioForegroundYellow = new System.Windows.Forms.RadioButton();
        this.radioForegroundRed = new System.Windows.Forms.RadioButton();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.radioForegroundOriginal = new System.Windows.Forms.RadioButton();
        this.radioFamilyWingDings = new System.Windows.Forms.RadioButton();
        this.radioFamilyTimes = new System.Windows.Forms.RadioButton();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.radioFamilyOriginal = new System.Windows.Forms.RadioButton();
        this.radioSizeTwelve = new System.Windows.Forms.RadioButton();
        this.radioSizeTen = new System.Windows.Forms.RadioButton();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.radioSizeOriginal = new System.Windows.Forms.RadioButton();
        this.radioStyleItalic = new System.Windows.Forms.RadioButton();
        this.groupBox5 = new System.Windows.Forms.GroupBox();
        this.radioStyleOriginal = new System.Windows.Forms.RadioButton();
        this.radioWeightBold = new System.Windows.Forms.RadioButton();
        this.groupBox6 = new System.Windows.Forms.GroupBox();
        this.radioWeightOriginal = new System.Windows.Forms.RadioButton();
        this.lblName = new System.Windows.Forms.Label();
        this.lblAddress = new System.Windows.Forms.Label();
        this.lblState = new System.Windows.Forms.Label();
        this.lblZip = new System.Windows.Forms.Label();
        this.lblCity = new System.Windows.Forms.Label();
        this.groupBox7 = new System.Windows.Forms.GroupBox();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.groupBox1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.groupBox4.SuspendLayout();
        this.groupBox5.SuspendLayout();
        this.groupBox6.SuspendLayout();
        this.groupBox7.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // panel1
        // 
        this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel1.Location = new System.Drawing.Point(333, 3);
        this.panel1.Name = "panel1";
        this.tableLayoutPanel1.SetRowSpan(this.panel1, 3);
        this.panel1.Size = new System.Drawing.Size(325, 285);
        this.panel1.TabIndex = 0;
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.radioBackgroundLightSalmon);
        this.groupBox1.Controls.Add(this.radioBackgroundLightGreen);
        this.groupBox1.Controls.Add(this.radioBackgroundOriginal);
        this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox1.Location = new System.Drawing.Point(3, 3);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(324, 91);
        this.groupBox1.TabIndex = 0;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Background Color";
        // 
        // radioBackgroundLightSalmon
        // 
        this.radioBackgroundLightSalmon.AutoSize = true;
        this.radioBackgroundLightSalmon.Location = new System.Drawing.Point(11, 66);
        this.radioBackgroundLightSalmon.Name = "radioBackgroundLightSalmon";
        this.radioBackgroundLightSalmon.Size = new System.Drawing.Size(94, 17);
        this.radioBackgroundLightSalmon.TabIndex = 2;
        this.radioBackgroundLightSalmon.Text = "LightSalmon";
        this.radioBackgroundLightSalmon.CheckedChanged += new System.EventHandler(this.radioBackgroundLightSalmon_CheckedChanged);
        // 
        // radioBackgroundLightGreen
        // 
        this.radioBackgroundLightGreen.AutoSize = true;
        this.radioBackgroundLightGreen.Location = new System.Drawing.Point(11, 43);
        this.radioBackgroundLightGreen.Name = "radioBackgroundLightGreen";
        this.radioBackgroundLightGreen.Size = new System.Drawing.Size(87, 17);
        this.radioBackgroundLightGreen.TabIndex = 1;
        this.radioBackgroundLightGreen.Text = "LightGreen";
        this.radioBackgroundLightGreen.CheckedChanged += new System.EventHandler(this.radioBackgroundLightGreen_CheckedChanged);
        // 
        // radioBackgroundOriginal
        // 
        this.radioBackgroundOriginal.AutoSize = true;
        this.radioBackgroundOriginal.Location = new System.Drawing.Point(11, 20);
        this.radioBackgroundOriginal.Name = "radioBackgroundOriginal";
        this.radioBackgroundOriginal.Size = new System.Drawing.Size(68, 17);
        this.radioBackgroundOriginal.TabIndex = 0;
        this.radioBackgroundOriginal.Text = "Original";
        this.radioBackgroundOriginal.CheckedChanged += new System.EventHandler(this.radioBackgroundOriginal_CheckedChanged);
        // 
        // radioForegroundYellow
        // 
        this.radioForegroundYellow.AutoSize = true;
        this.radioForegroundYellow.Location = new System.Drawing.Point(11, 66);
        this.radioForegroundYellow.Name = "radioForegroundYellow";
        this.radioForegroundYellow.Size = new System.Drawing.Size(62, 17);
        this.radioForegroundYellow.TabIndex = 2;
        this.radioForegroundYellow.Text = "Yellow";
        this.radioForegroundYellow.CheckedChanged += new System.EventHandler(this.radioForegroundYellow_CheckedChanged);
        // 
        // radioForegroundRed
        // 
        this.radioForegroundRed.AutoSize = true;
        this.radioForegroundRed.Location = new System.Drawing.Point(11, 43);
        this.radioForegroundRed.Name = "radioForegroundRed";
        this.radioForegroundRed.Size = new System.Drawing.Size(48, 17);
        this.radioForegroundRed.TabIndex = 1;
        this.radioForegroundRed.Text = "Red";
        this.radioForegroundRed.CheckedChanged += new System.EventHandler(this.radioForegroundRed_CheckedChanged);
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.radioForegroundYellow);
        this.groupBox2.Controls.Add(this.radioForegroundRed);
        this.groupBox2.Controls.Add(this.radioForegroundOriginal);
        this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox2.Location = new System.Drawing.Point(3, 100);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(324, 91);
        this.groupBox2.TabIndex = 1;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Foreground Color";
        // 
        // radioForegroundOriginal
        // 
        this.radioForegroundOriginal.AutoSize = true;
        this.radioForegroundOriginal.Location = new System.Drawing.Point(11, 19);
        this.radioForegroundOriginal.Name = "radioForegroundOriginal";
        this.radioForegroundOriginal.Size = new System.Drawing.Size(68, 17);
        this.radioForegroundOriginal.TabIndex = 0;
        this.radioForegroundOriginal.Text = "Original";
        this.radioForegroundOriginal.CheckedChanged += new System.EventHandler(this.radioForegroundOriginal_CheckedChanged);
        // 
        // radioFamilyWingDings
        // 
        this.radioFamilyWingDings.AutoSize = true;
        this.radioFamilyWingDings.Location = new System.Drawing.Point(11, 66);
        this.radioFamilyWingDings.Name = "radioFamilyWingDings";
        this.radioFamilyWingDings.Size = new System.Drawing.Size(86, 17);
        this.radioFamilyWingDings.TabIndex = 2;
        this.radioFamilyWingDings.Text = "WingDings";
        this.radioFamilyWingDings.CheckedChanged += new System.EventHandler(this.radioFamilyWingDings_CheckedChanged);
        // 
        // radioFamilyTimes
        // 
        this.radioFamilyTimes.AutoSize = true;
        this.radioFamilyTimes.Location = new System.Drawing.Point(11, 43);
        this.radioFamilyTimes.Name = "radioFamilyTimes";
        this.radioFamilyTimes.Size = new System.Drawing.Size(130, 17);
        this.radioFamilyTimes.TabIndex = 1;
        this.radioFamilyTimes.Text = "Times New Roman";
        this.radioFamilyTimes.CheckedChanged += new System.EventHandler(this.radioFamilyTimes_CheckedChanged);
        // 
        // groupBox3
        // 
        this.groupBox3.Controls.Add(this.radioFamilyWingDings);
        this.groupBox3.Controls.Add(this.radioFamilyTimes);
        this.groupBox3.Controls.Add(this.radioFamilyOriginal);
        this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox3.Location = new System.Drawing.Point(3, 294);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(324, 91);
        this.groupBox3.TabIndex = 2;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Font Family";
        // 
        // radioFamilyOriginal
        // 
        this.radioFamilyOriginal.AutoSize = true;
        this.radioFamilyOriginal.Location = new System.Drawing.Point(11, 20);
        this.radioFamilyOriginal.Name = "radioFamilyOriginal";
        this.radioFamilyOriginal.Size = new System.Drawing.Size(68, 17);
        this.radioFamilyOriginal.TabIndex = 0;
        this.radioFamilyOriginal.Text = "Original";
        this.radioFamilyOriginal.CheckedChanged += new System.EventHandler(this.radioFamilyOriginal_CheckedChanged);
        // 
        // radioSizeTwelve
        // 
        this.radioSizeTwelve.AutoSize = true;
        this.radioSizeTwelve.Location = new System.Drawing.Point(11, 66);
        this.radioSizeTwelve.Name = "radioSizeTwelve";
        this.radioSizeTwelve.Size = new System.Drawing.Size(39, 17);
        this.radioSizeTwelve.TabIndex = 2;
        this.radioSizeTwelve.Text = "12";
        this.radioSizeTwelve.CheckedChanged += new System.EventHandler(this.radioSizeTwelve_CheckedChanged);
        // 
        // radioSizeTen
        // 
        this.radioSizeTen.AutoSize = true;
        this.radioSizeTen.Location = new System.Drawing.Point(11, 43);
        this.radioSizeTen.Name = "radioSizeTen";
        this.radioSizeTen.Size = new System.Drawing.Size(39, 17);
        this.radioSizeTen.TabIndex = 1;
        this.radioSizeTen.Text = "10";
        this.radioSizeTen.CheckedChanged += new System.EventHandler(this.radioSizeTen_CheckedChanged);
        // 
        // groupBox4
        // 
        this.groupBox4.Controls.Add(this.radioSizeTwelve);
        this.groupBox4.Controls.Add(this.radioSizeTen);
        this.groupBox4.Controls.Add(this.radioSizeOriginal);
        this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox4.Location = new System.Drawing.Point(3, 197);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.Size = new System.Drawing.Size(324, 91);
        this.groupBox4.TabIndex = 3;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "Font Size";
        // 
        // radioSizeOriginal
        // 
        this.radioSizeOriginal.AutoSize = true;
        this.radioSizeOriginal.Location = new System.Drawing.Point(11, 20);
        this.radioSizeOriginal.Name = "radioSizeOriginal";
        this.radioSizeOriginal.Size = new System.Drawing.Size(68, 17);
        this.radioSizeOriginal.TabIndex = 0;
        this.radioSizeOriginal.Text = "Original";
        this.radioSizeOriginal.CheckedChanged += new System.EventHandler(this.radioSizeOriginal_CheckedChanged);
        // 
        // radioStyleItalic
        // 
        this.radioStyleItalic.AutoSize = true;
        this.radioStyleItalic.Location = new System.Drawing.Point(11, 43);
        this.radioStyleItalic.Name = "radioStyleItalic";
        this.radioStyleItalic.Size = new System.Drawing.Size(53, 17);
        this.radioStyleItalic.TabIndex = 1;
        this.radioStyleItalic.Text = "Italic";
        this.radioStyleItalic.CheckedChanged += new System.EventHandler(this.radioStyleItalic_CheckedChanged);
        // 
        // groupBox5
        // 
        this.groupBox5.Controls.Add(this.radioStyleItalic);
        this.groupBox5.Controls.Add(this.radioStyleOriginal);
        this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox5.Location = new System.Drawing.Point(3, 391);
        this.groupBox5.Name = "groupBox5";
        this.groupBox5.Size = new System.Drawing.Size(324, 66);
        this.groupBox5.TabIndex = 4;
        this.groupBox5.TabStop = false;
        this.groupBox5.Text = "Font Style";
        // 
        // radioStyleOriginal
        // 
        this.radioStyleOriginal.AutoSize = true;
        this.radioStyleOriginal.Location = new System.Drawing.Point(11, 20);
        this.radioStyleOriginal.Name = "radioStyleOriginal";
        this.radioStyleOriginal.Size = new System.Drawing.Size(64, 17);
        this.radioStyleOriginal.TabIndex = 0;
        this.radioStyleOriginal.Text = "Normal";
        this.radioStyleOriginal.CheckedChanged += new System.EventHandler(this.radioStyleOriginal_CheckedChanged);
        // 
        // radioWeightBold
        // 
        this.radioWeightBold.AutoSize = true;
        this.radioWeightBold.Location = new System.Drawing.Point(11, 43);
        this.radioWeightBold.Name = "radioWeightBold";
        this.radioWeightBold.Size = new System.Drawing.Size(50, 17);
        this.radioWeightBold.TabIndex = 1;
        this.radioWeightBold.Text = "Bold";
        this.radioWeightBold.CheckedChanged += new System.EventHandler(this.radioWeightBold_CheckedChanged);
        // 
        // groupBox6
        // 
        this.groupBox6.Controls.Add(this.radioWeightBold);
        this.groupBox6.Controls.Add(this.radioWeightOriginal);
        this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
        this.groupBox6.Location = new System.Drawing.Point(3, 463);
        this.groupBox6.Name = "groupBox6";
        this.groupBox6.Size = new System.Drawing.Size(324, 84);
        this.groupBox6.TabIndex = 5;
        this.groupBox6.TabStop = false;
        this.groupBox6.Text = "Font Weight";
        // 
        // radioWeightOriginal
        // 
        this.radioWeightOriginal.AutoSize = true;
        this.radioWeightOriginal.Location = new System.Drawing.Point(11, 20);
        this.radioWeightOriginal.Name = "radioWeightOriginal";
        this.radioWeightOriginal.Size = new System.Drawing.Size(68, 17);
        this.radioWeightOriginal.TabIndex = 0;
        this.radioWeightOriginal.Text = "Original";
        this.radioWeightOriginal.CheckedChanged += new System.EventHandler(this.radioWeightOriginal_CheckedChanged);
        // 
        // lblName
        // 
        this.lblName.AutoSize = true;
        this.lblName.Location = new System.Drawing.Point(6, 24);
        this.lblName.Name = "lblName";
        this.lblName.Size = new System.Drawing.Size(43, 13);
        this.lblName.TabIndex = 7;
        this.lblName.Text = "Name:";
        // 
        // lblAddress
        // 
        this.lblAddress.AutoSize = true;
        this.lblAddress.Location = new System.Drawing.Point(6, 47);
        this.lblAddress.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
        this.lblAddress.Name = "lblAddress";
        this.lblAddress.Size = new System.Drawing.Size(94, 13);
        this.lblAddress.TabIndex = 8;
        this.lblAddress.Text = "Street Address:";
        // 
        // lblState
        // 
        this.lblState.AutoSize = true;
        this.lblState.Location = new System.Drawing.Point(8, 97);
        this.lblState.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
        this.lblState.Name = "lblState";
        this.lblState.Size = new System.Drawing.Size(41, 13);
        this.lblState.TabIndex = 10;
        this.lblState.Text = "State:";
        // 
        // lblZip
        // 
        this.lblZip.AutoSize = true;
        this.lblZip.Location = new System.Drawing.Point(9, 121);
        this.lblZip.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
        this.lblZip.Name = "lblZip";
        this.lblZip.Size = new System.Drawing.Size(29, 13);
        this.lblZip.TabIndex = 11;
        this.lblZip.Text = "Zip:";
        // 
        // lblCity
        // 
        this.lblCity.AutoSize = true;
        this.lblCity.Location = new System.Drawing.Point(6, 70);
        this.lblCity.Margin = new System.Windows.Forms.Padding(3, 1, 2, 1);
        this.lblCity.Name = "lblCity";
        this.lblCity.Size = new System.Drawing.Size(32, 13);
        this.lblCity.TabIndex = 9;
        this.lblCity.Text = "City:";
        // 
        // groupBox7
        // 
        this.groupBox7.Controls.Add(this.lblZip);
        this.groupBox7.Controls.Add(this.lblState);
        this.groupBox7.Controls.Add(this.lblCity);
        this.groupBox7.Controls.Add(this.lblAddress);
        this.groupBox7.Controls.Add(this.lblName);
        this.groupBox7.Location = new System.Drawing.Point(333, 294);
        this.groupBox7.Name = "groupBox7";
        this.tableLayoutPanel1.SetRowSpan(this.groupBox7, 3);
        this.groupBox7.Size = new System.Drawing.Size(186, 155);
        this.groupBox7.TabIndex = 12;
        this.groupBox7.TabStop = false;
        this.groupBox7.Text = "Data from control";
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.AutoSize = true;
        this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tableLayoutPanel1.ColumnCount = 2;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel1.Controls.Add(this.groupBox6, 0, 5);
        this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
        this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
        this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 2);
        this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 3);
        this.tableLayoutPanel1.Controls.Add(this.groupBox5, 0, 4);
        this.tableLayoutPanel1.Controls.Add(this.groupBox7, 1, 3);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 6;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.Size = new System.Drawing.Size(661, 550);
        this.tableLayoutPanel1.TabIndex = 13;
        // 
        // Form1
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
        this.AutoSize = true;
        this.ClientSize = new System.Drawing.Size(661, 550);
        this.Controls.Add(this.tableLayoutPanel1);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "Form1";
        this.Text = "Windows Form Hosting Wpf Control";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.groupBox3.ResumeLayout(false);
        this.groupBox3.PerformLayout();
        this.groupBox4.ResumeLayout(false);
        this.groupBox4.PerformLayout();
        this.groupBox5.ResumeLayout(false);
        this.groupBox5.PerformLayout();
        this.groupBox6.ResumeLayout(false);
        this.groupBox6.PerformLayout();
        this.groupBox7.ResumeLayout(false);
        this.groupBox7.PerformLayout();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

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

