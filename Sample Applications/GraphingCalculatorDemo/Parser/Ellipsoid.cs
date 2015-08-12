// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    public sealed class Ellipsoid : FunctionMesh
    {
        public Ellipsoid(double ax, double by, double cz)
        {
            Init(new MultExpression(new ConstantExpression(ax), FunctionParser.Parse("cos(u)sin(v)")),
                new MultExpression(new ConstantExpression(by), FunctionParser.Parse("-cos(v)")),
                new MultExpression(new ConstantExpression(cz), FunctionParser.Parse("sin(-u)sin(v)")),
                -Math.PI, Math.PI, 0.0, Math.PI);
        }
    }
}