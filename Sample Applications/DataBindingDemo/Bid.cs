// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DataBindingDemo
{
    public class Bid
    {
        public Bid(int amount, User bidder)
        {
            Amount = amount;
            Bidder = bidder;
        }

        #region Property Getters and Setters

        public int Amount { get; }

        public User Bidder { get; }

        #endregion
    }
}