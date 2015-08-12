// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyStrokeCounter
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _numberOfHits;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var source = e.Source as TextBox;

            if (source != null)
            {
                // Change the TextBox color when it obtains focus.
                source.Background = Brushes.LightBlue;

                // Clear the TextBox.
                source.Clear();
            }
        }

        private void TextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var source = e.Source as TextBox;

            if (source != null)
            {
                // Change the TextBox color when it loses focus.
                source.Background = Brushes.White;

                // Set the  hit counter back to zero and updates the display.
                ResetCounter();
            }
        }

        public void ResetCounter()
        {
            _numberOfHits = 0;
            lblNumberOfTargetHits.Content = _numberOfHits;
        }

        // Compares the key which was pressed with a target key.
        // If they are the same, updates a label which keeps track
        // of the number of times the target key has been pressed.
        private void SourceTextKeyDown(object sender, KeyEventArgs e)
        {
            // The key converter.
            var converter = new KeyConverter();
            var target = Key.None;

            // Verifying there is only one character in the string.
            if (txtTargetKey.Text.Length == 1)
            {
                // Converting the string to a Key.
                target = (Key) converter.ConvertFromString(txtTargetKey.Text);
            }

            // If the pressed key is equal to the target key. 
            if (e.Key == target)
            {
                // Incrementing  the number of hits, and updating
                // the label which displays the number of hits.
                _numberOfHits++;
                lblNumberOfTargetHits.Content = _numberOfHits;
            }
        }
    }
}