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
    internal class CellModel : ModelBase
    {
        #region .: Properties :.

        /// <summary>
        /// Gets or sets the number written in the cell.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Gets cell border background.
        /// </summary>
        public Brush Background { get => highlighted ? Brushes.LightGray : Brushes.White; }

        /// <summary>
        /// Gets or sets indication that the cell is highlighted (because of mouse hovering).
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

        #endregion

        #region .: Private Variables :.

        private bool highlighted = false;

        #endregion
    }
}
