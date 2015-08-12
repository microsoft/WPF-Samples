// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace SortFilter
{
    public class Order : INotifyPropertyChanged
    {
        private int _customer;
        private DateTime _datefilled;
        private string _filled;
        private int _id;
        private string _name;
        private int _order;
        private DateTime _orderdate;

        public Order(int order, int customer, string name, int id, string filled, DateTime orderdate,
            DateTime datefilled)
        {
            OrderItem = order;
            Customer = customer;
            Name = name;
            Id = id;
            Filled = filled;
            OrderDate = orderdate;
            DateFilled = datefilled;
        }

        public Order(int order, int customer, string name, int id, string filled, DateTime orderdate)
        {
            OrderItem = order;
            Customer = customer;
            Name = name;
            Id = id;
            Filled = filled;
            OrderDate = orderdate;
        }

        public int OrderItem
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged("OrderItem");
            }
        }

        public int Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged("Customer");
            }
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

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Filled
        {
            get { return _filled; }
            set
            {
                _filled = value;
                OnPropertyChanged("Filled");
            }
        }

        public DateTime OrderDate
        {
            get { return _orderdate; }
            set
            {
                _orderdate = value;
                OnPropertyChanged("OrderDate");
            }
        }

        public DateTime DateFilled
        {
            get { return _datefilled; }
            set
            {
                _datefilled = value;
                OnPropertyChanged("DateFilled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}