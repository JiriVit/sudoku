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

        /// <summary>
        /// Gets array with instances of class <see cref="CellModel"/> that represent single sudoku cells.
        /// </summary>
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
                // unselect previously selected cell
                selectedCell.Selected = false;

                // disable highlighting of cells from same regions
                foreach (var c in Cells.Where(c => c.IsSameRegion(selectedCell)))
                {
                    c.Highlighted = false;
                }

                // unselect cells with the same number
                MarkCellsWithSelectedNumber(false);
            }

            // store the cell as selected (so we know which one to edit)
            selectedCell = cell;

            if (selectedCell != null)
            {
                // mark the cell as selected
                selectedCell.Selected = true;
                
                // highlight cells from same regions
                foreach (var c in Cells.Where(c => c.IsSameRegion(selectedCell)))
                {
                    c.Highlighted = true;
                }

                // select cells with the same number (only visual, not for editing)
                MarkCellsWithSelectedNumber(true);
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

            if ((selectedCell != null) && selectedCell.Editable)
            {
                MarkCellsWithSelectedNumber(false);
                selectedCell.Number = number;
                MarkCellsWithSelectedNumber(true);
                UpdateIncorrectStatus();
            }
        }

        /// <summary>
        /// Clears number in the selected cell.
        /// If no cell is selected, no action is taken.
        /// </summary>
        public void ClearNumberInSelectedCell()
        {
            if ((selectedCell != null) && selectedCell.Editable)
            {
                MarkCellsWithSelectedNumber(false);
                selectedCell.Number = null;
                UpdateIncorrectStatus();
            }
        }

        public void LoadSample()
        {
            SudokuGrid.CreateSample().ToCellArray(Cells);
            UpdateEditableCells();
        }

        public void Generate()
        {
            SudokuGenerator.Generate()?.ToCellArray(Cells);
        }

        public void Solve()
        {
            SudokuSolver.Solve(SudokuGrid.FromCellArray(Cells)).ToCellArray(Cells);
        }

        #endregion

        #region .: Private Methods :.

        /// <summary>
        /// Updates the <see cref="CellModel.Editable"/> property of all cells per their
        /// current contents - if they are not empty, the won't be editable.
        /// Invoke this after a new sudoku has been generated.
        /// </summary>
        private void UpdateEditableCells()
        {
            foreach (var item in Cells)
            {
                item.Editable = item.Number == null;
            }
        }

        /// <summary>
        /// Updates the <see cref="CellModel.Incorrect"/> property of selected cell.
        /// </summary>
        private void UpdateIncorrectStatus()
        {
            if (selectedCell != null)
            {
                // TODO Fix this to include also numbers in empty cells.
                selectedCell.Incorrect = Cells.Any(c => c.IsSameRegion(selectedCell) && c.HasSameNumber(selectedCell));
            }
        }

        private void MarkCellsWithSelectedNumber(bool isSelected)
        {
            if (selectedCell?.Number != null)
            {
                foreach (var c in Cells.Where(c => c.HasSameNumber(selectedCell)))
                {
                    c.Selected = isSelected;
                }
            }
        }

        #endregion
    }
}
