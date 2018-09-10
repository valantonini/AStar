using System.Collections.Generic;
using System.Drawing;

namespace AStar
{
    public interface IPathFinder
    {
        List<PathFinderNode> FindPath(Point start, Point end);
    }
}
