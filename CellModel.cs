using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sudoku
{
    /// <summary>
    /// Provides Model for a single cell in the sudoku grid.
    /// </summary>
    internal class CellModel(int row, int col) : ModelBase
    {
        #region .: Properties :.

        /// <summary>
        /// Gets or sets the number written in the cell.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets row number of the cell.
        /// </summary>
        public int Row { get; private set; } = row;
        
        /// <summary>
        /// Gets column number of the cell.
        /// </summary>
        public int Column { get; private set; } = col;

        /// <summary>
        /// Gets subgrid number of the cell.
        /// </summary>
        public int Subgrid { get; private set; } = (row / 3) * 3 + (col / 3);

        /// <summary>
        /// Gets cell border background.
        /// </summary>
        public Brush Background { get => selected ? Brushes.LightBlue : (highlighted ? Brushes.Gainsboro : Brushes.White); }

        /// <summary>
        /// Gets or sets indication that the cell is highlighted.
        /// </summary>
        public bool Highlighted
        {
            get => highlighted;
            set
            {
                if (highlighted != value)
                {
                    highlighted = value;
                    NotifyPropertyChanged(nameof(Background));
                }
            }
        }

        /// <summary>
        /// Gets or sets indication that the mouse is hovering over the cell.
        /// </summary>
        public bool MouseOver
        {
            get => mouseOver;
            set 
            {
                if (mouseOver != value)
                {
                    mouseOver = value;
                    MouseOverChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Sets indication that the cell is selected.
        /// </summary>
        public bool Selected
        {
            set
            {
                if (selected != value)
                {
                    selected = value;
                    NotifyPropertyChanged(nameof(Background));
                }
            }
        }

        #endregion

        #region .: Private Variables :.

        private bool highlighted = false;
        private bool mouseOver = false;
        private bool selected = false;

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Checks if this cell is in the same region (row, column, subgrid) as another cell.
        /// </summary>
        /// <param name="cell">The second cell to be checked for the same region.</param>
        /// <returns>Boolean value indicating the result.</returns>
        public bool IsSameRegion(CellModel cell) => 
            (cell.Row == Row) || 
            (cell.Column == Column) ||
            (cell.Subgrid == Subgrid);

        #endregion

        #region .: Events :.

        /// <summary>
        /// Occurs when the <see cref="MouseOver"/> property changes.
        /// </summary>
        public event EventHandler? MouseOverChanged;

        #endregion
    }
}
