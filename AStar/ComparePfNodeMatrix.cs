using System.Collections.Generic;

namespace AStar
{
    internal class ComparePfNodeMatrix : IComparer<Position>
    {
        readonly PathFinderNode[,] _matrix;

        public ComparePfNodeMatrix(PathFinderNode[,] matrix)
        {
            _matrix = matrix;
        }

        public int Compare(Position a, Position b)
        {
            if (_matrix[a.Row, a.Column].F > _matrix[b.Row, b.Column].F)
            {
                return 1;
            }

            if (_matrix[a.Row, a.Column].F < _matrix[b.Row, b.Column].F)
            {
                return -1;
            }
            return 0;
        }
    }
}