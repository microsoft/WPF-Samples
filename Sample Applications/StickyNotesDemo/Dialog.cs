// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StickyNotesDemo
{
    public class Dialog : Window
    {
        public static Color[] ColorArray =
        {
            Colors.Silver, Colors.Violet, Colors.SkyBlue, Colors.LawnGreen, Colors.Yellow,
            Colors.OrangeRed, Colors.Purple, Colors.Red
        };

        public static SolidColorBrush[] ColorArrayForeground =
        {
            Brushes.Black, Brushes.Red, Brushes.Green, Brushes.Blue,
            Brushes.Brown, Brushes.DarkMagenta, Brushes.DarkSalmon, Brushes.Lime
        };

        private readonly TabControl _tabControl;

        public Dialog(DateTime noteAlarm, string loadedRepetition)
        {
            WindowStyle = WindowStyle.ToolWindow;
            Height = 350;
            Width = 600;
            Title = "Options";
            Background = Brushes.LemonChiffon;
            Alarm = noteAlarm;
            AlarmRepetition = loadedRepetition;

            var sp = new StackPanel();

            _tabControl = new TabControl
            {
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Width = Width - 10,
                Height = Height - 60,
                Background = Brushes.Transparent
            };
            sp.Children.Add(_tabControl);

            var item1 = new TabItem();
            FirstTabColorSettings(item1);
            _tabControl.Items.Add(item1);

            var item2 = new TabItem();
            SecondTabMailSettings(item2);
            _tabControl.Items.Add(item2);

            var item3 = new TabItem();
            ThirdTabAlarmSettings(item3);
            _tabControl.Items.Add(item3);

            var ok = new Button
            {
                Content = "OK",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            ok.Click += ok_Click;
            sp.Children.Add(ok);

            Content = sp;

            Show();

            SizeChanged += Dialog_SizeChanged;
        }

        public object[] PropertyArray { get; } = new object[10];
        public DateTime Alarm { get; private set; }
        public string AlarmRepetition { get; private set; } = "null";

        private void ThirdTabAlarmSettings(TabItem item)
        {
            item.Header = "Alarm Settings";
            var sp = new StackPanel {Background = Brushes.Transparent};


            var g = new Grid {Background = Brushes.Transparent};
            var rdef1 = new RowDefinition();
            var rdef2 = new RowDefinition();
            var rdef3 = new RowDefinition();
            g.RowDefinitions.Add(rdef1);
            g.RowDefinitions.Add(rdef2);
            g.RowDefinitions.Add(rdef3);

            var cd1 = new ColumnDefinition();
            var gdl = new GridLength(300);
            cd1.Width = gdl;
            var cd2 = new ColumnDefinition();
            g.ColumnDefinitions.Add(cd1);
            g.ColumnDefinitions.Add(cd2);


            var l1 = new Label
            {
                Background = sp.Background,
                Content = "Date (mm/dd/yyyy)"
            };


            var date = new TextBox
            {
                Text = "/2006",
                Name = "date"
            };
            //if (Alarm.CompareTo(DateTime.Now) >= 0)
            //{
            //    date.Text = Alarm.Date.ToShortDateString();
            //}

            if (AlarmRepetition.Contains("null"))
            {
                if ((Alarm.CompareTo(DateTime.Now) >= 0) ||
                    (((DateTime.Now.TimeOfDay.Hours - Alarm.TimeOfDay.Hours) < 8) &&
                     ((DateTime.Now.TimeOfDay.Hours - Alarm.TimeOfDay.Hours) >= 0)))
                {
                    date.Text = Alarm.Date.ToShortDateString();
                }
            }
            else
            {
                date.Text = Alarm.Date.ToShortDateString();
            }


            Grid.SetRow(l1, 0);
            Grid.SetColumn(l1, 0);
            Grid.SetRow(date, 0);
            Grid.SetColumn(date, 1);


            var l2 = new Label
            {
                Background = sp.Background,
                Content = "Time (hr:min:sec)"
            };

            var time = new TextBox
            {
                Text = "00:00:00",
                Name = "time"
            };

            if (AlarmRepetition.Contains("null"))
            {
                if ((Alarm.CompareTo(DateTime.Now) >= 0) ||
                    (((DateTime.Now.TimeOfDay.Hours - Alarm.TimeOfDay.Hours) < 8) &&
                     ((DateTime.Now.TimeOfDay.Hours - Alarm.TimeOfDay.Hours) >= 0)))
                {
                    time.Text = Alarm.TimeOfDay.ToString();
                }
            }
            else
            {
                time.Text = Alarm.TimeOfDay.ToString();
            }


            Grid.SetRow(l2, 1);
            Grid.SetColumn(l2, 0);
            Grid.SetRow(time, 1);
            Grid.SetColumn(time, 1);


            g.Children.Add(l1);
            g.Children.Add(date);
            g.Children.Add(l2);
            g.Children.Add(time);

            var l3 = new Label
            {
                Background = sp.Background,
                Content = "Repeat Every:"
            };

            var repeatNumber = new TextBox
            {
                Text = "0",
                Name = "repeat",
                MaxLength = 3
            };
            repeatNumber.TextChanged += repeatNumber_TextChanged;
            if (AlarmRepetition.Contains("null") == false)
            {
                repeatNumber.Text = AlarmRepetition.Substring(1);
            }

            var space = new Label
            {
                Background = sp.Background,
                Content = " "
            };

            var daysCheck = new CheckBox
            {
                IsChecked = false,
                VerticalAlignment = VerticalAlignment.Center,
                Name = "daysCheck"
            };
            daysCheck.Checked += daysCheck_Checked;


            var l4 = new Label
            {
                Background = sp.Background,
                Content = "Day(s)"
            };

            var space1 = new Label();
            space.Background = sp.Background;
            space.Content = " ";

            var weeksCheck = new CheckBox
            {
                IsChecked = false,
                VerticalAlignment = VerticalAlignment.Center,
                Name = "weeksCheck"
            };
            weeksCheck.Checked += weeksCheck_Checked;
            try
            {
                if (AlarmRepetition.Contains("null") == false)
                {
                    var charr = AlarmRepetition.ToCharArray(0, 1);
                    switch (charr[0])
                    {
                        case 'D':
                            daysCheck.IsChecked = true;
                            break;

                        case 'W':
                            weeksCheck.IsChecked = true;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            var l5 = new Label
            {
                Background = sp.Background,
                Content = "Week(s)"
            };

            var wp = new WrapPanel();
            wp.Children.Add(l3);
            wp.Children.Add(repeatNumber);
            wp.Children.Add(space);
            wp.Children.Add(daysCheck);
            wp.Children.Add(l4);
            wp.Children.Add(space1);
            wp.Children.Add(weeksCheck);
            wp.Children.Add(l5);


            var status = new Label
            {
                Background = sp.Background,
                Name = "statusThirdTab"
            };

            sp.Children.Add(g);
            sp.Children.Add(wp);
            sp.Children.Add(status);

            item.Content = sp;
        }

        private void repeatNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var l = LogicalTreeHelper.FindLogicalNode(this, "statusThirdTab") as Label;
                l.Content = "";
                var str = (sender as TextBox)?.Text;
                var charArr = str.ToCharArray();
                var val = charArr.All(t => (t >= '0') && (t <= '9'));
                if (val == false)
                {
                    l.Content = "Input should consist of only numbers";
                    l.Foreground = Brushes.Red;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void daysCheck_Checked(object sender, RoutedEventArgs e)
        {
            var weeksCheck = LogicalTreeHelper.FindLogicalNode(this, "weeksCheck") as CheckBox;
            weeksCheck.IsChecked = false;
        }

        private void weeksCheck_Checked(object sender, RoutedEventArgs e)
        {
            var daysCheck = LogicalTreeHelper.FindLogicalNode(this, "daysCheck") as CheckBox;
            daysCheck.IsChecked = false;
        }

        private void SecondTabMailSettings(TabItem item)
        {
            item.Header = "Mail Settings";
            var item2Panel = new StackPanel();

            var l1 = new Label
            {
                Content = "SMTP Mail Server:",
                Background = Brushes.Transparent
            };

            var server = new TextBox
            {
                ToolTip = "smtp.gmail.com/smtp.mail.yahoo.com/smtp.aol.com/...",
                Name = "server"
            };

            var l2 = new Label
            {
                Content = "Mail Account",
                Background = Brushes.Transparent
            };

            var account = new TextBox
            {
                ToolTip = "Something like: xyz@aaa.com",
                Name = "account"
            };

            var l3 = new Label
            {
                Content = "Mail Account",
                Background = Brushes.Transparent
            };

            var port = new TextBox
            {
                ToolTip = "Port used: Usually 25",
                Text = "25",
                Name = "port"
            };

            var wp = new WrapPanel();
            var l4 = new Label
            {
                Content = "Use SSL",
                ToolTip = "Check if SMTP server uses SSL. (gmail uses, Yahoo/aol dont)",
                Background = Brushes.Transparent
            };

            var cb = new CheckBox
            {
                ToolTip = "Check if SMTP server uses SSL. (gmail uses, Yahoo/aol dont)",
                VerticalAlignment = VerticalAlignment.Center,
                Name = "sslCheck"
            };

            wp.Children.Add(l4);
            wp.Children.Add(cb);

            var l5 = new Label
            {
                Content = "",
                Background = Brushes.Transparent,
                Name = "status"
            };

            item2Panel.Children.Add(l1);
            item2Panel.Children.Add(server);
            item2Panel.Children.Add(l2);
            item2Panel.Children.Add(account);
            item2Panel.Children.Add(l3);
            item2Panel.Children.Add(port);
            item2Panel.Children.Add(wp);
            item2Panel.Children.Add(l5);


            item.Content = item2Panel;
        }

        private void FirstTabColorSettings(TabItem item)
        {
            item.Header = "Color Settings";
            var item1Panel = new StackPanel();
            var l1 = new Label
            {
                Content = "Background Color:",
                Background = Brushes.Transparent
            };

            var combo = new ComboBox();
            InitializeComboBox(combo);
            combo.SelectionChanged += combo_SelectionChanged;

            var l2 = new Label
            {
                Content = "Foreground Color:",
                Background = Brushes.Transparent
            };

            var combo1 = new ComboBox();
            InitializeComboBox1(combo1);
            combo1.SelectionChanged += combo1_SelectionChanged;

            item1Panel.Children.Add(l1);
            item1Panel.Children.Add(combo);
            item1Panel.Children.Add(l2);
            item1Panel.Children.Add(combo1);
            item1Panel.Background = Brushes.Transparent;

            item.Content = item1Panel;
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            var comboItem = cb.SelectedItem as ComboBoxItem;
            PropertyArray[(int) PropNames.ForegroundColor] = comboItem?.Tag;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            var server = LogicalTreeHelper.FindLogicalNode(this, "server") as TextBox;
            var account = LogicalTreeHelper.FindLogicalNode(this, "account") as TextBox;
            var date = LogicalTreeHelper.FindLogicalNode(this, "date") as TextBox;
            var time = LogicalTreeHelper.FindLogicalNode(this, "time") as TextBox;
            var repeatNumber = LogicalTreeHelper.FindLogicalNode(this, "repeat") as TextBox;
            var daysCheck = LogicalTreeHelper.FindLogicalNode(this, "daysCheck") as CheckBox;
            var weeksCheck = LogicalTreeHelper.FindLogicalNode(this, "weeksCheck") as CheckBox;

            var str = (daysCheck.IsChecked == true) ? "D" : "";
            str += (weeksCheck.IsChecked == true) ? "W" : "";
            if (str != string.Empty)
            {
                str += repeatNumber.Text;
            }
            else
            {
                str = "null";
            }

            if ((date.Text != string.Empty) && (date.Text != "/2006") &&
                (!(date.Text.Contains(Alarm.Date.ToShortDateString()))
                 || !(AlarmRepetition.Contains(str)) ||
                 !(Alarm.TimeOfDay.ToString().Contains(time.Text))))
            {
                var dateString = date.Text;
                var arr = new int[3];
                var timeString = time.Text;
                var arr1 = new int[3];

                var tail = 0;
                var head = dateString.IndexOf('/');
                head = (head >= 0) ? head : 0;

                try
                {
                    for (var i = 0; i < 3; i++)
                    {
                        head = (i == (arr.Length - 1)) ? (dateString.Length) : head;
                        arr[i] = int.Parse(dateString.Substring(tail, head - tail));
                        if (head == dateString.Length)
                        {
                            break;
                        }
                        tail = head + 1;
                        head = dateString.IndexOf('/', tail);
                    }
                    tail = 0;
                    head = timeString.IndexOf(':');
                    head = (head >= 0) ? head : 0;
                    for (var i = 0; i < 3; i++)
                    {
                        head = (i == (arr1.Length - 1)) ? (timeString.Length) : head;
                        arr1[i] = int.Parse(timeString.Substring(tail, head - tail));
                        if (head == timeString.Length)
                        {
                            break;
                        }
                        tail = head + 1;
                        head = timeString.IndexOf(':', tail);
                    }

                    var dt = new DateTime(arr[2], arr[0], arr[1], arr1[0], arr1[1], arr1[2]);
                    if (dt.CompareTo(DateTime.Now) >= 0)
                    {
                        Alarm = dt;
                        PropertyArray[(int) PropNames.Alarm] = dt;
                        if (repeatNumber.Text != string.Empty)
                        {
                            AlarmRepetition = (daysCheck.IsChecked == true) ? "D" : "";
                            AlarmRepetition += (weeksCheck.IsChecked == true) ? "W" : "";
                            if (AlarmRepetition != string.Empty)
                            {
                                AlarmRepetition += repeatNumber.Text;
                            }
                            else
                            {
                                AlarmRepetition = "null";
                            }
                        }
                        Tag = "OK";
                        Close();
                    }
                    else
                    {
                        Title = "AlarmSettings Error";
                        var l = LogicalTreeHelper.FindLogicalNode(this, "statusThirdTab") as Label;
                        l.Content = "Alarm can be set if the event is after Current time";
                        l.Foreground = Brushes.Red;
                    }
                }
                catch (Exception e1)
                {
                    var l = LogicalTreeHelper.FindLogicalNode(this, "statusThirdTab") as Label;
                    l.Content = e1.Message;
                    l.Foreground = Brushes.Red;
                    Title = "AlarmSettings Error";
                }
            }
            else
            {
                if (((server.Text == string.Empty) && (account.Text == string.Empty)) ||
                    ((server.Text != string.Empty) && (account.Text != string.Empty)))
                {
                    if (((server.Text != string.Empty) && (account.Text != string.Empty)))
                    {
                        var port = LogicalTreeHelper.FindLogicalNode(this, "port") as TextBox;
                        var cb = LogicalTreeHelper.FindLogicalNode(this, "sslCheck") as CheckBox;


                        var fs = new FileStream("MailServerInfo", FileMode.OpenOrCreate);
                        var writer = new StreamWriter(fs);
                        writer.WriteLine(server.Text);
                        writer.WriteLine(account.Text);
                        port.Text = (port.Text != string.Empty) ? port.Text : "25";
                        writer.WriteLine(port.Text);
                        var check = (cb.IsChecked == true) ? "true" : "false";
                        writer.WriteLine(check);

                        writer.Close();
                        fs.Close();
                    }
                    Tag = "OK";
                    Close();
                }
                else
                {
                    Title = "Mail settings are incomplete";
                    server.Background = account.Background = Window2.ChangeBackgroundColor(Colors.Red);
                    var status = LogicalTreeHelper.FindLogicalNode(this, "status") as Label;
                    status.Content = "Fill in highlighted fields";
                    status.Foreground = Brushes.Red;
                }
            }
        }

        private void InitializeComboBox1(ComboBox combo)
        {
            for (var i = 0; i < ColorArrayForeground.Length; i++)
            {
                var c = ColorArrayForeground[i];
                var comboItem = new ComboBoxItem();
                var l = new Label
                {
                    Foreground = c,
                    MinHeight = 20,
                    MinWidth = 20
                };
                comboItem.Background = l.Background = c;
                comboItem.Tag = i;
                comboItem.Content = l;
                combo.Items.Add(comboItem);
            }
        }

        private void InitializeComboBox(ComboBox combo)
        {
            for (var i = 0; i < ColorArray.Length; i++)
            {
                var c = ColorArray[i];
                var comboItem = new ComboBoxItem();
                var l = new Label
                {
                    Background = Window2.ChangeBackgroundColor(c),
                    MinHeight = 20,
                    MinWidth = 20
                };
                comboItem.Background = Window2.ChangeBackgroundColor(c);
                comboItem.Tag = i;
                comboItem.Content = l;
                combo.Items.Add(comboItem);
            }
        }

        private void Dialog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _tabControl.Width = ActualWidth;
            _tabControl.Height = (ActualHeight > 60) ? (ActualHeight - 60) : _tabControl.Height;
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            var comboItem = cb.SelectedItem as ComboBoxItem;
            PropertyArray[(int) PropNames.BackgroundColor] = comboItem.Tag;
        }
    }
}