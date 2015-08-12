// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CodeOnlyWindowsApplication
{
    public class EntryPoint
    {
        /// <summary>
        ///     WPF applications must run in a single-threaded apartment
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Start the WPF application
            var app = new App();
            app.Run();
        }
    }
}