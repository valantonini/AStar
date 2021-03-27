using System.Runtime.InteropServices;

namespace AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
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
        
        /// <summary>
        /// If the node is open or closed
        /// </summary>
        public bool? Open;
        
        /// <summary>
        /// If the node has been considered yet
        /// </summary>
        public bool HasBeenVisited => Open.HasValue;
    }
}
