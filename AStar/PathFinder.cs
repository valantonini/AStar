using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.PathFinderNodeGrid;
using AStar.Collections.PriorityQueue;
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

        public PathFinder(WorldGrid worldGrid, PathFinderOptions pathFinderOptions = null)
        {
            _world = worldGrid ?? throw new ArgumentNullException(nameof(worldGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
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
            var heuristicCalculator = HeuristicFactory.Create(_options.HeuristicFormula);
            var calculationGrid = new CalculationGrid(_world.Height, _world.Width);
            var open = new SimplePriorityQueue<PathFinderNode>(new ComparePathFinderNodeByFValue());

            var startNode = new PathFinderNode(position: start, g: 0, h: 2, parentNode: start, open: true);

            calculationGrid.Set(startNode);

            open.Push(startNode);

            while (open.Count > 0)
            {
                var q = open.Pop();
                
                if (q.Position == end)
                {
                    calculationGrid.CloseNodeAt(q.Position);
                    return OrderClosedListAsArray(calculationGrid, q.Position);
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    return new Position[0];
                }

                foreach (var successorPosition in _world.GetSuccessorPositions(q.Position, _options.UseDiagonals))
                {
                    if (_world[successorPosition] == ClosedValue)
                    {
                        continue;
                    }

                    var newG = q.G + DistanceBetweenNodes;

                    if (_options.PunishChangeDirection)
                    {
                        var qIsHorizontallyAdjacent = q.Position.Row - q.ParentNode.Row == 0;
                        var successorIsHorizontallyAdjacentToQ = successorPosition.Row - q.Position.Row != 0;
                        
                        if (successorIsHorizontallyAdjacentToQ)
                        {
                            if (qIsHorizontallyAdjacent)
                            {
                                newG += Math.Abs(successorPosition.Row - end.Row) + Math.Abs(successorPosition.Column - end.Column);
                            }
                        }

                        var successorIsVerticallyAdjacentToQ = successorPosition.Column - q.Position.Column != 0;
                        if (successorIsVerticallyAdjacentToQ)
                        {
                            if (!qIsHorizontallyAdjacent)
                            {
                                newG += Math.Abs(successorPosition.Row - end.Row) + Math.Abs(successorPosition.Column - end.Column);
                            }
                        }
                    }

                    if (IsUnvisitedOrHasHigherGValue(calculationGrid[successorPosition], newG))
                    {
                        var updatedSuccessor = new PathFinderNode(
                            position: successorPosition,
                            g: newG,
                            h:heuristicCalculator.CalculateHeuristic(successorPosition, end),
                            parentNode: q.Position,
                            open: true);
                        
                        calculationGrid.Set(updatedSuccessor);

                        open.Push(updatedSuccessor);
                    }
                }

                calculationGrid.CloseNodeAt(q.Position);

                nodesVisited++;
            }

            return new Position[0];
        }

        private bool IsUnvisitedOrHasHigherGValue(PathFinderNode pathFinderNode, int newG)
        {
            return !pathFinderNode.HasBeenVisited ||
                (pathFinderNode.HasBeenVisited && newG < pathFinderNode.G);
        }

        private static Position[] OrderClosedListAsArray(CalculationGrid calcGrid, Position end)
        {
            var path = new Stack<Position>();

            var endNode = calcGrid[end];

            var currentNode = new
            {
                Position = end,
                ParentPosition = endNode.ParentNode,
            };

            while (currentNode.Position != currentNode.ParentPosition)
            {
                path.Push(new Position(currentNode.Position.Row, currentNode.Position.Column));

                var parentNode = calcGrid[currentNode.ParentPosition];

                currentNode = new
                {
                    Position = currentNode.ParentPosition,
                    ParentPosition = parentNode.ParentNode,
                };
            }

            path.Push(new Position(currentNode.Position.Row, currentNode.Position.Column));

            return path.ToArray();
        }
    }
}