// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Divides first expression by the second.
    /// </summary>
    public sealed class DivExpression : BinaryExpression
    {
        public DivExpression(IExpression left, IExpression right) : base(left, right)
        {
        }

        protected override double Operate(double d1, double d2) => d1 / d2;

        public override IExpression Differentiate(string byVar) => new DivExpression(new SubExpression(new MultExpression(left.Differentiate(byVar), right),
                new MultExpression(left, right.Differentiate(byVar))),
                new ExpExpression(right, new ConstantExpression(2)));

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
                if (rightConst.Value == 0)
                {
                    throw new InvalidExpressionException("Divide by zero detected in function");
                }
                return new ConstantExpression(leftConst.Value/rightConst.Value);
            }
            if (leftConst != null && leftConst.Value == 0)
            {
                // 0 / y;  return 0;
                if (rightConst != null && rightConst.Value == 0)
                {
                    throw new InvalidExpressionException("Divide by zero detected in function");
                }
                return new ConstantExpression(0);
            }
            if (rightConst != null)
            {
                if (rightConst.Value == 0)
                {
                    // x / 0;
                    throw new InvalidExpressionException("Divide by zero detected in function");
                }
                if (rightConst.Value == 1)
                {
                    // x / 1;  return x;
                    return newLeft;
                }
                if (rightConst.Value == -1)
                {
                    // x / -1;  return -x;
                    if (leftNegate != null)
                    {
                        // x = -u (-x = --u);  return u;
                        return leftNegate.Child;
                    }
                    return new NegateExpression(newLeft);
                }
            }
            else if (leftNegate != null && rightNegate != null)
            {
                // -x / -y;  return x / y;
                return new DivExpression(leftNegate.Child, rightNegate.Child);
            }
            // x / y;  no simplification
            return new DivExpression(newLeft, newRight);
        }

        public override string ToString() => "(" + left.ToString() + "/" + right.ToString() + ")";
    }
}