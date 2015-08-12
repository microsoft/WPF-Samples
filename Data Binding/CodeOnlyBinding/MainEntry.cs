// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CodeOnlyBinding
{
    internal sealed class MainEntry
    {
        [STAThread]
        public static void Main()
        {
            var thisApp = new MyApp();
            thisApp.Run();
        }
    }
}