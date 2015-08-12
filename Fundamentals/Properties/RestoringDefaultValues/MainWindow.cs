// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RestoringDefaultValues
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RestoreDefaultProperties(object sender, RoutedEventArgs e)
        {
            var uic = Sandbox.Children;
            foreach (Shape uie in uic)
            {
                var locallySetProperties = uie.GetLocalValueEnumerator();
                while (locallySetProperties.MoveNext())
                {
                    var propertyToClear = locallySetProperties.Current.Property;
                    if (!propertyToClear.ReadOnly)
                    {
                        uie.ClearValue(propertyToClear);
                    }
                }
            }
        }

        private void MakeEverythingRed(object sender, RoutedEventArgs e)
        {
            var uic = Sandbox.Children;
            foreach (Shape uie in uic)
            {
                uie.Fill = new SolidColorBrush(Colors.Red);
            }
        }
    }
}