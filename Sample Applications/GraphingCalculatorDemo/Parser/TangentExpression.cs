// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Evaluate the tangent of an expression.
    /// </summary>
    public sealed class TangentExpression : UnaryExpression
    {
        public TangentExpression(IExpression child) : base(child)
        {
        }

        protected override double Operate(double d) => Math.Tan(d);

        public override IExpression Differentiate(string byVar) => new MultExpression(new ExpExpression(new CosineExpression(child),
                new ConstantExpression(-2)),
                child.Differentiate(byVar));

        public override IExpression Simplify()
        {
            var newChild = child.Simplify();
            var childConst = newChild as ConstantExpression;

            if (childConst != null)
            {
                // child is constant;  just evaluate it;
                return new ConstantExpression(Math.Tan(childConst.Value));
            }
            return new TangentExpression(newChild);
        }

        public override string ToString() => "tan(" + child.ToString() + ")";
    }
}