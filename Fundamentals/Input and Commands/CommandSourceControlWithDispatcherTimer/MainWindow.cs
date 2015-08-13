// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommandSourceControlWithDispatcherTimer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand CustomCommand = new RoutedCommand();
        //  The timer.
        private readonly DispatcherTimer _dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        //  System.Windows.Threading.DispatcherTimer.Tick handler
        //
        //  Updates the current seconds display and calls
        //  InvalidateRequerySuggested on the CommandManager to force 
        //  the Command to raise the CanExecuteChanged event.
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            lblSeconds.Content = DateTime.Now.Second;

            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
        }

        //  Executed Event Handler
        //
        //  Updates the output TextBox with the current seconds 
        //  and the target second, which is passed through Args.Parameter.
        private void CustomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            txtResults.Text = "Command Executed at " +
                              DateTime.Now.Second + " seconds after the minute \n\n" +
                              "The target second is set to " +
                              e.Parameter;
        }

        //CanExecute Event Handler
        //
        //  Retrun True, if the current seconds are greater than the target value
        //  which is set on the Slider that is defined in the XMAL file.
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

        // Moves the slider when the mouse wheel is rotated.
        private void OnSliderMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var source = e.Source as Slider;

            if (source != null)
            {
                if (e.Delta > 0)
                {
                    //execute the Slider DecreaseSmall RoutedCommand
                    //the slider.value propety is passed as the command parameter
                    Slider.DecreaseSmall.Execute(
                        source.Value, source);
                }
                else
                {
                    //execute the Slider IncreaseSmall RoutedCommand
                    //the slider.value propety is passed as the command parameter
                    Slider.IncreaseSmall.Execute(
                        source.Value, source);
                }
            }
        }

        // Moves the slider when the mouse extended buttons are pressed.
        private void OnSliderMouseUp(object sender, MouseButtonEventArgs e)
        {
            var source = e.Source as Slider;

            if (source != null)
            {
                if (e.ChangedButton == MouseButton.XButton1)
                {
                    //  Execute the Slider DecreaseSmall RoutedCommand
                    //  The slider.value propety is passed as the command parameter
                    Slider.DecreaseSmall.Execute(
                        source.Value, source);
                }
                if (e.ChangedButton == MouseButton.XButton2)
                {
                    //  Execute the Slider IncreaseSmall RoutedCommand
                    //  The slider.value propety is passed as the command parameter
                    Slider.IncreaseSmall.Execute(
                        source.Value, source);
                }
            }
        }
    }

    //Converter to convert the Slider value property from a Double to an Int
}