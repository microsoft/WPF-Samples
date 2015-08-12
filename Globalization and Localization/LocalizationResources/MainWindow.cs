// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Resources;
using System.Windows;

namespace LocalizationResources
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var rm = new ResourceManager
                ("LocalizationResources.data.stringtable", Assembly.GetExecutingAssembly());
            Title = rm.GetString("Title");
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var rm = new ResourceManager("LocalizationResources.data.stringtable",
                Assembly.GetExecutingAssembly());
            Text1.Text = rm.GetString("Message");
        }
    }
}