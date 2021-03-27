﻿using System.Runtime.InteropServices;

namespace AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PathFinderNodeFast
    {
        public int F_Gone_Plus_Heuristic; // f = gone + heuristic
        public int Gone;
        public Position Parent;
        public bool? Open;
        public bool HasBeenVisited => Open.HasValue;
    }
}
