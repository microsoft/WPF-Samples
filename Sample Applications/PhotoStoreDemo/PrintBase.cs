// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace PhotoStoreDemo
{
    public class PrintBase : INotifyPropertyChanged
    {
        private BitmapSource _photo;
        private PrintType _printType;
        private int _quantity;

        public PrintBase(BitmapSource photo, PrintType printtype, int quantity)
        {
            Photo = photo;
            PrintType = printtype;
            Quantity = quantity;
        }

        public PrintBase(BitmapSource photo, string description, double cost)
        {
            Photo = photo;
            PrintType = new PrintType(description, cost);
            Quantity = 0;
        }

        public BitmapSource Photo
        {
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
            }
            get { return _photo; }
        }

        public PrintType PrintType
        {
            set
            {
                _printType = value;
                OnPropertyChanged("PrintType");
            }
            get { return _printType; }
        }

        public int Quantity
        {
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
            get { return _quantity; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public override string ToString() => PrintType.ToString();
    }
}