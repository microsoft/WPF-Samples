// <snippet1>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LocalizingWpfInWf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ES");

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SimpleControl sc = new SimpleControl();

            this.elementHost1.Child = sc;
        }
    }
}
// </snippet1>