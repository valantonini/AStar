using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PathFinderNodeFast
    {
        public int F; // f = gone + heuristic
        public int G;
        public int PX; // Parent
        public int PY;
        public byte Status;
    }

    internal class ComparePfNodeMatrix : IComparer<Point>
    {
        readonly PathFinderNodeFast[,] _matrix;

        public ComparePfNodeMatrix(PathFinderNodeFast[,] matrix)
        {
            _matrix = matrix;
        }

        public int Compare(Point a, Point b)
        {
            if (_matrix[a.X, a.Y].F > _matrix[b.X, b.Y].F)
            {
                return 1;
            }

            if (_matrix[a.X, a.Y].F < _matrix[b.X, b.Y].F)
            {
                return -1;
            }
            return 0;
        }
    }
}
