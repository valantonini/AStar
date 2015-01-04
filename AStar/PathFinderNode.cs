namespace AStar
{
    public struct PathFinderNode
    {
        public int F;
        public int G;
        public int H;  // f = gone + heuristic
        public int X;
        public int Y;
        public int Px; // Parent
        public int Py;
    }
}
