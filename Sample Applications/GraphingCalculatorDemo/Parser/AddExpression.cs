// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Adds 2 expressions
    /// </summary>
    public sealed class AddExpression : BinaryExpression
    {
        public AddExpression(IExpression left, IExpression right) : base(left, right)
        {
        }

        protected override double Operate(double d1, double d2) => d1 + d2;

        public override IExpression Differentiate(string byVar) => new AddExpression(left.Differentiate(byVar), right.Differentiate(byVar));

        public override IExpression Simplify()
        {
            var newLeft = left.Simplify();
            var newRight = right.Simplify();

            var leftConst = newLeft as ConstantExpression;
            var rightConst = newRight as ConstantExpression;
            var leftNegate = newLeft as NegateExpression;
            var rightNegate = newRight as NegateExpression;

            if (leftConst != null && rightConst != null)
            {
                // two constants;  just evaluate it;
                return new ConstantExpression(leftConst.Value + rightConst.Value);
            }
            if (leftConst != null && leftConst.Value == 0)
            {
                // 0 + y;  return y;
                return newRight;
            }
            if (rightConst != null && rightConst.Value == 0)
            {
                // x + 0;  return x;
                return newLeft;
            }
            if (rightNegate != null)
            {
                // x + -y;  return x - y;  (this covers -x + -y case too)
                return new SubExpression(newLeft, rightNegate.Child);
            }
            if (leftNegate != null)
            {
                // -x + y;  return y - x;
                return new SubExpression(newRight, leftNegate.Child);
            }
            // x + y;  no simplification
            return new AddExpression(newLeft, newRight);
        }

        public override string ToString() => "(" + left.ToString() + "+" + right.ToString() + ")";
    }
}