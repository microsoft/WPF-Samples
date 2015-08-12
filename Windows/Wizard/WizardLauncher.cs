// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Navigation;

namespace Wizard
{
    public class WizardLauncher : PageFunction<WizardResult>
    {
        private readonly WizardData _wizardData = new WizardData();
        public event WizardReturnEventHandler WizardReturn;

        protected override void Start()
        {
            base.Start();

            // So we remember the WizardCompleted event registration
            KeepAlive = true;

            // Launch the wizard
            var wizardPage1 = new WizardPage1(_wizardData);
            wizardPage1.Return += wizardPage_Return;
            NavigationService?.Navigate(wizardPage1);
        }

        public void wizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            // Notify client that wizard has completed
            // NOTE: We need this custom event because the Return event cannot be
            // registered by window code - if WizardDialogBox registers an event handler with
            // the WizardLauncher's Return event, the event is not raised.
            WizardReturn?.Invoke(this, new WizardReturnEventArgs(e.Result, _wizardData));
            OnReturn(null);
        }
    }
}