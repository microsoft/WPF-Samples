// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace SlidePuzzleDemo
{
    internal class PuzzleLogic
    {
        public PuzzleLogic(int numRows)
        {
            _numRows = numRows;
            _cells = new short[_numRows, _numRows];

            short tileNumber = 1;
            for (var r = 0; r < _numRows; r++)
            {
                for (var c = 0; c < _numRows; c++)
                {
                    _cells[r, c] = tileNumber++;
                }
            }

            // overwrite empty cell
            _emptyCol = _numRows - 1;
            _emptyRow = _numRows - 1;
            _cells[_emptyRow, _emptyCol] = EmptyCellId;
        }

        public bool IsEmptyCell(int row, int col) => (row == _emptyRow && col == _emptyCol);

        public MoveStatus GetMoveStatus(int row, int col)
        {
            var rowDiff = _emptyRow - row;
            var colDiff = _emptyCol - col;

            if (rowDiff == 0 && colDiff == 1)
            {
                return MoveStatus.Right;
            }
            if (rowDiff == 0 && colDiff == -1)
            {
                return MoveStatus.Left;
            }
            if (colDiff == 0 && rowDiff == 1)
            {
                return MoveStatus.Down;
            }
            if (colDiff == 0 && rowDiff == -1)
            {
                return MoveStatus.Up;
            }
            return MoveStatus.BadMove;
        }

        /// <summary>
        ///     Moves the piece
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>The cell of the newly opened position</returns>
        public PuzzleCell MovePiece(int row, int col)
        {
            Debug.Assert(GetMoveStatus(row, col) != MoveStatus.BadMove);

            var cell = new PuzzleCell(_emptyRow, _emptyCol, EmptyCellId);

            var origCell = _cells[row, col];

            _cells[_emptyRow, _emptyCol] = origCell;
            _cells[row, col] = EmptyCellId;

            _emptyCol = col;
            _emptyRow = row;


            return cell;
        }

        public bool CheckForWin()
        {
            // Easy out with check for empty cell
            if (_emptyRow != _numRows - 1 || _emptyCol != _numRows - 1)
            {
                return false;
            }

            // Just walk through cells and make sure they're all consecutive values
            short tileNumber = 1;
            for (var r = 0; r < _numRows; r++)
            {
                for (var c = 0; c < _numRows; c++)
                {
                    if (tileNumber++ != _cells[r, c])
                    {
                        // Something is in the wrong place, unless we hit the empty cell.
                        if (!(r == _numRows - 1 && c == _numRows - 1))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public PuzzleCell FindCell(short cellNumber)
        {
            Debug.Assert(cellNumber < _numRows*_numRows && cellNumber > 0);

            // This is a slow, linear operation, but for the size puzzles we have, it doesn't matter.
            for (var r = 0; r < _numRows; r++)
            {
                for (var c = 0; c < _numRows; c++)
                {
                    if (_cells[r, c] == cellNumber)
                    {
                        return new PuzzleCell(r, c, cellNumber);
                    }
                }
            }

            Debug.Assert(false, "Should have found a matching cell");
            return new PuzzleCell(-1, -1, -1);
        }

        public void MixUpPuzzle()
        {
            // Ensure that we can still solve it by only emulating legal moves.
            var r = new Random();
            var i = 8*_numRows*_numRows; // fairly arbitrary choice of number of moves
            while (i > 0)
            {
                var choice = r.Next(4);
                var row = -1;
                var col = -1;

                // 0,1,2,3 - left, right, up, down from empty cell, when possible.  Skip when not.
                switch (choice)
                {
                    case 0:
                        if (_emptyCol != 0)
                        {
                            col = _emptyCol - 1;
                            row = _emptyRow;
                        }
                        break;

                    case 1:
                        if (_emptyCol != _numRows - 1)
                        {
                            col = _emptyCol + 1;
                            row = _emptyRow;
                        }
                        break;

                    case 2:
                        if (_emptyRow != 0)
                        {
                            row = _emptyRow - 1;
                            col = _emptyCol;
                        }
                        break;

                    case 3:
                        if (_emptyRow != _numRows - 1)
                        {
                            row = _emptyRow + 1;
                            col = _emptyCol;
                        }
                        break;
                }
                if (row != -1)
                {
                    MovePiece(row, col);
                    i--;
                }
            }
        }

        #region Private Data

        private int _emptyCol;
        private int _emptyRow;
        private readonly int _numRows;
        private readonly short[,] _cells;
        private const short EmptyCellId = -1;

        #endregion
    }
}