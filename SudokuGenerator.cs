using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static readonly int[,] sudoku = new int[9, 9];
        private static readonly List<int> numbersInRegion = [];

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Generates a new sudoku.
        /// </summary>
        public static void Generate()
        {
            Array.Clear(sudoku);
            AddNextNumber(0, 0);
        }

        #endregion

        #region .: Private Methods :.

        /// <summary>
        /// Adds next number to the sudoku and validates it.
        /// Recursively calls itself to fill the grid or to backtrack in case of wrong path.
        /// </summary>
        /// <param name="row">Row number of the cell to be filled (0-8).</param>
        /// <param name="col">Column number of the cell to be filled (0-8).</param>
        /// <returns>TRUE if all the sudoku has been filled, FALSE if no valid number has been found.</returns>
        private static bool AddNextNumber(int row, int col)
        {
            List<int> availableNumbers = [.. Enumerable.Range(1, 9)];
            bool valid = true;

            bool getAnotherNumber = true;
            while (getAnotherNumber)
            {
                // pick a random available number, then remove it from available numbers
                int index = random.Next(availableNumbers.Count);
                sudoku[row, col] = availableNumbers[index];
                availableNumbers.RemoveAt(index);

                // if the number doesn't invalidate the sudoku, recursively call this method
                valid = IsSudokuValid();
                if (valid)
                {
                    bool lastCell = (row == 8) && (col == 8);

                    if (!lastCell)
                    {
                        int nextCellRow = (col + 1) % 9;
                        int nextCellCol = (nextCellRow == 0) ? (row + 1) : row;

                        valid = AddNextNumber(nextCellCol, nextCellRow);
                    }
                }

                getAnotherNumber = !valid && (availableNumbers.Count > 0);
            }

            // if we run out of numbers without finding a valid one, we need to backtrack
            bool backtrack = !valid && (availableNumbers.Count == 0);
            if (backtrack)
            {
                sudoku[row, col] = 0;
            }

            return !backtrack;
        }

        /// <summary>
        /// Checks if the current sudoku (excluding empty cells) is valid, ie. doesn't
        /// violate rules for solved sudoku.
        /// </summary>
        /// <returns>Boolean value indicating validity.</returns>
        public static bool IsSudokuValid()
        {
            bool isValid = true;

            // check rows
            for (int row = 0; (row < 9) && isValid; row++)
            {
                numbersInRegion.Clear();
                for (int col = 0; (col < 9) && isValid; col++)
                {
                    isValid = !IsDuplicateNumber(row, col);
                }
            }

            // check columns
            for (int col = 0; (col < 9) && isValid; col++)
            {
                numbersInRegion.Clear();
                for (int row = 0; (row < 9) && isValid; row++)
                {
                    isValid = !IsDuplicateNumber(row, col);
                }
            }

            // check subgrids
            for (int subgrid = 0; (subgrid < 9) && isValid; subgrid++)
            {
                numbersInRegion.Clear();
                for (int subgridCell = 0; (subgridCell < 9) && isValid; subgridCell++)
                {
                    int row = 3 * (subgrid / 3) + subgridCell / 3;
                    int col = 3 * (subgrid % 3) + subgridCell % 3;
                    isValid = !IsDuplicateNumber(row, col);
                }
            }

            return isValid;
        }

        /// <summary>
        /// Checks if the number in a cell is duplicate with others in the region.
        /// Uses static list <see cref="numbersInRegion"/>, it needs to be cleared before starting
        /// with another region.
        /// </summary>
        /// <param name="row">Row index of the checked cell (0-8).</param>
        /// <param name="col">Column index of the checked cell (0-8).</param>
        /// <returns>Boolean value indicated duplicate number.</returns>
        private static bool IsDuplicateNumber(int row, int col)
        {
            bool duplicate = false;

            int number = sudoku[row, col];
            if (number > 0)
            {
                if (!numbersInRegion.Contains(number))
                {
                    numbersInRegion.Add(number);
                }
                else
                {
                    duplicate = true;
                }
            }

            return duplicate;
        }

        #endregion
    }
}
