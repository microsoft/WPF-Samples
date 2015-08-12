// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;

namespace ADODataSet
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _appPath;
        private DataSet _myDataSet;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string AppDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_appPath))
                {
                    _appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                return _appPath;
            }
        }

        private void OnInit(object sender, EventArgs e)
        {
            var mdbFile = Path.Combine(AppDataPath, "BookData.mdb");
            string connString = $"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={mdbFile}";
            var conn = new OleDbConnection(connString);
            var adapter = new OleDbDataAdapter("SELECT * FROM BookTable;", conn);

            _myDataSet = new DataSet();
            adapter.Fill(_myDataSet, "BookTable");

            // myListBox is a ListBox control.
            // Set the DataContext of the ListBox to myDataSet
            myListBox.DataContext = _myDataSet;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var myDataTable = _myDataSet.Tables["BookTable"];
            var row = myDataTable.NewRow();

            row["Title"] = "Microsoft C# Language Specifications";
            row["ISBN"] = "0-7356-1448-2";
            row["NumPages"] = 431;
            myDataTable.Rows.Add(row);
        }
    }
}