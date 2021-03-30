using System.Collections.Generic;
using System.Linq;
using AStar.Collections.MultiDimensional;
using AStar.Collections.PriorityQueue;

namespace AStar.Collections.PathFinder
{
    internal class PathFinderGraph : IModelAGraph<PathFinderNode>
    {
        private readonly bool _allowDiagonalTraversal;
        private readonly Grid<PathFinderNode> _internalGrid;
        private readonly SimplePriorityQueue<PathFinderNode> _open = new SimplePriorityQueue<PathFinderNode>(new ComparePathFinderNodeByFValue());

        public bool HasOpenNodes
        {
            get
            {
                return _open.Count > 0;
            }
        }
        public PathFinderGraph(int height, int width, bool allowDiagonalTraversal)
        {
            _allowDiagonalTraversal = allowDiagonalTraversal;
            _internalGrid = new Grid<PathFinderNode>(height, width);
            Initialise();
        }

        private void Initialise()
        {
            for (var row = 0; row < _internalGrid.Height; row++)
            {
                for (var column = 0; column < _internalGrid.Width; column++)
                {
                    _internalGrid[row, column] = new PathFinderNode(position: new Position(row, column),
                        g: 0,
                        h: 0,
                        parentNodePosition: default);
                }
            }
            
            _open.Clear();
        }

        public IEnumerable<PathFinderNode> GetSuccessors(PathFinderNode node)
        {
            return _internalGrid
                .GetSuccessorPositions(node.Position, _allowDiagonalTraversal)
                .Select(successorPosition => _internalGrid[successorPosition]);
        }

        public PathFinderNode GetParent(PathFinderNode node)
        {
            return _internalGrid[node.ParentNodePosition];
        }

        public void OpenNode(PathFinderNode node)
        {
            _internalGrid[node.Position] = node;
            _open.Push(node);
        }

        public PathFinderNode GetOpenNodeWithSmallestF()
        {
            return _open.Pop();
        }
    }
}