using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sudoku
{
    /// <summary>
    /// Represents a sudoku grid.
    /// </summary>
    internal class SudokuGrid
    {
        #region .: Private Fields :.

        private int[] numbers = new int[81];
        private readonly List<int> numbersInRegion = [];

        #endregion

        #region .: Properties :.

        /// <summary>
        /// Gets or sets number stored at given coordinates in the grid.
        /// </summary>
        /// <param name="row">Row (0-8).</param>
        /// <param name="col">Column (0-8).</param>
        /// <returns>Number stored at given coordinates.</returns>
        public int this[int row, int col]
        {
            get => numbers[ToIndex(row, col)];
            set => numbers[ToIndex(row, col)] = value;
        }

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Checks if the sudoku is valid (excl. empty cells), ie. if all rows, columns
        /// and subgrids contain only unique numbers.
        /// </summary>
        /// <returns>Boolean value indicating if the sudoku is valid.</returns>
        public bool IsValid()
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
        /// Checks if the sudoku is solved, ie. has no empty cells.
        /// </summary>
        /// <returns>Boolean value indicating if the sudoku is solved.</returns>
        public bool IsSolved() => numbers.All(n => n != 0);

        /// <summary>
        /// Copies the numbers to given array of instances of <see cref="CellModel"/>.
        /// </summary>
        /// <param name="cellArray">Array to copy the numbers to.</param>
        public void ToCellArray(CellModel[] cellArray)
        {
            for (int i = 0; i < 81; i++)
            {
                cellArray[i].Number = (numbers[i] > 0) ? numbers[i] : null;
            }
        }

        /// <summary>
        /// Creates a clone of the grid, with the same numbers in the cells.
        /// </summary>
        /// <returns>A clone of the grid.</returns>
        public SudokuGrid Clone()
        {
            SudokuGrid clone = new();
            Array.Copy(numbers, clone.numbers, numbers.Length);
            return clone;
        }

        public static SudokuGrid FromCellArray(CellModel[] cellArray)
        {
            SudokuGrid grid = new();
            for (int i = 0; i < 81; i++)
            {
                grid.numbers[i] = cellArray[i].Number ?? 0;
            }

            return grid;
        }

        public static SudokuGrid CreateSample()
        {
            return new SudokuGrid()
            {
                numbers = [
                //  0  1  2  3  4  5  6  7  8
                    2, 0, 5, 0, 0, 8, 0, 0, 0, // 0
                    0, 0, 0, 9, 0, 4, 8, 5, 2, // 1
                    0, 0, 0, 0, 0, 0, 0, 9, 7, // 2
                    0, 0, 0, 0, 0, 0, 7, 0, 1, // 3
                    6, 3, 7, 5, 2, 0, 4, 8, 9, // 4
                    0, 0, 8, 4, 0, 6, 0, 2, 5, // 5
                    3, 0, 1, 8, 0, 7, 2, 0, 6, // 6
                    0, 0, 9, 0, 0, 0, 0, 0, 0, // 7
                    0, 0, 2, 6, 3, 0, 0, 0, 8, // 8
                ]
            };
        }

        #endregion

        #region .: Private Methods :.

        /// <summary>
        /// Converts row and column coordinates to index in a flattened array.
        /// </summary>
        /// <param name="row">Row (0-8).</param>
        /// <param name="col">Column (0-8).</param>
        /// <returns>Index (0-80).</returns>
        private static int ToIndex(int row, int col) => 9 * row + col;

        /// <summary>
        /// Checks if the number in a cell is duplicate with others in the region.
        /// Uses private list <see cref="numbersInRegion"/>, it needs to be cleared before starting
        /// with another region.
        /// </summary>
        /// <param name="row">Row index of the checked cell (0-8).</param>
        /// <param name="col">Column index of the checked cell (0-8).</param>
        /// <returns>Boolean value indicating duplicate number.</returns>
        private bool IsDuplicateNumber(int row, int col)
        {
            bool isDuplicate = false;

            int number = this[row, col];
            if (number > 0)
            {
                if (!numbersInRegion.Contains(number))
                {
                    numbersInRegion.Add(number);
                }
                else
                {
                    isDuplicate = true;
                }
            }

            return isDuplicate;
        }

        #endregion
    }
}
