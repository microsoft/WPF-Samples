// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

namespace MultiBinding
{
    public class NameList : ObservableCollection<PersonName>
    {
        public NameList()
        {
            Add(new PersonName("Willa", "Cather"));
            Add(new PersonName("Isak", "Dinesen"));
            Add(new PersonName("Victor", "Hugo"));
            Add(new PersonName("Jules", "Verne"));
        }
    }
}