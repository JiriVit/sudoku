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
            }
        }

        public void LoadSample()
        {
            SudokuGrid sample = SudokuGrid.CreateSample();
            SudokuGrid solvedSample = SudokuSolver.Solve(sample);

            solvedSample.ToCellArray(Cells, SudokuGrid.AsCorrectNumbers);
            sample.ToCellArray(Cells, SudokuGrid.AsShownNumbers);
        }

        public void Generate()
        {
            (SudokuGrid? solvedSudoku, SudokuGrid? unsolvedSudoku) = SudokuGenerator.Generate();

            if ((solvedSudoku != null) && (unsolvedSudoku != null))
            {
                solvedSudoku.ToCellArray(Cells, SudokuGrid.AsCorrectNumbers);
                unsolvedSudoku.ToCellArray(Cells, SudokuGrid.AsShownNumbers);
            }
        }

        public void Solve()
        {
            SudokuSolver.Solve(SudokuGrid.FromCellArray(Cells)).ToCellArray(Cells, SudokuGrid.AsShownNumbers);
        }

        #endregion

        #region .: Private Methods :.

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
