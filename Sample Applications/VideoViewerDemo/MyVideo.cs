// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace VideoViewerDemo
{
    public class MyVideo
    {
        private string _name;
        private Uri _source;

        public MyVideo(string path)
        {
            Source = path;
            _source = new Uri(path);
        }

        public MyVideo(string path, string name)
        {
            _name = name;
            Source = path;
            _source = new Uri(path);
        }

        public string VideoTitle
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }

        public string Source { get; }
    }
}