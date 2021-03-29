using System.Runtime.InteropServices;

namespace AStar.Collections.PathFinderNodeGrid
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct PathFinderNode
    {
        /// <summary>
        /// The position of the node
        /// </summary>
        public Position Position { get; }
        
        /// <summary>
        /// Distance from home
        /// </summary>
        public int G { get; }
        
        /// <summary>
        /// Heuristic
        /// </summary>
        public int H { get; }

        /// <summary>
        /// This nodes parent
        /// </summary>
        public Position ParentNode { get; }

        /// <summary>
        /// If the node is open or closed
        /// </summary>
        public bool? Open { get; }

        /// <summary>
        /// Gone + Heuristic (H)
        /// </summary>
        public int F { get; }

        /// <summary>
        /// If the node has been considered yet
        /// </summary>
        public bool HasBeenVisited => Open.HasValue;

        public PathFinderNode(Position position, int g, int h, Position parentNode, bool? open = null)
        {
            Position = position;
            G = g;
            H = h;
            ParentNode = parentNode;
            Open = open;
            
            F = g + h;
        }
    }
}
