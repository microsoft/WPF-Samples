// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace WpfLayoutHostingWfWithXaml
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            var b = sender as Button;

            b.Top = 20;
            b.Left = 20;
        }

        private void Button2Click(object sender, EventArgs e)
        {
            host1.Visibility = Visibility.Hidden;
        }

        private void Button3Click(object sender, EventArgs e)
        {
            host1.Visibility = Visibility.Collapsed;
        }

        private void InitializeFlowLayoutPanel()
        {
            var flp =
                flowLayoutHost.Child as FlowLayoutPanel;

            flp.WrapContents = true;

            const int numButtons = 6;

            for (var i = 0; i < numButtons; i++)
            {
                var b = new Button
                {
                    Text = "Button",
                    BackColor = Color.AliceBlue,
                    FlatStyle = FlatStyle.Flat
                };

                flp.Controls.Add(b);
            }
        }
    }
}