namespace AStar.Collections.PathFinderNodeGrid
{
    internal class CalculationGrid
    {
        private readonly PathFinderNode[,] _internalGrid;

        public CalculationGrid(int height, int width)
        {
            _internalGrid = new PathFinderNode[height, width];
        }

        public PathFinderNode this[Position position]
        {
            get
            {
                return _internalGrid[position.Row, position.Column];
            }
            set
            {
                _internalGrid[position.Row, position.Column] = value;
            }
        }

        public void CloseNode(Position position)
        {
            SetNodeOpenStatus(position, false);
        }

        private void SetNodeOpenStatus(Position position, bool? openStatus)
        {
            this[position] = new PathFinderNode
            (
                this[position].G,
                this[position].H,
                this[position].ParentNode,
                openStatus
            );
        }
    }
}