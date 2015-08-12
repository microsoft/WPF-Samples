// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace DirectionalBinding
{
    public class NetIncome : INotifyPropertyChanged
    {
        private int _food;
        private int _misc;
        private int _rent = 2000;
        private int _savings;
        private int _totalIncome = 5000;

        public NetIncome()
        {
            _savings = _totalIncome - (_rent + _food + _misc);
        }

        public int TotalIncome
        {
            get { return _totalIncome; }
            set
            {
                if (TotalIncome != value)
                {
                    _totalIncome = value;
                    OnPropertyChanged("TotalIncome");
                }
            }
        }

        public int Rent
        {
            get { return _rent; }
            set
            {
                if (Rent != value)
                {
                    _rent = value;
                    OnPropertyChanged("Rent");
                    UpdateSavings();
                }
            }
        }

        public int Food
        {
            get { return _food; }
            set
            {
                if (Food != value)
                {
                    _food = value;
                    OnPropertyChanged("Food");
                    UpdateSavings();
                }
            }
        }

        public int Misc
        {
            get { return _misc; }
            set
            {
                if (Misc != value)
                {
                    _misc = value;
                    OnPropertyChanged("Misc");
                    UpdateSavings();
                }
            }
        }

        public int Savings
        {
            get { return _savings; }
            set
            {
                if (Savings != value)
                {
                    _savings = value;
                    OnPropertyChanged("Savings");
                    UpdateSavings();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateSavings()
        {
            Savings = TotalIncome - (Rent + Misc + Food);
            if (Savings < 0)
            {
            }
            else if (Savings >= 0)
            {
            }
        }

        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}