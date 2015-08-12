// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace Wizard
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

        private void RunWizardButton_Click(object sender, RoutedEventArgs e)
        {
            var wizard = new WizardDialogBox();
            var showDialog = wizard.ShowDialog();
            var dialogResult = showDialog != null && (bool) showDialog;
            MessageBox.Show(
                dialogResult
                    ? $"{wizard.WizardData.DataItem1}\n{wizard.WizardData.DataItem2}\n{wizard.WizardData.DataItem3}"
                    : "Canceled.");
        }
    }
}