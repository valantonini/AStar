namespace AStar.Collections
{
    public class CalculationGrid
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

        public void SetNodeOpenStatus(Position position, bool? openStatus)
        {
            this[position] = new PathFinderNode
            {
                G = this[position].G,
                H = this[position].H,
                ParentNode = this[position].ParentNode,
                Open = openStatus,
            };
        }
        
        public void UpdateG(Position position, int g)
        {
            this[position] = new PathFinderNode
            {
                G = g,
                H = this[position].H,
                ParentNode = this[position].ParentNode,
                Open = this[position].Open,
            };
        }
    }
}