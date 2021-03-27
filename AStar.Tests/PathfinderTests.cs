using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PathfinderTests
    {
        private byte[,] _grid;
        private PathFinder _pathFinder;

        [SetUp]
        public void SetUp()
        {
            _grid = CreateMatrix(8, 8);
            _pathFinder = new PathFinder(_grid);
        }

        [Test]
        public void ShouldPathRectangleGrid()
        {
            var grid = CreateMatrix(3, 5);
            var pathfinder = new PathFinder(grid);

            var path = pathfinder.FindPath(new Point(0, 0), new Point(2, 4));
            Console.WriteLine(Helper.PrintGrid(grid));
            Console.WriteLine(Helper.PrintPath(grid, path));

            path[0].X.ShouldBe(2);
            path[0].Y.ShouldBe(4);
            path[1].X.ShouldBe(2);
            path[1].Y.ShouldBe(3);
            path[2].X.ShouldBe(2);
            path[2].Y.ShouldBe(2);
            path[3].X.ShouldBe(1);
            path[3].Y.ShouldBe(1);
            path[4].X.ShouldBe(0);
            path[4].Y.ShouldBe(0);
        }

        [Test]
        public void ShouldPathToSelf()
        {
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(1, 1));
            path.Count.ShouldBe(1);

            var node = path[0];
            node.X.ShouldBe(1);
            node.Y.ShouldBe(1);
        }

        [Test]
        public void ShouldPathToAdjacent()
        {
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(2, 1));

            path.Count.ShouldBe(2);

            var node = path[0];

            node.X.ShouldBe(2);
            node.Y.ShouldBe(1);


            node = path[1];
            node.X.ShouldBe(1);
            node.Y.ShouldBe(1);


            Console.WriteLine(Helper.PrintGrid(_grid));
            Console.WriteLine(Helper.PrintPath(_grid, path));
        }

        [Test]
        public void ShouldDoSimplePath()
        {
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            path.Count.ShouldBe(4);

            var item = path[3];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(1);

            item = path[2];
            item.X.ShouldBe(2);
            item.Y.ShouldBe(2);

            item = path[1];
            item.X.ShouldBe(3);
            item.Y.ShouldBe(2);

            item = path[0];
            item.X.ShouldBe(4);
            item.Y.ShouldBe(2);
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonal()
        {
            var pathfinderOptions = new PathFinderOptions { Diagonals = false };
            _pathFinder = new PathFinder(_grid, pathfinderOptions);

            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            PrintCoordinates(path);

            path.Count.ShouldBe(5);

            var item = path[4];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(1);

            item = path[3];
            item.X.ShouldBe(2);
            item.Y.ShouldBe(1);

            item = path[2];
            item.X.ShouldBe(3);
            item.Y.ShouldBe(1);

            item = path[1];
            item.X.ShouldBe(4);
            item.Y.ShouldBe(1);

            item = path[0];
            item.X.ShouldBe(4);
            item.Y.ShouldBe(2);
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonalAroundObstacle()
        {
            var pathfinderOptions = new PathFinderOptions { Diagonals = false };
            _pathFinder = new PathFinder(_grid, pathfinderOptions);

            _grid[2, 0] = 0;
            _grid[2, 1] = 0;
            _grid[2, 2] = 0;

            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            PrintCoordinates(path);

            path.Count.ShouldBe(7);

            var item = path[6];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(1);

            item = path[5];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(2);

            item = path[4];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(3);

            item = path[3];
            item.X.ShouldBe(2);
            item.Y.ShouldBe(3);

            item = path[2];
            item.X.ShouldBe(3);
            item.Y.ShouldBe(3);

            item = path[1];
            item.X.ShouldBe(3);
            item.Y.ShouldBe(2);

            item = path[0];
            item.X.ShouldBe(4);
            item.Y.ShouldBe(2);

        }
        [Test]
        public void ShouldPathAroundObstacle()
        {
            _grid[2, 0] = 0;
            _grid[2, 1] = 0;
            _grid[2, 2] = 0;
            _grid[2, 3] = 0;
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            Helper.Print(_grid, path);
            path.Count.ShouldBe(6);

            var item = path[5];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(1);

            item = path[4];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(2);

            item = path[3];
            item.X.ShouldBe(1);
            item.Y.ShouldBe(3);

            item = path[2];
            item.X.ShouldBe(2);
            item.Y.ShouldBe(4);

            item = path[1];
            item.X.ShouldBe(3);
            item.Y.ShouldBe(3);

            item = path[0];
            item.X.ShouldBe(4);
            item.Y.ShouldBe(2);
        }

        [Test]
        public void ShouldFindNoPath()
        {
            _grid[2, 0] = 0;
            _grid[2, 1] = 0;
            _grid[2, 2] = 0;
            _grid[2, 3] = 0;
            _grid[2, 4] = 0;
            _grid[2, 5] = 0;
            _grid[2, 6] = 0;
            _grid[2, 7] = 0;
            var path = _pathFinder.FindPath(new Point(1, 1), new Point(4, 2));
            path.ShouldBe(null);
        }

        private static byte[,] CreateMatrix(int height, int width)
        {
            var mMatrix = new byte[height, width];

            for (var row = 0; row < mMatrix.GetLength(0); row++)
            {

                for (var column = 0; column < mMatrix.GetLength(1); column++)
                {
                    mMatrix[row, column] = 1;
                }
            }

            return mMatrix;
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

    }
}
