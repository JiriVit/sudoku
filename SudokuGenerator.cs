﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
    /// <summary>
    /// Provides sudoku generator.
    /// </summary>
    internal static class SudokuGenerator
    {
        #region .: Private Fields :.

        private static readonly Random random = new();
        private static readonly int numbersToRemove = 45;

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Generates a new sudoku.
        /// </summary>
        public static (SudokuGrid?, SudokuGrid?) Generate()
        {
            SudokuGrid? solvedSudoku;
            SudokuGrid? unsolvedSudoku = null;

            // recursively generate a solved puzzle
            solvedSudoku = AddNextNumber(new SudokuGrid());
            if (solvedSudoku != null)
            {
                // recursively empty required number of cells to create a solvable puzzle
                unsolvedSudoku = RemoveNextNumber(solvedSudoku);
            }            

            return (solvedSudoku, unsolvedSudoku);
        }

        #endregion

        #region .: Public Static Methods :.

        /// <summary>
        /// Convert [row; col] coordinates to [subgrid; cell].
        /// </summary>
        /// <param name="row">Row index (0-8).</param>
        /// <param name="col">Column index (0-8).</param>
        /// <returns>Tuple (subgrid, cell).</returns>
        public static (int, int) ConvertRC2SG(int row, int col) => (col / 3 + 3 * (row / 3), col % 3 + 3 * (row % 3));
        /// <summary>
        /// Convert [subgrid; cell] to [row; col] coordinates.
        /// </summary>
        /// <param name="subgrid">Subgrid index (0-8).</param>
        /// <param name="cell">Cell index within the subgrid (0-8)</param>
        /// <returns>Tuple (row, col).</returns>
        public static (int, int) ConvertSG2RC(int subgrid, int cell) => (3 * (subgrid / 3) + (cell / 3), 3 * (subgrid % 3) + (cell % 3));

        #endregion

        #region .: Private Methods :.

        /// <summary>
        /// Adds next number to the sudoku and validates it.
        /// Recursively calls itself to fill the grid or to backtrack in case of wrong path.
        /// </summary>
        /// <param name="grid">Sudoku grid to be filled</param>
        /// <param name="row">Row number of the cell to be filled (0-8).</param>
        /// <param name="col">Column number of the cell to be filled (0-8).</param>
        /// <returns>Filled sudoku grid or null if the path exhausted all possible numbers and backtracking is needed.</returns>
        private static SudokuGrid? AddNextNumber(SudokuGrid grid, int row = 0, int col = 0)
        {
            SudokuGrid? filledGrid = null;
            SudokuGrid nextGrid = grid.Clone();
            List<int> availableNumbers = [.. Enumerable.Range(1, 9)];

            bool getAnotherNumber = true;
            while (getAnotherNumber)
            {
                // pick a random available number, then remove it from available numbers
                int index = random.Next(availableNumbers.Count);
                nextGrid[row, col] = availableNumbers[index];
                availableNumbers.RemoveAt(index);

                // if the number doesn't invalidate the sudoku, recursively call this method
                bool valid = nextGrid.IsValid();
                if (valid)
                {
                    bool lastCell = (row == 8) && (col == 8);

                    if (!lastCell)
                    {
                        int nextCellRow = (col + 1) % 9;
                        int nextCellCol = (nextCellRow == 0) ? (row + 1) : row;

                        filledGrid = AddNextNumber(nextGrid, nextCellCol, nextCellRow);
                        valid = (filledGrid != null);
                    }
                    else 
                    {
                        filledGrid = nextGrid;
                    }
                }

                getAnotherNumber = !valid && (availableNumbers.Count > 0);
            }

            return filledGrid;
        }

        /// <summary>
        /// Removes a number from the puzzle and checks if it is still solvable.
        /// Recursively calls itself to empty required number of cells or to backtrack in case of
        /// blind path. Required number of emptied cells is defined by private field <see cref="numbersToRemove"/>.
        /// </summary>
        /// <param name="grid">Solved puzzle to be processed.</param>
        /// <returns>Puzzle with emptied cells per requirement; or null if the path exhausted all options
        /// and backtracking is needed.</returns>
        private static SudokuGrid? RemoveNextNumber(SudokuGrid grid)
        {
            SudokuGrid? updatedGrid = null;

            // collect indices of filled cells so we can clear them
            List<int> indicesOfFilledCells = [.. Enumerable.Range(0, 81).Where(i => grid[i] != 0)];
            int countOfEmptyCells = 81 - indicesOfFilledCells.Count;

            bool clearAnotherCell = true;
            while (clearAnotherCell)
            {
                updatedGrid = grid.Clone();

                // pick one of the filled cells and clear it
                int index = random.Next(indicesOfFilledCells.Count);
                updatedGrid[indicesOfFilledCells[index]] = 0;
                indicesOfFilledCells.RemoveAt(index);

                // is the puzzle still solvable?
                if (SudokuSolver.Solve(updatedGrid).IsSolved())
                {
                    if (countOfEmptyCells == (numbersToRemove - 1))
                    {
                        // we've reached the desired number of empty cells -> stop and return the current grid
                        clearAnotherCell = false;
                    }
                    else
                    {
                        updatedGrid = RemoveNextNumber(updatedGrid);
                        clearAnotherCell = (updatedGrid == null);
                    }
                }
                else
                {
                    if (indicesOfFilledCells.Count == 0)
                    {
                        // puzzle is not solvable and we ran out of options -> need to backtrack
                        clearAnotherCell = false;
                        updatedGrid = null;
                    }
                }
            }

            return updatedGrid;
        }

        #endregion
    }
}
