// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Subtracts 2 expressions.
    /// </summary>
    public sealed class SubExpression : BinaryExpression
    {
        public SubExpression(IExpression left, IExpression right) : base(left, right)
        {
        }

        protected override double Operate(double d1, double d2) => d1 - d2;

        public override IExpression Differentiate(string byVar) => new SubExpression(left.Differentiate(byVar), right.Differentiate(byVar));

        public override IExpression Simplify()
        {
            var newLeft = left.Simplify();
            var newRight = right.Simplify();

            var leftConst = newLeft as ConstantExpression;
            var rightConst = newRight as ConstantExpression;
            var rightNegate = newRight as NegateExpression;

            if (leftConst != null && rightConst != null)
            {
                // two constants;  just evaluate it;
                return new ConstantExpression(leftConst.Value - rightConst.Value);
            }
            if (leftConst != null && leftConst.Value == 0)
            {
                // 0 - y;  return -y;
                if (rightNegate != null)
                {
                    // y = -u (--u);  return u;
                    return rightNegate.Child;
                }
                return new NegateExpression(newRight);
            }
            if (rightConst != null && rightConst.Value == 0)
            {
                // x - 0;  return x;
                return newLeft;
            }
            if (rightNegate != null)
            {
                // x - -y;  return x + y;
                return new AddExpression(newLeft, rightNegate.Child);
            }
            // x - y;  no simplification
            return new SubExpression(newLeft, newRight);
        }

        public override string ToString() => "(" + left.ToString() + "-" + right.ToString() + ")";
    }
}