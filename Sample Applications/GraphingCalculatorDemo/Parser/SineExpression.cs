// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Evaluate the sine of an expression.
    /// </summary>
    public sealed class SineExpression : UnaryExpression
    {
        public SineExpression(IExpression child) : base(child)
        {
        }

        protected override double Operate(double d) => Math.Sin(d);

        public override IExpression Differentiate(string byVar) => new MultExpression(new CosineExpression(child), child.Differentiate(byVar));

        public override IExpression Simplify()
        {
            var newChild = child.Simplify();
            var childConst = newChild as ConstantExpression;

            if (childConst != null)
            {
                // child is constant;  just evaluate it;
                return new ConstantExpression(Math.Sin(childConst.Value));
            }
            return new SineExpression(newChild);
        }

        public override string ToString() => "sin(" + child.ToString() + ")";
    }
}