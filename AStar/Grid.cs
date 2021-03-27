namespace AStar
{
    public class Grid
    {
        private readonly int[] _grid;

        public Grid(int height, int width)
        {
            Height = height;
            Width = width;

            _grid = new int[height * width];
        }

        public int Height { get; }

        public int Width { get; }

        public int this[int row, int column]
        {
            get
            {
                return _grid[ConvertRowColumnToIndex(row, column)];
            }
            set
            {
                _grid[ConvertRowColumnToIndex(row, column)] = value;
            }
        }

        private int ConvertRowColumnToIndex(int row, int column)
        {
            return Width * row + column;
        }
    }
}