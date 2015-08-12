// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Highlighter
{
    internal static class NativeMethods
    {
        internal const int GwlExstyle = -20;
        internal const int SwShowna = 8;
        internal const int WsExToolwindow = 0x00000080;
        // SetWindowPos constants (used by highlight rect)
        internal const int SwpNoactivate = 0x0010;
        internal static readonly IntPtr HwndTopmost = new IntPtr(-1);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hwndAfter, int x, int y,
            int width, int height, int flags);

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex,
            int dwNewLong);

        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();
    }
}