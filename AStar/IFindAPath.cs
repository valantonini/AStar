namespace AStar
{
    public interface IFindAPath
    {
        /// <summary>
        /// Determines a path between 2 positions
        /// </summary>
        /// <param name="start">start/current position</param>
        /// <param name="end">target position</param>
        /// <returns>An </returns>
        PathFinderNode[] FindPath(Position start, Position end);
    }
}
