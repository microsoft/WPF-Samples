// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GraphingCalculatorDemo.Parser
{
    public interface IExpression
    {
        double Evaluate();
        IExpression Differentiate(string byVar);
        IExpression Simplify();
        string ToString();
    }
}