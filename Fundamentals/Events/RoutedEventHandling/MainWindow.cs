// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Windows;

namespace RoutedEventHandling
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StringBuilder _eventstr = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HandleClick(object sender, RoutedEventArgs args)
        {
            var fe = (FrameworkElement) sender;
            _eventstr.Append("Event handled by element named ");
            _eventstr.Append(fe.Name);
            _eventstr.Append("\n");
            var fe2 = (FrameworkElement) args.Source;
            _eventstr.Append("Event originated from source element of type ");
            _eventstr.Append(args.Source.GetType());
            _eventstr.Append(" with Name ");
            _eventstr.Append(fe2.Name);
            _eventstr.Append("\n");
            _eventstr.Append("Event used routing strategy ");
            _eventstr.Append(args.RoutedEvent.RoutingStrategy);
            _eventstr.Append("\n");
            results.Text = _eventstr.ToString();
        }
    }
}