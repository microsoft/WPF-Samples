// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Evaluate the cosine of an expression.
    /// </summary>
    public sealed class CosineExpression : UnaryExpression
    {
        public CosineExpression(IExpression child) : base(child)
        {
        }

        protected override double Operate(double d) => Math.Cos(d);

        public override IExpression Differentiate(string byVar) => new MultExpression(new NegateExpression(new SineExpression(child)),
                child.Differentiate(byVar));

        public override IExpression Simplify()
        {
            var newChild = child.Simplify();
            var childConst = newChild as ConstantExpression;

            if (childConst != null)
            {
                // child is constant;  just evaluate it;
                return new ConstantExpression(Math.Cos(childConst.Value));
            }
            return new CosineExpression(newChild);
        }

        public override string ToString() => "cos(" + child.ToString() + ")";
    }
}