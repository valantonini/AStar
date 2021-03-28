using AStar.Collections.Grid;

namespace AStar
{
    /// <summary>
    /// A world grid consisting of integers where a closed cell is represented by 0
    /// </summary>
    public class WorldGrid : Grid<short>
    {
        /// <summary>
        /// Creates a new world with the given dimensions initialised to closed
        /// </summary>
        /// <param name="height">height of the world (rows)</param>
        /// <param name="width">width of the world (columns)</param>
        public WorldGrid(int height, int width) : base(height, width)
        {
        }
    }
}