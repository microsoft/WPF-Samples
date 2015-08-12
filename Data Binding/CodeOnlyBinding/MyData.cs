// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace CodeOnlyBinding
{
    public class MyData : INotifyPropertyChanged
    {
        private string _myDataProperty;

        public MyData()
        {
        }

        public MyData(DateTime dateTime)
        {
            _myDataProperty = "Last bound time was " + dateTime.ToLongTimeString();
        }

        public string MyDataProperty
        {
            get { return _myDataProperty; }
            set
            {
                _myDataProperty = value;
                OnPropertyChanged("MyDataProperty");
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