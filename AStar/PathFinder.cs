using System;
using System.Collections.Generic;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private readonly PathfinderGrid _pathfinderGrid;
        private readonly IPriorityQueue<Position> _open;
        private readonly List<PathFinderNode> _closed = new List<PathFinderNode>();
        private readonly PathFinderNode[,] _mCalcGrid;
        private readonly PathFinderOptions _options;
        private int _horiz;

        public PathFinder(PathfinderGrid pathfinderGrid, PathFinderOptions pathFinderOptions = null)
        {
            _pathfinderGrid = pathfinderGrid ?? throw new ArgumentNullException(nameof(pathfinderGrid));

            _mCalcGrid = new PathFinderNode[pathfinderGrid.Height, pathfinderGrid.Width];

            _open = new PriorityQueueB<Position>(new ComparePfNodeMatrix(_mCalcGrid));

            _options = pathFinderOptions ?? new PathFinderOptions();
        }

        ///<inheritdoc/>
        public PathFinderNode[] FindPath(Position start, Position end)
        {
            lock (this)
            {
                var found = false;
                var closedNodeCounter = 0;
                _open.Clear();
                _closed.Clear();

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

                    var locationX = location.Row;
                    var locationY = location.Column;

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
                        _horiz = locationX - _mCalcGrid[location.Row, location.Column].ParentPosition.Row;
                    }

                    //Lets calculate each successors
                    foreach (var offsets in GridOffsets.GetOffsets(_options.Diagonals))
                    {
                        //unsign incase we went out of bounds

                        var newLocation = new Position( (ushort)(locationX + offsets.row),  (ushort)(locationY + offsets.column));

                        if (newLocation.Row >= _pathfinderGrid.Height || newLocation.Column >= +_pathfinderGrid.Width)
                        {
                            continue;
                        }

                        // Unbreakeable?
                        if (_pathfinderGrid[newLocation.Row, newLocation.Column] == 0)
                        {
                            continue;
                        }

                        int newG;
                        if (_options.HeavyDiagonals && !IsCardinalOffset(offsets))
                        {
                            newG = _mCalcGrid[location.Row, location.Column].G + (int)(_pathfinderGrid[newLocation.Row, newLocation.Column] * 2.41);
                        }
                        else
                        {
                            newG = _mCalcGrid[location.Row, location.Column].G + _pathfinderGrid[newLocation.Row, newLocation.Column];
                        }

                        if (_options.PunishChangeDirection)
                        {
                            if (newLocation.Row - locationX != 0)
                            {
                                if (_horiz == 0)
                                {
                                    newG += Math.Abs(newLocation.Row - end.Row) + Math.Abs(newLocation.Column - end.Column);
                                }
                            }
                            if (newLocation.Column - locationY != 0)
                            {
                                if (_horiz != 0)
                                {
                                    newG += Math.Abs(newLocation.Row - end.Row) + Math.Abs(newLocation.Column - end.Column);
                                }
                            }
                        }
                        
                        if (_mCalcGrid[newLocation.Row, newLocation.Column].HasBeenVisited)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (_mCalcGrid[newLocation.Row, newLocation.Column].G <= newG)
                            {
                                continue;
                            }
                        }

                        _mCalcGrid[newLocation.Row, newLocation.Column].ParentPosition = new Position(locationX, locationY);
                        _mCalcGrid[newLocation.Row, newLocation.Column].G = newG;

                        var h = Heuristic.DetermineH(_options.Formula, end, _options.HeuristicEstimate, newLocation.Column, newLocation.Row);

                        if (_options.TieBreaker)
                        {
                            var dx1 = locationX - end.Row;
                            var dy1 = locationY - end.Column;
                            var dx2 = start.Row - end.Row;
                            var dy2 = start.Column - end.Column;
                            var cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                            h = (int)(h + cross * 0.001);
                        }
                        _mCalcGrid[newLocation.Row, newLocation.Column].F = newG + h;

                        _open.Push(new Position(newLocation.Row, newLocation.Column));

                        _mCalcGrid[newLocation.Row, newLocation.Column].Open = true;
                    }

                    closedNodeCounter++;
                    _mCalcGrid[location.Row, location.Column].Open = false;

                }

                return !found ? null : OrderClosedListAsArray(end);
            }
        }

        private static bool IsCardinalOffset((sbyte row, sbyte column) offset)
        {
            return offset.row != 0 && offset.column != 0;
        }

        private PathFinderNode[] OrderClosedListAsArray(Position end)
        {
            _closed.Clear();

            var fNodeTmp = _mCalcGrid[end.Row, end.Column];

            var fNode = new PathFinderNode
            {
                F = fNodeTmp.F,
                G = fNodeTmp.G,
                H = 0,
                ParentPosition = fNodeTmp.ParentPosition,
                Position = new Position(end.Row, end.Column),
            };
 
            while (fNode.Position.Row != fNode.ParentPosition.Row || fNode.Position.Column != fNode.ParentPosition.Column)
            {
                _closed.Add(fNode);

                var posX = fNode.ParentPosition.Row;
                var posY = fNode.ParentPosition.Column;

                fNodeTmp = _mCalcGrid[posX, posY];
                fNode.F = fNodeTmp.F;
                fNode.G = fNodeTmp.G;
                fNode.H = 0;
                fNode.ParentPosition = fNodeTmp.ParentPosition;
                fNode.Position = new Position(posX, posY);
            }

            _closed.Add(fNode);

            return _closed.ToArray();
        }
    }
}
