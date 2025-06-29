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

        #region .: Contents :.

        /// <summary>
        /// Gets or sets the number written in the cell.
        /// </summary>
        public int? Number
        {
            get => number;
            set
            {
                number = value;
                NotifyPropertyChanged(nameof(Number));
            }
        }

        #endregion

        #region .: Position :.

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

        #endregion

        #region .: Appearance :.

        /// <summary>
        /// Gets cell border background.
        /// </summary>
        public Brush Background => selected ? Brushes.LightBlue : (highlighted ? Brushes.Gainsboro : Brushes.White);

        /// <summary>
        /// Gets cell text foreground.
        /// </summary>
        public Brush Foreground => editable ? (incorrect ? Brushes.Red : Brushes.Blue) : Brushes.Black;

        #endregion

        #region .: Status :.

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

        /// <summary>
        /// Get or sets indication that the cell is editable.
        /// </summary>
        public bool Editable
        {
            get => editable;
            set
            {
                if (editable != value)
                {
                    editable = value;
                    NotifyPropertyChanged(nameof(Foreground));
                }
            }
        }

        /// <summary>
        /// Sets indication that the number in the cell is incorrect.
        /// </summary>
        public bool Incorrect
        {
            set
            {
                if (incorrect != value)
                {
                    incorrect = value;
                    NotifyPropertyChanged(nameof(Foreground));
                }
            }
        }

        #endregion

        #endregion

        #region .: Private Variables :.

        private int? number = null;
        private bool highlighted = false;
        private bool mouseOver = false;
        private bool selected = false;
        private bool editable = false;
        private bool incorrect = false;

        #endregion

        #region .: Public Methods :.

        /// <summary>
        /// Checks if this cell is in the same region (row, column, subgrid) as another cell.
        /// </summary>
        /// <param name="cell">The second cell to be checked for the same region.</param>
        /// <returns>Boolean value indicating the result.</returns>
        public bool IsSameRegion(CellModel cell) => 
            (cell != this) &&
            ( (cell.Row == Row) || 
              (cell.Column == Column) ||
              (cell.Subgrid == Subgrid)
            );

        /// <summary>
        /// Checks if this cell has the same number as another cell, provided none of them
        /// is empty.
        /// </summary>
        /// <param name="cell">The second cell to be checked for the same number.</param>
        /// <returns>Boolean value indicating the result.</returns>
        public bool HasSameNumber(CellModel cell) =>
            (this != cell) &&
            (Number != null) &&
            (Number == cell.Number);

        #endregion

        #region .: Events :.

        /// <summary>
        /// Occurs when the <see cref="MouseOver"/> property changes.
        /// </summary>
        public event EventHandler? MouseOverChanged;

        #endregion
    }
}
