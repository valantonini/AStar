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

        /// <summary>
        /// The position of this node
        /// </summary>
        public Position Position;
        
        /// <summary>
        /// This nodes parent
        /// </summary>
        public Position ParentPosition;
    }
}
