// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    /// <summary>
    ///     Raise the first expression to the power of the second.
    /// </summary>
    public sealed class ExpExpression : BinaryExpression
    {
        public ExpExpression(IExpression left, IExpression right) : base(left, right)
        {
        }

        protected override double Operate(double d1, double d2) => Math.Pow(d1, d2);

        public override IExpression Differentiate(string byVar)
        {
            if (right is ConstantExpression)
            {
                //      f(x) = g(x)^n;
                //     f'(x) = n * g'(x) * g(x)^(n-1);
                return new MultExpression(new MultExpression(right, left.Differentiate(byVar)),
                    new ExpExpression(left,
                        new SubExpression(right,
                            new ConstantExpression(1))));
            }
            var simple = left.Simplify();
            if (simple is ConstantExpression)
            {
                //  f(x) = a^g(x);
                // f'(x) = (ln a) * g'(x) * a^g(x);
                var a = ((ConstantExpression) simple).Value;
                return new MultExpression(new MultExpression(new ConstantExpression(Math.Log(a)),
                    right.Differentiate(byVar)),
                    new ExpExpression(simple, right));
            }
            throw new CannotDifferentiateException("I do not support complex exponent differentiation");
        }

        public override IExpression Simplify()
        {
            var newLeft = left.Simplify();
            var newRight = right.Simplify();

            var leftConst = newLeft as ConstantExpression;
            var rightConst = newRight as ConstantExpression;

            if (leftConst != null && rightConst != null)
            {
                // two constants;  just evaluate it;
                return new ConstantExpression(Math.Pow(leftConst.Value, rightConst.Value));
            }
            if (rightConst != null)
            {
                if (rightConst.Value == 0)
                {
                    // x ^ 0;  return 1;
                    return new ConstantExpression(1);
                }
                if (rightConst.Value == 1)
                {
                    // x ^ 1;  return x;
                    return newLeft;
                }
            }
            else if (leftConst != null && leftConst.Value == 0)
            {
                // 0 ^ y;  return 0;
                return new ConstantExpression(0);
            }
            // x ^ y;  no simplification
            return new ExpExpression(newLeft, newRight);
        }

        public override string ToString() => "(" + left.ToString() + "^" + right.ToString() + ")";
    }
}