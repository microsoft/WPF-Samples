// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace LocalizingWpfInWf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var sc = new SimpleControlA();

            elementHost1.Child = sc;
        }
    }
}