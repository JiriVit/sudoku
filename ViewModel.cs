using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class ViewModel
    {
        #region .: Properties :.

        /// <summary>
        /// Gets number at certain coordinates. The index is calculated as row * 9 + col where
        /// row and col are 0-8.
        /// </summary>
        public int[] Numbers { get; private set; }

        // TODO Create a model representing a single cell. This array of ints is just a proof of
        // concept, but not good for permanent use. Any PropertyChanged event would be fired over
        // the whole array and all 81 UI elements bound to its elements would need to handle it.
        // It is better to have an array of models and bind each cell UI element to a single
        // model. This will allow to fire a PropertyChanged event only for the affected cells.

        #endregion

        #region .: Constructor :.

        /// <summary>
        /// Creates a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel() 
        {
            Numbers = new int[81];

            // fill the cells with sample number to verify binding
            for (int row = 0; row < 9; row++) 
            { 
                for (int col = 0;  col < 9; col++)
                {
                    int sampleNumber = 1 + (row % 3) * 3 + col % 3;
                    Numbers[row * 9 + col] = sampleNumber;
                }
            }
        }

        #endregion
    }
}
