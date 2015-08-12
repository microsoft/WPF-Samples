// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

// ObserverableCollection
// DirectoryInfo

// MessageBox

namespace VideoViewerDemo
{
    // MyVideos is a collection of MyVideo objects
    // This class has a Directory string property
    // The Update() method takes all .wmv files from the specified directory
    // and adds them as MyVideo objects into the collection
    public class MyVideos : ObservableCollection<MyVideo>
    {
        private DirectoryInfo _directory;

        public MyVideos()
        {
        }

        public MyVideos(string directory)
        {
            Directory = directory;
        }

        public string Directory
        {
            set
            {
                // Don't set path if directory is invalid
                if (!System.IO.Directory.Exists(value))
                {
                    MessageBox.Show("No Such Directory");
                }

                _directory = new DirectoryInfo(value);

                Update();
            }
            get { return _directory.FullName; }
        }

        private void Update()
        {
            // Don't update if no directory to get files from
            if (_directory == null) return;

            // Remove all MyVideo objects from this collection
            Clear();

            // Create MyVideo objects
            foreach (var f in _directory.GetFiles("*.wmv"))
            {
                Add(new MyVideo(f.FullName, f.Name));
            }
        }
    }

    // MyVideo class
    // Properties: VideoTitle and Source
}