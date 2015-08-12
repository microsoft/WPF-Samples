// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SlidePuzzleDemo
{
    internal struct PuzzleCell
    {
        public PuzzleCell(int row, int col, int cellNumber)
        {
            Row = row;
            Col = col;
            CellNumber = cellNumber;
        }

        public int Row { get; }
        public int Col { get; }
        public int CellNumber { get; }
    }
}