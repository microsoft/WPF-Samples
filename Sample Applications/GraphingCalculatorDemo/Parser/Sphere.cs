// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    //--------------------------------------------------------------

    public sealed class Sphere : FunctionMesh
    {
        public Sphere(double radius)
        {
            Init(new MultExpression(new ConstantExpression(radius), FunctionParser.Parse("cos(u)sin(v)")),
                new MultExpression(new ConstantExpression(radius), FunctionParser.Parse("-cos(v)")),
                new MultExpression(new ConstantExpression(radius), FunctionParser.Parse("sin(-u)sin(v)")),
                -Math.PI, Math.PI, 0.0, Math.PI);
        }
    }

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    //--------------------------------------------------------------

    /*
    public class                Apple : Function
    {
        // This will throw.  I don't support log(x) function yet
        public                  Apple( double radius )
        {
            Init( new MultExpression( radius * 0.125, FunctionParser.Parse( "cos(u)(4+3.8cos(v))" ) ),
                  new MultExpression( radius * 0.125, FunctionParser.Parse( "(cos(v)+sin(-v)-1)(1+sin(-v))log(1-pi*v/10)+7.5sin(-v)" ) ),
                  new MultExpression( radius * 0.125, FunctionParser.Parse( "sin(-u)(4+3.8cos(v))" ) ),
                  -Math.PI, Math.PI, 0.0, Math.PI )
        }
    }
    */
}