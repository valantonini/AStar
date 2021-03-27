namespace AStar
{
    public struct PathFinderNode
    {
        public int F_Gone_Plus_Heuristic;
        public int Gone;
        public int Heuristic;  // f = gone + heuristic
        public int X;
        public int Y;
        public Position Parent;
    }
}
