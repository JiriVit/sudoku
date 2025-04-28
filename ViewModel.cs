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
                    cellModel.MouseOverChanged += CellModel_MouseOverChanged;
                    
                    Cells[row * 9 + col] = cellModel;
                }
            }
        }

        #endregion

        #region .: Event Handlers :.

        private void CellModel_MouseOverChanged(object? sender, EventArgs e)
        {
            CellModel cellM = (CellModel)sender!;

            foreach (var c in Cells.Where(c => c.IsSameRegion(cellM)))
            {
                c.Highlighted = cellM.MouseOver;
            }

        }

        #endregion
    }
}
