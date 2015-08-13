// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;

namespace UsingSpeech
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void VolumeChanged(object sender, RoutedEventArgs args)
        {
            _speechSynthesizer.Volume = (int) ((Slider) args.OriginalSource).Value;
        }

        private void RateChanged(object sender, RoutedEventArgs args)
        {
            _speechSynthesizer.Rate = (int) ((Slider) args.OriginalSource).Value;
        }

        private void ButtonEchoOnClick(object sender, RoutedEventArgs args)
        {
            _speechSynthesizer.SpeakAsync(TextToDisplay.Text);
        }

        private void ButtonDateOnClick(object sender, RoutedEventArgs args)
        {
            _speechSynthesizer.SpeakAsync("Today is " + DateTime.Now.ToShortDateString());
        }

        private void ButtonTimeOnClick(object sender, RoutedEventArgs args)
        {
            _speechSynthesizer.SpeakAsync("The time is " + DateTime.Now.ToShortTimeString());
        }

        private void ButtonNameOnClick(object sender, RoutedEventArgs args)
        {
            _speechSynthesizer.SpeakAsync("My name is " + _speechSynthesizer.Voice.Name);
        }
    }
}