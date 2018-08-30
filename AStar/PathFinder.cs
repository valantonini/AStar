using System;
using System.Collections.Generic;
using System.Drawing;

namespace AStar
{
    public class PathFinder : IPathFinder
    {
        private readonly byte[,] _grid;
        private readonly IPriorityQueue<int> _open;
        private readonly List<PathFinderNode> _closed = new List<PathFinderNode>();
        private readonly PathFinderNodeFast[] _mCalcGrid;

        private readonly ushort _gridX;
        private readonly ushort _gridY;
        private readonly ushort _gridXMinus1;
        private readonly ushort _gridYLog2;

        private readonly bool _diagonals;
        private readonly sbyte[,] _direction;
        private readonly bool _heavyDiagonals;
        private readonly int _heuristicEstimate;
        private readonly bool _punishChangeDirection;
        private readonly bool _tieBreaker;
        private readonly int _searchLimit;
        private readonly HeuristicFormula _formula;

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
            _gridX = (ushort)(_grid.GetUpperBound(0) + 1);
            _gridY = (ushort)(_grid.GetUpperBound(1) + 1);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (Math.Log(_gridX, 2) != (int)Math.Log(_gridX, 2) || Math.Log(_gridY, 2) != (int)Math.Log(_gridY, 2))
            {
                throw new Exception("Invalid Grid, size in X and Y must be power of 2");
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            _gridXMinus1 = (ushort)(_gridX - 1);
            _gridYLog2 = (ushort)Math.Log(_gridY, 2);

            if (_mCalcGrid == null || _mCalcGrid.Length != (_gridX * _gridY))
            {
                _mCalcGrid = new PathFinderNodeFast[_gridX * _gridY];
            }

            _open = new PriorityQueueB<int>(new ComparePfNodeMatrix(_mCalcGrid));

            
            
            //set options
            if (pathFinderOptions == null)
            {
                pathFinderOptions = new PathFinderOptions();
            }

            _formula = pathFinderOptions.Formula;
            _heavyDiagonals = pathFinderOptions.HeavyDiagonals;
            _heuristicEstimate = pathFinderOptions.HeuristicEstimate;
            _punishChangeDirection = pathFinderOptions.PunishChangeDirection;
            _tieBreaker = pathFinderOptions.TieBreaker;
            _searchLimit = pathFinderOptions.SearchLimit;
            _diagonals = pathFinderOptions.Diagonals;
            _direction = pathFinderOptions.Diagonals
                    ? new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } }
                    : new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            lock (this)
            {

                // Is faster if we don't clear the matrix, just assign different values for open and close and ignore the rest
                // I could have user Array.Clear() but using unsafe code is faster, no much but it is.
                //fixed (PathFinderNodeFast* pGrid = tmpGrid) 
                //    ZeroMemory((byte*) pGrid, sizeof(PathFinderNodeFast) * 1000000);

                var found = false;
                var closedNodeCounter = 0;
                _openNodeValue += 2;
                _closeNodeValue += 2;
                _open.Clear();
                _closed.Clear();

                var location = (start.Y << _gridYLog2) + start.X;
                var endLocation = (end.Y << _gridYLog2) + end.X;
                _mCalcGrid[location].G = 0;
                _mCalcGrid[location].F = _heuristicEstimate;
                _mCalcGrid[location].PX = (ushort) start.X;
                _mCalcGrid[location].PY = (ushort) start.Y;
                _mCalcGrid[location].Status = _openNodeValue;

                _open.Push(location);
                while (_open.Count > 0)
                {
                    location = _open.Pop();

                    //Is it in closed list? means this node was already processed
                    if (_mCalcGrid[location].Status == _closeNodeValue)
                    {
                        continue;
                    }

                    var locationX = (ushort) (location & _gridXMinus1);
                    var locationY = (ushort) (location >> _gridYLog2);

                    if (location == endLocation)
                    {
                        _mCalcGrid[location].Status = _closeNodeValue;
                        found = true;
                        break;
                    }

                    if (closedNodeCounter > _searchLimit)
                    {
                        return null;
                    }

                    if (_punishChangeDirection)
                    {
                        _horiz = (locationX - _mCalcGrid[location].PX);
                    }

                    //Lets calculate each successors
                    for (var i = 0; i < (_diagonals ? 8 : 4); i++)
                    {
                        var newLocationX = (ushort) (locationX + _direction[i, 0]);
                        var newLocationY = (ushort) (locationY + _direction[i, 1]);
                        var newLocation = (newLocationY << _gridXLog2) + newLocationX;

                        if (newLocationX >= _gridX || newLocationY >= _gridY)
                        {
                            continue;
                        }

                        // Unbreakeable?
                        if (_grid[newLocationX, newLocationY] == 0)
                        {
                            continue;
                        }

                        int newG;
                        if (_heavyDiagonals && i > 3)
                        {
                            newG = _mCalcGrid[location].G + (int) (_grid[newLocationX, newLocationY]*2.41);
                        }
                        else
                        {
                            newG = _mCalcGrid[location].G + _grid[newLocationX, newLocationY];
                        }

                        if (_punishChangeDirection)
                        {
                            if ((newLocationX - locationX) != 0)
                            {
                                if (_horiz == 0)
                                {
                                    newG += Math.Abs(newLocationX - end.X) + Math.Abs(newLocationY - end.Y);
                                }
                            }
                            if ((newLocationY - locationY) != 0)
                            {
                                if (_horiz != 0)
                                {
                                    newG += Math.Abs(newLocationX - end.X) + Math.Abs(newLocationY - end.Y);
                                }
                            }
                        }

                        //Is it open or closed?
                        if (_mCalcGrid[newLocation].Status == _openNodeValue || _mCalcGrid[newLocation].Status == _closeNodeValue)
                        {
                            // The current node has less code than the previous? then skip this node
                            if (_mCalcGrid[newLocation].G <= newG)
                            {
                                continue;
                            }
                        }

                        _mCalcGrid[newLocation].PX = locationX;
                        _mCalcGrid[newLocation].PY = locationY;
                        _mCalcGrid[newLocation].G = newG;

                        var h = Heuristic.DetermineH(_formula, end, _heuristicEstimate, newLocationY, newLocationX);
                       
                        if (_tieBreaker)
                        {
                            var dx1 = locationX - end.X;
                            var dy1 = locationY - end.Y;
                            var dx2 = start.X - end.X;
                            var dy2 = start.Y - end.Y;
                            var cross = Math.Abs(dx1*dy2 - dx2*dy1);
                            h = (int) (h + cross*0.001);
                        }
                        _mCalcGrid[newLocation].F = newG + h;

                        //It is faster if we leave the open node in the priority queue
                        //When it is removed, it will be already closed, it will be ignored automatically
                        //if (tmpGrid[newLocation].Status == 1)
                        //{
                        //    //int removeX   = newLocation & gridXMinus1;
                        //    //int removeY   = newLocation >> gridYLog2;
                        //    mOpen.RemoveLocation(newLocation);
                        //}

                        //if (tmpGrid[newLocation].Status != 1)
                        //{
                        _open.Push(newLocation);
                        //}
                        _mCalcGrid[newLocation].Status = _openNodeValue;
                    }

                    closedNodeCounter++;
                    _mCalcGrid[location].Status = _closeNodeValue;

                }

                return !found ? null : OrderClosedListAsPath(end);
            }
        }

        private List<PathFinderNode> OrderClosedListAsPath(Point end)
        {
            _closed.Clear();

            var fNodeTmp = _mCalcGrid[(end.Y << _gridYLog2) + end.X];
            
            var fNode = new PathFinderNode
            {
                F = fNodeTmp.F,
                G = fNodeTmp.G,
                H = 0,
                Px = fNodeTmp.PX,
                Py = fNodeTmp.PY,
                X = end.X,
                Y = end.Y
            };

            while (fNode.X != fNode.Px || fNode.Y != fNode.Py)
            {
                _closed.Add(fNode);

                var posX = fNode.Px;
                var posY = fNode.Py;

                fNodeTmp = _mCalcGrid[(posY << _gridYLog2) + posX];
                fNode.F = fNodeTmp.F;
                fNode.G = fNodeTmp.G;
                fNode.H = 0;
                fNode.Px = fNodeTmp.PX;
                fNode.Py = fNodeTmp.PY;
                fNode.X = posX;
                fNode.Y = posY;
            }

            _closed.Add(fNode);

            return _closed;
        }
    }
}
