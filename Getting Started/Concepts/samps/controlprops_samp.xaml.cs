// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlProperties //needs to match the .xaml page
{
    public partial class Page1 : Page
    {
        private FontFamily _ffamily;
        private double _fsize;
        // Begin inserting any c# code-behind content here. These methods handle events in XAML files and can be ported from samples. Access modifiers may need to be updated.


        private string _str;
        private ToolTip _ttp;
        // This function checks the language filter settings to see which code to filter and also grays out tabs with no content
        public void CheckLang(object sender, EventArgs e)
        {
            if (xcsharpCheck.Content == null) // grays out xaml + c# tab
            {
                xamlcsharp.Background = Brushes.Gainsboro;
                xamlcsharp.Foreground = Brushes.White;
                _ttp = new ToolTip();
                ToolTipService.SetShowOnDisabled(xamlcsharp, true);
                _ttp.Content = "This sample is not available in XAML + C#.";
                xamlcsharp.ToolTip = (_ttp);
                xamlcsharp.IsEnabled = false;
            }
            else if (xcsharpCheck.Content != null)
            {
                xamlcsharp.IsEnabled = true;
            }

            if (xvbCheck.Content == null) // grays out xaml + vb tab
            {
                xamlvb.Background = Brushes.Gainsboro;
                xamlvb.Foreground = Brushes.White;
                _ttp = new ToolTip();
                ToolTipService.SetShowOnDisabled(xamlvb, true);
                _ttp.Content = "This sample is not available in XAML + Visual Basic.NET";
                xamlvb.ToolTip = (_ttp);
                xamlvb.IsEnabled = false;
            }
            else if (xvbCheck.Content != null)
            {
                xamlvb.IsEnabled = true;
            }

            if (xaml.Content == null) // grays out xaml
            {
                xaml.IsEnabled = false;
                xaml.Background = Brushes.Gainsboro;
                xaml.Foreground = Brushes.White;
                _ttp = new ToolTip();
                ToolTipService.SetShowOnDisabled(xaml, true);
                _ttp.Content = "This sample is not available in XAML.";
                xaml.ToolTip = (_ttp);
            }
            else if (xaml.Content != null)
            {
                xaml.IsEnabled = true;
            }

            if (csharp.Content == null) // grays out c#
            {
                csharp.IsEnabled = false;
                csharp.Background = Brushes.Gainsboro;
                csharp.Foreground = Brushes.White;
                _ttp = new ToolTip();
                ToolTipService.SetShowOnDisabled(csharp, true);
                _ttp.Content = "This sample is not available in C#.";
                csharp.ToolTip = (_ttp);
            }
            else if (csharp.Content != null)
            {
                csharp.IsEnabled = true;
            }

            if (vb.Content == null) // grays out vb
            {
                vb.IsEnabled = false;
                vb.Background = Brushes.Gainsboro;
                vb.Foreground = Brushes.White;
                _ttp = new ToolTip();
                ToolTipService.SetShowOnDisabled(vb, true);
                _ttp.Content = "This sample is not available in Visual Basic.NET.";
                vb.ToolTip = (_ttp);
            }
            else if (vb.Content != null)
            {
                vb.IsEnabled = true;
            }

            if (managedcpp.Content == null) // grays out cpp
            {
                managedcpp.IsEnabled = false;
                managedcpp.Background = Brushes.Gainsboro;
                managedcpp.Foreground = Brushes.White;
                _ttp = new ToolTip();
                ToolTipService.SetShowOnDisabled(managedcpp, true);
                _ttp.Content = "This sample is not available in Managed C++.";
                managedcpp.ToolTip = (_ttp);
            }
            else if (managedcpp.Content != null)
            {
                managedcpp.IsEnabled = true;
            }
            if (Welcome.Page1.MyDouble == 1) // XAML only
            {
                xaml.Visibility = Visibility.Visible;
                csharp.Visibility = Visibility.Collapsed;
                vb.Visibility = Visibility.Collapsed;
                managedcpp.Visibility = Visibility.Collapsed;
                xamlcsharp.Visibility = Visibility.Collapsed;
                xamlvb.Visibility = Visibility.Collapsed;
            }
            else if (Welcome.Page1.MyDouble == 2) // CSharp
            {
                csharp.Visibility = Visibility.Visible;
                xaml.Visibility = Visibility.Collapsed;
                vb.Visibility = Visibility.Collapsed;
                managedcpp.Visibility = Visibility.Collapsed;
                xamlcsharp.Visibility = Visibility.Collapsed;
                xamlvb.Visibility = Visibility.Collapsed;
            }
            else if (Welcome.Page1.MyDouble == 3) // Visual Basic
            {
                vb.Visibility = Visibility.Visible;
                xaml.Visibility = Visibility.Collapsed;
                csharp.Visibility = Visibility.Collapsed;
                managedcpp.Visibility = Visibility.Collapsed;
                xamlcsharp.Visibility = Visibility.Collapsed;
                xamlvb.Visibility = Visibility.Collapsed;
            }
            else if (Welcome.Page1.MyDouble == 4) // Managed CPP
            {
                managedcpp.Visibility = Visibility.Visible;
                xaml.Visibility = Visibility.Collapsed;
                csharp.Visibility = Visibility.Collapsed;
                vb.Visibility = Visibility.Collapsed;
                xamlcsharp.Visibility = Visibility.Collapsed;
                xamlvb.Visibility = Visibility.Collapsed;
            }
            else if (Welcome.Page1.MyDouble == 5) // No Filter
            {
                xaml.Visibility = Visibility.Visible;
                csharp.Visibility = Visibility.Visible;
                vb.Visibility = Visibility.Visible;
                managedcpp.Visibility = Visibility.Visible;
                xamlcsharp.Visibility = Visibility.Visible;
                xamlvb.Visibility = Visibility.Visible;
            }
            else if (Welcome.Page1.MyDouble == 6) // XAML + CSharp
            {
                xaml.Visibility = Visibility.Collapsed;
                csharp.Visibility = Visibility.Collapsed;
                vb.Visibility = Visibility.Collapsed;
                managedcpp.Visibility = Visibility.Collapsed;
                xamlcsharp.Visibility = Visibility.Visible;
                xamlvb.Visibility = Visibility.Collapsed;
            }
            else if (Welcome.Page1.MyDouble == 7) // XAML + VB
            {
                xaml.Visibility = Visibility.Collapsed;
                csharp.Visibility = Visibility.Collapsed;
                vb.Visibility = Visibility.Collapsed;
                managedcpp.Visibility = Visibility.Collapsed;
                xamlcsharp.Visibility = Visibility.Collapsed;
                xamlvb.Visibility = Visibility.Visible;
            }
        }

        // To use PaneLoaded put Loaded="PaneLoaded" in root element of .xaml file.
        // private void PaneLoaded(object sender, EventArgs e) {}
        // Sample event handler:  
        private void ChangeBackground(object sender, RoutedEventArgs e)
        {
            if (btn.Background == Brushes.Red)
            {
                btn.Background = new LinearGradientBrush(Colors.LightBlue, Colors.SlateBlue, 90);
                btn.Content = "Background";
            }
            else
            {
                btn.Background = Brushes.Red;
                btn.Content = "Control background changes from red to a blue gradient.";
            }
        }

        private void ChangeForeground(object sender, RoutedEventArgs e)
        {
            if (btn1.Foreground == Brushes.Green)
            {
                btn1.Foreground = Brushes.Black;
                btn1.Content = "Foreground";
            }
            else
            {
                btn1.Foreground = Brushes.Green;
                btn1.Content = "Control foreground(text) changes from black to green.";
            }
        }

        private void ChangeFontFamily(object sender, RoutedEventArgs e)
        {
            _ffamily = btn2.FontFamily;
            _str = _ffamily.ToString();
            if (_str == ("Arial Black"))
            {
                btn2.FontFamily = new FontFamily("Arial");
                btn2.Content = "FontFamily";
            }
            else
            {
                btn2.FontFamily = new FontFamily("Arial Black");
                btn2.Content = "Control font family changes from Arial to Arial Black.";
            }
        }

        private void ChangeFontSize(object sender, RoutedEventArgs e)
        {
            _fsize = btn3.FontSize;
            if (_fsize == 16.0)
            {
                btn3.FontSize = 10.0;
                btn3.Content = "FontSize";
            }
            else
            {
                btn3.FontSize = 16.0;
                btn3.Content = "Control font size changes from 10 to 16.";
            }
        }

        private void ChangeFontStyle(object sender, RoutedEventArgs e)
        {
            if (btn4.FontStyle == FontStyles.Italic)
            {
                btn4.FontStyle = FontStyles.Normal;
                btn4.Content = "FontStyle";
            }
            else
            {
                btn4.FontStyle = FontStyles.Italic;
                btn4.Content = "Control font style changes from Normal to Italic.";
            }
        }

        private void ChangeFontWeight(object sender, RoutedEventArgs e)
        {
            if (btn5.FontWeight == FontWeights.Bold)
            {
                btn5.FontWeight = FontWeights.Normal;
                btn5.Content = "FontWeight";
            }
            else
            {
                btn5.FontWeight = FontWeights.Bold;
                btn5.Content = "Control font weight changes from Normal to Bold.";
            }
        }

        private void ChangeBorderBrush(object sender, RoutedEventArgs e)
        {
            _str = ((btn6.BorderBrush).ToString());
            if (btn6.BorderBrush == Brushes.Red)
            {
                btn6.BorderBrush = Brushes.Black;
                btn6.Content = "BorderBrush";
            }
            else
            {
                btn6.BorderBrush = Brushes.Red;
                btn6.Content = "Control border brush changes from red to black.";
            }
        }
    }
}