namespace AStar
{
    public struct PathFinderNode
    {
        /// <summary>
        /// Gone + Heuristic (H)
        /// </summary>
        public int F;
        
        /// <summary>
        /// Distance from home
        /// </summary>
        public int G;
        
        /// <summary>
        /// Heuristic
        /// </summary>
        public int H;

        public Position Position;
        
        public Position Parent;
    }
}
