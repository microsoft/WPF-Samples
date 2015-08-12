// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace LayoutTransitionsDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _numItems = 8;
        private int _counter;
        private int _gridSize = 5;
        public ArrayList Buttons = new ArrayList();
        public ArrayList Hosts = new ArrayList();
        public ArrayList Panels = new ArrayList();
        public ArrayList Targets = new ArrayList();

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;

            Panels.Add(LTLGrid);
            Panels.Add(LTLGrid2);
            Panels.Add(LTLStackPanel);
            Panels.Add(LTLWrapPanel);

            Debug.WriteLine("Grid children: " + ButtonGrid.Children.Count);

            for (var i = 0; i < _numItems; i++)
            {
                var target = new LayoutToLayoutTarget();
                Targets.Add(target);
                target.Margin = new Thickness(5);
                target.MinWidth = 80;
                target.MinHeight = 50;
                target.BorderThickness = new Thickness(0);

                Grid.SetRow(target, i/5);
                Grid.SetColumn(target, i%5);
                LTLGrid.Children.Add(target);

                var host = new LayoutToLayoutHost();
                Hosts.Add(host);
                host.BorderThickness = new Thickness(0);

                var demoButton = new Button {Content = "# " + i};
                demoButton.Click += OnAdvanceClick;
                host.Child = demoButton;
                //host.Child = Buttons[i] as Button;

                Canvas.SetLeft(host, 0);
                Canvas.SetTop(host, 0);
                LTLCanvas.Children.Add(host);

                host.BindToTarget(target);
            }
        }

        /*
         * Increase the starting location in the grid by one
         * */

        private void OnAdvanceClick(object sender, RoutedEventArgs e)
        {
            _counter++;

            for (var i = 0; i < Hosts.Count; i++)
                MoveObject(Hosts[i] as LayoutToLayoutHost, _counter + i);
        }

        /*
         * Decrease the starting location in the grid by one
         * */

        private void OnRetreatClick(object sender, RoutedEventArgs e)
        {
            _counter--;

            for (var i = 0; i < Hosts.Count; i++)
                MoveObject(Hosts[i] as LayoutToLayoutHost, _counter + i);
        }

        /*
         * Set the Stackpanel's orientation to vertical
         * */

        private void OnStackVertical(object sender, RoutedEventArgs e)
        {
            foreach (var host in Hosts)
                (host as LayoutToLayoutHost)?.BeginAnimating(false);

            LTLStackPanel.Orientation = Orientation.Vertical;
        }

        /*
         * Set the stackpanel's orientation to vertical
         * */

        private void OnStackHorizontal(object sender, RoutedEventArgs e)
        {
            foreach (var host in Hosts)
                (host as LayoutToLayoutHost)?.BeginAnimating(false);

            LTLStackPanel.Orientation = Orientation.Horizontal;
        }

        /*
         * Set the wrappanel's orientation to vertical
         * */

        private void OnWrapVertical(object sender, RoutedEventArgs e)
        {
            foreach (var t in Hosts)
                (t as LayoutToLayoutHost)?.BeginAnimating(false);

            LTLWrapPanel.Orientation = Orientation.Vertical;
        }

        /*
         * Set the wrappanel's orientation to vertical
         * */

        private void OnWrapHorizontal(object sender, RoutedEventArgs e)
        {
            foreach (var host in Hosts)
                (host as LayoutToLayoutHost)?.BeginAnimating(false);

            LTLWrapPanel.Orientation = Orientation.Horizontal;
        }

        /*
         * Move all buttons into the 5x5 grid
         * */

        private void OnGrid5(object sender, RoutedEventArgs e)
        {
            ClearPanels();
            _gridSize = 5;
            _counter = 0;

            for (var i = 0; i < _numItems; i++)
            {
                var target = Targets[i] as LayoutToLayoutTarget;
                (Hosts[i] as LayoutToLayoutHost)?.BeginAnimating(false);
                Grid.SetRow(target, i/5);
                Grid.SetColumn(target, i%5);
                LTLGrid.Children.Add(target);
            }
        }

        /*
         * Move all buttons into the 3x3 grid
         * */

        private void OnGrid3(object sender, RoutedEventArgs e)
        {
            ClearPanels();
            _gridSize = 3;
            _counter = 0;

            for (var i = 0; i < _numItems; i++)
            {
                var target = Targets[i] as LayoutToLayoutTarget;
                (Hosts[i] as LayoutToLayoutHost)?.BeginAnimating(false);
                Grid.SetRow(target, i/3);
                Grid.SetColumn(target, i%3);
                LTLGrid2.Children.Add(target);
            }
        }

        /*
         * Move all buttons into the stack panel
         * */

        private void OnStackPanel(object sender, RoutedEventArgs e)
        {
            ClearPanels();
            for (var i = 0; i < _numItems; i++)
            {
                var target = Targets[i] as LayoutToLayoutTarget;
                (Hosts[i] as LayoutToLayoutHost)?.BeginAnimating(false);
                LTLStackPanel.Children.Add(target);
            }
        }

        /*
         * Move all buttons into the wrap panel
         * */

        private void OnWrapPanel(object sender, RoutedEventArgs e)
        {
            ClearPanels();
            for (var i = 0; i < _numItems; i++)
            {
                var target = Targets[i] as LayoutToLayoutTarget;
                (Hosts[i] as LayoutToLayoutHost).BeginAnimating(false);
                LTLWrapPanel.Children.Add(target);
            }
        }

        /*
         * move an object from one grid cell to another
         * */

        private void MoveObject(LayoutToLayoutHost obj, int position)
        {
            var max = _gridSize*_gridSize - 1;
            if (position > max)
                position = max;
            if (position < 0)
                position = 0;

            obj.BeginAnimating(false);
            Grid.SetRow(obj.Target, position/_gridSize);
            Grid.SetColumn(obj.Target, position%_gridSize);
        }

        /*
         * Remove all elements from all panels
         * */

        private void ClearPanels()
        {
            foreach (var panel in Panels)
            {
                (panel as Panel)?.Children.Clear();
            }
        }
    }
}