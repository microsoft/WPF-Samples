// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace StickyNotesDemo
{
    /// <summary>
    ///     Interaction logic for MailSettingsDialog.xaml
    /// </summary>
    public class MailSettingsDialog : Window
    {
        private readonly DispatcherTimer _timerClock;

        public MailSettingsDialog()
        {
            WindowStyle = WindowStyle.None;
            Height = 250;
            Width = 600;
            Title = "SetUp Email Server";
            BorderBrush = Brushes.Transparent;
            BorderThickness = new Thickness(0);
            Background = Brushes.Transparent;


            var sp = new StackPanel {Background = Brushes.Transparent};

            var b = new Button
            {
                Background = Window2.ChangeBackgroundColor(Colors.SkyBlue),
                Height = 245
            };

            var tb = new TextBlock
            {
                Background = Brushes.Transparent,
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Bickham Script Pro")
            };
            tb.Typography.StylisticSet1 = true;
            tb.FontSize = 50;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = "Enter Mail Server Info In \r\nPreferences Window";

            b.Content = tb;
            b.Click += b_Click;

            sp.Children.Add(b);

            _timerClock = _timerClock = new DispatcherTimer();
            _timerClock.Interval = new TimeSpan(0, 0, 5);
            _timerClock.IsEnabled = true;
            _timerClock.Tick += TimerClock_Tick;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Content = sp;
            Closing += MailSettingsDialog_Closing;
            ShowDialog();
        }

        private void MailSettingsDialog_Closing(object sender, CancelEventArgs e)
        {
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TimerClock_Tick(object sender, EventArgs e)
        {
            _timerClock.IsEnabled = false;
            Close();
        }
    }
}