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
        }
        
        public void Set(PathFinderNode pathFinderNode)
        { 
            _internalGrid[pathFinderNode.Position.Row, pathFinderNode.Position.Column] = pathFinderNode;
        }

        public void CloseNodeAt(Position position)
        {
            SetNodeOpenStatus(position, false);
        }

        private void SetNodeOpenStatus(Position position, bool? openStatus)
        {
            var node = new PathFinderNode
            (
                position: position,
                g: this[position].G,
                h: this[position].H,
                parentNode: this[position].ParentNode,
                open: openStatus
            );
            
            Set(node);
        }
    }
}