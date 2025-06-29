using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    /// <summary>
    /// Provides means to solve a sudoku.
    /// </summary>
    internal class SudokuSolver
    {
        #region .: Private Fields :.

        private static SudokuGrid grid = new();

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Solves a sudoku.
        /// </summary>
        /// <param name="sudoku">Sudoku to be solved.</param>
        /// <returns>Solved sudoku.</returns>
        public static SudokuGrid Solve(SudokuGrid sudoku)
        {
            grid = sudoku.Clone();

            bool keepSolving = true;
            while (keepSolving)
            {
                keepSolving = HiddenSingles();
            }

            return grid;
        }

        #endregion

        #region .: Private Methods :.

        /// <summary>
        /// Finds hidden singles.
        /// Iterates through the empty cells, evaluates possible candidates and if there is
        /// only one, fills it in, considering it a hidden single.
        /// </summary>
        /// <returns>Boolean value indicating if at least one hidden single has been found.</returns>
        private static bool HiddenSingles()
        {
            bool foundHiddenSingle = false;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        // create a list of all numbers that could go to the empty cell, initialized to whole range 1-9
                        List<int> possibleNumbers = [.. Enumerable.Range(1, 9)];

                        // remove numbers which already present in the same regions
                        for (int idx = 0; idx < 9; idx++)
                        {
                            possibleNumbers.Remove(grid[row, idx]);
                            possibleNumbers.Remove(grid[idx, col]);

                            (int subgrid, _) = ToSubgridAndCell(row, col);
                            (int r, int c) = ToRowAndColumn(subgrid, idx);
                            possibleNumbers.Remove(grid[r, c]);
                        }

                        if (possibleNumbers.Count == 1)
                        {
                            grid[row, col] = possibleNumbers[0];
                            foundHiddenSingle = true;
                        }
                    }
                }
            }

            return foundHiddenSingle;
        }

        /// <summary>
        /// Convert [row; col] coordinates to [subgrid; cell].
        /// </summary>
        /// <param name="row">Row index (0-8).</param>
        /// <param name="col">Column index (0-8).</param>
        /// <returns>Tuple (subgrid, cell).</returns>
        private static (int, int) ToSubgridAndCell(int row, int col) => (col / 3 + 3 * (row / 3), col % 3 + 3 * (row % 3));
        /// <summary>
        /// Convert [subgrid; cell] to [row; col] coordinates.
        /// </summary>
        /// <param name="subgrid">Subgrid index (0-8).</param>
        /// <param name="cell">Cell index within the subgrid (0-8)</param>
        /// <returns>Tuple (row, col).</returns>
        private static (int, int) ToRowAndColumn(int subgrid, int cell) => (3 * (subgrid / 3) + (cell / 3), 3 * (subgrid % 3) + (cell % 3));

        #endregion
    }
}
