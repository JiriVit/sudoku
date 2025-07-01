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
        /// If the cell has its <see cref="Editable"/> property reset due to previous update of
        /// <see cref="CorrectNumber"/>, setting of <see cref="Number"/> will update the <see cref="Editable"/>
        /// value according set value - null makes the cell editable (to be solved), number makes the
        /// cell not editable (to be preset).
        /// </summary>
        public int? Number
        {
            get => number;
            set
            {
                number = value;
                editable ??= (number == null);

                NotifyPropertyChanged(nameof(Number));
                NotifyPropertyChanged(nameof(Foreground));
            }
        }

        /// <summary>
        /// Gets or sets the correct number that belongs to the cell.
        /// Setting of this property resets the <see cref="Editable"/> property - do this only
        /// when assigning a freshly generated puzzle.
        /// </summary>
        public int CorrectNumber
        {
            get => correctNumber; 
            set
            {
                correctNumber = value;
                editable = null;
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
        public Brush Foreground => Editable ? (Incorrect ? Brushes.Red : Brushes.Blue) : Brushes.Black;

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
        /// Gets indication that the cell is user editable.
        /// </summary>
        public bool Editable 
        {
            get => editable ?? false;
            private set => editable = value;
        }

        /// <summary>
        /// Gets indication that the number in the cell is incorrect.
        /// </summary>
        private bool Incorrect
        {
            get => (CorrectNumber > 0) && (Number != CorrectNumber);
        }

        #endregion

        #endregion

        #region .: Private Variables :.

        private int? number = null;
        private int correctNumber;
        private bool highlighted = false;
        private bool mouseOver = false;
        private bool selected = false;
        private bool? editable = null;

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
