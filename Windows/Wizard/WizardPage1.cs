// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Navigation;

namespace Wizard
{
    public partial class WizardPage1 : PageFunction<WizardResult>
    {
        public WizardPage1(WizardData wizardData)
        {
            InitializeComponent();

            // Bind wizard state to UI
            DataContext = wizardData;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var wizardPage2 = new WizardPage2((WizardData) DataContext);
            wizardPage2.Return += wizardPage_Return;
            NavigationService?.Navigate(wizardPage2);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the wizard and don't return any data
            OnReturn(new ReturnEventArgs<WizardResult>(WizardResult.Canceled));
        }

        public void wizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            // If returning, wizard was completed (finished or canceled),
            // so continue returning to calling page
            OnReturn(e);
        }
    }
}