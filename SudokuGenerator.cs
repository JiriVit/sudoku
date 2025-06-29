using System;
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
        #region .: Properties :.

        /// <summary>
        /// Gets the generated sudoku array.
        /// </summary>
        public static int[,] Sudoku => sudoku;

        #endregion

        #region .: Private Variables :.

        private static readonly Random random = new();
        private static int[,] sudoku = new int[9, 9];

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Generates a new sudoku.
        /// </summary>
        public static SudokuGrid? Generate()
        {
            // recursively generate a solved sudoku
            return AddNextNumber(new SudokuGrid());
        }

        public static void Solve()
        {
            // sample sudoku for testing of solving techniques
            sudoku = new int[,] 
            {
            //   0  1  2  3  4  5  6  7  8
                {2, 0, 5, 0, 0, 8, 0, 0, 0 }, // 0
                {0, 0, 0, 9, 0, 4, 8, 5, 2 }, // 1
                {0, 0, 0, 0, 0, 0, 0, 9, 7 }, // 2
                {0, 0, 0, 0, 0, 0, 7, 0, 1 }, // 3
                {6, 3, 7, 5, 2, 0, 4, 8, 9 }, // 4
                {0, 0, 8, 4, 0, 6, 0, 2, 5 }, // 5
                {3, 0, 1, 8, 0, 7, 2, 0, 6 }, // 6
                {0, 0, 9, 0, 0, 0, 0, 0, 0 }, // 7
                {0, 0, 2, 6, 3, 0, 0, 0, 8 }, // 8
            };

            Print();

            bool keepSolving = true;
            while (keepSolving)
            {
                keepSolving = HiddenSingles();
            }

            Print();
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

        #region .: Generating :.

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

        #endregion

        #region .: Solving :.

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
                    if (sudoku[row, col] == 0)
                    {
                        // create a list of all numbers that could go to the empty cell, initialized to whole range 1-9
                        List<int> possibleNumbers = [.. Enumerable.Range(1, 9)];

                        // remove numbers which already present in the same regions
                        for (int idx = 0; idx < 9; idx++)
                        {
                            possibleNumbers.Remove(sudoku[row, idx]);
                            possibleNumbers.Remove(sudoku[idx, col]);

                            (int subgrid, _) = ConvertRC2SG(row, col);
                            (int r, int c) = ConvertSG2RC(subgrid, idx);
                            possibleNumbers.Remove(sudoku[r, c]);
                        }

                        if (possibleNumbers.Count == 1)
                        {
                            sudoku[row, col] = possibleNumbers[0];
                            foundHiddenSingle = true;
                            Debug.WriteLine($"[{row}, {col}] found hidden single {possibleNumbers[0]}");
                        }
                    }
                }
            }

            return foundHiddenSingle;
        }

        #endregion

        #endregion

        #region .: Debug Methods :.

        private static void Print()
        {
            for (int row = 0; row < 9; row++)
            {
                if ((row % 3) == 0)
                {
                    Debug.WriteLine("+---+---+---+");
                }

                for (int col = 0; col < 9; col++)
                {
                    if ((col % 3) == 0)
                    {
                        Debug.Write("|");
                    }
                    Debug.Write(sudoku[row, col]);
                }
                Debug.WriteLine("|");
            }
            Debug.WriteLine("+---+---+---+");
        }

        private static void PrintList(List<int> list)
        {
            Debug.Write($"{{{list.Count}}}: ");
            foreach (int item in list)
            {
                Debug.Write($"{item}, ");
            }
            Debug.WriteLine("");
        }

        #endregion
    }
}
