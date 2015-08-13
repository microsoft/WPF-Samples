// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vector
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowVars();
        }

        public void PerformOperation(object sender, RoutedEventArgs e)
        {
            var li = sender as RadioButton;

            // Strings used to display results
            string syntaxString, resultType, operationString;

            switch (li?.Name)
            {
                //begin switch

                case "rb1":
                {
                    // Translates a Point by a Vector using the overloaded + operator.

                    var point1 = new Point(10, 5);
                    var vector1 = new System.Windows.Vector(20, 30);

                    var pointResult = point1 + vector1;

                    // pointResult is equal to (-10,-25) 

                    // Displaying Results
                    syntaxString = "pointResult = point1 + vector1;";
                    resultType = "Point";
                    operationString = "Translating a Point by a Vector";
                    ShowResults(pointResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb2":
                {
                    // Adds a Vector to a Vector using the overloaded + operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);


                    // vectorResult is equal to (65,100)
                    var vectorResult = vector1 + vector2;


                    // Displaying Results
                    syntaxString = "vectorResult = vector1 + vector2;";
                    resultType = "Vector";
                    operationString = "Adding a Vector to a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb3":
                {
                    // Adds a Vector to a Vector using the static Add method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var vectorResult = System.Windows.Vector.Add(vector1, vector2);

                    // vectorResult is equal to (65,100)

                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Add(vector1, vector2);";
                    resultType = "Vector";
                    operationString = "Adding a Vector to a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb4":
                {
                    // Translates a Point by a Vector using the static Add method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var point1 = new Point(10, 5);

                    var pointResult = System.Windows.Vector.Add(vector1, point1);

                    // vectorResult is equal to (30,35)

                    // Displaying Results
                    syntaxString = "pointResult = Vector.Add(vector1, point1);";
                    resultType = "Point";
                    operationString = "Translating a Point by a Vector";
                    ShowResults(pointResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb5":
                {
                    // Subtracts a Vector from a Vector using the overloaded - operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var vectorResult = vector1 - vector2;

                    // vector Result is equal to (-25, -40)

                    // Displaying Results 
                    syntaxString = "vectorResult = vector1 - vector2;";
                    resultType = "Vector";
                    operationString = "Subtracting a Vector from a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb6":
                {
                    // Subtracts a Vector from a Vector using the static Subtract method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var vectorResult = System.Windows.Vector.Subtract(vector1, vector2);

                    // vector Result is equal to (-25, -40)

                    // Displaying Results
                    syntaxString = "Vector.Subtract(vector1, vector2);";
                    resultType = "Vector";
                    operationString = "Subtracting a Vector from a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }
                case "rb7":
                {
                    // Multiplies a Vector by a Scalar using the overloaded * operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    double scalar1 = 75;

                    var vectorResult = vector1*scalar1;

                    // vectorResult is equal to (1500,2250)

                    // Displaying Results
                    syntaxString = "vectorResult = vector1 * scalar1;";
                    resultType = "Vector";
                    operationString = "Multiplies a Vector by a Scalar";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb8":
                {
                    // Multiplies a Scalar by a Vector using the overloaded * operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    double scalar1 = 75;

                    var vectorResult = scalar1*vector1;

                    // vectorResult is equal to (1500,2250)

                    // Displaying Results
                    syntaxString = "vectorResult = scalar1 * vector1;";
                    resultType = "Vector";
                    operationString = "Multiplies a Scalar by a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }
                case "rb9":
                {
                    // Multiplies a Vector by a Vector using the overloaded * operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var doubleResult = vector1*vector2;

                    // doubleResult is equal to 3000

                    // Displaying Results
                    syntaxString = "doubleResult = vector1 * vector2;";
                    resultType = "Double";
                    operationString = "Multiplies a Vector by a Vector";
                    ShowResults(doubleResult.ToString(CultureInfo.InvariantCulture), syntaxString, resultType,
                        operationString);
                    break;
                }

                case "rb10":
                {
                    // Multiplies a Vector by a Matrix using the overloaded * operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var matrix1 = new Matrix(40, 50, 60, 70, 80, 90);

                    var vectorResult = vector1*matrix1;

                    // vector Result is equal to (2600,3100)


                    // Displaying Results
                    syntaxString = "vectorResult = vector1 * matrix1;";
                    resultType = "Vector";
                    operationString = "Multiplies a Vector by a Matrix";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb11":
                {
                    // Multiplies a Vector by a Scalar using the static Multiply method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    double scalar1 = 75;

                    var vectorResult = System.Windows.Vector.Multiply(vector1, scalar1);

                    // vectorResult is equal to (1500,2250)

                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Multiply(vector1, scalar1);";
                    resultType = "Vector";
                    operationString = "Multiplies a Vector by a Scalar";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb12":
                {
                    // Multiplies a Scalar by a Vector using the static Multiply method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    double scalar1 = 75;

                    var vectorResult = System.Windows.Vector.Multiply(scalar1, vector1);

                    // vectorResult is equal to (1500,2250)

                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Multiply(scalar1, vector1);";
                    resultType = "Vector";
                    operationString = "Multiplies a Scalar by a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb13":
                {
                    // Multiplies a Vector by a Vector using the static Multiply method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var doubleResult = System.Windows.Vector.Multiply(vector1, vector2);

                    // doubleResult is equal to 3000

                    // Displaying Results
                    syntaxString = "DoubleResult = Vector.Multiply(vector1,vector2);";
                    resultType = "Double";
                    operationString = "Multiplies a Vector by a Vector";
                    ShowResults(doubleResult.ToString(CultureInfo.InvariantCulture), syntaxString, resultType,
                        operationString);
                    break;
                }

                case "rb14":
                {
                    // Multiplies a Vector by a Matrix using the static Multiply method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var matrix1 = new Matrix(40, 50, 60, 70, 80, 90);

                    var vectorResult = System.Windows.Vector.Multiply(vector1, matrix1);

                    // vector Result is equal to (2600,3100)


                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Multiply(vector1,matrix1);";
                    resultType = "Vector";
                    operationString = "Multiplies a Vector by a Matrix";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb15":
                {
                    // Divides a Vector by a Scalar using the overloaded / operator.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    double scalar1 = 75;

                    var vectorResult = vector1/scalar1;

                    // vectorResult is approximately equal to (0.26667,0.4)

                    // Displaying Results
                    syntaxString = "vectorResult = vector1 / scalar1;";
                    resultType = "Vector";
                    operationString = "Dividing a Vector by a Scalar";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }
                case "rb16":
                {
                    // Divides a Vector by a Double using the static Divide method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    double scalar1 = 75;

                    var vectorResult = System.Windows.Vector.Divide(vector1, scalar1);

                    // vectorResult is approximately equal to (0.26667,0.4)

                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Divide(vector1, scalar1);";
                    resultType = "Vector";
                    operationString = "Dividing a Vector by a Scalar";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb17":
                {
                    // Gets the hashcode of a Vector structure

                    var vector1 = new System.Windows.Vector(20, 30);

                    var vectorHashCode = vector1.GetHashCode();

                    // Displaying Results
                    syntaxString = "vectorHashCode = vector1.GetHashCode();";
                    resultType = "int";
                    operationString = "Getting the hashcode of a Vector";
                    ShowResults(vectorHashCode.ToString(), syntaxString, resultType, operationString);
                    break;
                }


                case "rb18":
                {
                    // Gets the length of a Vector.  

                    var vector1 = new System.Windows.Vector(20, 30);

                    var length = vector1.Length;

                    // length is approximately equal to 36.0555


                    // Displaying Results
                    syntaxString = "length = vector1.Length();";
                    resultType = "Double";
                    operationString = "Getting the length of a Vector";
                    ShowResults(length.ToString(CultureInfo.InvariantCulture), syntaxString, resultType, operationString);
                    break;
                }

                case "rb19":
                {
                    // Gets the square of the length of a Vector.  

                    var vector1 = new System.Windows.Vector(20, 30);

                    var lengthSq = vector1.LengthSquared;

                    // lengthSq is equal to 1300

                    // Displaying Results
                    syntaxString = "lengthSq = vector1.LengthSquared;";
                    resultType = "Double";
                    operationString = "Getting the length square of a Vector";
                    ShowResults(lengthSq.ToString(CultureInfo.InvariantCulture), syntaxString, resultType,
                        operationString);
                    break;
                }
                case "rb20":
                {
                    // Normalizes a Vector using the Normalize method.  

                    var vector1 = new System.Windows.Vector(20, 30);

                    vector1.Normalize();

                    // vector1 is approximately equal to (0.5547,0.8321) 

                    // Displaying Results
                    syntaxString = "vector1.Normalize();";
                    resultType = "Vector";
                    operationString = "Normalizing a Vector";
                    ShowResults(vector1.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb21":
                {
                    // Calculates the angle between two Vectors using the static AngleBetween method. 

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var angleBetween = System.Windows.Vector.AngleBetween(vector1, vector2);

                    // angleBetween is approximately equal to 0.9548

                    // Displaying Results
                    syntaxString = "angleBetween = Vector.AngleBetween(vector1, vector2);";
                    resultType = "Double";
                    operationString = "Calculating the angle between two Vectors";
                    ShowResults(angleBetween.ToString(CultureInfo.InvariantCulture), syntaxString, resultType,
                        operationString);
                    break;
                }

                case "rb22":
                {
                    // Calculates the cross product of two Vectors using the static  CrossProduct method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var crossProduct = System.Windows.Vector.CrossProduct(vector1, vector2);

                    // crossProduct is equal to 50

                    // Displaying Results
                    syntaxString = "crossProduct = Vector.CrossProduct(vector1,vector2);";
                    resultType = "Double";
                    operationString = "Calculating the crossproduct of two Vectors";
                    ShowResults(crossProduct.ToString(CultureInfo.InvariantCulture), syntaxString, resultType,
                        operationString);
                    break;
                }


                case "rb23":
                {
                    // Calculates the determinant of two Vectors using the static Determinant method.  

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var determinant = System.Windows.Vector.Determinant(vector1, vector2);

                    // determinant is equal to 50

                    // Displaying Results
                    syntaxString = "determinant = Vector.Determinant(vector1, vector2);";
                    resultType = "Double";
                    operationString = "Calculating the determinant of two Vectors";
                    ShowResults(determinant.ToString(CultureInfo.InvariantCulture), syntaxString, resultType,
                        operationString);
                    break;
                }


                case "rb24":
                {
                    // Checks if two Vectors are equal using the overloaded equality operator.

                    // Declaring vecto1 and initializing x,y values
                    var vector1 = new System.Windows.Vector(20, 30);

                    // Declaring vector2 without initializing x,y values
                    var vector2 = new System.Windows.Vector
                    {
                        X = 45,
                        Y = 70
                    };

                    // Boolean to hold the result of the comparison

                    // assigning values to vector2

                    // Comparing Vectors for equality
                    var areEqual = (vector1 == vector2);

                    // areEqual is False

                    // Displaying Results
                    syntaxString = "areEqual = (vector1 == vector2);";
                    resultType = "Boolean";
                    operationString = "Checking if two vectors are equal";
                    ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);
                    break;
                }


                case "rb25":
                {
                    // Checks if two Vectors are equal using the static Equals method.

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var areEqual = System.Windows.Vector.Equals(vector1, vector2);

                    // areEqual is False    

                    // Displaying Results
                    syntaxString = "areEqual = Vector.Equals(vector1, vector2);";
                    resultType = "Boolean";
                    operationString = "Checking if two vectors are equal";
                    ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);
                    break;
                }
                case "rb26":
                {
                    // Compares an Object and a Vector for equality using the non-static Equals method.

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var areEqual = vector1.Equals(vector2);

                    // areEqual is False    


                    // Displaying Results
                    syntaxString = "areEqual = vector1.Equals(vector2);";
                    resultType = "Boolean";
                    operationString = "Checking if two vectors are equal";
                    ShowResults(areEqual.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb27":
                {
                    // Converts a string representation of a vector into a Vector structure

                    var vectorResult = System.Windows.Vector.Parse("1,3");

                    // vectorResult is equal to (1,3)

                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Parse(\"1,3\");";
                    resultType = "Vector";
                    operationString = "Converting a string into a Vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb28":
                {
                    // Checks if two Vectors are not equal using the overloaded inequality operator.

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    var areNotEqual = (vector1 != vector2);

                    // areNotEqual is True

                    // Displaying Results
                    syntaxString = "areNotEqual = (vector1 != vector2);";
                    resultType = "Boolean";
                    operationString = "Checking if two points are not equal";
                    ShowResults(areNotEqual.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb29":
                {
                    // Negates a Vector using the Negate method.

                    var vector1 = new System.Windows.Vector(20, 30);

                    vector1.Negate();

                    // vector1 is equal to (-20, -30)

                    // Displaying Results
                    syntaxString = "vector1.Negate();";
                    resultType = "void";
                    operationString = "Negating a vector";
                    ShowResults(vector1.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb30":
                {
                    // Negates a Vector using the overloaded unary negation operator.

                    var vector1 = new System.Windows.Vector(20, 30);

                    var vectorResult = -vector1;

                    // vectorResult is equal to (-20, -30)

                    // Displaying Results
                    syntaxString = "vectorResult = -vector1;";
                    resultType = "Vector";
                    operationString = "Negating a vector";
                    ShowResults(vectorResult.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb31":
                {
                    // Gets a String representation of a Vector structure

                    var vector1 = new System.Windows.Vector(20, 30);

                    var vectorString = vector1.ToString();

                    // vectorString is equal to 10,5

                    // Displaying Results
                    syntaxString = "vectorString = vector1.ToString();";
                    resultType = "String";
                    operationString = "Getting the string representation of a Vector";
                    ShowResults(vectorString, syntaxString, resultType, operationString);
                    break;
                }
                case "rb32":
                {
                    // Explicitly converts a Vector structure into a Size structure
                    // Returns a Size.

                    var vector1 = new System.Windows.Vector(20, 30);

                    var size1 = (Size) vector1;

                    // size1 has a width of 20 and a height of 30

                    // Displaying Results
                    syntaxString = "size1 = (Size)vector1;";
                    resultType = "Size";
                    operationString = "Expliciting casting a Vector into a Size";
                    ShowResults(size1.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                case "rb33":
                {
                    // Explicitly converts a Vector structure into a Point structure
                    // Returns a Point.

                    var vector1 = new System.Windows.Vector(20, 30);

                    var point1 = (Point) vector1;

                    // point1 is equal to (20, 30)

                    // Displaying Results
                    syntaxString = "point1 = (Point)vector1;";
                    resultType = "Point";
                    operationString = "Expliciting casting a Vector into a Point";
                    ShowResults(point1.ToString(), syntaxString, resultType, operationString);
                    break;
                }

                // task example.  this case statement is not referenced from the list of radio buttons
                case "rb40":
                {
                    // adds two vectors using Add and +

                    var vector1 = new System.Windows.Vector(20, 30);
                    var vector2 = new System.Windows.Vector(45, 70);

                    vector1 = vector1 + vector2;
                    // vector1 is now equal to (65, 100)

                    vector1 = System.Windows.Vector.Add(vector1, vector2);
                    // vector1 is now equal to (110, 170)

                    // Displaying Results
                    syntaxString = "vectorResult = Vector.Negate(vector1);";
                    resultType = "Vector";
                    operationString = "Negating a vector";
                    ShowResults(vector1.ToString(), syntaxString, resultType, operationString);
                    break;
                }
            } // end switch
        }

        // Method to display the results of the operations
        private void ShowResults(string resultValue, string syntax, string resultType, string opString)
        {
            // Displays the results of the operation
            txtResultValue.Text = resultValue;
            txtSyntax.Text = syntax;
            txtResultType.Text = resultType;
            txtOperation.Text = opString;
        }

        // Method to display the variables used in the operations
        public void ShowVars()
        {
            // Displays the values of the variables
            var p1 = new Point(10, 5);
            var p2 = new Point(15, 40);

            var v1 = new System.Windows.Vector(20, 30);
            var v2 = new System.Windows.Vector(45, 70);

            var m1 = new Matrix(40, 50, 60, 70, 80, 90);

            double s1 = 75;

            txtPoint1.Text = p1.ToString();
            txtPoint2.Text = p2.ToString();
            txtVector1.Text = v1.ToString();
            txtVector2.Text = v2.ToString();
            txtMatrix1.Text = m1.ToString();
            txtScalar1.Text = s1.ToString(CultureInfo.InvariantCulture);
        }
    }
}