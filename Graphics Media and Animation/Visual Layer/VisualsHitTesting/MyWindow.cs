// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace SDKSample
{
    internal class MyWindow
    {
        // Constant values from the "winuser.h" header file.
        internal const int WsChild = 0x40000000,
            WsVisible = 0x10000000;

        // Constant values from the "winuser.h" header file.
        internal const int WmLbuttonup = 0x0202,
            WmRbuttonup = 0x0205;

        public static int Width = Form.ActiveForm.ClientSize.Width;
        public static int Height = Form.ActiveForm.ClientSize.Height;
        public static HwndSource MyHwndSource;
        public static bool TopmostLayer = true;
        public static bool ChangeColor;

        public static void FillWithCircles(IntPtr parentHwnd)
        {
            // Fill the client area of the form with randomly placed circles.
            for (var i = 0; i < 200; i++)
            {
                CreateShape(parentHwnd);
            }
        }

        public static void CreateShape(IntPtr parentHwnd)
        {
            // Create an instance of the shape.
            var myShape = new MyShape();

            // Determine whether the host container window has been created.
            if (MyHwndSource == null)
            {
                // Create the host container window for the visual objects.
                CreateHostHwnd(parentHwnd);

                // Associate the shape with the host container window.
                MyHwndSource.RootVisual = myShape;
            }
            else
            {
                // Assign the shape as a child of the root visual.
                ((ContainerVisual) MyHwndSource.RootVisual).Children.Add(myShape);
            }
        }

        internal static void CreateHostHwnd(IntPtr parentHwnd)
        {
            // Set up the parameters for the host hwnd.
            var parameters = new HwndSourceParameters("Visual Hit Test", Width, Height)
            {
                WindowStyle = WsVisible | WsChild
            };
            parameters.SetPosition(0, 24);
            parameters.ParentWindow = parentHwnd;
            parameters.HwndSourceHook = ApplicationMessageFilter;

            // Create the host hwnd for the visuals.
            MyHwndSource = new HwndSource(parameters);

            // Set the hwnd background color to the form's background color.
            MyHwndSource.CompositionTarget.BackgroundColor = Brushes.OldLace.Color;
        }

        internal static IntPtr ApplicationMessageFilter(
            IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages passed to the visual.
            switch (message)
            {
                // Handle the left and right mouse button up messages.
                case WmLbuttonup:
                case WmRbuttonup:
                    var pt = new Point
                    {
                        X = (uint) lParam & 0x0000ffff,
                        Y = (uint) lParam >> 16
                    };
                    // LOWORD = x
                    // HIWORD = y
                    MyShape.OnHitTest(pt, message);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}