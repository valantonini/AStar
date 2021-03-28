using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.Grid;
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
            var open = new SimplePriorityQueue<Position>(new ComparePathFinderNodeByFValue(calculationGrid));

            var startNode = new PathFinderNode(g: 0, h: 2, parentNode: start, open: true);

            calculationGrid[start] = startNode;

            open.Push(start);

            while (open.Count > 0)
            {
                var qPosition = open.Pop();

                if (qPosition == end)
                {
                    calculationGrid.CloseNode(qPosition);
                    return OrderClosedListAsArray(calculationGrid, qPosition);
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    return new Position[0];
                }

                foreach (var successorOffset in GridOffsets.GetOffsets(_options.UseDiagonals))
                {
                    var successorRow = qPosition.Row + successorOffset.row;
                    var successorColumn = qPosition.Column + successorOffset.column;
                    var successorPosition = new Position(successorRow, successorColumn);

                    if (_world.IsOutOfBounds(successorPosition))
                    {
                        continue;
                    }

                    if (_world[successorPosition] == ClosedValue)
                    {
                        continue;
                    }

                    var newG = calculationGrid[qPosition].G + DistanceBetweenNodes;

                    if (_options.DiagonalOptions == DiagonalOptions.HeavyDiagonals && GridOffsets.IsDiagonal(successorOffset))
                    {
                        newG *= 2;
                    }

                    if (_options.PunishChangeDirection)
                    {
                        var isLaterallyAdjacent = qPosition.Row - calculationGrid[qPosition].ParentNode.Row == 0;
                        // var isVerticallyAdjacent = currentPosition.Column - calculationGrid[currentPosition].ParentNode.Column == 0;

                        if (successorPosition.Row - qPosition.Row != 0)
                        {
                            if (isLaterallyAdjacent)
                            {
                                newG += Math.Abs(successorPosition.Row - end.Row) + Math.Abs(successorPosition.Column - end.Column);
                            }
                        }

                        if (successorPosition.Column - qPosition.Column != 0)
                        {
                            if (!isLaterallyAdjacent)
                            {
                                newG += Math.Abs(successorPosition.Row - end.Row) + Math.Abs(successorPosition.Column - end.Column);
                            }
                        }
                    }

                    if (IsUnvisitedOrHasHigherGValue(calculationGrid[successorPosition], newG))
                    {
                        var newNeighbour = new PathFinderNode(newG,
                            heuristicCalculator.CalculateHeuristic(successorPosition, end),
                            parentNode: qPosition,
                            open: true);

                        calculationGrid[successorPosition] = newNeighbour;

                        open.Push(new Position(successorPosition.Row, successorPosition.Column));
                    }
                }

                calculationGrid.CloseNode(qPosition);

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
            var path = new List<Position>();

            var endNode = calcGrid[end];

            var currentNode = new
            {
                Position = end,
                ParentPosition = endNode.ParentNode,
            };

            while (currentNode.Position != currentNode.ParentPosition)
            {
                path.Add(new Position(currentNode.Position.Row, currentNode.Position.Column));

                var parentNode = calcGrid[currentNode.ParentPosition];

                currentNode = new
                {
                    Position = currentNode.ParentPosition,
                    ParentPosition = parentNode.ParentNode,
                };
            }

            path.Add(new Position(currentNode.Position.Row, currentNode.Position.Column));

            return path.ToArray();
        }
    }
}