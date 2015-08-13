// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     An expression which holds onto a single child expression.  All of its
    ///     functions call into the child functions.
    /// </summary>
    public abstract class UnaryExpression : IExpression
    {
        protected IExpression child;

        protected UnaryExpression(IExpression child)
        {
            this.child = child;
        }

        public IExpression Child => child;

        public double Evaluate() => Operate(child.Evaluate());

        string IExpression.ToString() => ToString();

        public abstract IExpression Differentiate(string byVar);
        public abstract IExpression Simplify();
        protected abstract double Operate(double d);
        public abstract override string ToString();
    }
}