// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AlternatingAppearanceOfItems
{
    public class Place
    {
        public Place()
        {
            CityName = "";
            State = "";
        }

        public Place(string name, string state)
        {
            CityName = name;
            State = state;
        }

        public string CityName { get; set; }
        public string State { get; set; }
    }
}