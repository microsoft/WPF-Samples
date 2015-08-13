// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     An expression which holds onto and negates a single child expression.
    ///     All of its functions call into the child functions and return the
    ///     negation.
    /// </summary>
    public sealed class NegateExpression : UnaryExpression
    {
        public NegateExpression(IExpression child) : base(child)
        {
        }

        protected override double Operate(double d) => -d;

        public override IExpression Differentiate(string byVar) => new NegateExpression(child.Differentiate(byVar));

        public override IExpression Simplify()
        {
            var newChild = child.Simplify();
            var childConst = newChild as ConstantExpression;

            if (childConst != null)
            {
                // child is constant;  just evaluate it;
                return new ConstantExpression(-childConst.Value);
            }
            return new NegateExpression(newChild);
        }

        public override string ToString() => "(-" + child.ToString() + ")";
    }
}