using System.Collections.Generic;

namespace AStar
{
    internal class ComparePfNodeMatrix : IComparer<Point>
    {
        readonly PathFinderNodeFast[,] _matrix;

        public ComparePfNodeMatrix(PathFinderNodeFast[,] matrix)
        {
            _matrix = matrix;
        }

        public int Compare(Point a, Point b)
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