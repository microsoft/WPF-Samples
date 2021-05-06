// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

// EventHandler
// ObservableCollection

// INotifyPropertyChanged, PropertyChangedEventArgs

namespace ExpenseItDemo
{
    public class ExpenseReport : INotifyPropertyChanged
    {
        private string _alias;
        private string _costCenter;
        private string _employeeNumber;
        private int _totalExpenses;
        private bool _initialized = false;

        public ExpenseReport()
        {
            LineItems = new LineItemCollection();
            LineItems.LineItemCostChanged += OnLineItemCostChanged;
        }

        /*
         * This method is needed because this app operates on a singleton ExpenseReport.
         * It preloads that report with real LineItems directly from the markup. 
         * Doing that, the markup call IList.Add avoiding the override created on the class.
         * So, we need to ensure the List class subscribes to the Item's events after its creation.
         * */
        public void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;
            LineItems.InitializeItems();
        }

        public string Alias
        {
            get { return _alias; }
            set
            {
                _alias = value;
                OnPropertyChanged("Alias");
            }
        }

        public string CostCenter
        {
            get { return _costCenter; }
            set
            {
                _costCenter = value;
                OnPropertyChanged("CostCenter");
            }
        }

        public string EmployeeNumber
        {
            get { return _employeeNumber; }
            set
            {
                _employeeNumber = value;
                OnPropertyChanged("EmployeeNumber");
            }
        }

        public int TotalExpenses
        {
            // calculated property, no setter
            get
            {
                RecalculateTotalExpense();
                return _totalExpenses;
            }
        }

        public LineItemCollection LineItems { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnLineItemCostChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("TotalExpenses");
        }

        private void RecalculateTotalExpense()
        {
            _totalExpenses = 0;
            foreach (var item in LineItems)
                _totalExpenses += item.Cost;
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}