// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace MergedResources
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _rdFileName = "myresourcedictionary3.xaml";
        private ResourceDictionary _rd;

        public MainWindow()
        {
            InitializeComponent();
        }

        private ResourceDictionary CreateOrLoadRd()
        {
            var fi = new FileInfo(_rdFileName);
            if (!fi.Exists)
            {
                var rd = new ResourceDictionary();
                root.Resources.MergedDictionaries.Add(rd);
                var fs = new FileStream(_rdFileName, FileMode.Create);
                XamlWriter.Save(rd, fs);
                fs.Close();
                return rd;
            }
            else
            {
                var fs = new FileStream(_rdFileName, FileMode.Open);
                var rd = (ResourceDictionary) XamlReader.Load(fs);
                root.Resources.MergedDictionaries.Add(rd);
                fs.Close();
                return rd;
            }
        }

        private void ChangeRd(ResourceDictionary rd, string newKey, object newValue)
        {
            if (!rd.Contains(newKey))
            {
                rd.Add(newKey, newValue);
                SaveChange();
            }
        }

        private void SaveChange()
        {
            var fs = new FileStream(_rdFileName, FileMode.Open);
            XamlWriter.Save(_rd, fs);
            fs.Close();
        }

        private void NewD(object sender, RoutedEventArgs e)
        {
            _rd = CreateOrLoadRd();
        }

        private void Add2NewD(object sender, RoutedEventArgs e)
        {
            ChangeRd(_rd, "BodyBrush", new SolidColorBrush(Colors.Green));
        }
    }
}