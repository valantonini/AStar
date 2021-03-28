using System.Runtime.InteropServices;

namespace AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct PathFinderNode
    {
        
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

        public PathFinderNode(int g, int h, Position parentNode, bool? open = null)
        {
            G = g;
            H = h;
            ParentNode = parentNode;
            Open = open;
            
            F = g + h;
        }
    }
}
