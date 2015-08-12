// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    [Serializable]
    public class InvalidExpressionException : FunctionParserException
    {
        public InvalidExpressionException(string msg) : base(msg)
        {
        }

        public InvalidExpressionException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}