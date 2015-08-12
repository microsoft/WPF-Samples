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
                    throw new ArgumentException("Price must be positive");
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