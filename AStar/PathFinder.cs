using System;
using System.Collections.Generic;
using AStar.Collections;
using AStar.Heuristics;

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
        public Position[] FindPath(Position start, Position end)
        {
            var nodesVisited = 0;
            var heuristicCalculator = HeuristicFactory.Create(_options.HeuristicFormula);
            var calculationGrid = new CalculationGrid(_world.Height, _world.Width);
            var open = new PriorityQueue<Position>(new ComparePfNodeMatrix(calculationGrid));

            var startNode = new PathFinderNode(g: 0,
                h: 2,
                parentNode: start,
                open: true);

            calculationGrid[start] = startNode;
            
            open.Push(start);

            while (open.Count > 0)
            {
                var currentPosition = open.Pop();

                if (currentPosition == end)
                {
                    calculationGrid.CloseNode(currentPosition);
                    return OrderClosedListAsArray(calculationGrid, currentPosition);
                }

                if (nodesVisited > _options.SearchLimit)
                {
                    return new Position[0];
                }

                //Lets calculate each successors
                foreach (var offsets in GridOffsets.GetOffsets(_options.UseDiagonals))
                {
                    //unsign incase we went out of bounds
                    var neighbourPosition = new Position((ushort) (currentPosition.Row + offsets.row), (ushort) (currentPosition.Column + offsets.column));

                    if (neighbourPosition.Row >= _world.Height || neighbourPosition.Column >= +_world.Width)
                    {
                        continue;
                    }

                    // Blocked
                    if (_world[neighbourPosition] == ClosedValue)
                    {
                        continue;
                    }

                    var newG = calculationGrid[currentPosition].G + DistanceBetweenNodes;
                    if (_options.DiagonalOptions == DiagonalOptions.HeavyDiagonals && GridOffsets.IsDiagonal(offsets))
                    {
                        newG *= 2;
                    }

                    if (_options.PunishChangeDirection)
                    {
                        var isLaterallyAdjacent = currentPosition.Row - calculationGrid[currentPosition].ParentNode.Row == 0;

                        // var isVerticallyAdjacent = location.Column - _mCalcGrid[location.Row, location.Column].ParentNode.Column == 0;

                        if (neighbourPosition.Row - currentPosition.Row != 0)
                        {
                            if (isLaterallyAdjacent)
                            {
                                newG += Math.Abs(neighbourPosition.Row - end.Row) + Math.Abs(neighbourPosition.Column - end.Column);
                            }
                        }

                        if (neighbourPosition.Column - currentPosition.Column != 0)
                        {
                            if (!isLaterallyAdjacent)
                            {
                                newG += Math.Abs(neighbourPosition.Row - end.Row) + Math.Abs(neighbourPosition.Column - end.Column);
                            }
                        }
                    }

                    if (IsUnvisitedOrHasHigherGValue(calculationGrid[neighbourPosition], newG))
                    {
                        var newNeighbour = new PathFinderNode(newG,
                            heuristicCalculator.CalculateHeuristic(neighbourPosition, end),
                            parentNode: currentPosition,
                            open: true);

                        calculationGrid[neighbourPosition] = newNeighbour;

                        open.Push(new Position(neighbourPosition.Row, neighbourPosition.Column));
                    }
                }
                
                calculationGrid.CloseNode(currentPosition);
                
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