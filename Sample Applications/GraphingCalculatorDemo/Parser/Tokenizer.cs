// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace GraphingCalculatorDemo.Parser
{
    //--------------------------------------------------------------

    public class Tokenizer
    {
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private readonly string _function;
        private int _index;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Tokenizer(string function)
        {
            if (function == null)
            {
                function = string.Empty;
            }
            _function = function;
            _index = 0;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public Token Next()
        {
            while (_index < _function.Length)
            {
                if (IsNumber(_function[_index]))
                {
                    var val = string.Empty;
                    val += _function[_index++];
                    while (_index < _function.Length && IsNumber(_function[_index]))
                    {
                        val += _function[_index++];
                    }
                    return new Token(val, TokenType.Constant);
                }
                if (IsAlpha(_function[_index]))
                {
                    var var = string.Empty;
                    var += _function[_index++];
                    while (_index < _function.Length && IsAlpha(_function[_index]))
                    {
                        var += _function[_index++];
                    }
                    switch (var.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "sin":
                            return new Token("sin", TokenType.Sine);
                        case "cos":
                            return new Token("cos", TokenType.Cosine);
                        case "tan":
                            return new Token("tan", TokenType.Tangent);
                        default:
                            return new Token(var, TokenType.Variable);
                    }
                }
                switch (_function[_index++])
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        continue;

                    case '+':
                        return new Token("+", TokenType.Plus);
                    case '-':
                        return new Token("-", TokenType.Minus);
                    case '*':
                        return new Token("*", TokenType.Multiply);
                    case '/':
                        return new Token("/", TokenType.Divide);
                    case '^':
                        return new Token("^", TokenType.Exponent);
                    case '(':
                        return new Token("(", TokenType.OpenParen);
                    case ')':
                        return new Token(")", TokenType.CloseParen);
                    default:
                        throw new InvalidSyntaxException("Invalid token '" + _function[_index - 1] + "' in function: " +
                                                         _function);
                }
            }
            return new Token("", TokenType.Eof);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private bool IsAlpha(char c) => (('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z'));

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private bool IsNumber(char c) => ('.' == c || ('0' <= c && c <= '9'));
    }
}