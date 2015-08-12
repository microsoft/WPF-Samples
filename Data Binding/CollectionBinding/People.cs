// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

namespace CollectionBinding
{
    public class People : ObservableCollection<Person>
    {
        public People()
        {
            Add(new Person("Michael", "Alexander", "Bellevue"));
            Add(new Person("Jeff", "Hay", "Redmond"));
            Add(new Person("Christina", "Lee", "Kirkland"));
            Add(new Person("Samantha", "Smith", "Seattle"));
        }
    }
}