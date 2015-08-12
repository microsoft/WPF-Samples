// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WPFHostingWin32Control
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal const int
            LbnSelchange = 0x00000001,
            WmCommand = 0x00000111,
            LbGetcursel = 0x00000188,
            LbGettextlen = 0x0000018A,
            LbAddstring = 0x00000180,
            LbGettext = 0x00000189,
            LbDeletestring = 0x00000182,
            LbGetcount = 0x0000018B;

        private Application _app;
        private IntPtr _hwndListBox;
        private int _itemCount;
        private ControlHost _listControl;
        private Window _myWindow;
        private int _selectedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void On_UIReady(object sender, EventArgs e)
        {
            _app = Application.Current;
            _myWindow = _app.MainWindow;
            _myWindow.SizeToContent = SizeToContent.WidthAndHeight;
            _listControl = new ControlHost(ControlHostElement.ActualHeight, ControlHostElement.ActualWidth);
            ControlHostElement.Child = _listControl;
            _listControl.MessageHook += ControlMsgFilter;
            _hwndListBox = _listControl.HwndListBox;
            for (var i = 0; i < 15; i++) //populate listbox
            {
                var itemText = "Item" + i;
                SendMessage(_hwndListBox, LbAddstring, IntPtr.Zero, itemText);
            }
            _itemCount = SendMessage(_hwndListBox, LbGetcount, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + _itemCount;
        }

        private void AppendText(object sender, EventArgs args)
        {
            if (txtAppend.Text != string.Empty)
            {
                SendMessage(_hwndListBox, LbAddstring, IntPtr.Zero, txtAppend.Text);
            }
            _itemCount = SendMessage(_hwndListBox, LbGetcount, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + _itemCount;
        }

        private void DeleteText(object sender, EventArgs args)
        {
            _selectedItem = SendMessage(_listControl.HwndListBox, LbGetcursel, IntPtr.Zero, IntPtr.Zero);
            if (_selectedItem != -1) //check for selected item
            {
                SendMessage(_hwndListBox, LbDeletestring, (IntPtr) _selectedItem, IntPtr.Zero);
            }
            _itemCount = SendMessage(_hwndListBox, LbGetcount, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + _itemCount;
        }

        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int textLength;

            handled = false;
            if (msg == WmCommand)
            {
                switch ((uint) wParam.ToInt32() >> 16 & 0xFFFF) //extract the HIWORD
                {
                    case LbnSelchange: //Get the item text and display it
                        _selectedItem = SendMessage(_listControl.HwndListBox, LbGetcursel, IntPtr.Zero, IntPtr.Zero);
                        textLength = SendMessage(_listControl.HwndListBox, LbGettextlen, IntPtr.Zero, IntPtr.Zero);
                        var itemText = new StringBuilder();
                        SendMessage(_hwndListBox, LbGettext, _selectedItem, itemText);
                        selectedText.Text = itemText.ToString();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
            int msg,
            int wParam,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hwnd,
            int msg,
            IntPtr wParam,
            string lParam);
    }
}