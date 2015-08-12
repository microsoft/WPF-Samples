// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace FindingElementInPanel
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _cCounter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FindIndex(object sender, RoutedEventArgs e)
        {
            _cCounter += 1;
            // Create a new Text element.
            var newText = new TextBlock();
            // Add this element to the UIElementCollection of the DockPanel element.
            MainDisplayPanel.Children.Add(newText);
            // Add a text node under the Text element. This text is displayed. 
            newText.Text = "New element #" + _cCounter;
            DockPanel.SetDock(newText, Dock.Top);
            // Display the Index number of the new element.    
            TxtDisplay.Text = "The Index of the new element is " + MainDisplayPanel.Children.IndexOf(newText);
        }
    }
}