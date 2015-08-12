// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StickyNotesDemo
{
    public class Window2 : Window
    {
        private Brush _backGround;
        private Dialog _dialog;
        private Brush _foreGround;
        private RichTextBox _rtb;
        private double _windowHeight;
        private double _windowWidth;

        public Window2()
        {
            Name = "Window2";

            var lgb = ChangeBackgroundColor(Colors.Yellow);
            Background = lgb;

            WindowStyle = WindowStyle.ToolWindow;
            Height = Width = 200;

            Content = InitializeStickyNote();

            Closing += Window2_Closing;
            SizeChanged += Window2_SizeChanged;
            PreviewMouseLeftButtonUp += Window2_PreviewMouseLeftButtonUp;
            ShowInTaskbar = false;
            Show();
            _rtb.Focus();
        }

        public int BackgroundColorIndex { get; set; } = -1;
        public int ForegroundColorIndex { get; set; } = -1;
        public DateTime Alarm { get; set; }
        public bool TopmostWindow { get; set; }
        public bool AlarmDismissed { get; set; }
        public string RepetitionNumber { get; set; } = "null";

        public static LinearGradientBrush ChangeBackgroundColor(Color c)
        {
            var gs1 = new GradientStop(Colors.White, 0);
            var gs2 = new GradientStop(c, 1);
            var gsc = new GradientStopCollection {gs1, gs2};
            var lgb = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = gsc
            };
            return lgb;
        }

        private void Window2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (WindowStyle.ToString() == WindowStyle.None.ToString())
            {
                WindowStyle = WindowStyle.ToolWindow;
                Height = _windowHeight;
                Width = _windowWidth;
                ResizeMode = ResizeMode.CanResize;
                Background = _backGround;
                Foreground = _foreGround;
                Cursor = Cursors.Arrow;
                BorderThickness = new Thickness(0);
                ShowInTaskbar = false;
            }
        }

        private void Window2_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _rtb.Width = ((ActualWidth - 10) > 0) ? (ActualWidth - 10) : _rtb.Width;
            _rtb.Height = ((ActualHeight - 50) > 0) ? (ActualHeight - 50) : _rtb.Height;
        }

        private StackPanel InitializeStickyNote()
        {
            var sp = new StackPanel();
            var menu = new Menu
            {
                Background = Brushes.Transparent,
                Width = 20,
                Height = 15,
                HorizontalAlignment = HorizontalAlignment.Left
            };


            var m0 = new MenuItem
            {
                Padding = new Thickness(0),
                Margin = new Thickness(0)
            };
            var menuButton = new Button
            {
                Width = 18,
                Height = 13,
                Background = Brushes.Red
            };
            menuButton.Click += menuButton_Click;
            m0.Header = menuButton;
            m0.BorderThickness = new Thickness(1);


            var m1 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "Minimize"
            };
            m1.Click += m1_Click;

            var m2 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "Preferences"
            };
            m2.Click += m2_Click;


            var m3 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "Close"
            };
            m3.Click += m3_Click;

            var m4 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "AlwaysOnTop"
            };
            m4.Click += m4_Click;
            m4.Name = "AlwaysOnTop";

            var m5 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "Email Note To:"
            };
            m5.Click += m5_Click;
            m5.Name = "email";

            var m6 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "Remove Alarm"
            };
            m6.Click += m6_Click;
            m6.Name = "alarm";
            m6.IsEnabled = false;

            var m7 = new MenuItem
            {
                Background = Brushes.BlanchedAlmond,
                Header = "Dismiss Alarm"
            };
            m7.Click += m7_Click;
            m7.Name = "DismissAlarm";
            m7.IsEnabled = false;


            m0.Items.Add(m1);
            m0.Items.Add(m2);
            m0.Items.Add(m4);
            m0.Items.Add(m5);
            m0.Items.Add(m6);
            m0.Items.Add(m7);
            m0.Items.Add(m3);

            menu.Items.Add(m0);

            var wp = new WrapPanel();
            wp.Children.Add(menu);

            var simpleImage = new Image {Margin = new Thickness(0)};

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("alarm3.PNG", UriKind.Relative);
            bi.EndInit();
            //Set image source
            simpleImage.Source = bi;
            simpleImage.HorizontalAlignment = HorizontalAlignment.Center;
            simpleImage.Visibility = Visibility.Hidden;
            simpleImage.Name = "AlarmIndicator";
            simpleImage.Width = 13;
            wp.Children.Add(simpleImage);

            sp.Children.Add(wp);

            _rtb = new RichTextBox
            {
                Name = "NoteBox",
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0, 1, 0, 0),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Height = Height - 50,
                Width = Width - 10
            };
            sp.Children.Add(_rtb);

            return sp;
        }

        private void m7_Click(object sender, RoutedEventArgs e)
        {
            var m = ((sender as MenuItem).Parent as MenuItem).Parent as Menu;
            (sender as MenuItem).IsEnabled = false;

            var w = ((Window2) (((StackPanel) (m.Parent as WrapPanel).Parent).Parent as Window));
            var m1 = sender as MenuItem;
            m1.IsEnabled = false;
            var m2 = LogicalTreeHelper.FindLogicalNode(w, "AlwaysOnTop") as MenuItem;
            w.Topmost = w.TopmostWindow;
            m2.IsChecked = w.Topmost;

            var menuItemRemoveAlarm = LogicalTreeHelper.FindLogicalNode(w, "alarm") as MenuItem;
            var im = LogicalTreeHelper.FindLogicalNode(w, "AlarmIndicator") as Image;
            if (w.RepetitionNumber.Contains("null"))
            {
                im.Visibility = Visibility.Hidden;
                menuItemRemoveAlarm.IsEnabled = false;
            }
            else
            {
                im.Visibility = Visibility.Visible;
                menuItemRemoveAlarm.IsEnabled = true;
            }

            w.BorderThickness = new Thickness(3);
            w.BorderBrush = Brushes.Red;
            AlarmDismissed = true;
        }

        private void m6_Click(object sender, RoutedEventArgs e)
        {
            var m = ((sender as MenuItem).Parent as MenuItem).Parent as Menu;
            var w = ((Window2) (((StackPanel) (m.Parent as WrapPanel).Parent).Parent as Window));
            w.Alarm = new DateTime();
            var m1 = sender as MenuItem;
            m1.IsEnabled = false;
            var m2 = LogicalTreeHelper.FindLogicalNode(w, "AlwaysOnTop") as MenuItem;
            w.Topmost = w.TopmostWindow;
            m2.IsChecked = w.Topmost;

            var menuItemDismissAlarm = LogicalTreeHelper.FindLogicalNode(w, "DismissAlarm") as MenuItem;
            menuItemDismissAlarm.IsEnabled = false;

            var im = LogicalTreeHelper.FindLogicalNode(w, "AlarmIndicator") as Image;
            im.Visibility = Visibility.Hidden;
            w.RepetitionNumber = "null";

            w.BorderThickness = new Thickness(0);
            w.BorderBrush = Brushes.Transparent;
        }

        private void m5_Click(object sender, RoutedEventArgs e)
        {
            var tr = new TextRange(_rtb.Document.ContentStart, _rtb.Document.ContentEnd);
            var emailDialog = new EmailDialog(tr.Text);
        }

        private void m2_Click(object sender, RoutedEventArgs e)
        {
            _dialog = new Dialog(Alarm, RepetitionNumber);
            _dialog.Closed += dialog_Closed;
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            var dia = sender as Dialog;
            if (dia.Tag != null)
            {
                dia.Tag = null;
                var arr = dia.PropertyArray;
                if (arr[0] != null)
                {
                    BackgroundColorIndex = (int) (arr[0]);
                    if (WindowStyle.ToString() == WindowStyle.None.ToString())
                    {
                        _backGround = ChangeBackgroundColor(Dialog.ColorArray[BackgroundColorIndex]);
                    }
                    else
                    {
                        Background = ChangeBackgroundColor(Dialog.ColorArray[BackgroundColorIndex]);
                        _backGround = Background;
                    }
                }
                else
                {
                    BackgroundColorIndex = (BackgroundColorIndex != -1) ? BackgroundColorIndex : -1;
                }
                if (arr[1] != null)
                {
                    ForegroundColorIndex = (int) (arr[1]);
                    var rtb = LogicalTreeHelper.FindLogicalNode(this, "NoteBox") as RichTextBox;
                    rtb.SelectAll();
                    rtb.Selection.ApplyPropertyValue(ForegroundProperty,
                        Dialog.ColorArrayForeground[ForegroundColorIndex]);
                    rtb.Selection.Select(rtb.Document.ContentStart, rtb.Document.ContentStart);
                    _foreGround = Foreground;
                }
                else
                {
                    ForegroundColorIndex = (ForegroundColorIndex != -1) ? ForegroundColorIndex : -1;
                }
                if (arr[2] != null)
                {
                    Alarm = dia.Alarm;


                    var w = this;
                    var m = LogicalTreeHelper.FindLogicalNode(w, "AlwaysOnTop") as MenuItem;
                    var m1 = LogicalTreeHelper.FindLogicalNode(w, "alarm") as MenuItem;
                    var m3 = LogicalTreeHelper.FindLogicalNode(w, "DismissAlarm") as MenuItem;
                    var m2 = LogicalTreeHelper.FindLogicalNode(w, "AlarmIndicator") as Image;
                    var alarm = w.Alarm;
                    var current = DateTime.Now;
                    m2.Visibility = (alarm.CompareTo(DateTime.Now) >= 0) ? (Visibility.Visible) : (Visibility.Hidden);

                    if (alarm.Date == current.Date)
                    {
                        var temp1 = alarm;
                        var temp2 = current;
                        temp1 = temp1.AddMinutes(30);
                        temp2 = temp2.AddMinutes(30);

                        if ((temp2.CompareTo(alarm) >= 0) && (temp1.CompareTo(current) >= 0))
                        {
                            w.Topmost = true;
                            m.IsChecked = true;
                            m1.IsEnabled = true;
                            w.BorderBrush = Brushes.Black;
                            w.BorderThickness = new Thickness(5);
                            m3.IsEnabled = true;
                        }
                    }
                    else
                    {
                        w.Topmost = w.TopmostWindow;
                        m.IsChecked = w.Topmost;
                        m1.IsEnabled = (alarm.CompareTo(DateTime.Now) >= 0) ? true : false;
                        m3.IsEnabled = false;

                        w.BorderBrush = Brushes.Transparent;
                        w.BorderThickness = new Thickness(0);
                    }
                }
                RepetitionNumber = dia.AlarmRepetition;
            }
        }

        private void m4_Click(object sender, RoutedEventArgs e)
        {
            var m = ((sender as MenuItem).Parent as MenuItem).Parent as Menu;
            var w = ((Window2) (((StackPanel) (m.Parent as WrapPanel).Parent).Parent as Window));
            if (w.Topmost == false)
            {
                w.Topmost = true;
                TopmostWindow = true;
                ((MenuItem) (sender)).IsChecked = true;
                w.UpdateLayout();
                w.BringIntoView();
            }
            else
            {
                TopmostWindow = w.Topmost = false;
                ((MenuItem) (sender)).IsChecked = false;
            }
        }

        private void m1_Click(object sender, RoutedEventArgs e)
        {
            var m = ((sender as MenuItem).Parent as MenuItem).Parent as Menu;
            var w = ((Window2) (((StackPanel) (m.Parent as WrapPanel).Parent).Parent as Window));
            _windowHeight = w.ActualHeight;
            _windowWidth = w.ActualWidth;
            w.Height = 20;
            w.Width = 20;

            _foreGround = w.Foreground;
            _backGround = w.Background;

            w.Cursor = Cursors.Hand;
            w.BorderThickness = new Thickness(2);
            w.WindowStyle = WindowStyle.None;
            w.ResizeMode = ResizeMode.NoResize;
            w.Background = Brushes.GreenYellow;
        }

        private void m3_Click(object sender, RoutedEventArgs e)
        {
            var m = ((sender as MenuItem).Parent as MenuItem).Parent as Menu;
            ((Window2) (((StackPanel) (m.Parent as WrapPanel).Parent).Parent as Window)).Close();
        }

        private void menuButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var menuItem = b.Parent as MenuItem;
            menuItem.Focus();
            menuItem.IsSubmenuOpen = true;
        }

        private void Window2_Closing(object sender, CancelEventArgs e)
        {
            var w2 = sender as Window2;
            var w1 = Application.Current.MainWindow as MainWindow;
            var arr = w1.WindowArray;
            var index = ((int) (w2.Tag));
            arr[index] = null;
            _dialog?.Close();
        }
    }
}