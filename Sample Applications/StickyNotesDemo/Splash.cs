// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace StickyNotesDemo
{
    // This only works with Windows 2000/XP
    internal class Splash : Form
    {
        public Splash(Bitmap bitmap)
        {
            // Window settings
            TopMost = true;
            ShowInTaskbar = false;
            Size = bitmap.Size;
            StartPosition = FormStartPosition.CenterScreen;
            Show();
            // Must be called before setting bitmap
            SelectBitmap(bitmap);
            BackColor = Color.Red;
            Click += Splash_Click;
            MouseClick += Splash_MouseClick;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Add the layered extended style (WS_EX_LAYERED) to this window
                var createParams = base.CreateParams;
                createParams.ExStyle |= NativeMethods.WsExLayered;
                return createParams;
            }
        }

        private void Splash_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void Splash_Click(object sender, EventArgs e)
        {
        }

        // Sets the current bitmap
        public void SelectBitmap(Bitmap bitmap)
        {
            // Does this bitmap contain an alpha channel?
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");
            }

            // Get device contexts
            var screenDc = NativeMethods.GetDC(IntPtr.Zero);
            var memDc = NativeMethods.CreateCompatibleDC(screenDc);
            var hBitmap = IntPtr.Zero;
            var hOldBitmap = IntPtr.Zero;

            try
            {
                // Get handle to the new bitmap and select it into the current device context
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                // Set parameters for layered window update
                var newSize = new NativeMethods.Size(bitmap.Width, bitmap.Height);
                // Size window to match bitmap
                var sourceLocation = new NativeMethods.Point(0, 0);
                var newLocation = new NativeMethods.Point(Left, Top); // Same as this window
                var blend = new NativeMethods.Blendfunction
                {
                    BlendOp = NativeMethods.AcSrcOver,
                    BlendFlags = 0,
                    SourceConstantAlpha = 255,
                    AlphaFormat = NativeMethods.AcSrcAlpha
                };
                // Only works with a 32bpp bitmap
                // Always 0
                // Set to 255 for per-pixel alpha values
                // Only works when the bitmap contains an alpha channel

                // Update the window
                NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref newLocation, ref newSize,
                    memDc, ref sourceLocation, 0, ref blend, NativeMethods.UlwAlpha);
            }
            finally
            {
                // Release device context
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, hOldBitmap);
                    NativeMethods.DeleteObject(hBitmap); // Remove bitmap resources
                }
                NativeMethods.DeleteDC(memDc);
            }
        }

        // Let Windows drag this window for us (thinks its hitting the title bar of the window)
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == NativeMethods.WmNchittest)
            {
                // Tell Windows that the user is on the title bar (caption)
                message.Result = (IntPtr) NativeMethods.Htcaption;
            }
            else
            {
                base.WndProc(ref message);
            }
        }
    }

    // Class to assist with Win32 API calls
}