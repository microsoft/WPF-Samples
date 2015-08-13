// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;

namespace CustomClassesWithDP
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shirt _myShirt;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnInit(object sender, EventArgs e)
        {
            _myShirt = new Shirt();
            PopulateListbox(ShirtChoice, Enum.GetValues(typeof (ShirtTypes)));
            PopulateListbox(ShirtColorChoice, Enum.GetValues(typeof (ShirtColors)));
            PopulateListbox(ButtonChoice, Enum.GetValues(typeof (ButtonColors)));
            _myShirt.ButtonColorChanged += UiButtonColorChanged;
        }

        private void PopulateListbox(FrameworkElement fe, Array values)
        {
            var lb = fe as ListBox;
            for (var j = 1; j < values.Length; j++)
            {
                var lbi = new ListBoxItem {Content = (Enum) values.GetValue(j)};
                lb.Items.Add(lbi);
            }
        }

        private void ChooseShirt(object sender, SelectionChangedEventArgs e)
        {
            var lbi = ((e.Source as ListBox).SelectedItem as ListBoxItem);
            _myShirt.ShirtType = (ShirtTypes) lbi.Content;
        }

        private void ChooseShirtColor(object sender, SelectionChangedEventArgs e)
        {
            var lbi = ((e.Source as ListBox).SelectedItem as ListBoxItem);
            _myShirt.ShirtColor = (ShirtColors) lbi.Content;
        }

        private void ChooseButtonColor(object sender, SelectionChangedEventArgs e)
        {
            var lbi = ((e.Source as ListBox).SelectedItem as ListBoxItem);
            _myShirt.ButtonColor = (ButtonColors) lbi.Content;
        }

        private void UiButtonColorChanged(object sender, RoutedEventArgs e)
        {
            var s = (Shirt) e.Source;
            var b = s.ButtonColor;
            if (b == ButtonColors.None)
            {
                ButtonChoice.Visibility = Visibility.Hidden;
                ButtonChoiceLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                ButtonChoice.Visibility = Visibility.Visible;
                ButtonChoiceLabel.Visibility = Visibility.Visible;
            }
        }
    }
}