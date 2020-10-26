// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DataBindingDemo
{
    public class AuctionItem : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Bid> _bids;
        private ProductCategory _category;
        private string _description;
        private SpecialFeatures _specialFeatures;
        private DateTime _startDate;
        private int _startPrice;

        public AuctionItem(string description, ProductCategory category, int startPrice, DateTime startDate, User owner,
            SpecialFeatures specialFeatures)
        {
            _description = description;
            _category = category;
            _startPrice = startPrice;
            _startDate = startDate;
            Owner = owner;
            _specialFeatures = specialFeatures;
            _bids = new ObservableCollection<Bid>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Exposing Bids as a ReadOnlyObservableCollection and adding an AddBid method so that CurrentPrice
        /// </summary>
        /// <param name="bid"></param>
        public void AddBid(Bid bid)
        {
            _bids.Add(bid);
            OnPropertyChanged("CurrentPrice");
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            // All important information conveyed visually in the AuctionItem must
            // be conveyed programmatically. The use of border color is purely 
            // decorative, and so does not need to be exposed programmatically. The
            // use of the star however in the visuals is important, as is all the 
            // text shown in the item. These data can be exposed programmatically 
            // through the UI Automation (UIA) Name property of the item by returning 
            // the desired string in ToString() here. That string will typically be
            // announced by a screen reader when arrowing to the item. The string is  
            // not overwhelming when announced, but the order in which the data is 
            // exposed in the string should match the order in which customers will 
            // want to access it. So when the customer arrows quickly through the 
            // list to reach an item of interest, the announcement should be ordered 
            // to enable that rapid navigation through the items.

            // Important: A shipping app would use localized text and currency
            // symbols here. It also would consider whether the simple concatenation
            // of the text would provide the expected experience in all regions.
            // In some regions the text might need to be returned in some other order.

            // Important: The text contained inside the item is exposed through the
            // Control view of the UI Automation (UIA) API. This indicates to a 
            // screen reader that the UI is of interest to customers. However, by 
            // default the star UI is not exposed in this way. Given that the 
            // information conveyed through the use of the star is very important, 
            // it must be exposed programmatically to customers. That is being 
            // achieved here by including related information in the accessible name 
            // of the item. If that information was not being included here, action  
            // must be taken to enable the star to be reached when a screen reader 
            // navigates around inside the item.

            return (_specialFeatures == SpecialFeatures.Highlight ?
                        "Special Features: Highlight, " : (_specialFeatures == SpecialFeatures.Color ? "Specieal Features: Color": "")) +
                "Description: " + _description + ", " +
                "Current price: $" + this.CurrentPrice;

        }
        

        #region Properties Getters and Setters

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public int StartPrice
        {
            get { return _startPrice; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price must be positive. Provide a positive price");
                }
                _startPrice = value;
                OnPropertyChanged("StartPrice");
                OnPropertyChanged("CurrentPrice");
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        public ProductCategory Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public User Owner { get; }

        public SpecialFeatures SpecialFeatures
        {
            get { return _specialFeatures; }
            set
            {
                _specialFeatures = value;
                OnPropertyChanged("SpecialFeatures");
            }
        }

        public ReadOnlyObservableCollection<Bid> Bids => new ReadOnlyObservableCollection<Bid>(_bids);

        public int CurrentPrice
        {
            get
            {
                var price = 0;
                // There is at least on bid on this product
                if (_bids.Count > 0)
                {
                    // Get the amount of the last bid
                    var lastBid = _bids[_bids.Count - 1];
                    price = lastBid.Amount;
                }
                // No bids on this product yet
                else
                {
                    price = _startPrice;
                }
                return price;
            }
        }

        #endregion
    }
}

