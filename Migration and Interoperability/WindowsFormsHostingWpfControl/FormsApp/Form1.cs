// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using WPFControls;

namespace FormsApp
{
    partial class Form1 : Form
    {
        private ElementHost _ctrlHost;
        private SolidColorBrush _initBackBrush;
        private FontFamily _initFontFamily;
        private double _initFontSize;
        private FontStyle _initFontStyle;
        private FontWeight _initFontWeight;
        private SolidColorBrush _initForeBrush;
        private MyControl _wpfAddressCtrl;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ctrlHost = new ElementHost {Dock = DockStyle.Fill};
            panel1.Controls.Add(_ctrlHost);
            _wpfAddressCtrl = new MyControl();
            _wpfAddressCtrl.InitializeComponent();
            _ctrlHost.Child = _wpfAddressCtrl;

            _wpfAddressCtrl.OnButtonClick +=
                avAddressCtrl_OnButtonClick;
            _wpfAddressCtrl.Loaded += avAddressCtrl_Loaded;
        }

        private void avAddressCtrl_Loaded(object sender, EventArgs e)
        {
            _initBackBrush = _wpfAddressCtrl.MyControlBackground;
            _initForeBrush = _wpfAddressCtrl.MyControlForeground;
            _initFontFamily = _wpfAddressCtrl.MyControlFontFamily;
            _initFontSize = _wpfAddressCtrl.MyControlFontSize;
            _initFontWeight = _wpfAddressCtrl.MyControlFontWeight;
            _initFontStyle = _wpfAddressCtrl.MyControlFontStyle;
        }

        private void avAddressCtrl_OnButtonClick(
            object sender,
            MyControlEventArgs args)
        {
            if (args.IsOk)
            {
                lblAddress.Text = @"Street Address: " + args.MyStreetAddress;
                lblCity.Text = @"City: " + args.MyCity;
                lblName.Text = "Name: " + args.MyName;
                lblState.Text = "State: " + args.MyState;
                lblZip.Text = "Zip: " + args.MyZip;
            }
            else
            {
                lblAddress.Text = "Street Address: ";
                lblCity.Text = "City: ";
                lblName.Text = "Name: ";
                lblState.Text = "State: ";
                lblZip.Text = "Zip: ";
            }
        }

        private void radioBackgroundOriginal_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlBackground = _initBackBrush;
        }

        private void radioBackgroundLightGreen_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlBackground = new SolidColorBrush(Colors.LightGreen);
        }

        private void radioBackgroundLightSalmon_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlBackground = new SolidColorBrush(Colors.LightSalmon);
        }

        private void radioForegroundOriginal_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlForeground = _initForeBrush;
        }

        private void radioForegroundRed_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlForeground = new SolidColorBrush(Colors.Red);
        }

        private void radioForegroundYellow_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlForeground = new SolidColorBrush(Colors.Yellow);
        }

        private void radioFamilyOriginal_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontFamily = _initFontFamily;
        }

        private void radioFamilyTimes_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontFamily = new FontFamily("Times New Roman");
        }

        private void radioFamilyWingDings_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontFamily = new FontFamily("WingDings");
        }

        private void radioSizeOriginal_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontSize = _initFontSize;
        }

        private void radioSizeTen_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontSize = 10;
        }

        private void radioSizeTwelve_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontSize = 12;
        }

        private void radioStyleOriginal_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontStyle = _initFontStyle;
        }

        private void radioStyleItalic_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontStyle = FontStyles.Italic;
        }

        private void radioWeightOriginal_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontWeight = _initFontWeight;
        }

        private void radioWeightBold_CheckedChanged(object sender, EventArgs e)
        {
            _wpfAddressCtrl.MyControlFontWeight = FontWeights.Bold;
        }
    }
}

// </snippet1>