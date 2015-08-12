// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using GraphingCalculatorDemo.Parser;
using Microsoft.Win32;

namespace GraphingCalculatorDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window //IB: was Window
    {
        private const string AnswerKey = "ans";
        private readonly string _regSaveBase = Registry.CurrentUser.Name + @"\Software\GraphCalc\Settings";
        private DrawAxisHelper _axisHelper;
        private TextBox _focusedBox;
        private IExpression _lastAnswer;
        private string _lastRendered;
        private IExpression _memory;
        private Point _selectionStart;
        private bool _selectionStarted;
        private Trackball _trackball;
        private Viewport3D _viewport;

        public MainWindow()
        {
            _memory = null;
            _lastAnswer = new ConstantExpression(0.0);
            _trackball = null;
            _viewport = null;
            VariableExpression.Define(AnswerKey, 0.0);
            Focusable = true;
        }

        private string Buffer
        {
            get { return _focusedBox.Text; }
            set
            {
                _focusedBox.Text = value;
                _focusedBox.Focus();
                _focusedBox.Select(value.Length, 0);
            }
        }

        private string ScreenBuffer
        {
            get { return screenText.Text; }
            set
            {
                screenText.Text = value;
                screenText.ScrollToEnd();
            }
        }

        private void OnLoaded(object sender, EventArgs args)
        {
            immediate.Focus();
            _viewport = new Viewport3D();
            _trackball = new Trackball();
            _trackball.Servants.Add(_viewport);
            _trackball.Attach(screen);
            _trackball.Enabled = true;

            graphNone.Header = Settings.FunctionNone;
            graph.Header = Settings.Function;
            graph2D.Header = Settings.Function2D;
            graph3D.Header = Settings.Function3D;
            graphOptions.Header = Settings.Function;
            graphOptions2D.Header = Settings.Function2D;
            graphOptions3D.Header = Settings.Function3D;

            // Workaround for registry bug (key base must exist before getting values - else default won't work)
            Registry.SetValue(_regSaveBase, "Version", "GraphCalc v.1.0", RegistryValueKind.String);

            xMin.Text = (string) Registry.GetValue(_regSaveBase, Settings.XMinReg, Settings.XMinDefault);
            xMax.Text = (string) Registry.GetValue(_regSaveBase, Settings.XMaxReg, Settings.XMaxDefault);
            yMin.Text = (string) Registry.GetValue(_regSaveBase, Settings.YMinReg, Settings.YMinDefault);
            yMax.Text = (string) Registry.GetValue(_regSaveBase, Settings.YMaxReg, Settings.YMaxDefault);

            xMinLabel.Text = Settings.XMin;
            xMaxLabel.Text = Settings.XMax;
            yMinLabel.Text = Settings.YMin;
            yMaxLabel.Text = Settings.YMax;

            xMin2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.XMin2DReg, Settings.XMin2DDefault);
            xMax2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.XMax2DReg, Settings.XMax2DDefault);
            yMin2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.YMin2DReg, Settings.YMin2DDefault);
            yMax2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.YMax2DReg, Settings.YMax2DDefault);
            tMin2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.Min2DReg, Settings.Min2DDefault);
            tMax2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.Max2DReg, Settings.Max2DDefault);
            tStep2D.Text = (string) Registry.GetValue(_regSaveBase, Settings.Step2DReg, Settings.Step2DDefault);

            xMin2DLabel.Text = Settings.XMin2D;
            xMax2DLabel.Text = Settings.XMax2D;
            yMin2DLabel.Text = Settings.YMin2D;
            yMax2DLabel.Text = Settings.YMax2D;
            tMin2DLabel.Text = Settings.Min2D;
            tMax2DLabel.Text = Settings.Max2D;
            tStep2DLabel.Text = Settings.Step2D;

            uMin.Text = (string) Registry.GetValue(_regSaveBase, Settings.UMinReg, Settings.UMinDefault);
            uMax.Text = (string) Registry.GetValue(_regSaveBase, Settings.UMaxReg, Settings.UMaxDefault);
            uGrid.Text = (string) Registry.GetValue(_regSaveBase, Settings.UGridReg, Settings.UGridDefault);
            vMin.Text = (string) Registry.GetValue(_regSaveBase, Settings.VMinReg, Settings.VMinDefault);
            vMax.Text = (string) Registry.GetValue(_regSaveBase, Settings.VMaxReg, Settings.VMaxDefault);
            vGrid.Text = (string) Registry.GetValue(_regSaveBase, Settings.VGridReg, Settings.VGridDefault);

            uMinLabel.Text = Settings.UMin;
            uMaxLabel.Text = Settings.UMax;
            uGridLabel.Text = Settings.UGrid;
            vMinLabel.Text = Settings.VMin;
            vMaxLabel.Text = Settings.VMax;
            vGridLabel.Text = Settings.VGrid;

            y.Text = (string) Registry.GetValue(_regSaveBase, Settings.YReg, Settings.YDefault);
            xt.Text = (string) Registry.GetValue(_regSaveBase, Settings.XtReg, Settings.XtDefault);
            yt.Text = (string) Registry.GetValue(_regSaveBase, Settings.YtReg, Settings.YtDefault);
            fx.Text = (string) Registry.GetValue(_regSaveBase, Settings.FxReg, Settings.FxDefault);
            fy.Text = (string) Registry.GetValue(_regSaveBase, Settings.FyReg, Settings.FyDefault);
            fz.Text = (string) Registry.GetValue(_regSaveBase, Settings.FzReg, Settings.FzDefault);

            yLabel.Text = Settings.Y;
            xtLabel.Text = Settings.Xt;
            ytLabel.Text = Settings.Yt;
            fxLabel.Text = Settings.Fx;
            fyLabel.Text = Settings.Fy;
            fzLabel.Text = Settings.Fz;
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (screenCanvas.Visibility == Visibility.Visible)
            {
                switch (_lastRendered)
                {
                    case Settings.Function:
                        GraphScene(false);
                        break;

                    case Settings.Function2D:
                        GraphScene2D(false);
                        break;

                    case Settings.Function3D:
                        GraphScene3D(false);
                        break;
                }
            }
        }

        private void ShowFunctionNone(object sender, RoutedEventArgs args)
        {
            ShowScreenText();
        }

        private void ShowFunction(object sender, RoutedEventArgs args)
        {
            ShowFunction();
        }

        private void ShowFunction2D(object sender, RoutedEventArgs args)
        {
            ShowFunction2D();
        }

        private void ShowFunction3D(object sender, RoutedEventArgs args)
        {
            ShowFunction3D();
        }

        private void ShowOptions(object sender, RoutedEventArgs args)
        {
            ShowOptions();
        }

        private void ShowOptions2D(object sender, RoutedEventArgs args)
        {
            ShowOptions2D();
        }

        private void ShowOptions3D(object sender, RoutedEventArgs args)
        {
            ShowOptions3D();
        }

        private void Clear(object sender, RoutedEventArgs args)
        {
            if (Buffer == string.Empty)
            {
                screenText.Text = string.Empty;
            }
            Buffer = string.Empty;
        }

        private void Differentiate(object sender, RoutedEventArgs args)
        {
            ScreenBuffer += "> d/dx(" + Buffer.Trim() + ")\n";
            ShowScreenText();

            try
            {
                var exp = FunctionParser.Parse(Buffer);
                exp = exp.Differentiate("x").Simplify();
                ScreenBuffer += exp.ToString() + "\n";
                Buffer = string.Empty;
            }
            catch (FunctionParserException)
            {
                ScreenBuffer += "Error!\n";
                Buffer = string.Empty;
            }
        }

        private void ComputeAnswer(object sender, RoutedEventArgs args)
        {
            if (ComputeAnswer())
            {
                ScreenBuffer += _lastAnswer.ToString() + "\n";
                Buffer = string.Empty;
            }
        }

        private bool ComputeAnswer()
        {
            immediate.Focus();
            Buffer = Buffer.Trim();

            if (Buffer.Length == 0)
            {
                Buffer = AnswerKey;
            }

            // Print what we're about to evaluate
            ScreenBuffer += "> " + Buffer.Trim() + "\n";
            ShowScreenText();

            try
            {
                var exp = FunctionParser.Parse(Buffer);
                _lastAnswer = exp.Simplify();
                if (_lastAnswer is ConstantExpression)
                {
                    VariableExpression.Define(AnswerKey, ((ConstantExpression) _lastAnswer).Value);
                }
                return true;
            }
            catch (FunctionParserException)
            {
                ScreenBuffer += "Error!\n";
                _lastAnswer = new ConstantExpression(0);
                VariableExpression.Define(AnswerKey, 0.0);
                Buffer = string.Empty;
                return false;
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Enter:
                    if (immediate.IsFocused)
                    {
                        ComputeAnswer(sender, null);
                        args.Handled = true;
                    }
                    else if (function.Visibility == Visibility.Visible)
                    {
                        GraphScene(false);
                        args.Handled = true;
                    }
                    else if (functions2D.Visibility == Visibility.Visible)
                    {
                        GraphScene2D(false);
                        args.Handled = true;
                    }
                    else if (functions3D.Visibility == Visibility.Visible)
                    {
                        GraphScene3D(false);
                        args.Handled = true;
                    }
                    break;
                case Key.Escape:
                    Clear(sender, null);
                    args.Handled = true;
                    break;

                case Key.D2:
                    if ((args.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                    {
                        AppendPow2(sender, null);
                    }
                    break;
            }
        }

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs args)
        {
            if (sender.Equals(screenText))
            {
                immediate.Focus();
                args.Handled = true;
            }
            else
            {
                _focusedBox = (TextBox) sender;
            }
        }

        private void AppendText(string text)
        {
            if (text.Length == 0)
            {
                return;
            }
            if (Buffer.Length == 0)
            {
                if (text == "+" || text == "-" || text == "*" || text == "/")
                {
                    text = AnswerKey + text;
                }
            }
            Buffer += text;
        }

        #region Toggle what's displayed on the screen

        private void ShowScreenCanvas()
        {
            screenCanvas.Visibility = Visibility.Visible;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Collapsed;
            immediate.Focus();
        }

        private void ShowScreenText()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Visible;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Collapsed;
        }

        private void ShowFunction()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Visible;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Collapsed;
        }

        private void ShowOptions()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Visible;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Collapsed;
        }

        private void ShowFunction2D()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Visible;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Collapsed;
        }

        private void ShowOptions2D()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Visible;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Collapsed;
        }

        private void ShowFunction3D()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Visible;
            options3D.Visibility = Visibility.Collapsed;
        }

        private void ShowOptions3D()
        {
            screenCanvas.Visibility = Visibility.Collapsed;
            screenText.Visibility = Visibility.Collapsed;
            function.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            functions2D.Visibility = Visibility.Collapsed;
            options2D.Visibility = Visibility.Collapsed;
            functions3D.Visibility = Visibility.Collapsed;
            options3D.Visibility = Visibility.Visible;
        }

        #endregion

        #region Graphing options and validation

        private void ShowOptions(object sender, EventArgs args)
        {
            ShowOptions();
        }

        private void SaveOptions(object sender, EventArgs args)
        {
            try
            {
                ValidateOptions();
                ShowFunction();
            }
            catch (FunctionParserException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n" + ex.InnerException.Message;
                }
                MessageBox.Show(message);
            }
        }

        private void ValidateOptions()
        {
            ValidateOption(xMin.Text, Settings.XMinReg);
            ValidateOption(xMax.Text, Settings.XMaxReg);
            ValidateOption(yMin.Text, Settings.YMinReg);
            ValidateOption(yMax.Text, Settings.YMaxReg);

            if (XMax == XMin)
            {
                throw new InvalidExpressionException("\"" + Settings.XMin + "\" and \"" + Settings.XMax +
                                                     "\" cannot have the same value");
            }
            if (XMax < XMin)
            {
                throw new InvalidExpressionException("\"" + Settings.XMin + "\" must be less than \"" + Settings.XMax +
                                                     "\"");
            }
            if (YMax == YMin)
            {
                throw new InvalidExpressionException("\"" + Settings.YMin + "\" and \"" + Settings.YMax +
                                                     "\" cannot have the same value");
            }
            if (YMax < YMin)
            {
                throw new InvalidExpressionException("\"" + Settings.YMin + "\" must be less than \"" + Settings.YMax +
                                                     "\"");
            }
        }

        private void ResetOptions(object sender, EventArgs args)
        {
            xMin.Text = Settings.XMinDefault;
            xMax.Text = Settings.XMaxDefault;
            yMin.Text = Settings.YMinDefault;
            yMax.Text = Settings.YMaxDefault;
            ValidateOptions();
        }

        private void ShowOptions2D(object sender, EventArgs args)
        {
            ShowOptions2D();
        }

        private void SaveOptions2D(object sender, EventArgs args)
        {
            try
            {
                ValidateOptions2D();
                ShowFunction2D();
            }
            catch (FunctionParserException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n" + ex.InnerException.Message;
                }
                MessageBox.Show(message);
            }
        }

        private void ValidateOptions2D()
        {
            ValidateOption(xMin2D.Text, Settings.XMin2DReg);
            ValidateOption(xMax2D.Text, Settings.XMax2DReg);
            ValidateOption(yMin2D.Text, Settings.YMin2DReg);
            ValidateOption(yMax2D.Text, Settings.YMax2DReg);
            ValidateOption(tMin2D.Text, Settings.Min2DReg);
            ValidateOption(tMax2D.Text, Settings.Max2DReg);
            ValidateOption(tStep2D.Text, Settings.Step2DReg);

            if (XMax2D == XMin2D)
            {
                throw new InvalidExpressionException("\"" + Settings.XMin2D + "\" and \"" + Settings.XMax2D +
                                                     "\" cannot have the same value");
            }
            if (XMax2D < XMin2D)
            {
                throw new InvalidExpressionException("\"" + Settings.XMin2D + "\" must be less than \"" +
                                                     Settings.XMax2D + "\"");
            }
            if (YMax2D == YMin2D)
            {
                throw new InvalidExpressionException("\"" + Settings.YMin2D + "\" and \"" + Settings.YMax2D +
                                                     "\" cannot have the same value");
            }
            if (YMax2D < YMin2D)
            {
                throw new InvalidExpressionException("\"" + Settings.YMin2D + "\" must be less than \"" +
                                                     Settings.YMax2D + "\"");
            }
        }

        private void ResetOptions2D(object sender, EventArgs args)
        {
            xMin2D.Text = Settings.XMin2DDefault;
            xMax2D.Text = Settings.XMax2DDefault;
            yMin2D.Text = Settings.YMin2DDefault;
            yMax2D.Text = Settings.YMax2DDefault;
            tMin2D.Text = Settings.Min2DDefault;
            tMax2D.Text = Settings.Max2DDefault;
            tStep2D.Text = Settings.Step2DDefault;
            ValidateOptions2D();
        }

        private void ShowOptions3D(object sender, EventArgs args)
        {
            ShowOptions3D();
        }

        private void SaveOptions3D(object sender, EventArgs args)
        {
            try
            {
                ValidateOptions3D();
                ShowFunction3D();
            }
            catch (FunctionParserException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n" + ex.InnerException.Message;
                }
                MessageBox.Show(message);
            }
        }

        private void ValidateOptions3D()
        {
            ValidateOption(uMin.Text, Settings.UMinReg);
            ValidateOption(uMax.Text, Settings.UMaxReg);
            ValidateOption(uGrid.Text, Settings.UGridReg);
            ValidateOption(vMin.Text, Settings.VMinReg);
            ValidateOption(vMax.Text, Settings.VMaxReg);
            ValidateOption(vGrid.Text, Settings.VGridReg);

            if (UGrid <= 0)
            {
                throw new InvalidExpressionException("\"" + Settings.UGrid + "\" must be greater than 0");
            }
            if (VGrid <= 0)
            {
                throw new InvalidExpressionException("\"" + Settings.VGrid + "\" must be greater than 0");
            }
        }

        private void ResetOptions3D(object sender, EventArgs args)
        {
            uMin.Text = Settings.UMinDefault;
            uMax.Text = Settings.UMaxDefault;
            uGrid.Text = Settings.UGridDefault;
            vMin.Text = Settings.VMinDefault;
            vMax.Text = Settings.VMaxDefault;
            vGrid.Text = Settings.VGridDefault;
            ValidateOptions3D();
        }

        private void ValidateOption(string value, string registryName)
        {
            try
            {
                var exp = FunctionParser.Parse(value).Simplify();
                if (!(exp is ConstantExpression))
                {
                    throw new InvalidExpressionException("The input expression must be constant");
                }
                Registry.SetValue(_regSaveBase, registryName, value, RegistryValueKind.String);
            }
            catch (FunctionParserException ex)
            {
                throw new InvalidExpressionException("Cannot save value for \"" + registryName + "\"", ex);
            }
        }

        #endregion

        #region Immediate Input

        private void AppendSin(object sender, RoutedEventArgs args)
        {
            AppendText("sin");
        }

        private void AppendCos(object sender, RoutedEventArgs args)
        {
            AppendText("cos");
        }

        private void AppendTan(object sender, RoutedEventArgs args)
        {
            AppendText("tan");
        }

        private void AppendPow2(object sender, RoutedEventArgs args)
        {
            AppendText("^2");
        }

        private void AppendPow(object sender, RoutedEventArgs args)
        {
            AppendText("^");
        }

        private void AppendPi(object sender, RoutedEventArgs args)
        {
            AppendText("pi");
        }

        private void AppendE(object sender, RoutedEventArgs args)
        {
            AppendText("e");
        }

        private void AppendFoo(object sender, RoutedEventArgs args)
        {
            AppendText("foo");
        }

        private void AppendX(object sender, RoutedEventArgs args)
        {
            AppendText("x");
        }

        private void AppendT(object sender, RoutedEventArgs args)
        {
            AppendText("t");
        }

        private void AppendU(object sender, RoutedEventArgs args)
        {
            AppendText("u");
        }

        private void AppendV(object sender, RoutedEventArgs args)
        {
            AppendText("v");
        }

        private void Append0(object sender, RoutedEventArgs args)
        {
            AppendText("0");
        }

        private void Append1(object sender, RoutedEventArgs args)
        {
            AppendText("1");
        }

        private void Append2(object sender, RoutedEventArgs args)
        {
            AppendText("2");
        }

        private void Append3(object sender, RoutedEventArgs args)
        {
            AppendText("3");
        }

        private void Append4(object sender, RoutedEventArgs args)
        {
            AppendText("4");
        }

        private void Append5(object sender, RoutedEventArgs args)
        {
            AppendText("5");
        }

        private void Append6(object sender, RoutedEventArgs args)
        {
            AppendText("6");
        }

        private void Append7(object sender, RoutedEventArgs args)
        {
            AppendText("7");
        }

        private void Append8(object sender, RoutedEventArgs args)
        {
            AppendText("8");
        }

        private void Append9(object sender, RoutedEventArgs args)
        {
            AppendText("9");
        }

        private void AppendLParen(object sender, RoutedEventArgs args)
        {
            AppendText("(");
        }

        private void AppendRParen(object sender, RoutedEventArgs args)
        {
            AppendText(")");
        }

        private void AppendMult(object sender, RoutedEventArgs args)
        {
            AppendText("*");
        }

        private void AppendDiv(object sender, RoutedEventArgs args)
        {
            AppendText("/");
        }

        private void AppendAdd(object sender, RoutedEventArgs args)
        {
            AppendText("+");
        }

        private void AppendMinus(object sender, RoutedEventArgs args)
        {
            AppendText("-");
        }

        private void AppendDecimal(object sender, RoutedEventArgs args)
        {
            AppendText(".");
        }

        private void AppendNegate(object sender, RoutedEventArgs args)
        {
            if (Buffer[Buffer.Length - 1] != '-')
            {
                AppendText("-");
            }
            else
            {
                Buffer = Buffer.Substring(0, Buffer.Length - 1);
            }
        }

        private void AppendAns(object sender, RoutedEventArgs args)
        {
            AppendText(AnswerKey);
        }

        private void Off(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Memory Operations

        private void MemoryAppend(object sender, RoutedEventArgs args)
        {
            if (ComputeAnswer())
            {
                if (_lastAnswer is ConstantExpression)
                {
                    if (_memory == null)
                    {
                        _memory = _lastAnswer;
                    }
                    else
                    {
                        _memory = new AddExpression(_memory, _lastAnswer);
                        _memory = _memory.Simplify();
                    }
                    ScreenBuffer += _memory.ToString() + "\n";
                }
                else
                {
                    ScreenBuffer += "Cannot store this value in memory\n";
                }
            }
        }

        private void MemoryRecall(object sender, RoutedEventArgs args)
        {
            if (_memory != null)
            {
                AppendText(_memory.ToString());
            }
        }

        private void MemoryClear(object sender, RoutedEventArgs args)
        {
            _memory = null;
        }

        #endregion

        #region Fill in equation fields

        private void SpiralClicked(object sender, RoutedEventArgs args)
        {
            xt.Text = Settings.XtDefault;
            yt.Text = Settings.YtDefault;
            xMin2D.Text = Settings.XMin2DDefault;
            xMax2D.Text = Settings.XMax2DDefault;
            yMin2D.Text = Settings.YMin2DDefault;
            yMax2D.Text = Settings.YMax2DDefault;
            tMin2D.Text = Settings.Min2DDefault;
            tMax2D.Text = Settings.Max2DDefault;
            tStep2D.Text = Settings.Step2DDefault;
            ValidateOptions2D();
        }

        private void EllipseClicked(object sender, RoutedEventArgs args)
        {
            xt.Text = "4cos(t)";
            yt.Text = "3sin(t)";
            xMin2D.Text = Settings.XMin2DDefault;
            xMax2D.Text = Settings.XMax2DDefault;
            yMin2D.Text = Settings.YMin2DDefault;
            yMax2D.Text = Settings.YMax2DDefault;
            tMin2D.Text = "0";
            tMax2D.Text = "2pi";
            tStep2D.Text = "pi/16";
            ValidateOptions2D();
        }

        private void SphereClicked(object sender, RoutedEventArgs args)
        {
            fx.Text = Settings.FxDefault;
            fy.Text = Settings.FyDefault;
            fz.Text = Settings.FzDefault;
            uMin.Text = Settings.UMinDefault;
            uMax.Text = Settings.UMaxDefault;
            uGrid.Text = Settings.UGridDefault;
            vMin.Text = Settings.VMinDefault;
            vMax.Text = Settings.VMaxDefault;
            vGrid.Text = Settings.VGridDefault;
            ValidateOptions3D();
        }

        private void ConeClicked(object sender, RoutedEventArgs args)
        {
            fx.Text = ".6(1.5-v)cos(u)";
            fy.Text = "v";
            fz.Text = ".6(1.5-v)sin(-u)";
            uMin.Text = "-pi";
            uMax.Text = "pi";
            uGrid.Text = "24";
            vMin.Text = "0";
            vMax.Text = "1.5";
            vGrid.Text = "12";
            ValidateOptions3D();
        }

        private void TorusClicked(object sender, RoutedEventArgs args)
        {
            fx.Text = "-(1+.25cos(v))cos(u)";
            fy.Text = "(1+.25cos(v))sin(u)";
            fz.Text = "-.25sin(v)";
            uMin.Text = "-pi";
            uMax.Text = "pi";
            uGrid.Text = "48";
            vMin.Text = "0";
            vMax.Text = "2pi";
            vGrid.Text = "24";
            ValidateOptions3D();
        }

        #endregion

        #region Graphing logic

        private double CanvasWidth => ((FrameworkElement) screenCanvas.Parent).ActualWidth;

        private double CanvasHeight => ((FrameworkElement) screenCanvas.Parent).ActualHeight;

        private Size CanvasSize => ((UIElement) screenCanvas.Parent).RenderSize;

        private void Graph(object sender, RoutedEventArgs args)
        {
            try
            {
                if (function.Visibility == Visibility.Visible)
                {
                    GraphScene(false);
                    _lastRendered = Settings.Function;
                }
                else if (functions2D.Visibility == Visibility.Visible)
                {
                    GraphScene2D(false);
                    _lastRendered = Settings.Function2D;
                }
                else if (functions3D.Visibility == Visibility.Visible)
                {
                    GraphScene3D(false);
                    _lastRendered = Settings.Function3D;
                }
                else
                {
                    ScreenBuffer += "No active functions to graph\n";
                }
            }
            catch (FunctionParserException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n" + ex.InnerException.Message;
                }
                MessageBox.Show(message, "Failed to graph equation");
            }
        }

        private void GraphScene(bool legacy)
        {
            var exp = Parse(y.Text, Settings.Y, Settings.YReg);

            var width = CanvasWidth;
            var height = CanvasHeight;
            var offsetX = -XMin;
            var offsetY = YMax;
            var graphToCanvasX = width/(XMax - XMin);
            var graphToCanvasY = height/(YMax - YMin);

            var points = new PointCollection();
            for (var x = XMin; x < XMax; x += 1/graphToCanvasX)
            {
                VariableExpression.Define("x", x);

                // Translate the origin based on the max/min parameters (y axis is flipped), then scale to canvas.
                var xCanvas = (x + offsetX)*graphToCanvasX;
                var yCanvas = (offsetY - exp.Evaluate())*graphToCanvasY;

                points.Add(ClampedPoint(xCanvas, yCanvas));
            }
            VariableExpression.Undefine("x");

            screenCanvas.Children.Clear();
            _axisHelper = new DrawAxisHelper(screenCanvas, CanvasSize);
            _axisHelper.DrawAxes(XMin, XMax, YMin, YMax);


            var graphLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Points = points
            };

            screenCanvas.Children.Add(graphLine);

            ShowScreenCanvas();
        }

        private void GraphScene2D(bool legacy)
        {
            var xExp = Parse(xt.Text, Settings.Xt, Settings.XtReg);
            var yExp = Parse(yt.Text, Settings.Yt, Settings.YtReg);

            var width = CanvasWidth;
            var height = CanvasHeight;
            var graphToCanvasX = width/(XMax2D - XMin2D);
            var graphToCanvasY = height/(YMax2D - YMin2D);

            // distance from origin of graph to origin of canvas
            var offsetX = -XMin2D;
            var offsetY = YMax2D;

            var points = new PointCollection();
            for (var t = Min2D; t <= Max2D + 0.000001; t += Step2D)
            {
                VariableExpression.Define("t", t);
                var xGraph = xExp.Evaluate();
                var yGraph = yExp.Evaluate();

                // Translate the origin based on the max/min parameters (y axis is flipped), then scale to canvas.
                var xCanvas = (xGraph + offsetX)*graphToCanvasX;
                var yCanvas = (offsetY - yGraph)*graphToCanvasY;

                points.Add(ClampedPoint(xCanvas, yCanvas));
            }
            VariableExpression.Undefine("t");

            screenCanvas.Children.Clear();
            _axisHelper = new DrawAxisHelper(screenCanvas, CanvasSize);
            _axisHelper.DrawAxes(XMin2D, XMax2D, YMin2D, YMax2D);


            var polyLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Points = points
            };
            screenCanvas.Children.Add(polyLine);

            ShowScreenCanvas();
        }

        private Point ClampedPoint(double x, double y)
        {
            if (double.IsPositiveInfinity(x) || x > CanvasWidth*2)
            {
                x = CanvasWidth*2;
            }
            else if (double.IsNegativeInfinity(x) || x < -CanvasWidth)
            {
                x = -CanvasWidth;
            }
            else if (double.IsNaN(x))
            {
                x = -CanvasWidth;
            }
            if (double.IsPositiveInfinity(y) || y > CanvasHeight*2)
            {
                y = CanvasHeight*2;
            }
            else if (double.IsNegativeInfinity(y) || y < -CanvasHeight)
            {
                y = -CanvasHeight;
            }
            else if (double.IsNaN(x))
            {
                y = -CanvasHeight;
            }
            return new Point(x, y);
        }

        private void GraphScene3D(bool legacy)
        {
            // We do this so we can get good error information.
            // The values return by these calls are ignored.
            Parse(fx.Text, Settings.Fx, Settings.FxReg);
            Parse(fy.Text, Settings.Fy, Settings.FyReg);
            Parse(fz.Text, Settings.Fz, Settings.FzReg);

            var camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 5),
                LookDirection = new Vector3D(0, 0, -2),
                UpDirection = new Vector3D(0, 1, 0),
                NearPlaneDistance = 1,
                FarPlaneDistance = 100,
                FieldOfView = 45
            };

            Model3DGroup group = null;

            if (legacy)
            {
                var model = new FunctionWireframeModel(fx.Text, fy.Text, fz.Text, UMin, UMax, VMin, VMax);
                group = model.CreateWireframeModel(UGrid + 1, VGrid + 1);
            }
            else
            {
                group = new Model3DGroup();
                var mesh = new FunctionMesh(fx.Text, fy.Text, fz.Text, UMin, UMax, VMin, VMax);
                group.Children.Add(new GeometryModel3D(mesh.CreateMesh(UGrid + 1, VGrid + 1),
                    new DiffuseMaterial(Brushes.Blue)));
                group.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1)));
            }


            // Should draw axis here.

            // Save the transform added by the trackball;            
            if (_viewport.Children.Count > 0)
            {
                group.Transform = ((Model3DGroup) ((ModelVisual3D) _viewport.Children[0]).Content).Transform;
            }

            //<newcode>
            var sceneVisual = new ModelVisual3D {Content = @group};
            _viewport.Children.Clear();
            _viewport.Children.Add(sceneVisual);
            //</newcode>


            //viewport.Models = group;
            _viewport.Camera = camera;
            _viewport.Width = CanvasWidth;
            _viewport.Height = CanvasHeight;
            _viewport.ClipToBounds = true;

            screenCanvas.Children.Clear();
            screenCanvas.Children.Add(_viewport);
            ShowScreenCanvas();
        }

        private IExpression Parse(string equation, string labelName, string registryName)
        {
            try
            {
                var exp = FunctionParser.Parse(equation);
                Registry.SetValue(_regSaveBase, registryName, equation, RegistryValueKind.String);
                return exp;
            }
            catch (FunctionParserException ex)
            {
                throw new InvalidExpressionException("Error in equation: \"" + labelName + "\"", ex);
            }
        }

        #endregion

        #region Options

        // These will be verified by the options tab before the property is accessed in real code
        // An exception should never be thrown

        private double XMin
        {
            get
            {
                var exp = FunctionParser.Parse(xMin.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double XMax
        {
            get
            {
                var exp = FunctionParser.Parse(xMax.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double YMin
        {
            get
            {
                var exp = FunctionParser.Parse(yMin.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double YMax
        {
            get
            {
                var exp = FunctionParser.Parse(yMax.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        #endregion

        #region Options 2D

        // These will be verified by the options tab before the property is accessed in real code
        // An exception should never be thrown

        private double XMin2D
        {
            get
            {
                var exp = FunctionParser.Parse(xMin2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double XMax2D
        {
            get
            {
                var exp = FunctionParser.Parse(xMax2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double YMin2D
        {
            get
            {
                var exp = FunctionParser.Parse(yMin2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double YMax2D
        {
            get
            {
                var exp = FunctionParser.Parse(yMax2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double Min2D
        {
            get
            {
                var exp = FunctionParser.Parse(tMin2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double Max2D
        {
            get
            {
                var exp = FunctionParser.Parse(tMax2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double Step2D
        {
            get
            {
                var exp = FunctionParser.Parse(tStep2D.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        #endregion

        #region Options3D

        // These will be verified by the options tab before the property is accessed in real code
        // An exception should never be thrown

        private double UMin
        {
            get
            {
                var exp = FunctionParser.Parse(uMin.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double UMax
        {
            get
            {
                var exp = FunctionParser.Parse(uMax.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private int UGrid
        {
            get
            {
                var exp = FunctionParser.Parse(uGrid.Text).Simplify();
                return (int) ((ConstantExpression) exp).Value;
            }
        }

        private double VMin
        {
            get
            {
                var exp = FunctionParser.Parse(vMin.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private double VMax
        {
            get
            {
                var exp = FunctionParser.Parse(vMax.Text).Simplify();
                return ((ConstantExpression) exp).Value;
            }
        }

        private int VGrid
        {
            get
            {
                var exp = FunctionParser.Parse(vGrid.Text).Simplify();
                return (int) ((ConstantExpression) exp).Value;
            }
        }

        #endregion

        #region Selection/Zoom logic

        private void OnCanvasClickStart(object sender, MouseButtonEventArgs args)
        {
            switch (_lastRendered)
            {
                case Settings.Function:
                case Settings.Function2D:
                    _selectionStarted = true;
                    _selectionStart = args.GetPosition(screenCanvas);
                    selection.Width = 0;
                    selection.Height = 0;
                    selection.Visibility = Visibility.Visible;
                    break;

                case Settings.Function3D:
                    break;
            }
        }

        private void OnCanvasClickFinish(object sender, MouseButtonEventArgs args)
        {
            if (_selectionStarted)
            {
                var zoomIn = new Rect(_selectionStart, args.GetPosition(screenCanvas));
                selection.Visibility = Visibility.Collapsed;
                if (zoomIn.Width <= 1 || zoomIn.Height <= 1)
                {
                    return;
                }
                if (_lastRendered == Settings.Function)
                {
                    ZoomViewportTo(zoomIn);
                }
                else if (_lastRendered == Settings.Function2D)
                {
                    ZoomViewport2DTo(zoomIn);
                }
                _selectionStarted = false;
            }
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs args)
        {
            if (_selectionStarted)
            {
                var rect = new Rect(_selectionStart, args.GetPosition(screenCanvas));
                selection.RenderTransform = new TranslateTransform(rect.X, rect.Y);
                selection.Width = rect.Width;
                selection.Height = rect.Height;
            }
        }

        private void OnCanvasRightClick(object sender, MouseEventArgs args)
        {
            if (_selectionStarted)
            {
                _selectionStarted = false;
                selection.Visibility = Visibility.Collapsed;
            }
            else
            {
                var canvasSize = CanvasSize;
                var xBorder = canvasSize.Width/2;
                var yBorder = canvasSize.Height/2;

                var zoomOut = new Rect(-xBorder, -yBorder, xBorder*4, yBorder*4);
                switch (_lastRendered)
                {
                    case Settings.Function:
                        ZoomViewportTo(zoomOut);
                        break;
                    case Settings.Function2D:
                        ZoomViewport2DTo(zoomOut);
                        break;
                }
            }
        }

        private void ZoomViewportTo(Rect canvasSelection)
        {
            var canvasSize = CanvasSize;
            var selectionOffset = new Vector(canvasSelection.X, canvasSelection.Y);
            var selectionScale = new Vector(canvasSelection.Width/canvasSize.Width,
                canvasSelection.Height/canvasSize.Height);

            var graphSize = new Size(XMax - XMin, YMax - YMin);
            var canvasToGraphScale = new Vector(graphSize.Width/canvasSize.Width, graphSize.Height/canvasSize.Height);
            graphSize.Width *= selectionScale.X;
            graphSize.Height *= selectionScale.Y;
            var graphOffset = new Vector(selectionOffset.X*canvasToGraphScale.X, selectionOffset.Y*canvasToGraphScale.Y);
            var newViewport = new Rect(XMin + graphOffset.X, YMax - graphOffset.Y, graphSize.Width, graphSize.Height);
            xMin.Text = newViewport.Left.ToString(CultureInfo.InvariantCulture);
            xMax.Text = newViewport.Right.ToString(CultureInfo.InvariantCulture);
            yMax.Text = newViewport.Top.ToString(CultureInfo.InvariantCulture);
            yMin.Text = (newViewport.Top - graphSize.Height).ToString(CultureInfo.InvariantCulture);
            ValidateOptions();
            GraphScene(false);
        }

        private void ZoomViewport2DTo(Rect canvasSelection)
        {
            var canvasSize = CanvasSize;
            var selectionOffset = new Vector(canvasSelection.X, canvasSelection.Y);
            var selectionScale = new Vector(canvasSelection.Width/canvasSize.Width,
                canvasSelection.Height/canvasSize.Height);

            var graphSize = new Size(XMax2D - XMin2D, YMax2D - YMin2D);
            var canvasToGraphScale = new Vector(graphSize.Width/canvasSize.Width, graphSize.Height/canvasSize.Height);
            graphSize.Width *= selectionScale.X;
            graphSize.Height *= selectionScale.Y;
            var graphOffset = new Vector(selectionOffset.X*canvasToGraphScale.X, selectionOffset.Y*canvasToGraphScale.Y);
            var newViewport = new Rect(XMin2D + graphOffset.X, YMax2D - graphOffset.Y, graphSize.Width, graphSize.Height);
            xMin2D.Text = newViewport.Left.ToString(CultureInfo.InvariantCulture);
            xMax2D.Text = newViewport.Right.ToString(CultureInfo.InvariantCulture);
            yMax2D.Text = newViewport.Top.ToString(CultureInfo.InvariantCulture);
            yMin2D.Text = (newViewport.Top - graphSize.Height).ToString(CultureInfo.InvariantCulture);
            ValidateOptions2D();
            GraphScene2D(false);
        }

        #endregion
    }
}