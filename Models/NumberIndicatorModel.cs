using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sudoku.Models
{
    /// <summary>
    /// Provides Model for a number indicator.
    /// </summary>
    internal class NumberIndicatorModel : ModelBase
    {
        #region .: Properties :.

        /// <summary>
        /// Gets or sets number of missing occurrences in the grid.
        /// </summary>
        public int Indicator 
        { 
            get => indicator; 
            set
            {
                indicator = value;
                NotifyPropertyChanged(nameof(Indicator));
                NotifyPropertyChanged(nameof(Visible));
            }
        }

        /// <summary>
        /// Gets visibility of the indicator.
        /// </summary>
        public Visibility Visible => (indicator != 0) ? Visibility.Visible : Visibility.Hidden;

        #endregion

        #region .: Private Fields :.

        private int indicator = 0;

        #endregion
    }
}
