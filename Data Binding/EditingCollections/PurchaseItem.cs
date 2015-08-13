// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace EditingCollections
{
    public class PurchaseItem : INotifyPropertyChanged, IEditableObject
    {
        private ItemData _copyData;
        private ItemData _currentData;

        public PurchaseItem()
            : this("New item", 0, DateTime.Now)
        {
        }

        public PurchaseItem(string desc, double price, DateTime endDate)
        {
            Description = desc;
            Price = price;
            OfferExpires = endDate;
        }

        public string Description
        {
            get { return _currentData.Description; }
            set
            {
                if (_currentData.Description != value)
                {
                    _currentData.Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public double Price
        {
            get { return _currentData.Price; }
            set
            {
                if (_currentData.Price != value)
                {
                    _currentData.Price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }

        public DateTime OfferExpires
        {
            get { return _currentData.OfferExpires; }
            set
            {
                if (value != _currentData.OfferExpires)
                {
                    _currentData.OfferExpires = value;
                    NotifyPropertyChanged("OfferExpires");
                }
            }
        }

        public override string ToString() => $"{Description}, {Price:c}, {OfferExpires:D}";

        private struct ItemData
        {
            internal string Description;
            internal DateTime OfferExpires;
            internal double Price;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
            _copyData = _currentData;
        }

        public void CancelEdit()
        {
            _currentData = _copyData;
            NotifyPropertyChanged("");
        }

        public void EndEdit()
        {
            _copyData = new ItemData();
        }

        #endregion
    }
}