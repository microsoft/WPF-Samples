// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace WPFControls
{
    public class MyControlEventArgs : EventArgs
    {
        public MyControlEventArgs(bool result,
            string name,
            string address,
            string city,
            string state,
            string zip)
        {
            IsOk = result;
            MyName = name;
            MyStreetAddress = address;
            MyCity = city;
            MyState = state;
            MyZip = zip;
        }

        public string MyName { get; set; }
        public string MyStreetAddress { get; set; }
        public string MyCity { get; set; }
        public string MyState { get; set; }
        public string MyZip { get; set; }
        public bool IsOk { get; set; }
    }
}