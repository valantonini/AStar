using System.Drawing;

namespace AStar
{
    public interface IFindAPath
    {
        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of positions from the start to end position or empty[] if unreachable</returns>
        Position[] FindPath(Position start, Position end);
        
        /// <summary>
        /// Determines a path between 2 positions where the point's X
        /// represents the column and the point's Y represents the row
        /// </summary>
        /// <param name="start">start position</param>
        /// <param name="end">target position</param>
        /// <returns>An array of points from the start to end points or empty[] if unreachable</returns>
        Point[] FindPath(Point start, Point end);
    }
}
