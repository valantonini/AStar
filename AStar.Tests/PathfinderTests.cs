using System;
using NUnit.Framework;
using Shouldly;

namespace AStar.Tests
{
    [TestFixture]
    public class PathfinderTests
    {
        private PathfinderGrid _pathfinderGrid;
        private PathFinder _pathFinder;

        [SetUp]
        public void SetUp()
        {
            _pathfinderGrid = CreateGridInitializedToOpen(8, 8);
            _pathFinder = new PathFinder(_pathfinderGrid);
        }

        [Test]
        public void ShouldPathRectangleGrid()
        {
            var grid = CreateGridInitializedToOpen(3, 5);
            var pathfinder = new PathFinder(grid);

            var path = pathfinder.FindPath(new Position(0, 0), new Position(2, 4));
            Console.WriteLine(Helper.PrintGrid(grid));
            Console.WriteLine(Helper.PrintPath(grid, path));

            path[0].Position.Row.ShouldBe(2);
            path[0].Position.Column.ShouldBe(4);
            path[1].Position.Row.ShouldBe(2);
            path[1].Position.Column.ShouldBe(3);
            path[2].Position.Row.ShouldBe(2);
            path[2].Position.Column.ShouldBe(2);
            path[3].Position.Row.ShouldBe(1);
            path[3].Position.Column.ShouldBe(1);
            path[4].Position.Row.ShouldBe(0);
            path[4].Position.Column.ShouldBe(0);
        }

        [Test]
        public void ShouldPathToSelf()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(1, 1));
            path.Length.ShouldBe(1);

            var node = path[0];
            node.Position.Row.ShouldBe(1);
            node.Position.Column.ShouldBe(1);
        }

        [Test]
        public void ShouldPathToAdjacent()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(2, 1));

            path.Length.ShouldBe(2);

            var node = path[0];

            node.Position.Row.ShouldBe(2);
            node.Position.Column.ShouldBe(1);


            node = path[1];
            node.Position.Row.ShouldBe(1);
            node.Position.Column.ShouldBe(1);


            Console.WriteLine(Helper.PrintGrid(_pathfinderGrid));
            Console.WriteLine(Helper.PrintPath(_pathfinderGrid, path));
        }

        [Test]
        public void ShouldDoSimplePath()
        {
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_pathfinderGrid, path);
            path.Length.ShouldBe(4);

            var item = path[3];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(1);

            item = path[2];
            item.Position.Row.ShouldBe(2);
            item.Position.Column.ShouldBe(2);

            item = path[1];
            item.Position.Row.ShouldBe(3);
            item.Position.Column.ShouldBe(2);

            item = path[0];
            item.Position.Row.ShouldBe(4);
            item.Position.Column.ShouldBe(2);
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonal()
        {
            var pathfinderOptions = new PathFinderOptions { Diagonals = false };
            _pathFinder = new PathFinder(_pathfinderGrid, pathfinderOptions);

            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_pathfinderGrid, path);
            PrintCoordinates(path);

            path.Length.ShouldBe(5);

            var item = path[4];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(1);

            item = path[3];
            item.Position.Row.ShouldBe(2);
            item.Position.Column.ShouldBe(1);

            item = path[2];
            item.Position.Row.ShouldBe(3);
            item.Position.Column.ShouldBe(1);

            item = path[1];
            item.Position.Row.ShouldBe(4);
            item.Position.Column.ShouldBe(1);

            item = path[0];
            item.Position.Row.ShouldBe(4);
            item.Position.Column.ShouldBe(2);
        }

        [Test]
        public void ShouldDoSimplePathWithNoDiagonalAroundObstacle()
        {
            var pathfinderOptions = new PathFinderOptions { Diagonals = false };
            _pathFinder = new PathFinder(_pathfinderGrid, pathfinderOptions);

            _pathfinderGrid[2, 0] = 0;
            _pathfinderGrid[2, 1] = 0;
            _pathfinderGrid[2, 2] = 0;

            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_pathfinderGrid, path);
            PrintCoordinates(path);

            path.Length.ShouldBe(7);

            var item = path[6];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(1);

            item = path[5];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(2);

            item = path[4];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(3);

            item = path[3];
            item.Position.Row.ShouldBe(2);
            item.Position.Column.ShouldBe(3);

            item = path[2];
            item.Position.Row.ShouldBe(3);
            item.Position.Column.ShouldBe(3);

            item = path[1];
            item.Position.Row.ShouldBe(3);
            item.Position.Column.ShouldBe(2);

            item = path[0];
            item.Position.Row.ShouldBe(4);
            item.Position.Column.ShouldBe(2);

        }
        [Test]
        public void ShouldPathAroundObstacle()
        {
            _pathfinderGrid[2, 0] = 0;
            _pathfinderGrid[2, 1] = 0;
            _pathfinderGrid[2, 2] = 0;
            _pathfinderGrid[2, 3] = 0;
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            Helper.Print(_pathfinderGrid, path);
            path.Length.ShouldBe(6);

            var item = path[5];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(1);

            item = path[4];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(2);

            item = path[3];
            item.Position.Row.ShouldBe(1);
            item.Position.Column.ShouldBe(3);

            item = path[2];
            item.Position.Row.ShouldBe(2);
            item.Position.Column.ShouldBe(4);

            item = path[1];
            item.Position.Row.ShouldBe(3);
            item.Position.Column.ShouldBe(3);

            item = path[0];
            item.Position.Row.ShouldBe(4);
            item.Position.Column.ShouldBe(2);
        }

        [Test]
        public void ShouldFindNoPath()
        {
            _pathfinderGrid[2, 0] = 0;
            _pathfinderGrid[2, 1] = 0;
            _pathfinderGrid[2, 2] = 0;
            _pathfinderGrid[2, 3] = 0;
            _pathfinderGrid[2, 4] = 0;
            _pathfinderGrid[2, 5] = 0;
            _pathfinderGrid[2, 6] = 0;
            _pathfinderGrid[2, 7] = 0;
            var path = _pathFinder.FindPath(new Position(1, 1), new Position(4, 2));
            path.ShouldBe(null);
        }

        private static PathfinderGrid CreateGridInitializedToOpen(int height, int width)
        {
            var grid = new PathfinderGrid(height, width);

            for (var row = 0; row < grid.Height; row++)
            {
                for (var column = 0; column < grid.Width; column++)
                {
                    grid[row, column] = 1;
                }
            }

            return grid;
        }

        private static void PrintCoordinates(PathFinderNode[] path)
        {
            foreach (var node in path)
            {
                Console.WriteLine(node.Position.Row);
                Console.WriteLine(node.Position.Column);
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
