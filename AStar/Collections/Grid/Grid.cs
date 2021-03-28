using System;

namespace AStar.Collections.Grid
{
    public class Grid<T> : IModelAGrid<T>
    {
        private readonly T[] _grid;
        protected Grid(int height, int width)
        {
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
            
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            
            Height = height;
            Width = width;

            _grid = new T[height * width];
        }

        public int Height { get; }

        public int Width { get; }
        public bool IsOutOfBounds(Position position)
        {
            return position.Row < 0 ||
                position.Row >= Height ||
                position.Column < 0 ||
                position.Column >= Width;
        }

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