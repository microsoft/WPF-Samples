// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace StickyNotesDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _alarmTimer;
        private DispatcherTimer _timerClock;
        private string[] _windowContents;

        public MainWindow()
        {
            InitializeComponent();
        }

        public int ArrayPos { get; set; }
        public object[] WindowArray { get; private set; }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            WindowArray = new object[100];

            _timerClock = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 4),
                IsEnabled = true
            };

            _alarmTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 30, 0),
                IsEnabled = true
            };
            _alarmTimer.Tick += AlarmTimer_Tick;

            LoadNotes();

            ContextMenuService.SetPlacement(AppWindow, PlacementMode.Bottom);

            AppWindow.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            AppWindow.Closing += MainWindow_Closing;
        }

        private void LoadNotes()
        {
            var count = 0;
            if (File.Exists("NotesFile"))
            {
                var fs = new FileStream("NotesFile", FileMode.Open);
                if (fs != null)
                {
                    int contentIndicator;
                    double height;
                    double width;
                    double x;
                    double y;
                    bool topmost;
                    int backgroundColorIndex;
                    int foregroundColorIndex;
                    DateTime alarmTime;
                    string loadRepeatNumber;

                    var reader = new StreamReader(fs);
                    var str = reader.ReadLine();
                    ExtractValues(out contentIndicator, str, out height, out width, out x, out y, out topmost,
                        out backgroundColorIndex,
                        out foregroundColorIndex, out alarmTime, out loadRepeatNumber);
                    AppWindow.Height = height;
                    AppWindow.Width = width;
                    AppWindow.Top = y;
                    AppWindow.Left = x;
                    AppWindow.Topmost = topmost;

                    count = contentIndicator;
                    for (var i = 0; i < count; i++)
                    {
                        var line = reader.ReadLine();

                        ExtractValues(out contentIndicator, line, out height, out width, out x, out y, out topmost,
                            out backgroundColorIndex, out foregroundColorIndex, out alarmTime, out loadRepeatNumber);

                        var filename = i.ToString();
                        filename = filename.Trim();

                        var w2 = new Window2
                        {
                            Title = "Sticky Note",
                            Tag = ArrayPos,
                            Height = height,
                            Width = width,
                            Left = x,
                            Top = y,
                            ShowInTaskbar = false,
                            Topmost = topmost,
                            TopmostWindow = topmost
                        };

                        if (w2.Topmost)
                        {
                            try
                            {
                                var m = LogicalTreeHelper.FindLogicalNode(w2, "AlwaysOnTop") as MenuItem;
                                m.IsChecked = true;
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (backgroundColorIndex > -1)
                        {
                            w2.Background = Window2.ChangeBackgroundColor(Dialog.ColorArray[backgroundColorIndex]);
                            w2.BackgroundColorIndex = backgroundColorIndex;
                        }
                        if (foregroundColorIndex > -1)
                        {
                            w2.Foreground = Dialog.ColorArrayForeground[foregroundColorIndex];
                            w2.ForegroundColorIndex = foregroundColorIndex;
                        }
                        w2.Alarm = alarmTime;
                        w2.RepetitionNumber = loadRepeatNumber.Contains("null") ? "null" : loadRepeatNumber;


                        CheckIfAlarmFires(w2);

                        WindowArray[ArrayPos] = w2;
                        ArrayPos++;

                        var noteFiles = new FileStream(filename, FileMode.Open);

                        var rtb = ((RichTextBox) ((StackPanel) (w2.Content)).Children[1]);
                        var tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                        try
                        {
                            tr.Load(noteFiles, contentIndicator == 0 ? DataFormats.XamlPackage : DataFormats.Xaml);
                        }
                        catch (Exception)
                        {
                        }
                        noteFiles.Close();
                    }
                    fs.Close();
                }
            }
            for (var i = ++count; i < 100; i++)
            {
                if (File.Exists(i.ToString()))
                {
                    File.Delete(i.ToString());
                }
            }
        }

        private void ExtractValues(out int contentIndicator, string line, out double height, out double width,
            out double x,
            out double y, out bool topmost, out int backgroundColorIndex, out int foregroundColorIndex,
            out DateTime alarmTime,
            out string loadRepeatNumber)
        {
            var index = line.IndexOf('|');
            var firstToken = line.Substring(0, index);
            contentIndicator = int.Parse(firstToken);

            var tail = index + 1;
            index = line.IndexOf('|', tail);
            y = GetStoredValues(line, index, tail);

            tail = index + 1;
            index = line.IndexOf('|', tail);
            x = GetStoredValues(line, index, tail);

            tail = index + 1;
            index = line.IndexOf('|', tail);
            height = GetStoredValues(line, index, tail);

            tail = index + 1;
            index = line.IndexOf('|', tail);
            width = GetStoredValues(line, index, tail);

            tail = index + 1;
            index = line.IndexOf('|', tail);
            var token = line.Substring(tail, (index - tail));
            topmost = (token.Contains(bool.TrueString)) ? true : false;

            tail = index + 1;
            index = line.IndexOf('|', tail);
            token = line.Substring(tail, (index - tail));
            backgroundColorIndex = int.Parse(token);

            tail = index + 1;
            index = line.IndexOf('|', tail); //line.Length;
            token = line.Substring(tail, (index - tail));
            foregroundColorIndex = int.Parse(token);

            tail = index + 1;
            index = line.IndexOf(':', tail);
            token = line.Substring(tail, (index - tail));
            var hours = int.Parse(token);

            tail = index + 1;
            index = line.IndexOf(':', tail);
            token = line.Substring(tail, (index - tail));
            var minutes = int.Parse(token);

            tail = index + 1;
            index = line.IndexOf('|', tail);
            token = line.Substring(tail, (index - tail));
            var seconds = (int) double.Parse(token);

            tail = index + 1;
            index = line.IndexOf('/', tail);
            token = line.Substring(tail, (index - tail));
            var month = int.Parse(token);

            tail = index + 1;
            index = line.IndexOf('/', tail);
            token = line.Substring(tail, (index - tail));
            var day = int.Parse(token);

            tail = index + 1;
            index = line.IndexOf('|', tail);
            token = line.Substring(tail, (index - tail));
            var year = int.Parse(token);


            alarmTime = new DateTime(year, month, day, hours, minutes, seconds);

            tail = index + 1;
            index = line.Length;
            loadRepeatNumber = line.Substring(tail, (index - tail));
        }

        private double GetStoredValues(string line, int index, int tail)
        {
            var token = line.Substring(tail, (index - tail));
            return double.Parse(token);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var contentIndicator = new string[ArrayPos];

            var index = 0;

            for (var i = 0; i < ArrayPos; i++)
            {
                if (WindowArray[i] != null)
                {
                    var w = ((Window2) (WindowArray[i]));
                    var rtb = ((RichTextBox) (((StackPanel) (w.Content)).Children[1]));
                    var tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

                    var mstream = new MemoryStream();
                    XamlWriter.Save(rtb.Document, mstream);
                    mstream.Seek(0, SeekOrigin.Begin);
                    var stringReader = new StreamReader(mstream);
                    var str = stringReader.ReadToEnd();
                    stringReader.Close();

                    var fs = new FileStream(index.ToString(), FileMode.Create);
                    var width = (w.ActualWidth > 50) ? (w.ActualWidth) : 200;
                    var height = (w.ActualHeight > 60) ? w.ActualHeight : 200;
                    if (str.Contains("payload"))
                    {
                        contentIndicator[index] = "0 |" + w.Top + "|" + w.Left + "|" +
                                                  height + "|" +
                                                  width + "|" + w.Topmost +
                                                  "|" + w.BackgroundColorIndex + "|" +
                                                  w.ForegroundColorIndex + "|" +
                                                  w.Alarm.TimeOfDay + "|" + w.Alarm.Date.ToShortDateString() +
                                                  "|" + w.RepetitionNumber;
                        tr.Save(fs, DataFormats.XamlPackage);
                    }
                    else
                    {
                        contentIndicator[index] = "1 |" + w.Top + "|" + w.Left + "|" +
                                                  height + "|" +
                                                  width + "|" + w.Topmost +
                                                  "|" + w.BackgroundColorIndex + "|" +
                                                  w.ForegroundColorIndex + "|" +
                                                  w.Alarm.TimeOfDay + "|" + w.Alarm.Date.ToShortDateString() +
                                                  "|" + w.RepetitionNumber;
                        tr.Save(fs, DataFormats.Xaml);
                    }
                    index++;
                    fs.Close();
                    ((Window2) (WindowArray[i])).Close();
                }
            }

            if (File.Exists("NotesFile"))
            {
                File.Delete("NotesFile");
            }
            var mainDataFile = new FileStream("NotesFile", FileMode.OpenOrCreate);
            var writer = new StreamWriter(mainDataFile);
            writer.WriteLine(index + "|" + AppWindow.Top + "|" + AppWindow.Left + "|" +
                             AppWindow.ActualHeight + "|" + AppWindow.ActualWidth + "|" +
                             AppWindow.Topmost + "|-1" + "|-1" + "|" + DateTime.Now.TimeOfDay +
                             "|" +
                             DateTime.Now.Date.ToShortDateString() + "|null");
            for (var i = 0; i < index; i++)
            {
                writer.WriteLine(contentIndicator[i]);
            }
            writer.Close();
            mainDataFile.Close();
        }

        private void AlarmTimer_Tick(object sender, EventArgs e)
        {
            var index = 0;
            _windowContents = new string[ArrayPos];
            for (var i = 0; i < ArrayPos; i++)
            {
                if (WindowArray[i] != null)
                {
                    var w = ((Window2) (WindowArray[i]));
                    StoreContents(index++);
                    CheckIfAlarmFires(w);
                }
            }

            WriteNotesFile(index);
        }

        private static void CheckIfAlarmFires(Window2 w)
        {
            var current = DateTime.Now;
            var m = LogicalTreeHelper.FindLogicalNode(w, "AlwaysOnTop") as MenuItem;
            var m1 = LogicalTreeHelper.FindLogicalNode(w, "alarm") as MenuItem;
            var m3 = LogicalTreeHelper.FindLogicalNode(w, "DismissAlarm") as MenuItem;
            var alarm = w.Alarm;

            var m2 =
                LogicalTreeHelper.FindLogicalNode(w, "AlarmIndicator") as Image;
            m2.Visibility = (alarm.CompareTo(DateTime.Now) >= 0) ? (Visibility.Visible) : (Visibility.Hidden);

            if (w.RepetitionNumber.Contains("null") == false)
            {
                if ((alarm.CompareTo(current) < 0) &&
                    (!(((current.TimeOfDay.Hours - alarm.TimeOfDay.Hours) < 8) &&
                       ((current.TimeOfDay.Hours - alarm.TimeOfDay.Hours) >= 0))
                     || !(current.Date.ToShortDateString().Contains(alarm.Date.ToShortDateString()))))
                {
                    var chArr = w.RepetitionNumber.ToCharArray(0, 1);
                    var val = w.RepetitionNumber.Substring(1);
                    var repetitionValue = int.Parse(val);
                    switch (chArr[0])
                    {
                        case 'W':
                            while (w.Alarm.Date.CompareTo(current.Date) < 0)
                            {
                                w.Alarm = w.Alarm.AddDays(7*repetitionValue);
                            }

                            break;

                        case 'D':
                            while (w.Alarm.Date.CompareTo(current.Date) < 0)
                            {
                                w.Alarm = w.Alarm.AddDays(repetitionValue);
                            }
                            break;

                        default:
                            break;
                    }
                    alarm = w.Alarm;
                }
            }

            if (alarm.Date == current.Date)
            {
                var temp1 = alarm;
                var temp2 = current;
                temp1 = temp1.AddMinutes(30);
                temp2 = temp2.AddMinutes(30);

                if ((temp2.CompareTo(alarm) >= 0) && (temp1.CompareTo(current) >= 0) && (w.AlarmDismissed == false))
                {
                    w.Topmost = true;
                    m.IsChecked = true;
                    m1.IsEnabled = true;
                    m3.IsEnabled = true;
                    m2.Visibility = Visibility.Visible;

                    w.BorderBrush = Brushes.Black;
                    w.BorderThickness = new Thickness(5);
                }
                else if (((current.TimeOfDay.Hours - alarm.TimeOfDay.Hours) < 8) &&
                         ((current.TimeOfDay.Hours - alarm.TimeOfDay.Hours) >= 0))
                {
                    w.Topmost = w.TopmostWindow;
                    m.IsChecked = w.Topmost;
                    m1.IsEnabled = (!w.RepetitionNumber.Contains("null"));
                    m2.Visibility = m1.IsEnabled ? (Visibility.Visible) : (Visibility.Hidden);
                    m3.IsEnabled = false;
                    w.BorderBrush = Brushes.Tomato;
                    w.BorderThickness = new Thickness(3);
                }
                else
                {
                    w.AlarmDismissed = false;
                    m3.IsEnabled = false;
                    m1.IsEnabled = (!w.RepetitionNumber.Contains("null"));
                    m2.Visibility = m1.IsEnabled ? (Visibility.Visible) : (Visibility.Hidden);
                }
            }
            else
            {
                w.Topmost = w.TopmostWindow;
                m.IsChecked = w.Topmost;
                m1.IsEnabled = (!w.RepetitionNumber.Contains("null"));
                m2.Visibility = m1.IsEnabled ? (Visibility.Visible) : (Visibility.Hidden);
                m3.IsEnabled = false;

                w.BorderBrush = Brushes.Transparent;
                w.BorderThickness = new Thickness(0);
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var w2 = new Window2
            {
                Title = "Sticky Note",
                Tag = ArrayPos
            };
            WindowArray[ArrayPos] = w2;
            ArrayPos++;
        }

        private void StoreContents(int index)
        {
            var w = ((Window2) (WindowArray[index]));
            var rtb = ((RichTextBox) (((StackPanel) (w.Content)).Children[1]));
            var tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            var mstream = new MemoryStream();
            XamlWriter.Save(rtb.Document, mstream);
            mstream.Seek(0, SeekOrigin.Begin);
            var stringReader = new StreamReader(mstream);
            var str = stringReader.ReadToEnd();
            stringReader.Close();

            var fs = new FileStream(index.ToString(), FileMode.Create);
            var width = (w.ActualWidth > 50) ? (w.ActualWidth) : 200;
            var height = (w.ActualHeight > 60) ? w.ActualHeight : 200;
            if (str.Contains("payload"))
            {
                _windowContents[index] = "0 |" + w.Top + "|" + w.Left + "|" + height +
                                         "|" +
                                         width + "|" + w.Topmost +
                                         "|" + w.BackgroundColorIndex + "|" +
                                         w.ForegroundColorIndex + "|" +
                                         w.Alarm.TimeOfDay + "|" + w.Alarm.Date.ToShortDateString() +
                                         "|" + w.RepetitionNumber;
                tr.Save(fs, DataFormats.XamlPackage);
            }
            else
            {
                _windowContents[index] = "1 |" + w.Top + "|" + w.Left + "|" + height +
                                         "|" +
                                         width + "|" + w.Topmost +
                                         "|" + w.BackgroundColorIndex + "|" +
                                         w.ForegroundColorIndex + "|" +
                                         w.Alarm.TimeOfDay + "|" + w.Alarm.Date.ToShortDateString() +
                                         "|" + w.RepetitionNumber;
                tr.Save(fs, DataFormats.Xaml);
            }
            fs.Close();
        }

        private void WriteNotesFile(int index)
        {
            if (File.Exists("NotesFile"))
            {
                File.Delete("NotesFile");
            }
            var mainDataFile = new FileStream("NotesFile", FileMode.Create);
            var writer = new StreamWriter(mainDataFile);
            writer.WriteLine(index + "|" + AppWindow.Top + "|" + AppWindow.Left + "|" +
                             AppWindow.ActualHeight + "|" + AppWindow.ActualWidth + "|" +
                             AppWindow.Topmost + "|-1" + "|-1" + "|" + DateTime.Now.TimeOfDay +
                             "|" +
                             DateTime.Now.Date.ToShortDateString() + "|null");
            for (var i = 0; i < index; i++)
            {
                writer.WriteLine(_windowContents[i]);
            }
            writer.Close();
        }

        // Sample event handler:  
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            AppWindow.Close();
        }

        private void cb_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            AppWindow.Topmost = (cb.IsChecked == true) ? true : false;
            AppWindow.Focus();
        }

        private void l_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cb.IsChecked = !cb.IsChecked;
            AppWindow.Topmost = (cb.IsChecked == true) ? true : false;
        }
    }
}