// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    [Serializable]
    public class InvalidSyntaxException : FunctionParserException
    {
        public InvalidSyntaxException(string msg) : base(msg)
        {
        }
    }
}