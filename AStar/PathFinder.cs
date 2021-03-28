using System;
using System.Collections.Generic;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private readonly List<PathFinderNode> _closed = new List<PathFinderNode>();
        private readonly PathFinderOptions _options;
        private readonly PathfinderGrid _pathfinderGrid;
        
        private PathFinderNode[,] _mCalcGrid;
        private IPriorityQueue<Position> _open;
        private int _horiz;

        public PathFinder(PathfinderGrid pathfinderGrid, PathFinderOptions pathFinderOptions = null)
        {
            _pathfinderGrid = pathfinderGrid ?? throw new ArgumentNullException(nameof(pathfinderGrid));
            _options = pathFinderOptions ?? new PathFinderOptions();
        }

        private void ResetCalcGrid()
        {
            _mCalcGrid = new PathFinderNode[_pathfinderGrid.Height, _pathfinderGrid.Width];
            _open = new PriorityQueueB<Position>(new ComparePfNodeMatrix(_mCalcGrid));
            _closed.Clear();
        }

        ///<inheritdoc/>
        public Position[] FindPath(Position start, Position end)
        {
            lock (this)
            {
                var found = false;
                var closedNodeCounter = 0;
                ResetCalcGrid();

                _mCalcGrid[start.Row, start.Column].G = 0;
                _mCalcGrid[start.Row, start.Column].F = _options.HeuristicEstimate;
                _mCalcGrid[start.Row, start.Column].ParentPosition = new Position(start.Row, start.Column);
                _mCalcGrid[start.Row, start.Column].Open = true;

                _open.Push(start);

                while (_open.Count > 0)
                {
                    var location = _open.Pop();

                    //Is it in closed list? means this node was already processed
                    if (!_mCalcGrid[location.Row, location.Column].Open.HasValue)
                    {
                        continue;
                    }

                    if (location == end)
                    {
                        _mCalcGrid[location.Row, location.Column].Open = false;
                        found = true;
                        break;
                    }

                    if (closedNodeCounter > _options.SearchLimit)
                    {
                        return null;
                    }

                    if (_options.PunishChangeDirection)
                    {
                        _horiz = location.Row - _mCalcGrid[location.Row, location.Column].ParentPosition.Row;
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
                            newG = _mCalcGrid[location.Row, location.Column].G + (int) (_pathfinderGrid[nextCandidate.Row, nextCandidate.Column] * 2.41);
                        }
                        else
                        {
                            newG = _mCalcGrid[location.Row, location.Column].G + _pathfinderGrid[nextCandidate.Row, nextCandidate.Column];
                        }

                        if (_options.PunishChangeDirection)
                        {
                            if (nextCandidate.Row - location.Row != 0)
                            {
                                if (_horiz == 0)
                                {
                                    newG += Math.Abs(nextCandidate.Row - end.Row) + Math.Abs(nextCandidate.Column - end.Column);
                                }
                            }

                            if (nextCandidate.Column - location.Column != 0)
                            {
                                if (_horiz != 0)
                                {
                                    newG += Math.Abs(nextCandidate.Row - end.Row) + Math.Abs(nextCandidate.Column - end.Column);
                                }
                            }
                        }

                        if (_mCalcGrid[nextCandidate.Row, nextCandidate.Column].HasBeenVisited)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (_mCalcGrid[nextCandidate.Row, nextCandidate.Column].G <= newG)
                            {
                                continue;
                            }
                        }

                        _mCalcGrid[nextCandidate.Row, nextCandidate.Column].ParentPosition = new Position(location.Row, location.Column);
                        _mCalcGrid[nextCandidate.Row, nextCandidate.Column].G = newG;

                        var h = Heuristic.DetermineH(_options.Formula, end, _options.HeuristicEstimate, nextCandidate.Column, nextCandidate.Row);

                        if (_options.TieBreaker)
                        {
                            var dx1 = location.Row - end.Row;
                            var dy1 = location.Column - end.Column;
                            var dx2 = start.Row - end.Row;
                            var dy2 = start.Column - end.Column;
                            var cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                            h = (int) (h + cross * 0.001);
                        }

                        _mCalcGrid[nextCandidate.Row, nextCandidate.Column].F = newG + h;

                        _open.Push(new Position(nextCandidate.Row, nextCandidate.Column));

                        _mCalcGrid[nextCandidate.Row, nextCandidate.Column].Open = true;
                    }

                    closedNodeCounter++;
                    _mCalcGrid[location.Row, location.Column].Open = false;
                }

                return !found ? null : OrderClosedListAsArray(end);
            }
        }

        private Position[] OrderClosedListAsArray(Position end)
        {
            var path = new List<Position>();

            var endNode = _mCalcGrid[end.Row, end.Column];

            var currentNode = new
            {
                Position = end,
                endNode.ParentPosition,
            };
            
            while (currentNode.Position != currentNode.ParentPosition)
            {
                path.Add(new Position(currentNode.Position.Row, currentNode.Position.Column));

                var parentNode = _mCalcGrid[currentNode.ParentPosition.Row, currentNode.ParentPosition.Column];

                currentNode = new
                {
                    Position = currentNode.ParentPosition,
                    parentNode.ParentPosition,
                };
            }

            path.Add(new Position(currentNode.Position.Row, currentNode.Position.Column));

            return path.ToArray();
        }
    }
}