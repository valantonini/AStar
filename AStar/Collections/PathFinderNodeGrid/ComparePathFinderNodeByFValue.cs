using System.Collections.Generic;

namespace AStar.Collections.PathFinderNodeGrid
{
    internal class ComparePathFinderNodeByFValue : IComparer<PathFinderNode>
    {
        public int Compare(PathFinderNode a, PathFinderNode b)
        {
            if (a.F > b.F)
            {
                return 1;
            }

            if (a.F < b.F)
            {
                return -1;
            }
            
            return 0;
        }
    }
}