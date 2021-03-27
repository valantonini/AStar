using System.Collections.Generic;

namespace AStar
{
    public interface IPathFinder
    {
        List<PathFinderNode> FindPath(Position start, Position end);
    }
}
