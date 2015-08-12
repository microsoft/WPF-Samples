// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace FormsApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            //Run the application
            Application.EnableVisualStyles();
            //System.Windows.Forms.Application.EnableRTLMirroring();
            Application.Run(new Form1());
        }
    }
}