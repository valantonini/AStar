using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PathFinderNodeFast
    {
        public int F; // f = gone + heuristic
        public int G;
        public ushort PX; // Parent
        public ushort PY;
        public byte Status;
    }

    internal class ComparePfNodeMatrix : IComparer<int>
    {
        readonly PathFinderNodeFast[] _matrix;

        public ComparePfNodeMatrix(PathFinderNodeFast[] matrix)
        {
            _matrix = matrix;
        }

        public int Compare(int a, int b)
        {
            if (_matrix[a].F > _matrix[b].F)
            {
                return 1;
            }

            if (_matrix[a].F < _matrix[b].F)
            {
                return -1;
            }
            return 0;
        }
    }
}
