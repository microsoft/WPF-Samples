// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace GraphingCalculatorDemo.Parser
{
    //--------------------------------------------------------------

    /// <summary>
    ///     ConstantExpression - An expression which always resolves to a constant
    ///     value.
    /// </summary>
    public sealed class ConstantExpression : IExpression
    {
        public ConstantExpression(double value)
        {
            Value = value;
        }

        public double Value { get; }

        /// <summary>
        ///     A constant always evaluates to its value.
        /// </summary>
        public double Evaluate()
        {
            return Value;
        }

        /// <summary>
        ///     The derivative of a constant is 0.
        /// </summary>
        public IExpression Differentiate(string byVar)
        {
            //      f(x) = c;
            // d( f(x) ) = c * d( c );
            //    d( c ) = 0;
            //     f'(x) = 0;
            return new ConstantExpression(0);
        }

        public IExpression Simplify()
        {
            return new ConstantExpression(Value);
        }

        string IExpression.ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    //--------------------------------------------------------------

    #region Unary Expressions

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    #endregion

    #region Binary Expressions

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    #endregion

    // TODO: support Tertiary Expressions (conditionals)
}