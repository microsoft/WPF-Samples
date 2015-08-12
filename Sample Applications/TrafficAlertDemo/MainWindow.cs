// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;

namespace TrafficAlertDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //static void label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Label label = sender as Label;
        //        DockPanel dp = label.Parent as DockPanel;
        //        Uri url = new Uri(dp.Tag.ToString());
        //        NavigationWindow nw = new NavigationWindow();
        //        nw.Source = url;
        //        nw.Show();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private DispatcherTimer _timerClock;
        private string _userZipCodeString = "";
        private Button _b;
        private TextBox _tb;
        private Window _w;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _w = new Window();
            var sp = new StackPanel();
            var wp = new WrapPanel();
            _tb = new TextBox {MaxLength = 6};

            var l = new Label
            {
                Foreground = Brushes.White,
                Content = "Enter ZipCode (OnlyDigits):"
            };

            wp.Children.Add(l);
            wp.Children.Add(_tb);
            wp.Background = Brushes.Transparent;

            _b = new Button {Content = "OK"};
            _b.Click += b_Click;

            var gs1 = new GradientStop(Colors.Red, 0);
            var gs2 = new GradientStop(Colors.Yellow, 1);
            var gsc = new GradientStopCollection {gs1, gs2};
            var lgb = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = gsc
            };
            _b.Background = lgb;
            _b.HorizontalAlignment = HorizontalAlignment.Center;
            _b.Margin = new Thickness(4);

            sp.Children.Add(wp);
            sp.Children.Add(_b);
            sp.Background = Brushes.Beige;

            var gs01 = new GradientStop(Colors.Red, 0);
            var gs02 = new GradientStop(Colors.Black, 1);
            var gsc1 = new GradientStopCollection {gs01, gs02};
            var lgb1 = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = gsc1
            };
            sp.Background = lgb1;

            _w.Content = sp;
            _w.Height = 100;
            _w.Width = 350;
            _w.WindowStyle = WindowStyle.ToolWindow;
            _w.Show();
            _tb.Focus();
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var val = int.Parse(_tb.Text);
                _w.Close();

                // w1.Height = 600;
                w1.Height = 700;
                tv.Height = 600;

                _userZipCodeString = "http://maps.yahoo.com/traffic.rss?csz=" + val + "&mag=5&minsev=1";
                tv.Items.Clear();
                tv.MinHeight = 200;
                GetRsSfeed();
                w1.Width = (tv.ActualWidth > 400) ? tv.ActualWidth : 400;
            }
            catch (FormatException)
            {
                _tb.Text = "";
                _w.Title = "Please Input Only numbers";
            }
        }

        // To use Loaded event put Loaded="WindowLoaded" attribute in root element of .xaml file.
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            GetRsSfeed();
            _timerClock = _timerClock = new DispatcherTimer();
            _timerClock.Interval = new TimeSpan(1, 0, 0);
            _timerClock.IsEnabled = true;
            _timerClock.Tick += TimerClock_Tick;
        }

        private void TimerClock_Tick(object sender, EventArgs e)
        {
            tv.Items.Clear();
            GetRsSfeed();
        }

        private void tv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // if (count++ != 0)
            {
                //if ((tv.ActualHeight ) > 600)
                //{
                //    tv.Height = 600;
                //}
                //    w1.Height = tv.ActualHeight + 100;
                tv.Height = 600;
                w1.Height = 700;
                if (sv.ViewportWidth <= sv.ExtentWidth)
                {
                    w1.Width = tv.ActualWidth + 50;
                }
                w1.Width = (tv.ActualWidth > w1.Width) ? (tv.ActualWidth + 20) : w1.Width;
            }
        }

        private void tv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            w1.Height = tv.ActualHeight + 50;
        }

        private void GetRsSfeed()
        {
            var url = "http://maps.yahoo.com/traffic.rss?csz=98052&mag=5&minsev=1";
            if (_userZipCodeString != "")
            {
                url = _userZipCodeString;
            }
            var req = WebRequest.Create(url);
            var res = req.GetResponse();

            var rsstream = res.GetResponseStream();
            var rssdoc = new XmlDataDocument();

            rssdoc.Load(rsstream);

            var rssitems = rssdoc.SelectNodes("rss/channel/item");
            var title = "";
            var description = "";
            var link = "";
            var category = "";
            var severity = "";

            for (var i = 0; i < rssitems.Count; i++)
            {
                XmlNode rssdetail;

                rssdetail = rssitems.Item(i).SelectSingleNode("title");
                if (rssdetail != null)
                {
                    title = rssdetail.InnerText;

                    rssdetail = rssitems.Item(i).SelectSingleNode("description");
                    description = rssdetail.InnerText;

                    rssdetail = rssitems.Item(i).SelectSingleNode("link");


                    link = rssdetail?.InnerText ?? "";

                    rssdetail = rssitems.Item(i).SelectSingleNode("category");
                    category = rssdetail?.InnerText ?? "";

                    rssdetail = rssitems.Item(i).SelectSingleNode("severity");
                    severity = rssdetail?.InnerText ?? "";

                    PopulateList(title, description, link, category, severity);
                }
                else
                {
                    title = "";
                }
            }

            try
            {
                rssitems = rssdoc.SelectNodes("rss/channel");
                XmlNode rssdetail1;
                rssdetail1 = rssitems.Item(0).SelectSingleNode("title");
                title = rssdetail1.InnerText;
                var index = title.IndexOf("--", StringComparison.Ordinal);
                var str = (index > -1) ? (title.Substring(index + 3)) : "Wrong Zip Code";
                Location.Content = str;
            }
            catch (Exception)
            {
            }
        }

        private void PopulateList(string title, string description, string link, string category, string severity)
        {
            title = title.Replace("\n", "");
            var index = title.IndexOf(", On ", 0, StringComparison.Ordinal);
            var str = (index > -1) ? title.Substring(index + 5) : "NA";

            string[] charr = {" At "};
            var strarray = str.Split(charr, StringSplitOptions.None);
            str = strarray[0];

            var count = tv.Items.Count;
            var found = false;
            var counter = 0;
            while ((found == false) && (counter < count))
            {
                if (((TreeViewItem) (tv.Items[counter])).Header.ToString() == str)
                {
                    found = true;
                    var dp = new DockPanel();
                    CreateDockPanelForTreeViewItem(title, description, severity, dp, category, link);
                    ((TreeViewItem) (tv.Items[counter])).Items.Add(dp);
                }
                counter++;
            }
            if (found == false)
            {
                var tvItem = new TreeViewItem
                {
                    Header = str,
                    Foreground = Brushes.White
                };
                var dp = new DockPanel();
                CreateDockPanelForTreeViewItem(title, description, severity, dp, category, link);
                tvItem.Items.Add(dp);
                tvItem.Expanded += tvItem_Expanded;
                tvItem.Collapsed += tvItem_Collapsed;
                tvItem.LostFocus += tvItem_LostFocus;
                tv.Items.Add(tvItem);
            }
        }

        private void tvItem_Collapsed(object sender, RoutedEventArgs e)
        {
            var tvItem = sender as TreeViewItem;
            tvItem.Focus();
        }

        private void tvItem_Expanded(object sender, RoutedEventArgs e)
        {
            var tvItem = sender as TreeViewItem;
            tvItem.Focus();
        }

        private void tvItem_LostFocus(object sender, RoutedEventArgs e)
        {
            var tvItem = sender as TreeViewItem;
            tvItem.IsSelected = false;
        }

        private void CreateDockPanelForTreeViewItem(string title, string description, string severity, DockPanel dp,
            string category, string link)
        {
            var tb = CreateNewLabel(title, severity);
            dp.Children.Add(GetImage(severity, category, tb.FontSize));

            var tp = new ToolTip
            {
                Background = Brushes.Wheat,
                Padding = new Thickness(4),
                BorderBrush = Brushes.Gray
            };

            var sp = new StackPanel {Width = 250};

            description = description.Replace("\n", "");
            var strArr = new string[6];
            string[] splitArr = {"From Milepost", "Severity:", "Started: ", "Estimated End: ", "Last Updated: "};
            var _description = description;
            for (var i = splitArr.Length - 1; i >= 0; i--)
            {
                string[] temp1 = {splitArr[i]};
                var strArr1 = _description.Split(temp1, StringSplitOptions.None);
                var index = _description.IndexOf(splitArr[i], StringComparison.Ordinal);
                _description = (index > -1) ? _description.Substring(0, index) : _description;
                strArr[i + 1] = (strArr1.Length > 1) ? strArr1[1] : null;
                if (strArr1.Length > 1)
                {
                    strArr[0] = strArr1[0];
                }
            }
            var top = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Background = Brushes.LightSteelBlue
            };
            top.FontSize = top.FontSize - 2;
            top.Text = title;

            var bottom = new TextBlock {TextWrapping = TextWrapping.Wrap};
            bottom.FontSize = bottom.FontSize - 3;
            bottom.Background = Brushes.Wheat;

            bottom.Inlines.Add(strArr[0] + "\r\n");

            for (var i = 1; i < strArr.Length; i++)
            {
                if (strArr[i] != null)
                {
                    bottom.Inlines.Add(new Bold(new Run(splitArr[i - 1])));
                    bottom.Inlines.Add(strArr[i] + "\r\n");
                }
            }

            sp.Children.Add(top);
            sp.Children.Add(bottom);

            tp.Content = sp;
            tb.ToolTip = tp;

            ToolTipService.SetInitialShowDelay(tb, 600);
            ToolTipService.SetBetweenShowDelay(tb, 600);
            ToolTipService.SetShowDuration(tb, 30000);


            ((ToolTip) (tb.ToolTip)).Opened += Window1_Opened;
            dp.Children.Add(tb);
            dp.Background = Brushes.Green;
            dp.Tag = link;
            dp.GotFocus += dp_GotFocus;
            dp.LostFocus += dp_LostFocus;
            dp.Focusable = true;
        }

        private void dp_LostFocus(object sender, RoutedEventArgs e)
        {
            var dp = sender as DockPanel;
            ((ToolTip) (((Label) (dp.Children[1])).ToolTip)).IsOpen = false;
        }

        private void dp_GotFocus(object sender, RoutedEventArgs e)
        {
            var dp = sender as DockPanel;
            ((Label) (dp.Children[1])).Focus();
            if (e.Source.ToString().Contains("System.Windows.Controls.Label"))
            {
            }
            else
            {
                ((ToolTip) (((Label) (dp.Children[1])).ToolTip)).IsOpen = true;
            }
        }

        private void Window1_Opened(object sender, RoutedEventArgs e)
        {
            var tp = sender as ToolTip;
            tp.StaysOpen = true;
        }

        private static void SetColorCodeForLabel(string severity, Label label)
        {
            switch (severity)
            {
                case "Minor":
                case "Moderate":
                    var lgb = SetGradientColorHelper(Colors.Yellow);
                    label.Background = lgb;
                    label.Tag = "moderate";
                    break;

                case "Major":
                    lgb = SetGradientColorHelper(Colors.Orange);
                    label.Background = lgb;
                    label.Tag = "major";
                    break;

                case "Critical":
                    lgb = SetGradientColorHelper(Colors.OrangeRed);
                    label.Background = lgb;
                    label.Tag = "critical";
                    break;

                default:
                    break;
            }
        }

        private static LinearGradientBrush SetGradientColorHelper(Color color)
        {
            var gs1 = new GradientStop(color, 0);
            var gs2 = new GradientStop(Colors.WhiteSmoke, 1);
            var gsc = new GradientStopCollection {gs1, gs2};
            var lgb = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1.10),
                GradientStops = gsc
            };
            return lgb;
        }

        private Image GetImage(string severity, string category, double height)
        {
            switch (severity)
            {
                case "Minor":
                case "Moderate":
                    category = category.Trim();
                    if (category == "Construction")
                    {
                        return CreateImage(@"sampleImages\construction.gif", height);
                    }
                    return CreateImage(@"sampleImages\minor.gif", height);
                case "Major":
                    return CreateImage(@"sampleImages\moderate.gif", height);
                case "Critical":
                    return CreateImage(@"sampleImages\severe.gif", height);
                default:
                    return CreateImage(@"sampleImages\green.gif", height);
            }
        }

        private static Image CreateImage(string uri, double width)
        {
            var simpleImage = new Image
            {
                Width = 200,
                Margin = new Thickness(0)
            };
            var bi = new BitmapImage(new Uri(uri, UriKind.Relative));

            //set image source
            simpleImage.Source = bi;
            simpleImage.Width = width;
            return simpleImage;
        }

        private static Label CreateNewLabel(string title, string severity)
        {
            var label = new Label
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            // label.Content = title;
            var hl = new Hyperlink(new Run(title)) {Foreground = label.Foreground};


            label.Content = title;
            label.IsEnabled = true;
            label.Focusable = true;
            label.Cursor = Cursors.Hand;
            //System.Windows.Input.CursorType.Hand;

            //label.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(label_MouseDoubleClick);
            label.GotFocus += label_GotFocus;
            label.LostFocus += label_LostFocus;
            label.MouseEnter += label_MouseEnter;
            label.MouseLeave += label_MouseLeave;
            label.MouseLeftButtonUp += label_MouseLeftButtonUp;
            SetColorCodeForLabel(severity, label);
            return label;
        }

        private static void label_MouseLeave(object sender, MouseEventArgs e)
        {
            var label = sender as Label;
            ((ToolTip) (label.ToolTip)).IsOpen = false;
        }

        private static void label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var label = sender as Label;
                var dp = label.Parent as DockPanel;
                var url = new Uri(dp.Tag.ToString());
                var nw = new NavigationWindow {Source = url};
                nw.Show();
            }
            catch (Exception)
            {
            }
        }

        private static void label_MouseEnter(object sender, MouseEventArgs e)
        {
            var label = sender as Label;
            label.Focus();
        }

        private static void label_LostFocus(object sender, RoutedEventArgs e)
        {
            var label = sender as Label;
            ((ToolTip) (label.ToolTip)).IsOpen = false;
            if (label.Tag != null)
            {
                label.Background = label.Foreground;
                label.Foreground = Brushes.Black;
            }
            else
            {
                label.Background = Brushes.Green;
                label.Foreground = Brushes.Black;
            }
        }

        private static void label_GotFocus(object sender, RoutedEventArgs e)
        {
            var label = sender as Label;
            ((TreeView) (((TreeViewItem) ((DockPanel) (label.Parent)).Parent).Parent)).Items.MoveCurrentTo(
                ((TreeViewItem) ((DockPanel) (label.Parent)).Parent));
            ((TreeView) (((TreeViewItem) ((DockPanel) (label.Parent)).Parent).Parent)).Items.Refresh();
            ((ToolTip) (label.ToolTip)).Placement = PlacementMode.Right;
            ((ToolTip) (label.ToolTip)).PlacementTarget = label;


            label.Foreground = label.Background;
            if (label.Tag != null)
            {
                label.Background = Brushes.Gray;
            }
            else
            {
                label.Background = Brushes.Yellow;
                label.Foreground = Brushes.Black;
            }
        }
    }
}