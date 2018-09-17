using System.Collections.Generic;

namespace AStar
{
    public interface IPathFinder
    {
        List<Point> FindPath(Point start, Point end);
    }
}
