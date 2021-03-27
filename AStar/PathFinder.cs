using System;
using System.Collections.Generic;

namespace AStar
{
    public class PathFinder : IPathFinder
    {
        private readonly Grid _grid;
        
        private readonly IPriorityQueue<Point> _open;
        private readonly List<PathFinderNode> _closed = new List<PathFinderNode>();
        private readonly PathFinderNodeFast[,] _mCalcGrid;
        private readonly sbyte[,] _direction;
        private PathFinderOptions _options;
        private int _horiz;
        private byte _openNodeValue = 1;
        private byte _closeNodeValue = 2;

        public PathFinder(Grid grid, PathFinderOptions pathFinderOptions = null)
        {
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));

            _mCalcGrid = new PathFinderNodeFast[grid.Height, grid.Width];

            _open = new PriorityQueueB<Point>(new ComparePfNodeMatrix(_mCalcGrid));

            _options = pathFinderOptions ?? new PathFinderOptions();

            _direction = _options.Diagonals
                    ? new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } }
                    : new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            lock (this)
            {
                var found = false;
                var closedNodeCounter = 0;
                _openNodeValue += 2;//increment for subsequent runs
                _closeNodeValue += 2;
                _open.Clear();
                _closed.Clear();

                _mCalcGrid[start.Row, start.Column].Gone = 0;
                _mCalcGrid[start.Row, start.Column].F_Gone_Plus_Heuristic = _options.HeuristicEstimate;
                _mCalcGrid[start.Row, start.Column].ParentX = (ushort)start.Row;
                _mCalcGrid[start.Row, start.Column].ParentY = (ushort)start.Column;
                _mCalcGrid[start.Row, start.Column].Status = _openNodeValue;

                _open.Push(start);

                while (_open.Count > 0)
                {
                    var location = _open.Pop();

                    //Is it in closed list? means this node was already processed
                    if (_mCalcGrid[location.Row, location.Column].Status == _closeNodeValue)
                    {
                        continue;
                    }

                    var locationX = location.Row;
                    var locationY = location.Column;

                    if (location == end)
                    {
                        _mCalcGrid[location.Row, location.Column].Status = _closeNodeValue;
                        found = true;
                        break;
                    }

                    if (closedNodeCounter > _options.SearchLimit)
                    {
                        return null;
                    }

                    if (_options.PunishChangeDirection)
                    {
                        _horiz = (locationX - _mCalcGrid[location.Row, location.Column].ParentX);
                    }

                    //Lets calculate each successors
                    for (var i = 0; i < _direction.GetLength(0); i++)
                    {
                        //unsign incase we went out of bounds
                        var newLocationX = (ushort)(locationX + _direction[i, 0]);
                        var newLocationY = (ushort)(locationY + _direction[i, 1]);

                        if (newLocationX >= _grid.Height || newLocationY >= +_grid.Width)
                        {
                            continue;
                        }

                        // Unbreakeable?
                        if (_grid[newLocationX, newLocationY] == 0)
                        {
                            continue;
                        }

                        int newG;
                        if (_options.HeavyDiagonals && i > 3)
                        {
                            newG = _mCalcGrid[location.Row, location.Column].Gone + (int)(_grid[newLocationX, newLocationY] * 2.41);
                        }
                        else
                        {
                            newG = _mCalcGrid[location.Row, location.Column].Gone + _grid[newLocationX, newLocationY];
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

                        //Is it open or closed?
                        if (_mCalcGrid[newLocationX, newLocationY].Status == _openNodeValue || _mCalcGrid[newLocationX, newLocationY].Status == _closeNodeValue)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (_mCalcGrid[newLocationX, newLocationY].Gone <= newG)
                            {
                                continue;
                            }
                        }

                        _mCalcGrid[newLocationX, newLocationY].ParentX = locationX;
                        _mCalcGrid[newLocationX, newLocationY].ParentY = locationY;
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

                        _open.Push(new Point(newLocationX, newLocationY));

                        _mCalcGrid[newLocationX, newLocationY].Status = _openNodeValue;
                    }

                    closedNodeCounter++;
                    _mCalcGrid[location.Row, location.Column].Status = _closeNodeValue;

                }

                return !found ? null : OrderClosedListAsPath(end);
            }
        }

        private List<PathFinderNode> OrderClosedListAsPath(Point end)
        {
            _closed.Clear();

            var fNodeTmp = _mCalcGrid[end.Row, end.Column];

            var fNode = new PathFinderNode
            {
                F_Gone_Plus_Heuristic = fNodeTmp.F_Gone_Plus_Heuristic,
                Gone = fNodeTmp.Gone,
                Heuristic = 0,
                ParentX = fNodeTmp.ParentX,
                ParentY = fNodeTmp.ParentY,
                X = end.Row,
                Y = end.Column
            };

            while (fNode.X != fNode.ParentX || fNode.Y != fNode.ParentY)
            {
                _closed.Add(fNode);

                var posX = fNode.ParentX;
                var posY = fNode.ParentY;

                fNodeTmp = _mCalcGrid[posX, posY];
                fNode.F_Gone_Plus_Heuristic = fNodeTmp.F_Gone_Plus_Heuristic;
                fNode.Gone = fNodeTmp.Gone;
                fNode.Heuristic = 0;
                fNode.ParentX = fNodeTmp.ParentX;
                fNode.ParentY = fNodeTmp.ParentY;
                fNode.X = posX;
                fNode.Y = posY;
            }

            _closed.Add(fNode);

            return _closed;
        }
    }
}
