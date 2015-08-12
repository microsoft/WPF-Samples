// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Wizard
{
    public class WizardDialogBox : NavigationWindow
    {
        public WizardDialogBox()
        {
            InitializeComponent();

            // Launch the wizard
            var wizardLauncher = new WizardLauncher();
            wizardLauncher.WizardReturn += wizardLauncher_WizardReturn;
            Navigate(wizardLauncher);
        }

        public WizardData WizardData { get; private set; }

        private void wizardLauncher_WizardReturn(object sender, WizardReturnEventArgs e)
        {
            // Handle wizard return
            WizardData = e.Data as WizardData;
            if (DialogResult == null)
            {
                DialogResult = (e.Result == WizardResult.Finished);
            }
        }
    }
}