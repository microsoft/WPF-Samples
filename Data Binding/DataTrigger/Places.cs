// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

namespace DataTrigger
{
    public class Places : ObservableCollection<Place>
    {
        public Places()
        {
            Add(new Place("Bellevue", "WA"));
            Add(new Place("Gold Beach", "OR"));
            Add(new Place("Kirkland", "WA"));
            Add(new Place("Los Angeles", "CA"));
            Add(new Place("Portland", "ME"));
            Add(new Place("Portland", "OR"));
            Add(new Place("Redmond", "WA"));
            Add(new Place("San Diego", "CA"));
            Add(new Place("San Francisco", "CA"));
            Add(new Place("San Jose", "CA"));
            Add(new Place("Seattle", "WA"));
        }
    }
}