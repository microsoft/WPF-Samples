// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

// EventArgs
// Window, MessageBoxXxx, RoutedEventArgs
// TextBox, TextChangedEventArgs

// Regex

namespace DialogBox
{
    public partial class FindDialogBox : Window
    {
        // Text to search
        private readonly TextBox _textBoxToSearch;
        // Find results
        private MatchCollection _matches;
        private int _matchIndex;

        public FindDialogBox(TextBox textBoxToSearch)
        {
            InitializeComponent();

            _textBoxToSearch = textBoxToSearch;

            // If text box that's being searched is changed, reset search
            _textBoxToSearch.TextChanged += textBoxToSearch_TextChanged;
        }

        // Search results

        public int Index { get; set; }
        public int Length { get; set; }
        public event TextFoundEventHandler TextFound;

        protected virtual void OnTextFound()
        {
            var textFound = TextFound;
            textFound?.Invoke(this, EventArgs.Empty);
        }

        private void findNextButton_Click(object sender, RoutedEventArgs e)
        {
            // Find matches
            if (_matches == null)
            {
                var pattern = findWhatTextBox.Text;

                // Match whole word?
                if ((bool) matchWholeWordCheckBox.IsChecked) pattern = @"(?<=\W{0,1})" + pattern + @"(?=\W)";

                // Case sensitive
                if (!(bool) caseSensitiveCheckBox.IsChecked) pattern = "(?i)" + pattern;

                // Find matches
                _matches = Regex.Matches(_textBoxToSearch.Text, pattern);
                _matchIndex = 0;

                // Word not found?
                if (_matches.Count == 0)
                {
                    MessageBox.Show("'" + findWhatTextBox.Text + "' not found.", "Find");
                    _matches = null;
                    return;
                }
            }

            // Start at beginning of matches if the last find selected the last match
            if (_matchIndex == _matches.Count)
            {
                var result = MessageBox.Show("Nmore matches found. Start at beginning?", "Find",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                // Reset
                _matchIndex = 0;
            }

            // Return match details to client so it can select the text
            var match = _matches[_matchIndex];
            if (TextFound != null)
            {
                // Text found
                Index = match.Index;
                Length = match.Length;
                OnTextFound();
            }
            _matchIndex++;
        }

        private void textBoxToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResetFind();
        }

        private void findWhatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResetFind();
        }

        private void criteria_Click(object sender, RoutedEventArgs e)
        {
            ResetFind();
        }

        private void ResetFind()
        {
            findNextButton.IsEnabled = true;
            _matches = null;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            // Close dialog box
            Close();
        }
    }
}

//</SnippetFindDialogCloseCODEBEHIND2>