using System;
using System.Collections.Generic;

namespace AStar
{
    public class PathFinder : IFindAPath
    {
        private readonly PathfinderGrid _pathfinderGrid;
        private readonly IPriorityQueue<Position> _open;
        private readonly List<PathFinderNode> _closed = new List<PathFinderNode>();
        private readonly PathFinderNodeFast[,] _mCalcGrid;
        private readonly PathFinderOptions _options;
        private int _horiz;

        public PathFinder(PathfinderGrid pathfinderGrid, PathFinderOptions pathFinderOptions = null)
        {
            _pathfinderGrid = pathfinderGrid ?? throw new ArgumentNullException(nameof(pathfinderGrid));

            _mCalcGrid = new PathFinderNodeFast[pathfinderGrid.Height, pathfinderGrid.Width];

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

                _mCalcGrid[start.Row, start.Column].Gone = 0;
                _mCalcGrid[start.Row, start.Column].F_Gone_Plus_Heuristic = _options.HeuristicEstimate;
                _mCalcGrid[start.Row, start.Column].Parent = new Position(start.Row, start.Column);
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
                        _horiz = (locationX - _mCalcGrid[location.Row, location.Column].Parent.Row);
                    }

                    //Lets calculate each successors
                    foreach (var offsets in GridOffsets.GetOffsets(_options.Diagonals))
                    {
                        //unsign incase we went out of bounds
                        var newLocationX = (ushort)(locationX + offsets.row);
                        var newLocationY = (ushort)(locationY + offsets.column);

                        if (newLocationX >= _pathfinderGrid.Height || newLocationY >= +_pathfinderGrid.Width)
                        {
                            continue;
                        }

                        // Unbreakeable?
                        if (_pathfinderGrid[newLocationX, newLocationY] == 0)
                        {
                            continue;
                        }

                        int newG;
                        if (_options.HeavyDiagonals && !IsCardinalOffset(offsets))
                        {
                            newG = _mCalcGrid[location.Row, location.Column].Gone + (int)(_pathfinderGrid[newLocationX, newLocationY] * 2.41);
                        }
                        else
                        {
                            newG = _mCalcGrid[location.Row, location.Column].Gone + _pathfinderGrid[newLocationX, newLocationY];
                        }

                        if (_options.PunishChangeDirection)
                        {
                            if ((newLocationX - locationX) != 0)
                            {
                                if (_horiz == 0)
                                {
                                    newG += Math.Abs(newLocationX - end.Row) + Math.Abs(newLocationY - end.Column);
                                }
                            }
                            if ((newLocationY - locationY) != 0)
                            {
                                if (_horiz != 0)
                                {
                                    newG += Math.Abs(newLocationX - end.Row) + Math.Abs(newLocationY - end.Column);
                                }
                            }
                        }
                        
                        if (_mCalcGrid[newLocationX, newLocationY].HasBeenVisited)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (_mCalcGrid[newLocationX, newLocationY].Gone <= newG)
                            {
                                continue;
                            }
                        }

                        _mCalcGrid[newLocationX, newLocationY].Parent = new Position(locationX, locationY);
                        _mCalcGrid[newLocationX, newLocationY].Gone = newG;

                        var h = Heuristic.DetermineH(_options.Formula, end, _options.HeuristicEstimate, newLocationY, newLocationX);

                        if (_options.TieBreaker)
                        {
                            var dx1 = locationX - end.Row;
                            var dy1 = locationY - end.Column;
                            var dx2 = start.Row - end.Row;
                            var dy2 = start.Column - end.Column;
                            var cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                            h = (int)(h + cross * 0.001);
                        }
                        _mCalcGrid[newLocationX, newLocationY].F_Gone_Plus_Heuristic = newG + h;

                        _open.Push(new Position(newLocationX, newLocationY));

                        _mCalcGrid[newLocationX, newLocationY].Open = true;
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
                F = fNodeTmp.F_Gone_Plus_Heuristic,
                G = fNodeTmp.Gone,
                H = 0,
                Parent = fNodeTmp.Parent,
                Position = new Position(end.Row, end.Column),
            };
 
            while (fNode.Position.Row != fNode.Parent.Row || fNode.Position.Column != fNode.Parent.Column)
            {
                _closed.Add(fNode);

                var posX = fNode.Parent.Row;
                var posY = fNode.Parent.Column;

                fNodeTmp = _mCalcGrid[posX, posY];
                fNode.F = fNodeTmp.F_Gone_Plus_Heuristic;
                fNode.G = fNodeTmp.Gone;
                fNode.H = 0;
                fNode.Parent = fNodeTmp.Parent;
                fNode.Position = new Position(posX, posY);
            }

            _closed.Add(fNode);

            return _closed.ToArray();
        }
    }
}
