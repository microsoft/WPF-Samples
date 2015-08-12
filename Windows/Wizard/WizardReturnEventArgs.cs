// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Wizard
{
    public class WizardReturnEventArgs
    {
        public WizardReturnEventArgs(WizardResult result, object data)
        {
            Result = result;
            Data = data;
        }

        public WizardResult Result { get; }
        public object Data { get; }
    }
}