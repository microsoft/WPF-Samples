// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Media.Media3D;

namespace GraphingCalculatorDemo.Parser
{
    public class FunctionWireframeModel
    {
        private double _du;
        private double _dv;
        // - - - - - - - - - - - - - - - - - - - - - - - - - -

        private IExpression _fx;
        private IExpression _fxDu;
        private IExpression _fxDv;
        private IExpression _fy;
        private IExpression _fyDu;
        private IExpression _fyDv;
        private IExpression _fz;
        private IExpression _fzDu;
        private IExpression _fzDv;
        private int _lengthX;
        private int _lengthY;
        private Point3D[,] _positions;
        protected double UMax;
        // - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected double UMin;
        protected double VMax;
        protected double VMin;

        protected FunctionWireframeModel()
        {
            // We open this class up for extension only by those who promise to call Init
            //  at some time during their constructors.  If they do not, CreateMesh will fail.
        }

        public FunctionWireframeModel(
            string fx,
            string fy,
            string fz,
            double uMin,
            double uMax,
            double vMin,
            double vMax
            )
        {
            Init(
                FunctionParser.Parse(fx),
                FunctionParser.Parse(fy),
                FunctionParser.Parse(fz),
                uMin,
                uMax,
                vMin,
                vMax
                );
        }

        protected Point3D this[int x, int y] => _positions[x, y];

        protected void Init(
            IExpression fx,
            IExpression fy,
            IExpression fz,
            double uMin,
            double uMax,
            double vMin,
            double vMax
            )
        {
            _fx = fx.Simplify();
            _fy = fy.Simplify();
            _fz = fz.Simplify();

            _fxDu = _fx.Differentiate("u").Simplify();
            _fxDv = _fx.Differentiate("v").Simplify();
            _fyDu = _fy.Differentiate("u").Simplify();
            _fyDv = _fy.Differentiate("v").Simplify();
            _fzDu = _fz.Differentiate("u").Simplify();
            _fzDv = _fz.Differentiate("v").Simplify();

            UMin = uMin;
            UMax = uMax;
            VMin = vMin;
            VMax = vMax;
        }

        /// <summary>
        ///     Creates a mesh with a horizontal and vertical resolution indicated
        ///     by the input parameters.  It will generate
        ///     (precisionU-1)*(precisionV-1) quads.
        /// </summary>
        public Model3DGroup CreateWireframeModel(int precisionU, int precisionV)
        {
            _lengthX = precisionU;
            _lengthY = precisionV;
            _du = (UMax - UMin)/(precisionU - 1);
            _dv = (VMax - VMin)/(precisionV - 1);

            _positions = new Point3D[_lengthX, _lengthY];

            var v = VMin;
            for (var y = 0; y < _lengthY; y++)
            {
                var u = UMin;
                if (y == _lengthY - 1)
                {
                    v = VMax;
                }
                for (var x = 0; x < _lengthX; x++)
                {
                    if (x == _lengthX - 1)
                    {
                        u = UMax;
                    }
                    VariableExpression.Define("u", u);
                    VariableExpression.Define("v", v);
                    _positions[x, y] = Evaluate();
                    u += _du;
                }
                v += _dv;
            }
            VariableExpression.Undefine("u");
            VariableExpression.Undefine("v");

            var group = new Model3DGroup();

            // TODO: Remove
            //ScreenSpaceLines3D lines;
            /*
            // Create Horizontal lines
            for ( int y = 0; y < lengthY; y++ )
            {
                lines = new ScreenSpaceLines3D();
                lines.Color = Colors.Black;
                lines.Thickness = 1;
                for ( int x = 0; x < lengthX; x++ )
                {
                    lines.Points.Add( positions[ x,y ] );
                }
                group.Children.Add( lines );
            }

            // Create Vertical lines
            for ( int x = 0; x < lengthX; x++ )
            {
                lines = new ScreenSpaceLines3D();
                lines.Color = Colors.Black;
                lines.Thickness = 1;
                for ( int y = 0; y < lengthY; y++ )
                {
                    lines.Points.Add( positions[ x,y ] );
                }
                group.Children.Add( lines );
            }
            */

            return group;
        }

        /// <summary>
        ///     Evaluates the mesh position at the indicated parameters.
        /// </summary>
        private Point3D Evaluate()
        {
            var p = new Point3D
            {
                X = _fx.Evaluate(),
                Y = _fy.Evaluate(),
                Z = _fz.Evaluate()
            };


            return p;
        }
    }
}