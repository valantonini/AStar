namespace AStar.Collections
{
    public class Grid<T> : IModelAGrid<T>
    {
        private readonly T[] _grid;

        public Grid(int height, int width)
        {
            Height = height;
            Width = width;

            _grid = new T[height * width];
        }

        public int Height { get; }

        public int Width { get; }

        public T this[Position position]
        {
            get
            {
                return _grid[ConvertRowColumnToIndex(position.Row, position.Column)];
            }
            set
            {
                _grid[ConvertRowColumnToIndex(position.Row, position.Column)] = value;
            }
        }
        public T this[int row, int column]
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