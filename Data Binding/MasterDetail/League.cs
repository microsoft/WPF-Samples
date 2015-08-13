// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MasterDetail
{
    public class League
    {
        public League(string name)
        {
            Name = name;
            Divisions = new DivisionList();
        }

        public string Name { get; }
        public DivisionList Divisions { get; }

        public override string ToString() => Name;
    }
}