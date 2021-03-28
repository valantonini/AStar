using System;
using System.Collections.Generic;
using AStar.Collections;
using AStar.Heuristics;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private readonly PathFinderOptions _options;
        private readonly PathfinderGrid _pathfinderGrid;

        public PathFinder(PathfinderGrid pathfinderGrid, PathFinderOptions pathFinderOptions = null)
        {
            _pathfinderGrid = pathfinderGrid ?? throw new ArgumentNullException(nameof(pathfinderGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
        }

        ///<inheritdoc/>
        public Position[] FindPath(Position start, Position end)
        {
            var found = false;
            var closedNodeCounter = 0;
            var heuristicCalculator = HeuristicFactory.Create(_options.HeuristicFormula);
            var calcGrid = new CalculationGrid(_pathfinderGrid.Height, _pathfinderGrid.Width);
            var open = new PriorityQueue<Position>(new ComparePfNodeMatrix(calcGrid));

            var startNode = new PathFinderNode
            (
                g: 0,
                h: 2,
                parentNode: start,
                open: true
            );
            
            calcGrid[start] = startNode;

            open.Push(start);

            while (open.Count > 0)
            {
                var currentPosition = open.Pop();

                //Is it in closed list? means this node was already processed
                if (!calcGrid[currentPosition].Open.HasValue)
                {
                    continue;
                }

                if (currentPosition == end)
                {
                    calcGrid.CloseNode(currentPosition);
                    found = true;
                    break;
                }

                if (closedNodeCounter > _options.SearchLimit)
                {
                    return null;
                }

                //Lets calculate each successors
                foreach (var offsets in GridOffsets.GetOffsets(_options.Diagonals))
                {
                    //unsign incase we went out of bounds
                    var neighbour = new Position((ushort) (currentPosition.Row + offsets.row), (ushort) (currentPosition.Column + offsets.column));

                    if (neighbour.Row >= _pathfinderGrid.Height || neighbour.Column >= +_pathfinderGrid.Width)
                    {
                        continue;
                    }

                    // Blocked
                    if (_pathfinderGrid[neighbour.Row, neighbour.Column] == 0)
                    {
                        continue;
                    }

                    int newG;
                    if (_options.HeavyDiagonals && !GridOffsets.IsCardinalOffset(offsets))
                    {
                        newG = calcGrid[currentPosition].G + (int) (_pathfinderGrid[neighbour.Row, neighbour.Column] * 2.41);
                    }
                    else
                    {
                        newG = calcGrid[currentPosition].G + _pathfinderGrid[neighbour.Row, neighbour.Column];
                    }

                    if (_options.PunishChangeDirection)
                    {
                        var isLaterallyAdjacent = currentPosition.Row - calcGrid[currentPosition].ParentNode.Row == 0;

                        // var isVerticallyAdjacent = location.Column - _mCalcGrid[location.Row, location.Column].ParentNode.Column == 0;

                        if (neighbour.Row - currentPosition.Row != 0)
                        {
                            if (isLaterallyAdjacent)
                            {
                                newG += Math.Abs(neighbour.Row - end.Row) + Math.Abs(neighbour.Column - end.Column);
                            }
                        }

                        if (neighbour.Column - currentPosition.Column != 0)
                        {
                            if (!isLaterallyAdjacent)
                            {
                                newG += Math.Abs(neighbour.Row - end.Row) + Math.Abs(neighbour.Column - end.Column);
                            }
                        }
                    }

                    if (calcGrid[neighbour].HasBeenVisited && calcGrid[neighbour].G <= newG)
                    {
                        continue;
                    }

                    var newNeighbour = new PathFinderNode(
                        newG,
                        heuristicCalculator.CalculateHeuristic(neighbour, end),
                        parentNode: currentPosition,
                        open: true
                    );

                    calcGrid[neighbour] = newNeighbour;

                    open.Push(new Position(neighbour.Row, neighbour.Column));
                }

                closedNodeCounter++;
                calcGrid.CloseNode(currentPosition);
            }

            return !found ? null : OrderClosedListAsArray(calcGrid, end);
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