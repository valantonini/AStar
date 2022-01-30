using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.PathFinder;
using AStar.Heuristics;
using AStar.Options;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private const int ClosedValue = 0;
        private const int DistanceBetweenNodes = 1;
        private readonly PathFinderOptions _options;
        private readonly WorldGrid _world;
        private readonly ICalculateHeuristic _heuristic;

        public PathFinder(WorldGrid worldGrid, PathFinderOptions pathFinderOptions = null)
        {
            _world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
            _heuristic = HeuristicFactory.Create(_options.HeuristicFormula);
        }
        
        ///<inheritdoc/>
        public Point[] FindPath(Point start, Point end)
        {
            return FindPath(new Position(start.Y, start.X), new Position(end.Y, end.X))
                .Select(position => new Point(position.Column, position.Row))
                .ToArray();
        }
        
        ///<inheritdoc/>
        public Position[] FindPath(Position start, Position end)
        {
            var nodesVisited = 0;
            IModelAGraph<PathFinderNode> graph = new PathFinderGraph(_world.Height, _world.Width, _options.UseDiagonals);

            var startNode = new PathFinderNode(position: start, g: 0, h: 2, parentNodePosition: start);
            graph.OpenNode(startNode);

            while (graph.HasOpenNodes)
            {
                var q = graph.GetOpenNodeWithSmallestF();
                
                if (q.Position == end)
                {
                    return OrderClosedNodesAsArray(graph, q);
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    return new Position[0];
                }

                foreach (var successor in graph.GetSuccessors(q))
                {
                    if (_world[successor.Position] == ClosedValue)
                    {
                        continue;
                    }

                    var newG = q.G + DistanceBetweenNodes;

                    if (_options.PunishChangeDirection)
                    {
                        newG += CalculateModifierToG(q, successor, end);
                    }

                    var updatedSuccessor = new PathFinderNode(
                        position: successor.Position,
                        g: newG,
                        h:_heuristic.Calculate(successor.Position, end),
                        parentNodePosition: q.Position);
                    
                    if (BetterPathToSuccessorFound(updatedSuccessor, successor))
                    {
                        graph.OpenNode(updatedSuccessor);
                    }
                }

                nodesVisited++;
            }

            return new Position[0];
        }

        private int CalculateModifierToG(PathFinderNode q, PathFinderNode successor, Position end)
        {
            if (q.Position == q.ParentNodePosition)
            {
                return 0;
            }
            
            var gPunishment = Math.Abs(successor.Position.Row - end.Row) + Math.Abs(successor.Position.Column - end.Column);
            
            var successorIsVerticallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;

            if (successorIsVerticallyAdjacentToQ)
            {
                var qIsVerticallyAdjacentToParent = q.Position.Row - q.ParentNodePosition.Row == 0;
                if (qIsVerticallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            var successorIsHorizontallyAdjacentToQ = successor.Position.Row - q.Position.Row != 0;

            if (successorIsHorizontallyAdjacentToQ)
            {
                var qIsHorizontallyAdjacentToParent = q.Position.Row - q.ParentNodePosition.Row == 0;
                if (qIsHorizontallyAdjacentToParent)
                {
                    return gPunishment;
                }
            }

            if (_options.UseDiagonals)
            {
                var successorIsDiagonallyAdjacentToQ = (successor.Position.Column - successor.Position.Row) == (q.Position.Column - q.Position.Row);
                if (successorIsDiagonallyAdjacentToQ)
                {
                    var qIsDiagonallyAdjacentToParent = (q.Position.Column - q.Position.Row) == (q.ParentNodePosition.Column - q.ParentNodePosition.Row)
                                                        && IsStraightLine(q.ParentNodePosition, q.Position, successor.Position);
                    if (qIsDiagonallyAdjacentToParent)
                    {
                        return gPunishment;
                    }
                }
            }

            return 0;
        }

        private bool IsStraightLine(Position a, Position b, Position c)
        {
            // area of triangle == 0
            return (a.Column * (b.Row - c.Row) + b.Column * (c.Row - a.Row) + c.Column * (a.Row - b.Row)) / 2 == 0;
        }

        private bool BetterPathToSuccessorFound(PathFinderNode updateSuccessor, PathFinderNode currentSuccessor)
        {
            return !currentSuccessor.HasBeenVisited ||
                (currentSuccessor.HasBeenVisited && updateSuccessor.F < currentSuccessor.F);
        }

        private static Position[] OrderClosedNodesAsArray(IModelAGraph<PathFinderNode> graph, PathFinderNode endNode)
        {
            var path = new Stack<Position>();

            var currentNode = endNode;

            while (currentNode.Position != currentNode.ParentNodePosition)
            {
                path.Push(currentNode.Position);
                currentNode = graph.GetParent(currentNode);
            }

            path.Push(currentNode.Position);

            return path.ToArray();
        }
    }
}