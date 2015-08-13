// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GraphingCalculatorDemo.Parser
{
    public class FunctionMesh
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
        private ArrayList _indices;
        private int _lengthX;
        private int _lengthY;
        private Vector3D[,] _normals;
        private Point3D[,] _positions;
        private Point[,] _textureCoords;
        protected double UMax;
        // - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected double UMin;
        protected double VMax;
        protected double VMin;

        protected FunctionMesh()
        {
            // We open this class up for extension only by those who promise to call Init
            //  at some time during their constructors.  If they do not, CreateMesh will fail.
        }

        public FunctionMesh(
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
        public MeshGeometry3D CreateMesh(int precisionU, int precisionV)
        {
            _lengthX = precisionU;
            _lengthY = precisionV;
            _du = (UMax - UMin)/(precisionU - 1);
            _dv = (VMax - VMin)/(precisionV - 1);

            _positions = new Point3D[_lengthX, _lengthY];
            _normals = new Vector3D[_lengthX, _lengthY];
            _textureCoords = new Point[_lengthX, _lengthY];
            _indices = new ArrayList();

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
                    _normals[x, y] = GetNormal(u, v);
                    u += _du;
                }
                v += _dv;
            }
            VariableExpression.Undefine("u");
            VariableExpression.Undefine("v");

            SetTextureCoordinates();
            SetIndices();

            var mesh = new MeshGeometry3D();
            for (var y = 0; y < _lengthY; y++)
            {
                for (var x = 0; x < _lengthX; x++)
                {
                    mesh.Positions.Add(_positions[x, y]);
                    mesh.Normals.Add(_normals[x, y]);
                    mesh.TextureCoordinates.Add(_textureCoords[x, y]);
                }
            }
            mesh.TriangleIndices = new Int32Collection();
            foreach (int index in _indices)
                mesh.TriangleIndices.Add(index);

            return mesh;
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

        /// <summary>
        ///     Returns the normal of the mesh at the indicated parameters.
        /// </summary>
        private Vector3D GetNormal(double u, double v)
        {
            var a = ToVector3D(_fxDu, _fyDu, _fzDu);
            var b = ToVector3D(_fxDv, _fyDv, _fzDv);

            var normal = Vector3D.CrossProduct(a, b);
            normal.Normalize();

            if (double.IsNaN(normal.X) || double.IsNaN(normal.Y) || double.IsNaN(normal.Z))
            {
                // This is sort-of what Michael Kallay said to do in his paper, I think...
                // It seems to work okay.

                // We take a second derivative of the equation, this time using the other variable,
                //  then we re-evaluate it.  The direction of the normal is dependent on where we
                //  are in the mesh grid
                if (a.Length < b.Length)
                {
                    a = TryToVector3DAgain(_fxDu, _fyDu, _fzDu, "v");
                    normal = v < (VMax + VMin)/2 ? Vector3D.CrossProduct(a, b) : Vector3D.CrossProduct(b, a);
                }
                else
                {
                    b = TryToVector3DAgain(_fxDv, _fyDv, _fzDv, "u");
                    normal = u < (UMax + UMin)/2 ? Vector3D.CrossProduct(a, b) : Vector3D.CrossProduct(b, a);
                }

                normal.Normalize();
                if (double.IsNaN(normal.X) || double.IsNaN(normal.Y) || double.IsNaN(normal.Z))
                {
                    Debug.Assert(false, "Persistent degenerate normal.  Bailing out!");
                    normal = new Vector3D(0, 1, 0);
                }
            }
            return normal;
        }

        private Vector3D ToVector3D(
            IExpression fx,
            IExpression fy,
            IExpression fz
            ) => new Vector3D(fx.Evaluate(), fy.Evaluate(), fz.Evaluate());

        private Vector3D TryToVector3DAgain(
            IExpression fx,
            IExpression fy,
            IExpression fz,
            string otherVar
            )
        {
            var fxDn = fx.Differentiate(otherVar).Simplify();
            var fyDn = fy.Differentiate(otherVar).Simplify();
            var fzDn = fz.Differentiate(otherVar).Simplify();

            return ToVector3D(fxDn, fyDn, fzDn);
        }

        private void SetTextureCoordinates()
        {
            var scaleU = 1.0/(_lengthX - 1);
            var scaleV = 1.0/(_lengthY - 1);
            var v = 0.0;

            for (var y = 0; y < _lengthY; y++)
            {
                var u = 0.0;
                if (y == _lengthY - 1)
                {
                    v = 1.0;
                }
                for (var x = 0; x < _lengthX; x++)
                {
                    if (x == _lengthX - 1)
                    {
                        u = 1.0;
                    }
                    // WPF textures things upside down
                    _textureCoords[x, y] = new Point(u, 1.0 - v);
                    u += scaleU;
                }
                v += scaleV;
            }
        }

        private void SetIndices()
        {
            // Connect the dots (vertices)...

            for (var y = 0; y < _lengthY - 1; y++)
            {
                var rowX = y*_lengthX;
                var nextRowX = rowX + _lengthX;
                for (var x = 0; x < _lengthX - 1; x++)
                {
                    _indices.Add(rowX + x);
                    _indices.Add(rowX + x + 1);
                    _indices.Add(nextRowX + x);
                    _indices.Add(nextRowX + x + 1);
                    _indices.Add(nextRowX + x);
                    _indices.Add(rowX + x + 1);
                }
            }
            Debug.Assert(_indices.Count == (_lengthX - 1)*(_lengthY - 1)*6, "indices = " + _indices.Count);
        }
    }
}