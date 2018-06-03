using Xamarin.Forms;

namespace DiceRoller.Controls
{
    internal struct LayoutData
    {
        public int VisibleChildCount { get; }

        public Size CellSize { get; }

        public int Rows { get; }

        public int Columns { get; }

        public LayoutData(int visibleChildCount, Size cellSize, int rows, int columns) : this()
        {
            VisibleChildCount = visibleChildCount;
            CellSize = cellSize;
            Rows = rows;
            Columns = columns;
        }

    }
}
