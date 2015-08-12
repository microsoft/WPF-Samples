// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PropertyChangeNotification //needs to match the .xaml page
{
    public partial class Page1 : Page
    {
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
    }

    public class Bid : INotifyPropertyChanged
    {
        private string _biditemname = "Unset";
        private decimal _biditemprice;

        public Bid(string newBidItemName, decimal newBidItemPrice)
        {
            _biditemname = newBidItemName;
            _biditemprice = newBidItemPrice;
        }

        public string BidItemName
        {
            get { return _biditemname; }
            set
            {
                if (_biditemname.Equals(value) == false)
                {
                    _biditemname = value;
                    // Call OnPropertyChanged whenever the property is updated
                    OnPropertyChanged("BidItemName");
                }
            }
        }

        public decimal BidItemPrice
        {
            get { return _biditemprice; }
            set
            {
                if (_biditemprice.Equals(value) == false)
                {
                    _biditemprice = value;
                    // Call OnPropertyChanged whenever the property is updated
                    OnPropertyChanged("BidItemPrice");
                }
            }
        }

        // Declare event
        public event PropertyChangedEventHandler PropertyChanged;
        // OnPropertyChanged event handler to update property value in binding
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class BidCollection : ObservableCollection<Bid>
    {
        private readonly Bid _item1 = new Bid("Perseus Vase", (decimal) 24.95);
        private readonly Bid _item2 = new Bid("Hercules Statue", (decimal) 16.05);
        private readonly Bid _item3 = new Bid("Odysseus Painting", (decimal) 100.0);

        public BidCollection()
        {
            Add(_item1);
            Add(_item2);
            Add(_item3);
            CreateTimer();
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            _item1.BidItemPrice += (decimal) 1.25;
            _item2.BidItemPrice += (decimal) 2.45;
            _item3.BidItemPrice += (decimal) 10.55;
        }

        private void CreateTimer()
        {
            var timer1 = new Timer
            {
                Enabled = true,
                Interval = 2000
            };
            timer1.Elapsed += Timer1_Elapsed;
        }
    }
}