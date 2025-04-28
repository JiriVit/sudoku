using System;
using System.Collections.Generic;
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

            // fill the cells with sample number to verify binding
            for (int row = 0; row < 9; row++) 
            { 
                for (int col = 0;  col < 9; col++)
                {
                    CellModel cellModel = new(row, col)
                    {
                        Number = 1 + (row % 3) * 3 + col % 3,
                    };
                    
                    Cells[row * 9 + col] = cellModel;
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

        #endregion
    }
}
