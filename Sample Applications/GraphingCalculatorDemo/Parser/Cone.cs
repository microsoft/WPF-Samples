// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    public sealed class Cone : FunctionMesh
    {
        public Cone(double radius, double height)
        {
            IExpression fx = new MultExpression(
                new MultExpression(
                    new SubExpression(
                        new ConstantExpression(height),
                        new VariableExpression("v")),
                    new ConstantExpression(radius/height)),
                new CosineExpression(
                    new VariableExpression("u")));
            IExpression fy = new VariableExpression("v");
            IExpression fz = new MultExpression(
                new MultExpression(
                    new SubExpression(
                        new ConstantExpression(height),
                        new VariableExpression("v")),
                    new ConstantExpression(radius/height)),
                new SineExpression(
                    new NegateExpression(
                        new VariableExpression("u"))));

            Init(fx, fy, fz, -Math.PI, Math.PI, 0.0, height);
        }
    }
}