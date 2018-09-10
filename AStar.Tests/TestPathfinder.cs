using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;
using FluentAssertions;

namespace AStar.Tests
{
    public class TestPathfinder
    {
        private byte[,] _grid;
        private PathFinder _pathFinder;

        public void SetUp()
        {
            _grid = CreateMatrix(8);
            _pathFinder = new PathFinder(_grid);
        }

        [Fact]
        public void ShouldPathToSelf()
        {
            SetUp();
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(1, 1));
            path.Count.Should().Be(1);
            
            var node = path[0];
            node.X.Should().Be(1);
            node.Y.Should().Be(1);
        }

        [Fact]
        public void ShouldPathToAdjacent()
        {
            SetUp();
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(2, 1));
            
            path.Count.Should().Be(2);

            var node = path[0];
            
            node.X.Should().Be(2);
            node.Y.Should().Be(1);

            node = path[1];
            node.X.Should().Be(1);
            node.Y.Should().Be(1);
        }

        [Fact]
        public void ShouldDoSimplePath()
        {
            SetUp();
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            path.Count.Should().Be(4);

            var item = path[3];
            item.X.Should().Be(1);
            item.Y.Should().Be(1);

            item = path[2];
            item.X.Should().Be(2);
            item.Y.Should().Be(2);

            item = path[1];
            item.X.Should().Be(3);
            item.Y.Should().Be(2);

            item = path[0];
            item.X.Should().Be(4);
            item.Y.Should().Be(2);
        }

        [Fact]
        public void ShouldDoSimplePathWithNoDiagonal()
        {
            SetUp();
            var pathfinderOptions = new PathFinderOptions { Diagonals = false};
            _pathFinder = new PathFinder(_grid, pathfinderOptions);

            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            PrintCoordinates(path);

            path.Count.Should().Be(5);

            var item = path[4];
            item.X.Should().Be(1);
            item.Y.Should().Be(1);

            item = path[3];
            item.X.Should().Be(2);
            item.Y.Should().Be(1);
            
            item = path[2];
            item.X.Should().Be(3);
            item.Y.Should().Be(1);
            
            item = path[1];
            item.X.Should().Be(4);
            item.Y.Should().Be(1);
            
            item = path[0];
            item.X.Should().Be(4);
            item.Y.Should().Be(2);
        }

        [Fact]
        public void ShouldDoSimplePathWithNoDiagonalAroundObstacle()
        {
            SetUp();
            var pathfinderOptions = new PathFinderOptions { Diagonals = false };
            _pathFinder = new PathFinder(_grid, pathfinderOptions);

            _grid[2, 0] = 0;
            _grid[2, 1] = 0;
            _grid[2, 2] = 0;

            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            PrintCoordinates(path);

            path.Count.Should().Be(7);

            var item = path[6];
            item.X.Should().Be(1);
            item.Y.Should().Be(1);

            item = path[5];
            item.X.Should().Be(1);
            item.Y.Should().Be(2);

            item = path[4];
            item.X.Should().Be(1);
            item.Y.Should().Be(3);

            item = path[3];
            item.X.Should().Be(2);
            item.Y.Should().Be(3);

            item = path[2];
            item.X.Should().Be(3);
            item.Y.Should().Be(3);

            item = path[1];
            item.X.Should().Be(3);
            item.Y.Should().Be(2);

            item = path[0];
            item.X.Should().Be(4);
            item.Y.Should().Be(2);
           
        }

        private static void PrintCoordinates(List<PathFinderNode> path)
        {
            foreach (var node in path)
            {
                Console.WriteLine(node.X);
                Console.WriteLine(node.Y);
                Console.WriteLine(Environment.NewLine);
            }
        }

        [Fact]
        public void ShouldPathAroundObstacle()
        {
            SetUp();
            _grid[2, 0] = 0;
            _grid[2, 1] = 0;
            _grid[2, 2] = 0;
            _grid[2, 3] = 0;
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            path.Count.Should().Be(6);

            var item = path[5];
            item.X.Should().Be(1);
            item.Y.Should().Be(1);

            item = path[4];
            item.X.Should().Be(1);
            item.Y.Should().Be(2);

            item = path[3];
            item.X.Should().Be(1);
            item.Y.Should().Be(3);

            item = path[2];
            item.X.Should().Be(2);
            item.Y.Should().Be(4);

            item = path[1];
            item.X.Should().Be(3);
            item.Y.Should().Be(3);

            item = path[0];
            item.X.Should().Be(4);
            item.Y.Should().Be(2);
        }

        [Fact]
        public void ShouldFindNoPath()
        {
            SetUp();
            _grid[2, 0] = 0;
            _grid[2, 1] = 0;
            _grid[2, 2] = 0;
            _grid[2, 3] = 0;
            _grid[2, 4] = 0;
            _grid[2, 5] = 0;
            _grid[2, 6] = 0;
            _grid[2, 7] = 0;
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            path.Should().BeNull();
        }


        private static byte[,] CreateMatrix(int size)
        {
            var mMatrix = new byte[size, size];
            
            for (var y = 0; y < mMatrix.GetUpperBound(1); y++)
            {

                for (var x = 0; x < mMatrix.GetUpperBound(0); x++)
                {
                    mMatrix[x, y] = 1;
                }
            }

            return mMatrix;
        }
    }
}
