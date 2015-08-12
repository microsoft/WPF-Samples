// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Windows.Media;

namespace Colors
{
    public class ColorItem : INotifyPropertyChanged
    {
        public enum Sources
        {
            UserDefined,
            BuiltIn
        };

        private byte _alpha;
        private byte _blue;
        private byte _green;
        private double _hue;
        private string _name;
        private byte _red;
        private double _saturation;
        private double _value;

        public ColorItem(string name, SolidColorBrush brush)
        {
            Source = Sources.BuiltIn;
            _name = name;
            Brush = brush;
            var color = brush.Color;
            _alpha = color.A;
            _red = color.R;
            _green = color.G;
            _blue = color.B;
            HsvFromRgb();
        }

        public ColorItem(ColorItem item)
        {
            Source = Sources.UserDefined;
            _name = "New Color";
            _red = item._red;
            _green = item._green;
            _blue = item._blue;
            _hue = item._hue;
            _saturation = item._saturation;
            _value = item._value;
            _alpha = item._alpha;
            Luminance = item.Luminance;
            Brush = new SolidColorBrush(item.Brush.Color);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Sources Source { get; } = Sources.UserDefined;
        public double Luminance { get; set; }
        public SolidColorBrush Brush { get; private set; }

        public byte Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                OnPropertyChanged("Alpha");
                UpdateBrush();
            }
        }

        public byte Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged("Red");
                HsvFromRgb();
            }
        }

        public byte Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged("Green");
                HsvFromRgb();
            }
        }

        public byte Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged("Blue");
                HsvFromRgb();
            }
        }

        public double Hue
        {
            get { return _hue; }
            set
            {
                if (value > 360.0) value = 360.0;
                if (value < 0.0) value = 0.0;
                _hue = value;
                OnPropertyChanged("Hue");
                RgbFromHsv();
            }
        }

        public double Saturation
        {
            get { return _saturation; }
            set
            {
                if (value > 1.0) value = 1.0;
                if (value < 0.0) value = 0.0;
                _saturation = value;
                OnPropertyChanged("Saturation");
                RgbFromHsv();
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                if (value > 1.0) value = 1.0;
                if (value < 0.0) value = 0.0;
                _value = value;
                OnPropertyChanged("Value");
                RgbFromHsv();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void HsvFromRgb()
        {
            int imax = _red, imin = _red;
            if (_green > imax) imax = _green;
            else if (_green < imin) imin = _green;
            if (_blue > imax) imax = _blue;
            else if (_blue < imin) imin = _blue;
            double max = imax/255.0, min = imin/255.0;

            var value = max;
            var saturation = (max > 0) ? (max - min)/max : 0.0;
            var hue = _hue;

            if (imax > imin)
            {
                var f = 1.0/((max - min)*255.0);
                hue = (imax == _red)
                    ? 0.0 + f*(_green - _blue)
                    : (imax == _green)
                        ? 2.0 + f*(_blue - _red)
                        : 4.0 + f*(_red - _green);
                hue = hue*60.0;
                if (hue < 0.0)
                    hue += 360.0;
            }

            // now update the real values as necessary
            if (hue != _hue)
            {
                _hue = hue;
                OnPropertyChanged("Hue");
            }
            if (saturation != _saturation)
            {
                _saturation = saturation;
                OnPropertyChanged("Saturation");
            }
            if (value != _value)
            {
                _value = value;
                OnPropertyChanged("Value");
            }

            UpdateBrush();
        }

        private void RgbFromHsv()
        {
            double red = 0.0, green = 0.0, blue = 0.0;
            if (_saturation == 0.0)
            {
                red = green = blue = _value;
            }
            else
            {
                var h = _hue;
                while (h >= 360.0)
                    h -= 360.0;

                h = h/60.0;
                var i = (int) h;

                var f = h - i;
                var r = _value*(1.0 - _saturation);
                var s = _value*(1.0 - _saturation*f);
                var t = _value*(1.0 - _saturation*(1.0 - f));

                switch (i)
                {
                    case 0:
                        red = _value;
                        green = t;
                        blue = r;
                        break;
                    case 1:
                        red = s;
                        green = _value;
                        blue = r;
                        break;
                    case 2:
                        red = r;
                        green = _value;
                        blue = t;
                        break;
                    case 3:
                        red = r;
                        green = s;
                        blue = _value;
                        break;
                    case 4:
                        red = t;
                        green = r;
                        blue = _value;
                        break;
                    case 5:
                        red = _value;
                        green = r;
                        blue = s;
                        break;
                }
            }

            byte iRed = (byte) (red*255.0), iGreen = (byte) (green*255.0), iBlue = (byte) (blue*255.0);
            if (iRed != _red)
            {
                _red = iRed;
                OnPropertyChanged("Red");
            }
            if (iGreen != _green)
            {
                _green = iGreen;
                OnPropertyChanged("Green");
            }
            if (iBlue != _blue)
            {
                _blue = iBlue;
                OnPropertyChanged("Blue");
            }

            UpdateBrush();
        }

        private void UpdateBrush()
        {
            var color = Brush.Color;
            if (_alpha != color.A || _red != color.R || _green != color.G || _blue != color.B)
            {
                color = Color.FromArgb(_alpha, _red, _green, _blue);
                Brush = new SolidColorBrush(color);
                OnPropertyChanged("Brush");
            }

            var luminance = (0.30*_red + 0.59*_green + 0.11*_blue)/255.0;
            if (Luminance != luminance)
            {
                Luminance = luminance;
                OnPropertyChanged("Luminance");
            }
        }
    }
}