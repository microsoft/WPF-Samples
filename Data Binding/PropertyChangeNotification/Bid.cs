// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace PropertyChangeNotification
{
    public class Bid : INotifyPropertyChanged
    {
        private string _biditemname;
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
        // OnPropertyChanged to update property value in binding
        private void OnPropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}