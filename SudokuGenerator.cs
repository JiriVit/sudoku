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

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Generates a new sudoku.
        /// </summary>
        public static void Generate()
        {
            Reset();
            AddNextNumber(0, 0);
        }

        #endregion

        #region .: Private Methods :.

        private static void Reset()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sudoku[row, col] = 0;
                }
            }
        }

        private static bool AddNextNumber(int row, int col)
        {
            List<int> availableNumbers = [.. Enumerable.Range(1, 9)];
            bool valid = true;
            bool backtrack = false;

            bool getAnotherNumber = true;
            while (getAnotherNumber)
            {
                int index = random.Next(availableNumbers.Count);
                sudoku[row, col] = availableNumbers[index];
                availableNumbers.RemoveAt(index);

                valid = Validate();
                backtrack = false;

                if (valid)
                {
                    bool lastCell = (row == 8) && (col == 8);

                    if (!lastCell)
                    {
                        int newCol = (col + 1) % 9;
                        int newRow = (newCol == 0) ? (row + 1) : row;

                        backtrack = !AddNextNumber(newRow, newCol);
                    }
                }

                getAnotherNumber = (!valid || backtrack) && (availableNumbers.Count > 0);
            }

            bool fail = (!valid || backtrack) && (availableNumbers.Count == 0);

            if (fail)
            {
                sudoku[row, col] = 0;
            }

            return !fail;
        }

        public static bool Validate()
        {
            bool valid = true;
            bool stop = false;

            // check rows
            for (int row = 0; (row < 9) && !stop; row++)
            {
                List<int> numbers = [];

                for (int col = 0; (col < 9) && !stop; col++)
                {
                    int n = sudoku[row, col];
                    if (n > 0)
                    {
                        if (!numbers.Contains(n))
                        {
                            numbers.Add(n);
                        }
                        else
                        {
                            valid = false;
                            stop = true;
                        }
                    }
                }
            }

            // check columns
            for (int col = 0; (col < 9) && !stop; col++)
            {
                List<int> numbers = [];

                for (int row = 0; (row < 9) && !stop; row++)
                {
                    int n = sudoku[row, col];
                    if (n > 0)
                    {
                        if (!numbers.Contains(n))
                        {
                            numbers.Add(n);
                        }
                        else
                        {
                            valid = false;
                            stop = true;
                        }
                    }
                }
            }

            // check subgrids
            for (int sg = 0; (sg < 9) && !stop; sg++)
            {
                List<int> numbers = [];

                for (int cell = 0; (cell < 9) && !stop; cell++)
                {
                    int row = 3 * (sg / 3) + cell / 3;
                    int col = 3 * (sg % 3) + cell % 3;
                    int n = sudoku[row, col];
                    if (n > 0)
                    {
                        if (!numbers.Contains(n))
                        {
                            numbers.Add(n);
                        }
                        else
                        {
                            valid = false;
                            stop = true;
                        }
                    }
                }
            }

            return valid;
        }

        #endregion
    }
}
