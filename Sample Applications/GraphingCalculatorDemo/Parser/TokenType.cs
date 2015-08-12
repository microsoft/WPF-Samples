// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    public enum TokenType
    {
        None = 0x0,
        Constant = 0x1,
        Variable = 0x2,
        Plus = 0x4,
        Minus = 0x8,
        Multiply = 0x10,
        Divide = 0x20,
        Exponent = 0x40,
        Sine = 0x80,
        Cosine = 0x100,
        Tangent = 0x200,
        OpenParen = 0x400,
        CloseParen = 0x800,
        Eof = 0x1000
    }
}