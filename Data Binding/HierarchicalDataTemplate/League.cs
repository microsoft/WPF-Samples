// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace HierarchicalDataTemplate
{
    public class League
    {
        public League(string name)
        {
            Name = name;
            Divisions = new List<Division>();
        }

        public string Name { get; }
        public List<Division> Divisions { get; }
    }
}