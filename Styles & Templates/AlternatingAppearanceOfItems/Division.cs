// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace AlternatingAppearanceOfItems
{
    public class Division
    {
        public Division(string name)
        {
            Name = name;
            Teams = new List<Team>();
        }

        public string Name { get; }
        public List<Team> Teams { get; }
    }
}