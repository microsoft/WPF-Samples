// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataBindingDemo
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
        public ObservableCollection<AuctionItem> AuctionItems { get; set; } = new ObservableCollection<AuctionItem>();

        private void AppStartup(object sender, StartupEventArgs args)
        {
            LoadAuctionData();
        }

        private void LoadAuctionData()
        {
            CurrentUser = new User("John", 12, new DateTime(2003, 4, 20));

            #region Add Products to the auction

            var userMary = new User("Mary", 10, new DateTime(2000, 5, 2));
            var userAnna = new User("Anna", 5, new DateTime(2001, 9, 13));
            var userMike = new User("Mike", 13, new DateTime(1999, 11, 23));
            var userMark = new User("Mark", 15, new DateTime(2004, 6, 3));

            var camera = new AuctionItem("Digital camera - good condition", ProductCategory.Electronics, 300,
                new DateTime(2005, 8, 23), userAnna, SpecialFeatures.None);
            camera.AddBid(new Bid(310, userMike));
            camera.AddBid(new Bid(312, userMark));
            camera.AddBid(new Bid(314, userMike));
            camera.AddBid(new Bid(320, userMark));

            var snowBoard = new AuctionItem("Snowboard and bindings", ProductCategory.Sports, 120,
                new DateTime(2005, 7, 12),
                userMike, SpecialFeatures.Highlight);
            snowBoard.AddBid(new Bid(140, userAnna));
            snowBoard.AddBid(new Bid(142, userMary));
            snowBoard.AddBid(new Bid(150, userAnna));

            var insideCSharp = new AuctionItem("Inside C#, second edition", ProductCategory.Books, 10,
                new DateTime(2005, 5, 29),
                CurrentUser, SpecialFeatures.Color);
            insideCSharp.AddBid(new Bid(11, userMark));
            insideCSharp.AddBid(new Bid(13, userAnna));
            insideCSharp.AddBid(new Bid(14, userMary));
            insideCSharp.AddBid(new Bid(15, userAnna));

            var laptop = new AuctionItem("Laptop - only 1 year old", ProductCategory.Computers, 500,
                new DateTime(2005, 8, 15),
                userMark, SpecialFeatures.Highlight);
            laptop.AddBid(new Bid(510, CurrentUser));

            var setOfChairs = new AuctionItem("Set of 6 chairs", ProductCategory.Home, 120, new DateTime(2005, 2, 20),
                userMike,
                SpecialFeatures.Color);

            var myDvdCollection = new AuctionItem("My DVD Collection", ProductCategory.DvDs, 5, new DateTime(2005, 8, 3),
                userMary, SpecialFeatures.Highlight);
            myDvdCollection.AddBid(new Bid(6, userMike));
            myDvdCollection.AddBid(new Bid(8, CurrentUser));

            var tvDrama = new AuctionItem("TV Drama Series", ProductCategory.DvDs, 40, new DateTime(2005, 7, 28),
                userAnna,
                SpecialFeatures.None);
            tvDrama.AddBid(new Bid(42, userMike));
            tvDrama.AddBid(new Bid(45, userMark));
            tvDrama.AddBid(new Bid(50, userMike));
            tvDrama.AddBid(new Bid(51, CurrentUser));

            var squashRacket = new AuctionItem("Squash racket", ProductCategory.Sports, 60, new DateTime(2005, 4, 4),
                userMark,
                SpecialFeatures.Highlight);
            squashRacket.AddBid(new Bid(62, userMike));
            squashRacket.AddBid(new Bid(65, userAnna));

            AuctionItems.Add(camera);
            AuctionItems.Add(snowBoard);
            AuctionItems.Add(insideCSharp);
            AuctionItems.Add(laptop);
            AuctionItems.Add(setOfChairs);
            AuctionItems.Add(myDvdCollection);
            AuctionItems.Add(tvDrama);
            AuctionItems.Add(squashRacket);

            #endregion
        }
    }
}