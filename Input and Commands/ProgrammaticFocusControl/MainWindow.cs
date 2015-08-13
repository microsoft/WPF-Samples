// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgrammaticFocusControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The direction to move/predict focus.
        private FocusNavigationDirection _focusMoveValue;
        // Used to keep track of when a PredictFocus has happened.
        private bool _focusPredicted;
        private Control _predictedControl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Sets keyboard focus on the first Button in the sample.
            Keyboard.Focus(firstButton);
        }

        private void OnPredictFocus(object sender, RoutedEventArgs e)
        {
            DependencyObject predictionElement = null;

            var elementWithFocus = Keyboard.FocusedElement as UIElement;

            if (elementWithFocus != null)
            {
                // Only these four directions are currently supported
                // by PredictFocus, so we need to filter on these only.
                if ((_focusMoveValue == FocusNavigationDirection.Up) ||
                    (_focusMoveValue == FocusNavigationDirection.Down) ||
                    (_focusMoveValue == FocusNavigationDirection.Left) ||
                    (_focusMoveValue == FocusNavigationDirection.Right))
                {
                    // Get the element which would receive focus if focus were changed.
                    predictionElement = elementWithFocus.PredictFocus(_focusMoveValue);

                    var controlElement = predictionElement as Control;

                    // If a ContentElement.
                    if (controlElement != null)
                    {
                        controlElement.Foreground = Brushes.DarkBlue;
                        controlElement.FontSize += 10;
                        controlElement.FontWeight = FontWeights.ExtraBold;

                        // Fields used to reset the UI when the mouse 
                        // button is released.
                        _focusPredicted = true;
                        _predictedControl = controlElement;
                    }
                }
            }
        }

        private void OnMoveFocus(object sender, RoutedEventArgs e)
        {
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            var focusDirection = _focusMoveValue;

            // MoveFocus takes a TraveralReqest as its argument.
            var request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            var elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            elementWithFocus?.MoveFocus(request);
        }

        // Sets the FocusNavigationDirection.
        private void OnFocusSelected(object sender, RoutedEventArgs e)
        {
            var source = e.Source as RadioButton;

            if (source != null)
            {
                _focusMoveValue = (FocusNavigationDirection) Enum.Parse(
                    typeof (FocusNavigationDirection), (string) source.Content);
            }
        }

        // Resets the UI after PredictFocus changes the UI.
        private void OnPredictFocusMouseUp(object sender, RoutedEventArgs e)
        {
            if (_focusPredicted)
            {
                _predictedControl.Foreground = Brushes.Black;
                _predictedControl.FontSize -= 10;
                _predictedControl.FontWeight = FontWeights.Normal;

                _focusPredicted = false;
            }
        }
    }
}