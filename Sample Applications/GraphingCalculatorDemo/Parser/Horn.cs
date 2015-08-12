// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    public sealed class Horn : FunctionMesh
    {
        public Horn(double scale)
        {
            Init(new MultExpression(new ConstantExpression(scale), FunctionParser.Parse("-(1+.15u cos(v))cos(u)")),
                new MultExpression(new ConstantExpression(scale), FunctionParser.Parse("(1+.15u cos(v))sin(u)")),
                new MultExpression(new ConstantExpression(scale), FunctionParser.Parse("-.15u sin(v)")),
                -Math.PI/2.0, 0.0, 0.0, 2*Math.PI);
        }
    }
}