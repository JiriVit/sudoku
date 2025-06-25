using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sudoku
{
    /// <summary>
    /// Provides a ViewModel for the application.
    /// </summary>
    internal class ViewModel
    {
        #region .: Properties :.

        public CellModel[] Cells { get; private set; }

        #endregion

        #region .: Private Variables :.

        private CellModel? selectedCell;

        #endregion

        #region .: Constructor :.

        /// <summary>
        /// Creates a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel() 
        {
            Cells = new CellModel[81];

            for (int row = 0; row < 9; row++) 
            { 
                for (int col = 0;  col < 9; col++)
                {
                    Cells[row * 9 + col] = new(row, col);
                }
            }
        }

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Selects defined cell so it can be edited.
        /// </summary>
        /// <param name="cell">The cell to be selected.</param>
        public void SelectCell(CellModel cell)
        {
            if (selectedCell != null)
            {
                selectedCell.Selected = false;
                foreach (var c in Cells.Where(c => c.IsSameRegion(selectedCell)))
                {
                    c.Highlighted = false;
                }
            }

            selectedCell = cell;

            if (selectedCell != null)
            {
                selectedCell.Selected = true;
                foreach (var c in Cells.Where(c => c.IsSameRegion(selectedCell)))
                {
                    c.Highlighted = true;
                }
            }
        }

        /// <summary>
        /// Sets the number in the selected cell.
        /// If no cell is selected, no action is taken.
        /// </summary>
        /// <param name="number">Number to be set.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the number is out of range 1-9.</exception>
        public void SetNumberInSelectedCell(int number)
        {
            if ((number < 1) || (number > 9))
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Must be in range 1-9.");
            }

            if (selectedCell != null)
            {
                selectedCell.Number = number;
            }
        }

        /// <summary>
        /// Clears number in the selected cell.
        /// If no cell is selected, no action is taken.
        /// </summary>
        public void ClearNumberInSelectedCell()
        {
            if (selectedCell != null)
            {
                selectedCell.Number = null;
            }
        }

        public void Generate()
        {
            //SudokuGenerator.Generate();

            //for (int row = 0; row < 9; row++)
            //{
            //    for (int col = 0; col < 9; col++)
            //    {
            //        Cells[row * 9 + col].Number = SudokuGenerator.Sudoku[row, col];
            //    }
            //}

            SudokuGenerator.Solve();
        }

        #endregion
    }
}
