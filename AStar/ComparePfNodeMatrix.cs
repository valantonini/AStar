using System.Collections.Generic;

namespace AStar
{
    internal class ComparePfNodeMatrix : IComparer<Position>
    {
        readonly PathFinderNodeFast[,] _matrix;

        public ComparePfNodeMatrix(PathFinderNodeFast[,] matrix)
        {
            _matrix = matrix;
        }

        public int Compare(Position a, Position b)
        {
            if (_matrix[a.Row, a.Column].F_Gone_Plus_Heuristic > _matrix[b.Row, b.Column].F_Gone_Plus_Heuristic)
            {
                return 1;
            }

            if (_matrix[a.Row, a.Column].F_Gone_Plus_Heuristic < _matrix[b.Row, b.Column].F_Gone_Plus_Heuristic)
            {
                return -1;
            }
            return 0;
        }
    }
}