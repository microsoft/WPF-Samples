// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ICommandSourceImplementation
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand FontUpdateCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
        }

        // The ExecutedRoutedEvent Handler
        // If the command target is the TextBox, changes the fontsize to that
        // Of the value passed in through the Command Parameter
        private void SliderUpdateExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var source = sender as TextBox;

            if (source != null)
            {
                if (e.Parameter != null)
                {
                    try
                    {
                        if ((int) e.Parameter > 0 && (int) e.Parameter <= 60)
                        {
                            source.FontSize = (int) e.Parameter;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("in Command \n Parameter: " + e.Parameter);
                    }
                }
            }
        }

        // The CanExecuteRoutedEvent Handler
        // If the Command Source is a TextBox, then set CanExecute to true;
        // Otherwise, set it to false
        private void SliderUpdateCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var source = sender as TextBox;

            if (source != null)
            {
                e.CanExecute = !source.IsReadOnly;
            }
        }

        // If the Readonly Box is checked, we need to force the CommandManager
        // To raise the RequerySuggested event.  This will cause the Command Source
        // To query the command to see if it can execute or not.
        private void OnReadOnlyChecked(object sender, RoutedEventArgs e)
        {
            if (txtBoxTarget != null)
            {
                txtBoxTarget.IsReadOnly = true;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // If the Readonly Box is checked, we need to force the CommandManager
        // To raise the RequerySuggested event.  This will cause the Command Source
        // To query the command to see if it can execute or not.
        private void OnReadOnlyUnChecked(object sender, RoutedEventArgs e)
        {
            if (txtBoxTarget != null)
            {
                txtBoxTarget.IsReadOnly = false;
                UpdateLayout();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // CanExecute handler for the IncreaseSamll and DescreaseSmall commands.
        // Disables the command sources if the Slider is disabled.  
        private void IncreaseDecreaseCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var target = e.Source as Slider;
            if (target != null)
            {
                if (target.IsEnabled)
                {
                    e.CanExecute = true;
                    e.Handled = true;
                }
                else
                {
                    e.CanExecute = false;
                    e.Handled = true;
                }
            }
        }
    }


    // Converter to convert the Slider value property to an int.

    // Converter to convert the Slider value property to an int.
}
