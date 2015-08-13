// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace LocalAnimations
{
    internal sealed class EntryClass
    {
        [STAThread]
        private static void Main()
        {
            var app = new App();
            app.Run();
        }
    }
}