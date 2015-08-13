// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommandSourceControlUsingSystemTimer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand CustomCommand = new RoutedCommand();
        // The timer. 

        public MainWindow()
        {
            InitializeComponent();

            // System Timer setup.
            var timer = new Timer();
            timer.Elapsed += timer_Elapsed;
            timer.Interval = 1000;
            timer.Enabled = true;
        }

        // System.Timers.Timer.Elapsed handler
        // Places the delegate onto the UI Thread's Dispatcher
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Place delegate on the Dispatcher.
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new TimerDispatcherDelegate(TimerWorkItem));
        }

        // Method to place onto the UI thread's dispatcher.
        // Updates the current second display and calls 
        // InvalidateRequerySuggested on the CommandManager to force the
        // Command to raise the CanExecuteChanged event.
        private void TimerWorkItem()
        {
            // Update current second display.
            lblSeconds.Content = DateTime.Now.Second;

            // Forcing the CommandManager to raie the RequerySuggested event.
            CommandManager.InvalidateRequerySuggested();
        }

        // Executed Event Handler.
        //
        // Updates the result TextBox with the current seconds and the 
        // target second, which is the value passed from the command source.
        private void CustomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            txtResults.Text = "Command Executed at " +
                              DateTime.Now.Second + " seconds after the minute \n\n" +
                              "The target second is set to " +
                              e.Parameter;
        }

        // CanExecute Event Handler.
        //
        // True if the current seconds are greater than the target value that is 
        // set on the Slider, which is defined in the XMAL file.
        private void CustomCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (secondSlider != null)
            {
                e.CanExecute = DateTime.Now.Second > secondSlider.Value;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void OnSliderMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var source = e.Source as Slider;
            if (source != null)
            {
                if (e.Delta > 0)
                {
                    // Execute the Slider DecreaseSmall RoutedCommand.
                    // The slider.value propety is passed as the command parameter.
                    Slider.DecreaseSmall.Execute(
                        source.Value, source);
                }
                else
                {
                    // Execute the Slider IncreaseSmall RoutedCommand.
                    // The slider.value propety is passed as the command parameter.
                    Slider.IncreaseSmall.Execute(
                        source.Value, source);
                }
            }
        }

        // Moves the slider when the mouse extended buttons are pressed
        private void OnSliderMouseUp(object sender, MouseButtonEventArgs e)
        {
            var source = e.Source as Slider;

            if (source != null)
            {
                if (e.ChangedButton == MouseButton.XButton1)
                {
                    // Execute the Slider DecreaseSmall RoutedCommand.
                    // The slider.value propety is passed as the command parameter.
                    Slider.DecreaseSmall.Execute(
                        source.Value, source);
                }
                if (e.ChangedButton == MouseButton.XButton2)
                {
                    // Execute the Slider IncreaseSmall RoutedCommand.
                    // The slider.value propety is passed as the command parameter.
                    Slider.IncreaseSmall.Execute(
                        source.Value, source);
                }
            }
        }

        // Delegate for thr worker item to place on the Dispatcher
        private delegate void TimerDispatcherDelegate();
    }

    // Converter to convert the Slider value property to an int32.
}