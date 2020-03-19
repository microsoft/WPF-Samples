using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpellChecking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static IReadOnlyList<string> GetSuggestions(string word)
        {
            TextBox textBox = new TextBox()
            { 
                Text = word
            };
            
            textBox.SpellCheck.IsEnabled = true;

            return textBox.GetSpellingError(0).Suggestions.ToList().AsReadOnly();
        }

        private void OnCheckWordForSpellingSuggestions(object sender, RoutedEventArgs e)
        {
            var word = txtBoxWord.Text;
            if (!string.IsNullOrEmpty(word))
            {
                var suggestions = GetSuggestions(word);
                if (suggestions?.Count > 0)
                {
                    lbSuggestions.ItemsSource = suggestions;
                }
            }
        }
    }
}
