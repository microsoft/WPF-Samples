// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFControls
{
    /// <summary>
    ///     Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
        public delegate void MyControlEventHandler(object sender, MyControlEventArgs args);

        private SolidColorBrush _background;
        private FontFamily _fontFamily;
        private double _fontSize;
        private FontStyle _fontStyle;
        private FontWeight _fontWeight;
        private SolidColorBrush _foreground;

        public MyControl()
        {
            InitializeComponent();
        }

        public FontWeight MyControlFontWeight
        {
            get { return _fontWeight; }
            set
            {
                _fontWeight = value;
                nameLabel.FontWeight = value;
                addressLabel.FontWeight = value;
                cityLabel.FontWeight = value;
                stateLabel.FontWeight = value;
                zipLabel.FontWeight = value;
            }
        }

        public double MyControlFontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                nameLabel.FontSize = value;
                addressLabel.FontSize = value;
                cityLabel.FontSize = value;
                stateLabel.FontSize = value;
                zipLabel.FontSize = value;
            }
        }

        public FontStyle MyControlFontStyle
        {
            get { return _fontStyle; }
            set
            {
                _fontStyle = value;
                nameLabel.FontStyle = value;
                addressLabel.FontStyle = value;
                cityLabel.FontStyle = value;
                stateLabel.FontStyle = value;
                zipLabel.FontStyle = value;
            }
        }

        public FontFamily MyControlFontFamily
        {
            get { return _fontFamily; }
            set
            {
                _fontFamily = value;
                nameLabel.FontFamily = value;
                addressLabel.FontFamily = value;
                cityLabel.FontFamily = value;
                stateLabel.FontFamily = value;
                zipLabel.FontFamily = value;
            }
        }

        public SolidColorBrush MyControlBackground
        {
            get { return _background; }
            set
            {
                _background = value;
                rootElement.Background = value;
            }
        }

        public SolidColorBrush MyControlForeground
        {
            get { return _foreground; }
            set
            {
                _foreground = value;
                nameLabel.Foreground = value;
                addressLabel.Foreground = value;
                cityLabel.Foreground = value;
                stateLabel.Foreground = value;
                zipLabel.Foreground = value;
            }
        }

        public event MyControlEventHandler OnButtonClick;

        private void Init(object sender, EventArgs e)
        {
            //They all have the same style, so use nameLabel to set initial values.
            _fontWeight = nameLabel.FontWeight;
            _fontSize = nameLabel.FontSize;
            _fontFamily = nameLabel.FontFamily;
            _fontStyle = nameLabel.FontStyle;
            _foreground = (SolidColorBrush) nameLabel.Foreground;
            _background = (SolidColorBrush) rootElement.Background;
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            var retvals = new MyControlEventArgs(true,
                txtName.Text,
                txtAddress.Text,
                txtCity.Text,
                txtState.Text,
                txtZip.Text);
            if (sender == btnCancel)
            {
                retvals.IsOk = false;
            }
            OnButtonClick?.Invoke(this, retvals);
        }
    }
}