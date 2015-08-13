// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UsingElements
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button _btn, _btn1, _btn2, _btn3;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton(object sender, MouseButtonEventArgs e)
        {
            sp1.Children.Clear();
            _btn = new Button {Content = "New Button"};
            sp1.Children.Add(_btn);
        }

        private void RemoveButton(object sender, MouseButtonEventArgs e)
        {
            if ((sp1.Children.IndexOf(_btn) >= 0) || (sp1.Children.IndexOf(_btn1) >= 0) ||
                (sp1.Children.IndexOf(_btn2) >= 0) || (sp1.Children.IndexOf(_btn3) >= 0))
            {
                sp1.Children.RemoveAt(0);
            }
        }

        private void InsertButton(object sender, MouseButtonEventArgs e)
        {
            sp1.Children.Clear();
            _btn = new Button {Content = "Click to insert button"};
            sp1.Children.Add(_btn);
            _btn.Click += (InsertControls);
            _btn1 = new Button {Content = "Click to insert button"};
            sp1.Children.Add(_btn1);
            _btn1.Click += (InsertControls);
        }

        private void InsertControls(object sender, RoutedEventArgs e)
        {
            _btn2 = new Button {Content = "Inserted Button"};
            sp1.Children.Insert(1, _btn2);
        }

        private void ShowIndex(object sender, MouseButtonEventArgs e)
        {
            sp1.Children.Clear();
            _btn = new Button {Content = "Click for index"};
            _btn.Click += (PrintIndex);
            sp1.Children.Add(_btn);

            _btn1 = new Button {Content = "Click for index"};
            sp1.Children.Add(_btn1);
            _btn1.Click += (PrintIndex1);

            _btn2 = new Button {Content = "Click for index"};
            sp1.Children.Add(_btn2);
            _btn2.Click += (PrintIndex2);

            _btn3 = new Button {Content = "Click for index"};
            sp1.Children.Add(_btn3);
            _btn3.Click += (PrintIndex3);
        }

        private void PrintIndex(object sender, RoutedEventArgs e)
        {
            _btn.Content = ((sp1.Children.IndexOf(_btn)).ToString());
        }

        private void PrintIndex1(object sender, RoutedEventArgs e)
        {
            _btn1.Content = ((sp1.Children.IndexOf(_btn1)).ToString());
        }

        private void PrintIndex2(object sender, RoutedEventArgs e)
        {
            _btn2.Content = ((sp1.Children.IndexOf(_btn2)).ToString());
        }

        private void PrintIndex3(object sender, RoutedEventArgs e)
        {
            _btn3.Content = ((sp1.Children.IndexOf(_btn3)).ToString());
        }

        private void ClearButtons(object sender, MouseButtonEventArgs e)
        {
            sp1.Children.Clear();
            _btn = new Button {Content = "Click to clear"};
            sp1.Children.Add(_btn);
            _btn.Click += (ClearControls);
            _btn1 = new Button {Content = "Click to clear"};
            sp1.Children.Add(_btn1);
            _btn1.Click += (ClearControls);
            _btn2 = new Button {Content = "Click to clear"};
            sp1.Children.Add(_btn2);
            _btn2.Click += (ClearControls);
            _btn3 = new Button {Content = "Click to clear"};
            sp1.Children.Add(_btn3);
            _btn3.Click += (ClearControls);
        }

        private void ClearControls(object sender, RoutedEventArgs e)
        {
            sp1.Children.Clear();
        }

        private void ContainsElement(object sender, RoutedEventArgs e)
        {
            var txt1 = new TextBlock();
            sp1.Children.Add(txt1);
            txt1.Text = "This StackPanel contains UIElement btn1: " + sp1.Children.Contains(_btn1);
        }

        private void GetItem(object sender, RoutedEventArgs e)
        {
            var txt2 = new TextBlock();
            sp1.Children.Add(txt2);
            txt2.Text = "UIElement at Index position [0] is " + sp1.Children[0];
        }

        private void GetCount(object sender, RoutedEventArgs e)
        {
            var txt3 = new TextBlock();
            sp1.Children.Add(txt3);
            txt3.Text = "UIElement Count is equal to " + sp1.Children.Count;
        }
    }
}