// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace InsertTextClient
{
// This class *MUST* be internal for security purposes
    internal class UnmanagedClass
    {
        //
        // Control style information
        //
        public const int GclStyle = -16;
        // Editbox styles
        internal const int EsReadonly = 0x0800;
        // Editbox messages
        internal const int WmSettext = 0x000C;
        internal const int EmGetlimittext = 0x00D5;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(
            Hwnd hwnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int uTimeout, out IntPtr pResult);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SendMessageTimeout(
            IntPtr hwnd, int uMsg, IntPtr wParam, StringBuilder lParam, int flags, int uTimeout, out IntPtr result);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(
            Hwnd hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(
            Hwnd hWnd, int nMsg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32", ExactSpelling = true)]
        internal static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowLong(Hwnd hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowEnabled(Hwnd hwnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct Hwnd
        {
            public IntPtr h;

            public static Hwnd Cast(IntPtr h)
            {
                var hTemp = new Hwnd {h = h};
                return hTemp;
            }

            public static implicit operator IntPtr(Hwnd h) => h.h;

            public static Hwnd Null
            {
                get
                {
                    var hTemp = new Hwnd {h = IntPtr.Zero};
                    return hTemp;
                }
            }

            public static bool operator ==(Hwnd hl, Hwnd hr) => hl.h == hr.h;

            public static bool operator !=(Hwnd hl, Hwnd hr) => hl.h != hr.h;

            public override bool Equals(object oCompare)
            {
                var hr = Cast((Hwnd) oCompare);
                return h == hr.h;
            }

            public override int GetHashCode() => (int)h;
        }
    }
}