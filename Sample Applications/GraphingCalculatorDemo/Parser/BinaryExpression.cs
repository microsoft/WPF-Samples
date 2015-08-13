// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Combines the results of 2 expressions, with the binary operation.
    /// </summary>
    public abstract class BinaryExpression : IExpression
    {
        protected IExpression left;
        protected IExpression right;

        protected BinaryExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
        }

        public IExpression Left => left;
        public IExpression Right => right;

        public double Evaluate() => Operate(left.Evaluate(), right.Evaluate());

        string IExpression.ToString() => ToString();

        public abstract IExpression Differentiate(string byVar);
        public abstract IExpression Simplify();
        protected abstract double Operate(double d1, double d2);
        public abstract override string ToString();
    }
}