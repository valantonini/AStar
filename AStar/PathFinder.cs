using System;
using System.Collections.Generic;
using AStar.Heuristics;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private readonly PathFinderOptions _options;
        private readonly PathfinderGrid _pathfinderGrid;
        
        private bool isLaterallyAdjacent;

        public PathFinder(PathfinderGrid pathfinderGrid, PathFinderOptions pathFinderOptions = null)
        {
            _pathfinderGrid = pathfinderGrid ?? throw new ArgumentNullException(nameof(pathfinderGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
        }

        ///<inheritdoc/>
        public Position[] FindPath(Position start, Position end)
        {
            lock (this)
            {
                var found = false;
                var closedNodeCounter = 0;
                var heuristicCalculator = HeuristicFactory.Create(_options.HeuristicFormula);
                var calcGrid = new PathFinderNode[_pathfinderGrid.Height, _pathfinderGrid.Width];
                var open = new PriorityQueueB<Position>(new ComparePfNodeMatrix(calcGrid));

                calcGrid[start.Row, start.Column].G = 0;
                calcGrid[start.Row, start.Column].F = 2;
                calcGrid[start.Row, start.Column].ParentNode = new Position(start.Row, start.Column);
                calcGrid[start.Row, start.Column].Open = true;

                open.Push(start);

                while (open.Count > 0)
                {
                    var location = open.Pop();

                    //Is it in closed list? means this node was already processed
                    if (!calcGrid[location.Row, location.Column].Open.HasValue)
                    {
                        continue;
                    }

                    if (location == end)
                    {
                        calcGrid[location.Row, location.Column].Open = false;
                        found = true;
                        break;
                    }

                    if (closedNodeCounter > _options.SearchLimit)
                    {
                        return null;
                    }

                    if (_options.PunishChangeDirection)
                    {
                        isLaterallyAdjacent = location.Row - calcGrid[location.Row, location.Column].ParentNode.Row == 0;
                        //isVerticallyAdjacent = location.Column - _mCalcGrid[location.Row, location.Column].ParentNode.Column == 0;
                    }

                    //Lets calculate each successors
                    foreach (var offsets in GridOffsets.GetOffsets(_options.Diagonals))
                    {
                        //unsign incase we went out of bounds
                        var nextCandidate = new Position((ushort) (location.Row + offsets.row), (ushort) (location.Column + offsets.column));

                        if (nextCandidate.Row >= _pathfinderGrid.Height || nextCandidate.Column >= +_pathfinderGrid.Width)
                        {
                            continue;
                        }

                        // Blocked
                        if (_pathfinderGrid[nextCandidate.Row, nextCandidate.Column] == 0)
                        {
                            continue;
                        }

                        int newG;
                        if (_options.HeavyDiagonals && !GridOffsets.IsCardinalOffset(offsets))
                        {
                            newG = calcGrid[location.Row, location.Column].G + (int) (_pathfinderGrid[nextCandidate.Row, nextCandidate.Column] * 2.41);
                        }
                        else
                        {
                            newG = calcGrid[location.Row, location.Column].G + _pathfinderGrid[nextCandidate.Row, nextCandidate.Column];
                        }

                        if (_options.PunishChangeDirection)
                        {
                            if (nextCandidate.Row - location.Row != 0)
                            {
                                if (isLaterallyAdjacent)
                                {
                                    newG += Math.Abs(nextCandidate.Row - end.Row) + Math.Abs(nextCandidate.Column - end.Column);
                                }
                            }

                            if (nextCandidate.Column - location.Column != 0)
                            {
                                if (!isLaterallyAdjacent)
                                {
                                    newG += Math.Abs(nextCandidate.Row - end.Row) + Math.Abs(nextCandidate.Column - end.Column);
                                }
                            }
                        }

                        if (calcGrid[nextCandidate.Row, nextCandidate.Column].HasBeenVisited)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (calcGrid[nextCandidate.Row, nextCandidate.Column].G <= newG)
                            {
                                continue;
                            }
                        }

                        calcGrid[nextCandidate.Row, nextCandidate.Column].ParentNode = new Position(location.Row, location.Column);
                        calcGrid[nextCandidate.Row, nextCandidate.Column].G = newG;

                        var h = heuristicCalculator.CalculateHeuristic(nextCandidate, end);

                        if (_options.TieBreaker)
                        {
                            var dx1 = location.Row - end.Row;
                            var dy1 = location.Column - end.Column;
                            var dx2 = start.Row - end.Row;
                            var dy2 = start.Column - end.Column;
                            var cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                            h = (int) (h + cross * 0.001);
                        }

                        calcGrid[nextCandidate.Row, nextCandidate.Column].F = newG + h;

                        open.Push(new Position(nextCandidate.Row, nextCandidate.Column));

                        calcGrid[nextCandidate.Row, nextCandidate.Column].Open = true;
                    }

                    closedNodeCounter++;
                    calcGrid[location.Row, location.Column].Open = false;
                }

                return !found ? null : OrderClosedListAsArray(calcGrid, end);
            }
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