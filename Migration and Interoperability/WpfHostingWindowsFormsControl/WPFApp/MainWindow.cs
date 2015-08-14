// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using MyControls;

namespace WPFApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Application _app;
        private SolidColorBrush _initBackBrush;
        private FontFamily _initFontFamily;
        private double _initFontSize;
        private FontStyle _initFontStyle;
        private FontWeight _initFontWeight;
        private SolidColorBrush _initForeBrush;
        private MainWindow _myWindow;
        private bool _uiIsReady;

        public MainWindow() {
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            Init(sender, e);
        }

        private void Init(object sender, EventArgs e)
        {
            _app = Application.Current;
            _myWindow = (MainWindow)_app.MainWindow;
            _myWindow.SizeToContent = SizeToContent.WidthAndHeight;
            wfh.TabIndex = 10;
            _initFontSize = wfh.FontSize;
            _initFontWeight = wfh.FontWeight;
            _initFontFamily = wfh.FontFamily;
            _initFontStyle = wfh.FontStyle;
            _initBackBrush = (SolidColorBrush) wfh.Background;
            _initForeBrush = (SolidColorBrush) wfh.Foreground;
            (wfh.Child as MyControl).OnButtonClick += Pane1_OnButtonClick;
            _uiIsReady = true;
        }

        private void BackColorChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnBackGreen)
                wfh.Background = new SolidColorBrush(Colors.LightGreen);
            else if (sender == rdbtnBackSalmon)
                wfh.Background = new SolidColorBrush(Colors.LightSalmon);
            else if (_uiIsReady)
                wfh.Background = _initBackBrush;
        }

        private void ForeColorChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnForeRed)
                wfh.Foreground = new SolidColorBrush(Colors.Red);
            else if (sender == rdbtnForeYellow)
                wfh.Foreground = new SolidColorBrush(Colors.Yellow);
            else if (_uiIsReady)
                wfh.Foreground = _initForeBrush;
        }

        private void FontChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnTimes)
                wfh.FontFamily = new FontFamily("Times New Roman");
            else if (sender == rdbtnWingdings)
                wfh.FontFamily = new FontFamily("Wingdings");
            else if (_uiIsReady)
                wfh.FontFamily = _initFontFamily;
        }

        private void FontSizeChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnTen)
                wfh.FontSize = 10;
            else if (sender == rdbtnTwelve)
                wfh.FontSize = 12;
            else if (_uiIsReady)
                wfh.FontSize = _initFontSize;
        }

        private void StyleChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnItalic)
                wfh.FontStyle = FontStyles.Italic;
            else if (_uiIsReady)
                wfh.FontStyle = _initFontStyle;
        }

        private void WeightChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnBold)
                wfh.FontWeight = FontWeights.Bold;
            else if (_uiIsReady)
                wfh.FontWeight = _initFontWeight;
        }

        //Handle button clicks on the Windows Form control
        private void Pane1_OnButtonClick(object sender, MyControlEventArgs args)
        {
            txtName.Inlines.Clear();
            txtAddress.Inlines.Clear();
            txtCity.Inlines.Clear();
            txtState.Inlines.Clear();
            txtZip.Inlines.Clear();

            if (args.IsOk)
            {
                txtName.Inlines.Add(" " + args.MyName);
                txtAddress.Inlines.Add(" " + args.MyStreetAddress);
                txtCity.Inlines.Add(" " + args.MyCity);
                txtState.Inlines.Add(" " + args.MyState);
                txtZip.Inlines.Add(" " + args.MyZip);
            }
        }
    }
}