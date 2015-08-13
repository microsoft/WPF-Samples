// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

//------------------------------------------------------------------

/*
    Metacharacters/Symbology:
    -------------------------
    < >    == Non-terminal
    { }*   == Kleene-star (0 or more occurrences)
    { }+   == Kleene-plus (1 or more occurrences)
    { }?   == Optional (0 or 1 occurrences)
    [ ]    == Madatory (1 must occur)
    |      == Vertical bar (logical OR)
    " "    == Terminal symbol (literal)

    The Grammar Rules:
    ------------------
    <start>        := <AddExp>
    <AddExp>       := <MultExp> { { "+" | "-" }? <MultExp> }*
    <MultExp>      := <ExpExp> { { "*" | "/" }? <ExpExp> }*
    <ExpExp>       := <UnaryExp> { { "^" }? <UnaryExp> }?
    <UnaryExp>     := { "-" }? <FactorPrefix>
    <FactorPrefix> := [ <Constant> { <Factor> }? | <Factor> ]
    <Factor>       := { <Variable> | <Function> | "(" <AddExp> ")" }+
    <Function>     := [ "sin" | "cos" | "tan" ] "(" <AddExp> ")"

    Non-terminals defined/parsed by Tokenizer:
    ------------------------------------------
    <Constant> := anything that can be parsed by Convert.ToDouble()
    <Variable> := any string containing only letters (a-z and A-Z)

    The algorithm:
    --------------
    Parsing is most easily performed when there is a 1:1 correspondence
    between non-terminals and functions.  Therefore, each private function
    below parses exactly one grammar rule.

    What do those odd "First" variables mean?
    -----------------------------------------
    The syntax checking algorithm decides whether syntax is correct
    by checking if the current token in the queue is expected.  I call
    the set of valid tokens to check against the "First" set because
    that's how we referred to it in my compiler class in school.  It
    could just as easily be referred to as the "NextValidTokenFor<blah>"
    set, but that's a little too verbose for code.
*/

namespace GraphingCalculatorDemo.Parser
{
    //--------------------------------------------------------------

