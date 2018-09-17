namespace AStar
{
    public struct PathFinderNode
    {
        public int F;
        public int Gone;
        public int heuristic;  // f = gone + heuristic
        public int Row;
        public int Column;
        public int ParentRow; // Parent
        public int ParentColumn;
    }
}
