// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    //--------------------------------------------------------------

    public struct Token
    {
        public readonly TokenType Type;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public readonly string Value;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }
    }
}