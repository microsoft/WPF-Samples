// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Forms;

namespace WmpAxLib
{
    public partial class WmpAxControl : UserControl
    {
        public WmpAxControl()
        {
            InitializeComponent();
        }

        public void Play(string url)
        {
            axWindowsMediaPlayer1.URL = url;
        }
    }
}

//</snippet100>