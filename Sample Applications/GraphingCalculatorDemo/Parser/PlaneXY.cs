// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    public sealed class PlaneXy : FunctionMesh
    {
        public PlaneXy(double lengthX, double lengthY)
        {
            Init(new VariableExpression("u"),
                new VariableExpression("v"),
                new ConstantExpression(0.0),
                -lengthX/2.0, lengthX/2.0,
                -lengthY/2.0, lengthY/2.0
                );
        }
    }
}