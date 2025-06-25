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
        private static readonly List<int> numbersInRegion = [];

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Generates a new sudoku.
        /// </summary>
        public static void Generate()
        {
            Array.Clear(sudoku);
            
            // recursively generate a solved sudoku
            AddNextNumber(0, 0);

            // TODO Empty randomly selected cells until the sudoku is solvable.
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
