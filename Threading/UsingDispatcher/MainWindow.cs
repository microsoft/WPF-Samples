// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace UsingDispatcher
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard _hideClockFaceStoryboard;
        private Storyboard _hideWeatherImageStoryboard;
        // Storyboards for the animations.
        private Storyboard _showClockFaceStoryboard;
        private Storyboard _showWeatherImageStoryboard;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load the storyboard resources.
            _showClockFaceStoryboard =
                (Storyboard) Resources["ShowClockFaceStoryboard"];
            _hideClockFaceStoryboard =
                (Storyboard) Resources["HideClockFaceStoryboard"];
            _showWeatherImageStoryboard =
                (Storyboard) Resources["ShowWeatherImageStoryboard"];
            _hideWeatherImageStoryboard =
                (Storyboard) Resources["HideWeatherImageStoryboard"];
        }

        private void ForecastButtonHandler(object sender, RoutedEventArgs e)
        {
            // Change the status image and start the rotation animation.
            fetchButton.IsEnabled = false;
            fetchButton.Content = "Contacting Server";
            weatherText.Text = "";
            _hideWeatherImageStoryboard.Begin(this);

            // Start fetching the weather forecast asynchronously.
            var fetcher = new NoArgDelegate(
                FetchWeatherFromServer);

            fetcher.BeginInvoke(null, null);
        }

        private void FetchWeatherFromServer()
        {
            // Simulate the delay from network access.
            Thread.Sleep(4000);

            // Tried and true method for weather forecasting - random numbers.
            var rand = new Random();
            string weather;

            weather = rand.Next(2) == 0 ? "rainy" : "sunny";

            // Schedule the update function in the UI thread.
            tomorrowsWeather.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new OneArgDelegate(UpdateUserInterface),
                weather);
        }

        private void UpdateUserInterface(string weather)
        {
            //Set the weather image
            if (weather == "sunny")
            {
                weatherIndicatorImage.Source = (ImageSource) Resources[
                    "SunnyImageSource"];
            }
            else if (weather == "rainy")
            {
                weatherIndicatorImage.Source = (ImageSource) Resources[
                    "RainingImageSource"];
            }

            //Stop clock animation
            _showClockFaceStoryboard.Stop(this);
            _hideClockFaceStoryboard.Begin(this);

            //Update UI text
            fetchButton.IsEnabled = true;
            fetchButton.Content = "Fetch Forecast";
            weatherText.Text = weather;
        }

        private void HideClockFaceStoryboard_Completed(object sender,
            EventArgs args)
        {
            _showWeatherImageStoryboard.Begin(this);
        }

        private void HideWeatherImageStoryboard_Completed(object sender,
            EventArgs args)
        {
            _showClockFaceStoryboard.Begin(this, true);
        }

        private delegate void NoArgDelegate();

        private delegate void OneArgDelegate(string arg);
    }
}