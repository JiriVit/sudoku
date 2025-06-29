using System.Diagnostics;
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
        #region .: Private Variables :.

        private readonly ViewModel viewModel = new();

        #endregion

        #region .: Constructor :.

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

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

            // iterate through 3x3 subgrids
            for (int subgridRow = 0; subgridRow < 3; subgridRow++)
            {
                for (int subgridCol = 0; subgridCol < 3; subgridCol++)
                {
                    double thicknessLeft = (subgridCol > 0) ? 1 : 0;
                    double thicknessTop = (subgridRow > 0) ? 1 : 0;
                    double thicknessRight = (subgridCol < 2) ? 1 : 0;
                    double thicknessBottom = (subgridRow < 2) ? 1 : 0;

                    Border subgridBorder = new()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(thicknessLeft, thicknessTop, thicknessRight, thicknessBottom),
                        SnapsToDevicePixels = true,
                    };

                    Grid.SetColumn(subgridBorder, subgridCol);
                    Grid.SetRow(subgridBorder, subgridRow);

                    InsertSubgrid(subgridBorder, subgridRow, subgridCol);

                    gridMain.Children.Add(subgridBorder);
                }
            }

        }

        private void InsertSubgrid(Border subgridBorder, int subgridRow, int subgridCol)
        {
            Grid subgrid = new();

            for (int i = 0; i < 3; i++)
            {
                subgrid.RowDefinitions.Add(new RowDefinition());
                subgrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // iterate through subgrid cells
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
                    };

                    // calculate cell index within a 1D array
                    int totalCol = subgridCol * 3 + col;
                    int totalRow = subgridRow * 3 + row;
                    int cellIndex = totalRow * 9 + totalCol;

                    // configure bindings
                    textBlock.DataContext = viewModel.Cells[cellIndex];
                    textBlock.SetBinding(TextBlock.TextProperty, "Number");
                    textBlock.SetBinding(TextBlock.ForegroundProperty, "Foreground");
                    cellBorder.DataContext = viewModel.Cells[cellIndex];
                    cellBorder.SetBinding(Border.BackgroundProperty, "Background");

                    // configure event handlers
                    cellBorder.MouseEnter += CellBorder_MouseEnter;
                    cellBorder.MouseLeave += CellBorder_MouseLeave;
                    cellBorder.MouseDown += CellBorder_MouseDown;

                    // place the TextBlock to the Border
                    cellBorder.Child = textBlock;
                }
            }

            subgridBorder.Child = subgrid;
        }

        #endregion

        #region .: Event Handlers :.

        #region .: CellBorder :.

        private static void CellBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            CellModel cellModel = (CellModel)((Border)sender).DataContext;

            cellModel.MouseOver = true;
        }

        private static void CellBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            CellModel cellModel = (CellModel)((Border)sender).DataContext;

            cellModel.MouseOver = false;
        }

        private void CellBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CellModel cell = (CellModel)((Border)sender).DataContext;
            viewModel.SelectCell(cell);
        }

        #endregion

        #region .: Window :.

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                viewModel.SetNumberInSelectedCell(e.Key - Key.D0);
            }
            else if (e.Key >= Key.NumPad1 && e.Key <= Key.NumPad9)
            {
                viewModel.SetNumberInSelectedCell(e.Key - Key.NumPad0);
            }
            else if (e.Key == Key.Delete)
            {
                viewModel.ClearNumberInSelectedCell();
            }
        }

        #endregion

        #region .: Button :.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                if (b.Tag is string s)
                {
                    switch(s)
                    {
                        case "Sample":
                            viewModel.LoadSample();
                            break;
                        case "Generate":
                            viewModel.Generate();
                            break;
                        case "Solve":
                            viewModel.Solve();
                            break;
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}