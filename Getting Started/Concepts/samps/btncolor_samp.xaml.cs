// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ButtonColor //needs to match the .xaml page
{
    public partial class Page1 : Page
    {
        // Begin inserting any c# code-behind content here. These methods handle events in XAML files and can be ported from samples. Access modifiers may need to be updated.

        private static int _newcolor;
        private Button _btncsharp;
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

        private void OnClick1(object sender, RoutedEventArgs e)
        {
            btn1.Background = Brushes.LightBlue;
        }

        private void OnClick2(object sender, RoutedEventArgs e)
        {
            btn2.Background = Brushes.Pink;
        }

        private void OnClick3(object sender, RoutedEventArgs e)
        {
            btn1.Background = Brushes.Pink;
            btn2.Background = Brushes.LightBlue;
        }

        private void OnClick4(object sender, RoutedEventArgs e)
        {
            switch (_newcolor)
            {
                case 0:
                    btn4.Background = Brushes.Red;
                    btn4.Foreground = Brushes.Beige;
                    btn4.Content = "Font Size 10";
                    btn4.FontSize = 10;
                    break;

                case 1:
                    btn4.Background = Brushes.CadetBlue;
                    btn4.Foreground = Brushes.LemonChiffon;
                    btn4.Content = "Font Size 12";
                    btn4.FontSize = 12;
                    break;

                case 2:
                    btn4.Background = Brushes.Purple;
                    btn4.Foreground = Brushes.PeachPuff;
                    btn4.Content = "Font Size 14";
                    btn4.FontSize = 14;
                    break;

                case 3:
                    btn4.Background = Brushes.BlanchedAlmond;
                    btn4.Foreground = Brushes.DarkRed;
                    btn4.Content = "Font Size 16";
                    btn4.FontSize = 16;
                    break;

                case 4:
                    btn4.Background = Brushes.Green;
                    btn4.Foreground = Brushes.White;
                    btn4.Content = "Font Size 18";
                    btn4.FontSize = 18;
                    break;
            }
            _newcolor = _newcolor + 1;
            if (_newcolor > 4)
            {
                _newcolor = 0;
            }
        }

        private void OnClick5(object sender, RoutedEventArgs e)
        {
            btn6.FontSize = 16;
            btn6.Content = "This is my favorite photo.";
            btn6.Background = Brushes.Red;
        }

        private void OnClick6(object sender, RoutedEventArgs e)
        {
            btn7.FontSize = 16;
            txt.Text = "You clicked the button.";
            btn7.Background = Brushes.Yellow;
        }

        private void OnClick7(object sender, RoutedEventArgs e)
        {
            _btncsharp = new Button
            {
                Content = "Created with C# code.",
                Background = SystemColors.ControlDarkDarkBrush,
                FontSize = SystemFonts.CaptionFontSize
            };
            cv2.Children.Add(_btncsharp);
        }
    }
}