// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Globalization;

namespace BindingToMethod
{
    public class TemperatureScale : INotifyPropertyChanged
    {
        private TempType _type;

        public TemperatureScale()
        {
        }

        public TemperatureScale(TempType type)
        {
            _type = type;
        }

        public TempType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ConvertTemp(double degree, TempType temptype)
        {
            Type = temptype;
            switch (temptype)
            {
                case TempType.Celsius:
                    return (degree*9/5 + 32).ToString(CultureInfo.InvariantCulture) + " " + "Fahrenheit";
                case TempType.Fahrenheit:
                    return ((degree - 32)/9*5).ToString(CultureInfo.InvariantCulture) + " " + "Celsius";
            }
            return "Unknown Type";
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}