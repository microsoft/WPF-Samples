// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PhotoStoreDemo
{
    public class PrintType
    {
        public PrintType(string description, double cost)
        {
            Description = description;
            Cost = cost;
        }

        public string Description { get; }
        public double Cost { get; }

        public override string ToString() => Description;
    }
}