    internal class FunctionParser
    {
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static Tokenizer _tokenizer;
        private static Token _currentToken;
        private static TokenSet _firstAddExp;
        private static readonly TokenSet FirstMultExp;
        private static readonly TokenSet FirstExpExp;
        private static readonly TokenSet FirstUnaryExp;
        private static readonly TokenSet FirstFactorPrefix;
        private static readonly TokenSet FirstFactor;
        private static readonly TokenSet FirstFunction;
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        static FunctionParser()
        {
            FirstFunction = new TokenSet(TokenType.Sine | TokenType.Cosine | TokenType.Tangent);
            FirstFactor = FirstFunction + new TokenSet(TokenType.Variable | TokenType.OpenParen);
            FirstFactorPrefix = FirstFactor + TokenType.Constant;
            FirstUnaryExp = FirstFactorPrefix + TokenType.Minus;
            FirstExpExp = new TokenSet(FirstUnaryExp);
            FirstMultExp = new TokenSet(FirstUnaryExp);
            _firstAddExp = new TokenSet(FirstUnaryExp);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     Parse a string representation of a parametric function into an
        ///     expression tree that can be later evaluated.
        /// </summary>
        /// <param name="function">The function to parse</param>
        /// <returns>An expression tree representing the function parsed</returns>
        public static IExpression Parse(string function)
        {
            _tokenizer = new Tokenizer(function);
            _currentToken = new Token("", TokenType.None);

            if (!Next())
            {
                throw new InvalidExpressionException("Cannot parse an empty function");
            }
            var exp = ParseAddExpression();
            var leftover = string.Empty;
            while (_currentToken.Type != TokenType.Eof)
            {
                leftover += _currentToken.Value;
                Next();
            }
            if (!string.IsNullOrEmpty(leftover))
            {
                throw new TrailingTokensException("Trailing characters: " + leftover);
            }
            return exp;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseAddExpression()
        {
            if (Check(FirstMultExp))
            {
                var exp = ParseMultExpression();

                while (Check(new TokenSet(TokenType.Plus | TokenType.Minus)))
                {
                    var opType = _currentToken.Type;
                    Eat(opType);
                    if (!Check(FirstMultExp))
                    {
                        throw new InvalidSyntaxException("Expected an expression after + or - operator");
                    }
                    var right = ParseMultExpression();

                    switch (opType)
                    {
                        case TokenType.Plus:
                            exp = new AddExpression(exp, right);
                            break;

                        case TokenType.Minus:
                            exp = new SubExpression(exp, right);
                            break;

                        default:
                            throw new UnexpectedBehaviorException("Expected plus or minus, got: " + opType);
                    }
                }

                return exp;
            }
            throw new InvalidSyntaxException("Invalid expression");
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseMultExpression()
        {
            if (Check(FirstExpExp))
            {
                var exp = ParseExpExpression();

                while (Check(new TokenSet(TokenType.Multiply | TokenType.Divide)))
                {
                    var opType = _currentToken.Type;
                    Eat(opType);
                    if (!Check(FirstExpExp))
                    {
                        throw new InvalidSyntaxException("Expected an expression after * or / operator");
                    }
                    var right = ParseExpExpression();

                    switch (opType)
                    {
                        case TokenType.Multiply:
                            exp = new MultExpression(exp, right);
                            break;

                        case TokenType.Divide:
                            exp = new DivExpression(exp, right);
                            break;

                        default:
                            throw new UnexpectedBehaviorException("Expected mult or divide, got: " + opType);
                    }
                }

                return exp;
            }
            throw new InvalidSyntaxException("Invalid expression");
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseExpExpression()
        {
            if (Check(FirstUnaryExp))
            {
                var exp = ParseUnaryExpression();

                if (Check(new TokenSet(TokenType.Exponent)))
                {
                    var opType = _currentToken.Type;
                    Eat(opType);
                    if (!Check(FirstUnaryExp))
                    {
                        throw new InvalidSyntaxException("Expected an expression after ^ operator");
                    }
                    var right = ParseUnaryExpression();

                    switch (opType)
                    {
                        case TokenType.Exponent:
                            exp = new ExpExpression(exp, right);
                            break;

                        default:
                            throw new UnexpectedBehaviorException("Expected exponent, got: " + opType);
                    }
                }

                return exp;
            }
            throw new InvalidSyntaxException("Invalid expression");
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseUnaryExpression()
        {
            var negate = false;
            if (_currentToken.Type == TokenType.Minus)
            {
                Eat(TokenType.Minus);
                negate = true;
            }

            if (Check(FirstFactorPrefix))
            {
                var exp = ParseFactorPrefix();

                if (negate)
                {
                    return new NegateExpression(exp);
                }
                return exp;
            }
            throw new InvalidSyntaxException("Invalid expression");
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseFactorPrefix()
        {
            IExpression exp = null;
            if (_currentToken.Type == TokenType.Constant)
            {
                exp = new ConstantExpression(Convert.ToDouble(_currentToken.Value));
                Eat(TokenType.Constant);
            }

            if (Check(FirstFactor))
            {
                if (exp == null)
                {
                    return ParseFactor();
                }
                return new MultExpression(exp, ParseFactor());
            }
            // This should be impossible because bad symbols are caught by Tokenizer,
            //  constants would have been parsed in the if-statement above, and
            //  anything else is treated as a Factor (UndefinedVariableException
            //  will be thrown when you try to evaluate the function).
            if (exp == null)
            {
                throw new InvalidSyntaxException("Invalid Expression");
            }
            return exp;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseFactor()
        {
            IExpression exp = null;
            do
            {
                IExpression right = null;
                switch (_currentToken.Type)
                {
                    case TokenType.Variable:
                        right = new VariableExpression(_currentToken.Value);
                        Eat(TokenType.Variable);
                        break;

                    case TokenType.Sine:
                    case TokenType.Cosine:
                    case TokenType.Tangent:
                        right = ParseFunction();
                        break;

                    case TokenType.OpenParen:
                        Eat(TokenType.OpenParen);
                        right = ParseAddExpression();
                        Eat(TokenType.CloseParen);
                        break;

                    default:
                        throw new UnexpectedBehaviorException("Unexpected token in Factor: " + _currentToken.Type);
                }

                exp = (exp == null) ? right : new MultExpression(exp, right);
            } while (Check(FirstFactor));

            return exp;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private static IExpression ParseFunction()
        {
            var opType = _currentToken.Type;
            Eat(opType);
            Eat(TokenType.OpenParen);
            var exp = ParseAddExpression();
            Eat(TokenType.CloseParen);

            switch (opType)
            {
                case TokenType.Sine:
                    return new SineExpression(exp);

                case TokenType.Cosine:
                    return new CosineExpression(exp);

                case TokenType.Tangent:
                    return new TangentExpression(exp);

                default:
                    throw new UnexpectedBehaviorException("Unexpected Function type: " + opType);
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     Assign the next token in the queue to CurrentToken
        /// </summary>
        /// <returns>
        ///     true if there are still more tokens in the queue,
        ///     false if we have looked at all available tokens already
        /// </returns>
        private static bool Next()
        {
            if (_currentToken.Type == TokenType.Eof)
            {
                throw new OutOfTokensException("Parsed past the end of the function");
            }
            _currentToken = _tokenizer.Next();

            return _currentToken.Type != TokenType.Eof;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     Assign the next token in the queue to CurrentToken if the CurrentToken's
        ///     type matches that of the specified parameter.  If the CurrentToken's
        ///     type does not match the parameter, throw a syntax exception
        /// </summary>
        /// <param name="type">The type that your syntax expects CurrentToken to be</param>
        private static void Eat(TokenType type)
        {
            if (_currentToken.Type != type)
            {
                throw new InvalidSyntaxException("Missing: " + type);
            }
            Next();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     Check if the CurrentToken is a member of a set Token types
        /// </summary>
        /// <param name="tokens">The set of Token types to check against</param>
        /// <returns>
        ///     true if the CurrentToken's type is in the set
        ///     false if it is not
        /// </returns>
        private static bool Check(TokenSet tokens) => tokens.Contains(_currentToken.Type);
    }
}