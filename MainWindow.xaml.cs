using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region .: Constructor :.

        public MainWindow()
        {
            InitializeComponent();
            CreateSudokuElements();
        }

        #endregion

        #region .: Private Methods :.

        /// <summary>
        /// Creates UI elements of the sudoku grid (grids, borders, text blocks).
        /// </summary>
        private void CreateSudokuElements()
        {
            // clear all children created by the designer
            gridMain.Children.Clear();

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    double thicknessLeft = (col > 0) ? 1 : 0;
                    double thicknessTop = (row > 0) ? 1 : 0;
                    double thicknessRight = (col < 2) ? 1 : 0;
                    double thicknessBottom = (row < 2) ? 1 : 0;

                    Border subgridBorder = new()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(thicknessLeft, thicknessTop, thicknessRight, thicknessBottom),
                        SnapsToDevicePixels = true,
                    };

                    Grid.SetColumn(subgridBorder, col);
                    Grid.SetRow(subgridBorder, row);

                    InsertSubgrid(subgridBorder);

                    gridMain.Children.Add(subgridBorder);
                }
            }

        }

        private static void InsertSubgrid(Border subgridBorder)
        {
            Grid subgrid = new();

            subgrid.RowDefinitions.Add(new RowDefinition());
            subgrid.RowDefinitions.Add(new RowDefinition());
            subgrid.RowDefinitions.Add(new RowDefinition());
            subgrid.ColumnDefinitions.Add(new ColumnDefinition());
            subgrid.ColumnDefinitions.Add(new ColumnDefinition());
            subgrid.ColumnDefinitions.Add(new ColumnDefinition());

            int cellNumber = 1;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    double thicknessLeft = (col > 0) ? 1 : 0;
                    double thicknessTop = (row > 0) ? 1 : 0;
                    double thicknessRight = 0;
                    double thicknessBottom = 0;

                    Border cellBorder = new()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(thicknessLeft, thicknessTop, thicknessRight, thicknessBottom),
                        SnapsToDevicePixels = true,
                    };

                    Grid.SetColumn(cellBorder, col);
                    Grid.SetRow(cellBorder, row);

                    subgrid.Children.Add(cellBorder);

                    TextBlock textBlock = new()
                    {
                        FontSize = 30,
                        FontFamily = new FontFamily("Arial"),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = cellNumber.ToString(),
                    };

                    cellBorder.Child = textBlock;
                    cellNumber++;
                }
            }

            subgridBorder.Child = subgrid;
        }

        #endregion
    }
}