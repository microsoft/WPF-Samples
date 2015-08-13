// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ValidateItemsInItemsControl
{
    public class ServiceRep
    {
        public ServiceRep()
        {
        }

        public ServiceRep(string name, Region area)
        {
            Name = name;
            Area = area;
        }

        public string Name { get; set; }
        public Region Area { get; set; }

        public override string ToString() => Name + " - " + Area;
    }
}