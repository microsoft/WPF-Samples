// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace BusinessLayerValidation
{
    public class Person : IDataErrorInfo
    {
        public int Age { get; set; }
        public string Error => null;

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "Age")
                {
                    if (Age < 0 || Age > 150)
                    {
                        result = "Age must not be less than 0 or greater than 150.";
                    }
                }
                return result;
            }
        }
    }
}