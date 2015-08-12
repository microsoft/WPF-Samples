// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SlidePuzzleDemo
{
    /// <summary>
    ///     Interaction logic for PuzzleGrid.xaml
    /// </summary>
    public partial class PuzzleGrid : Grid
    {
        #region Brush Tiling Logic

        private void PuzzleGridLoaded(object sender, RoutedEventArgs e)
        {
            if (IsApplyingStyle)
            {
                Width = _masterPuzzleSize.Width;
                Height = _masterPuzzleSize.Height;

                var edgeSize = 1.0f/_numRows;
                var pieceRowHeight = _masterPuzzleSize.Height/_numRows;
                var pieceColWidth = _masterPuzzleSize.Width/_numRows;

                // Set up viewbox appropriately for each tile.
                foreach (Button b in Children)
                {
                    var root = (Border) b.Template.FindName("TheTemplateRoot", b);

                    var row = (int) b.GetValue(RowProperty);
                    var col = (int) b.GetValue(ColumnProperty);

                    var vb = new VisualBrush(_elementForPuzzle)
                    {
                        Viewbox = new Rect(col*edgeSize, row*edgeSize, edgeSize, edgeSize),
                        ViewboxUnits = BrushMappingMode.RelativeToBoundingBox
                    };

                    root.Background = vb;
                    root.Height = pieceRowHeight;
                    root.Width = pieceColWidth;
                }
            }
        }

        #endregion

        #region Grid Setup Logic

        public PuzzleGrid()
        {
            InitializeComponent();

            // Centralize handling of all clicks in the puzzle grid.
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClick));
        }

        private void SetupThePuzzleGridStructure()
        {
            _puzzleLogic = new PuzzleLogic(_numRows);

            // Define rows and columns in the Grid
            for (var row = 0; row < _numRows; row++)
            {
                var r = new RowDefinition {Height = GridLength.Auto};
                RowDefinitions.Add(r);

                var c = new ColumnDefinition {Width = GridLength.Auto};
                ColumnDefinitions.Add(c);
            }

            var buttonStyle = (Style) Resources["PuzzleButtonStyle"];

            // Now add the buttons in
            var i = 1;
            for (var row = 0; row < _numRows; row++)
            {
                for (var col = 0; col < _numRows; col++)
                {
                    // lower right cell is empty
                    if (_numRows != 1 && row == _numRows - 1 && col == _numRows - 1)
                    {
                        continue;
                    }
                    var b = new Button {FontSize = 24};

                    // Styling comes in only here...
                    if (IsApplyingStyle)
                    {
                        b.Style = buttonStyle;
                    }

                    b.SetValue(RowProperty, row);
                    b.SetValue(ColumnProperty, col);
                    b.Content = i.ToString();
                    i++;
                    Children.Add(b);
                }
            }
        }

        #endregion

        #region Interaction Logic

        private void OnPuzzleButtonClick(object sender, RoutedEventArgs e)
        {
            var b = e.Source as Button;
            if (b != null)
            {
                var row = (int) b.GetValue(RowProperty);
                var col = (int) b.GetValue(ColumnProperty);

                var moveStatus = _puzzleLogic.GetMoveStatus(row, col);

                MoveMade?.Invoke(this, new HandledEventArgs(moveStatus != MoveStatus.BadMove));

                if (moveStatus != MoveStatus.BadMove)
                {
                    if (!IsApplyingStyle || !ShouldAnimateInteractions)
                    {
                        // Not templated, just move piece
                        MovePiece(b, row, col);
                    }
                    else
                    {
                        AnimatePiece(b, row, col, moveStatus);
                    }
                }
            }
        }


        // Assumed to be a valid move.
        private void AnimatePiece(Button b, int row, int col, MoveStatus moveStatus)
        {
            var root = (FrameworkElement) b.Template.FindName("TheTemplateRoot", b);

            double distance;
            bool isMoveHorizontal;

            Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
            {
                isMoveHorizontal = true;
                distance = (moveStatus == MoveStatus.Left ? -1 : 1)*root.Width;
            }
            else
            {
                isMoveHorizontal = false;
                distance = (moveStatus == MoveStatus.Up ? -1 : 1)*root.Height;
            }

            // pull the animation after it's complete, because we move change Grid cells.
            var slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.3), FillBehavior.Stop);
            slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
            {
                // Anonymous delegate -- invoke when done.
                var clock = (Clock) sender2;
                if (clock.CurrentState != ClockState.Active)
                {
                    // remove the render transform and really move the piece in the Grid.
                    MovePiece(b, row, col);
                }
            };

            var directionProperty =
                isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

            root.RenderTransform.BeginAnimation(directionProperty, slideAnim);
        }


        // Assumed to be a valid move.
        private void MovePiece(Button b, int row, int col)
        {
            var newPosition = _puzzleLogic.MovePiece(row, col);

            b.SetValue(ColumnProperty, newPosition.Col);
            b.SetValue(RowProperty, newPosition.Row);

            if (_puzzleLogic.CheckForWin())
            {
                PuzzleWon?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Less Consequential Portions

        public void MixUpPuzzle()
        {
            _puzzleLogic.MixUpPuzzle();

            short cellNumber = 1;
            foreach (Button b in Children)
            {
                var location = _puzzleLogic.FindCell(cellNumber++);
                b.SetValue(ColumnProperty, location.Col);
                b.SetValue(RowProperty, location.Row);
            }
        }

        public void ShowNumbers(bool showNumbers)
        {
            var nlb = ((SolidColorBrush) Resources["NumberLabelBrush"]).Clone();
            nlb.Color = showNumbers ? Colors.Yellow : Colors.Transparent;
            Resources["NumberLabelBrush"] = nlb;
        }

        #endregion

        #region Public Properties

        public int NumRows
        {
            get { return _numRows; }
            set
            {
                // Only support setting this once per PuzzleGrid instance.
                if (_numRows != -1)
                {
                    throw new InvalidOperationException("NumRows already initialized for this PuzzleGrid instance.");
                }
                _numRows = value;
                CheckToSetup();
            }
        }

        public UIElement ElementToChopUp
        {
            get { return _elementForPuzzle; }
            set
            {
                if (_elementForPuzzle != null)
                {
                    throw new InvalidOperationException(
                        "ElementForPuzzle already initialized for this PuzzleGrid instance.");
                }
                _elementForPuzzle = value;
                CheckToSetup();
            }
        }

        public bool IsApplyingStyle { get; set; } = true;

        public bool ShouldAnimateInteractions { get; set; } = false;

        public Size PuzzleSize
        {
            get { return _masterPuzzleSize; }
            set
            {
                if (!_masterPuzzleSize.IsEmpty)
                {
                    throw new InvalidOperationException(
                        "SizeForPuzzle already initialized for this PuzzleGrid instance.");
                }
                _masterPuzzleSize = value;
                CheckToSetup();
            }
        }

        public event EventHandler PuzzleWon;
        public event EventHandler<HandledEventArgs> MoveMade;

        private void CheckToSetup()
        {
            if (_numRows != -1 && (!IsApplyingStyle || _elementForPuzzle != null) && !_masterPuzzleSize.IsEmpty)
            {
                SetupThePuzzleGridStructure();
            }
        }

        #endregion

        #region Private Data

        private PuzzleLogic _puzzleLogic;
        private Size _masterPuzzleSize = Size.Empty;
        private UIElement _elementForPuzzle;
        private int _numRows = -1;

        #endregion
    }
}