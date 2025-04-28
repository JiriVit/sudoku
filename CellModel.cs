using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    /// <summary>
    /// Provides Model for a single cell in the sudoku grid.
    /// </summary>
    internal class CellModel : ModelBase
    {
        #region .: Properties :.

        public int Number { get; set; }

        #endregion
    }
}
