// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Guide
{
    public partial class Page1 : Page
    {
        private Application _app;
        public bool RealTimeUpdate = true;

        public void MenuExit(object sender, RoutedEventArgs args)
        {
            _app = Application.Current;
            _app.Shutdown();
        }

        public void SetXaml(object sender, EventArgs e)
        {
            myCombo.SelectedValue = cbItem5;
        }

        //Begin XAMLPAD integration

        protected void HandleTextChanged(object sender, TextChangedEventArgs me)
        {
            if (RealTimeUpdate) ParseCurrentBuffer();
        }

        private void ParseCurrentBuffer()
        {
            try
            {
                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);
                var str = TextBox1.Text;
                sw.Write(str);
                sw.Flush();
                ms.Flush();
                ms.Position = 0;
                try
                {
                    var content = XamlReader.Load(ms);
                    if (content != null)
                    {
                        cc.Children.Clear();
                        cc.Children.Add((UIElement) content);
                    }
                    TextBox1.Foreground = Brushes.Black;
                    ErrorText.Text = "";
                }

                catch (XamlParseException xpe)
                {
                    TextBox1.Foreground = Brushes.Red;
                    ErrorText.Text = xpe.Message;
                }
            }
            catch (Exception)
            {
            }
        }

        // End integration of XAMLPAD

        public void SelectLang(object sender, RoutedEventArgs e)
        {
            if (cbItem1.IsSelected)
            {
                Welcome.Page1.MyDouble = 1;
                frame2.Refresh();
            }
            else if (cbItem2.IsSelected)
            {
                Welcome.Page1.MyDouble = 2;
                frame2.Refresh();
            }
            else if (cbItem3.IsSelected)
            {
                Welcome.Page1.MyDouble = 3;
                frame2.Refresh();
            }
            else if (cbItem4.IsSelected)
            {
                Welcome.Page1.MyDouble = 4;
                frame2.Refresh();
            }
            else if (cbItem5.IsSelected)
            {
                Welcome.Page1.MyDouble = 5;
                frame2.Refresh();
            }
            else if (cbItem6.IsSelected)
            {
                Welcome.Page1.MyDouble = 6;
                frame2.Refresh();
            }
            else if (cbItem7.IsSelected)
            {
                Welcome.Page1.MyDouble = 7;
                frame2.Refresh();
            }
        }
    }
}