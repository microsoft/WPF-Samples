// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DataBindingDemo
{
    public class User
    {
        public User(string name, int rating, DateTime memberSince)
        {
            Name = name;
            Rating = rating;
            MemberSince = memberSince;
        }

        #region Property Getters and Setters

        public string Name { get; }

        public int Rating { get; set; }

        public DateTime MemberSince { get; }

        #endregion
    }
}