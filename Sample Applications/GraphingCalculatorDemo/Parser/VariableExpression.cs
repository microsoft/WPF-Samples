// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Expression based on a single variable.
    ///     Variables must be set before they can be evaluated
    /// </summary>
    public sealed class VariableExpression : IExpression
    {
        private static readonly SortedList Environment;
        private readonly string _identifier;

        static VariableExpression()
        {
            Environment = new SortedList {{"pi", Math.PI}, {"e", Math.E}};
        }

        public VariableExpression(string identifier)
        {
            _identifier = identifier.ToLowerInvariant();
        }

        public bool HasValue => Environment.ContainsKey(_identifier.ToLowerInvariant());

        public double Value
        {
            get
            {
                if (HasValue)
                {
                    return (double) Environment[_identifier];
                }
                throw new UndefinedVariableException(_identifier + " is not defined");
            }
        }

        /// <summary>
        ///     Return the value this variable represents
        /// </summary>
        public double Evaluate() => Value;

        /// <summary>
        ///     Differentiate the Variable by input variable.
        /// </summary>
        public IExpression Differentiate(string byVar)
        {
            if (byVar == _identifier)
            {
                //      f(x) = x;
                // d( f(x) ) = 1 * d( x );
                //    d( x ) = 1;
                //     f'(x) = 1;
                return new ConstantExpression(1);
            }
            //      f(x) = c;
            // d( f(x) ) = c * d( c );
            //    d( c ) = 0;
            //     f'(x) = 0;
            return new ConstantExpression(0);
        }

        public IExpression Simplify()
        {
            if (_identifier == "t" || _identifier == "u" || _identifier == "v" || _identifier == "x" ||
                _identifier == "y")
            {
                // special iteration variables
                return new VariableExpression(_identifier);
            }
            if (HasValue)
            {
                return new ConstantExpression(Value);
            }
            return new VariableExpression(_identifier);
        }

        string IExpression.ToString() => _identifier;

        public static void Define(string variableName, double value)
        {
            variableName = variableName.ToLowerInvariant();
            if (Environment.ContainsKey(variableName))
            {
                Environment[variableName] = value;
            }
            else
            {
                Environment.Add(variableName, value);
            }
        }

        public static void Undefine(string variableName)
        {
            variableName = variableName.ToLowerInvariant();
            if (Environment.ContainsKey(variableName))
            {
                Environment.Remove(variableName);
            }
        }
    }
}