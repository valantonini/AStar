using System;
using System.Collections.Generic;

namespace AStar
{
    public class PathFinder : IPathFinder
    {
        private readonly byte[,] _grid;
        private ushort _gridRows => (ushort)(_grid.GetLength(0));
        private ushort _gridColumns => (ushort)(_grid.GetLength(1));
        private readonly IPriorityQueue<Point> _open;
        private readonly List<PathFinderNode> _closed = new List<PathFinderNode>();
        private readonly PathFinderNodeFast[,] _mCalcGrid;
        private readonly sbyte[,] _direction;
        private PathFinderOptions _options;
        private int _horiz;
        private byte _openNodeValue = 1;
        private byte _closeNodeValue = 2;

        public PathFinder(byte[,] grid, PathFinderOptions pathFinderOptions = null)
        {
            if (grid == null)
            {
                throw new Exception("Grid cannot be null");
            }

            _grid = grid;

            if (_mCalcGrid == null || _mCalcGrid.GetLength(0) != _grid.GetLength(0) || _mCalcGrid.GetLength(1) != _grid.GetLength(1))
            {
                _mCalcGrid = new PathFinderNodeFast[_gridRows, _gridColumns];
            }

            _open = new PriorityQueueB<Point>(new ComparePfNodeMatrix(_mCalcGrid));

            _options = pathFinderOptions ?? new PathFinderOptions();

            _direction = _options.Diagonals
                    ? new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } }
                    : new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
        }

        public List<Point> FindPath(Point start, Point end)
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
                _mCalcGrid[start.Row, start.Column].ParentRow = (ushort)start.Row;
                _mCalcGrid[start.Row, start.Column].ParentColumn = (ushort)start.Column;
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

                    var locationRow = location.Row;
                    var locationColumn = location.Column;

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
                        _horiz = (locationRow - _mCalcGrid[location.Row, location.Column].ParentRow);
                    }

                    //Lets calculate each successors
                    for (var i = 0; i < _direction.GetLength(0); i++)
                    {
                        //unsign incase we went out of bounds
                        var newLocationRow = (ushort)(locationRow + _direction[i, 0]);
                        var newLocationColumn = (ushort)(locationColumn + _direction[i, 1]);

                        if (newLocationRow >= _gridRows || newLocationColumn >= _gridColumns)
                        {
                            continue;
                        }

                        // Unbreakeable?
                        if (_grid[newLocationRow, newLocationColumn] == 0)
                        {
                            continue;
                        }

                        int newG;
                        if (_options.HeavyDiagonals && i > 3)
                        {
                            newG = _mCalcGrid[location.Row, location.Column].Gone + (int)(_grid[newLocationRow, newLocationColumn] * 2.41);
                        }
                        else
                        {
                            newG = _mCalcGrid[location.Row, location.Column].Gone + _grid[newLocationRow, newLocationColumn];
                        }

                        if (_options.PunishChangeDirection)
                        {
                            if ((newLocationRow - locationRow) != 0)
                            {
                                if (_horiz == 0)
                                {
                                    newG += Math.Abs(newLocationRow - end.Row) + Math.Abs(newLocationColumn - end.Column);
                                }
                            }
                            if ((newLocationColumn - locationColumn) != 0)
                            {
                                if (_horiz != 0)
                                {
                                    newG += Math.Abs(newLocationRow - end.Row) + Math.Abs(newLocationColumn - end.Column);
                                }
                            }
                        }

                        //Is it open or closed?
                        if (_mCalcGrid[newLocationRow, newLocationColumn].Status == _openNodeValue || _mCalcGrid[newLocationRow, newLocationColumn].Status == _closeNodeValue)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (_mCalcGrid[newLocationRow, newLocationColumn].Gone <= newG)
                            {
                                continue;
                            }
                        }

                        _mCalcGrid[newLocationRow, newLocationColumn].ParentRow = locationRow;
                        _mCalcGrid[newLocationRow, newLocationColumn].ParentColumn = locationColumn;
                        _mCalcGrid[newLocationRow, newLocationColumn].Gone = newG;

                        var h = Heuristic.DetermineH(_options.Formula, end, _options.HeuristicEstimate, newLocationColumn, newLocationRow);

                        if (_options.TieBreaker)
                        {
                            var dx1 = locationRow - end.Row;
                            var dy1 = locationColumn - end.Column;
                            var dx2 = start.Row - end.Row;
                            var dy2 = start.Column - end.Column;
                            var cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                            h = (int)(h + cross * 0.001);
                        }
                        _mCalcGrid[newLocationRow, newLocationColumn].F_Gone_Plus_Heuristic = newG + h;

                        _open.Push(new Point(newLocationRow, newLocationColumn));

                        _mCalcGrid[newLocationRow, newLocationColumn].Status = _openNodeValue;
                    }

                    closedNodeCounter++;
                    _mCalcGrid[location.Row, location.Column].Status = _closeNodeValue;

                }

                return !found ? null : OrderClosedListAsPath(end);
            }
        }

        private List<Point> OrderClosedListAsPath(Point end)
        {
            var path = new List<Point>();

            var fNodeTmp = _mCalcGrid[end.Row, end.Column];

            var fNode = new PathFinderNode
            {
                F = fNodeTmp.F_Gone_Plus_Heuristic,
                Gone = fNodeTmp.Gone,
                heuristic = 0,
                ParentRow = fNodeTmp.ParentRow,
                ParentColumn = fNodeTmp.ParentColumn,
                Row = end.Row,
                Column = end.Column
            };

            while (fNode.Row != fNode.ParentRow || fNode.Column != fNode.ParentColumn)
            {
                path.Add(new Point(fNode.Column, fNode.Row));

                var posX = fNode.ParentRow;
                var posY = fNode.ParentColumn;

                fNodeTmp = _mCalcGrid[posX, posY];
                fNode.F = fNodeTmp.F_Gone_Plus_Heuristic;
                fNode.Gone = fNodeTmp.Gone;
                fNode.heuristic = 0;
                fNode.ParentRow = fNodeTmp.ParentRow;
                fNode.ParentColumn = fNodeTmp.ParentColumn;
                fNode.Row = posX;
                fNode.Column = posY;
            }

            path.Add(new Point(fNode.Column, fNode.Row));

            return path;
        }
    }
}
