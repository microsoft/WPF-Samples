// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace IntroToStylingAndTemplating
{
    public class Photo
    {
        public Photo(string path)
        {
            Source = path;
        }

        public string Source { get; }

        public override string ToString() => Source;
    }
}