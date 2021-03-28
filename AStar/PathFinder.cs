using System;
using System.Collections.Generic;
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
            var calcGrid = new PathFinderNode[_pathfinderGrid.Height, _pathfinderGrid.Width];
            var open = new PriorityQueueB<Position>(new ComparePfNodeMatrix(calcGrid));

            var startNode = new PathFinderNode
            {
                G = 0,
                H = 2,
                ParentNode = start,
                Open = true,
            };
            
            calcGrid[start.Row, start.Column] = startNode;

            open.Push(start);

            while (open.Count > 0)
            {
                var currentPosition = open.Pop();

                //Is it in closed list? means this node was already processed
                if (!calcGrid[currentPosition.Row, currentPosition.Column].Open.HasValue)
                {
                    continue;
                }

                if (currentPosition == end)
                {
                    calcGrid[currentPosition.Row, currentPosition.Column].Open = false;
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
                        newG = calcGrid[currentPosition.Row, currentPosition.Column].G + (int) (_pathfinderGrid[neighbour.Row, neighbour.Column] * 2.41);
                    }
                    else
                    {
                        newG = calcGrid[currentPosition.Row, currentPosition.Column].G + _pathfinderGrid[neighbour.Row, neighbour.Column];
                    }

                    if (_options.PunishChangeDirection)
                    {
                        var isLaterallyAdjacent = currentPosition.Row - calcGrid[currentPosition.Row, currentPosition.Column].ParentNode.Row == 0;

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

                    if (calcGrid[neighbour.Row, neighbour.Column].HasBeenVisited && calcGrid[neighbour.Row, neighbour.Column].G <= newG)
                    {
                        continue;
                    }

                    var newNeighbour = new PathFinderNode
                    {
                        G = newG,
                        H = heuristicCalculator.CalculateHeuristic(neighbour, end),
                        ParentNode = currentPosition,
                        Open = true,
                    };

                    calcGrid[neighbour.Row, neighbour.Column] = newNeighbour;

                    open.Push(new Position(neighbour.Row, neighbour.Column));
                }

                closedNodeCounter++;
                calcGrid[currentPosition.Row, currentPosition.Column].Open = false;
            }

            return !found ? null : OrderClosedListAsArray(calcGrid, end);
        }

        private static Position[] OrderClosedListAsArray(PathFinderNode[,] calcGrid, Position end)
        {
            var path = new List<Position>();

            var endNode = calcGrid[end.Row, end.Column];

            var currentNode = new
            {
                Position = end,
                ParentPosition = endNode.ParentNode,
            };

            while (currentNode.Position != currentNode.ParentPosition)
            {
                path.Add(new Position(currentNode.Position.Row, currentNode.Position.Column));

                var parentNode = calcGrid[currentNode.ParentPosition.Row, currentNode.ParentPosition.Column];

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